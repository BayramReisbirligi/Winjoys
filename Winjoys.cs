using System.Runtime.InteropServices;
using ReisProduction.Winjoys.Enums;
using System.Diagnostics;
using Windows.Graphics;
using Windows.System;
using System.Text;
#pragma warning disable IDE0079
#pragma warning disable SYSLIB1054
namespace ReisProduction.Winjoys;
public static class InputInjector
{
    private static readonly int ProcessorCountX2 = Environment.ProcessorCount * 2;
    private const int
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        KEYEVENTF_EXTENDEDKEY = 0x0001,
        KEYEVENTF_KEYUP = 0x0002,
        KEYEVENTF_SCANCODE = 0x0008,
        MOUSEEVENTF_WHEEL = 0x0800,
        MOUSEEVENTF_XDOWN = 0x0080,
        MOUSEEVENTF_XUP = 0x0100,
        XBUTTON1 = 0x0001,
        XBUTTON2 = 0x0002,
        WHEEL_DELTA = -120,
        INPUT_KEYBOARD = 1,
        INPUT_MOUSE = 0,
        MOUSEEVENTF_MOVE = 0x0001,
        MOUSEEVENTF_LEFTDOWN = 0x0002,
        MOUSEEVENTF_LEFTUP = 0x0004,
        MOUSEEVENTF_RIGHTDOWN = 0x0008,
        MOUSEEVENTF_RIGHTUP = 0x0010,
        MOUSEEVENTF_MIDDLEDOWN = 0x0020,
        MOUSEEVENTF_MIDDLEUP = 0x0040,
        MOUSEEVENTF_ABSOLUTE = 0x8000;
    [StructLayout(LayoutKind.Sequential)]
    private struct POINT { public int X, Y; }
    [StructLayout(LayoutKind.Sequential)]
    private struct INPUT
    {
        internal int type;
        internal INPUTUNION u;
    }
    [StructLayout(LayoutKind.Explicit)]
    private struct INPUTUNION
    {
        [FieldOffset(0)] internal MOUSEINPUT mi;
        [FieldOffset(0)] internal KEYBDINPUT ki;
        [FieldOffset(0)] internal HARDWAREINPUT hi;
    }
    [StructLayout(LayoutKind.Sequential)]
    private struct KEYBDINPUT
    {
        internal ushort wVk;
        internal ushort wScan;
        internal int dwFlags;
        internal int time;
        internal nint dwExtraInfo;
    }
    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEINPUT
    {
        internal int dx;
        internal int dy;
        internal int mouseData;
        internal int dwFlags;
        internal int time;
        internal nint dwExtraInfo;
    }
    [StructLayout(LayoutKind.Sequential)]
    private struct HARDWAREINPUT
    {
        internal int uMsg;
        internal short wParamL;
        internal short wParamH;
    }
    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
    [DllImport("user32.dll", SetLastError = true)]
    private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool PostMessage(nint hWnd, uint Msg, nint wParam, nint lParam);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern nint SendMessage(nint hWnd, uint Msg, nint wParam, nint lParam);
    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint MapVirtualKey(uint uCode, uint uMapType);
    [DllImport("user32.dll", SetLastError = true)]
    private static extern nint GetMessageExtraInfo();
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool GetCursorPos(out POINT lpPoint);
    [DllImport("user32.dll", SetLastError = true)]
    private static extern nint GetForegroundWindow();
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern nint FindWindow(string? lpClassName, string lpWindowName);
    private static readonly List<InputType> _specialTypes =
    [
        InputType.LeftButton, InputType.RightButton, InputType.MiddleButton,
        InputType.XButton1, InputType.XButton2, InputType.MouseScrollUp, InputType.MouseScrollDown,
        InputType.MouseNavigateUp, InputType.MouseNavigateUpSmooth,
        InputType.MouseNavigateDown, InputType.MouseNavigateDownSmooth,
        InputType.MouseNavigateLeft, InputType.MouseNavigateLeftSmooth,
        InputType.MouseNavigateRight, InputType.MouseNavigateRightSmooth,
        InputType.MouseNavigateToXY, InputType.MouseNavigateToXYSmooth
    ];
    public static async Task HandleInputs(InputType[] inputs, bool isDown, ActionType actionType = ActionType.SendGameKey, string windowTitle = "", SizeInt32? screenSize = null, PointInt32? point = null, int durationMs = 0, int pixelsPerStepDelay = 23, int stepTime = 1)
    {
        InputType[] specialInputs = [.. inputs.Where(_specialTypes.Contains)];
        InputType[] normalInputs = [.. inputs.Where(i => !_specialTypes.Contains(i))];
        SpecialType[] specials = [.. specialInputs.Select(i => i.ToSpecial())];
        VirtualKey[] keys = [.. normalInputs.Select(i => i.ToVirtualKey())];
        if (keys.Length > 0)
        {
            var states = new bool[keys.Length];
            for (int i = 0; i < keys.Length; i++)
                states[i] = isDown;
            await HandleKeys(keys, states, actionType, windowTitle);
        }
        if (specials.Length > 0)
            await HandleSpecials(specials, isDown, screenSize, point, durationMs, pixelsPerStepDelay, stepTime);
    }
    public static async Task HandleKeys(VirtualKey[] keys, bool[] states, ActionType actionType = ActionType.SendGameKey, string windowTitle = "")
    {
        Action<VirtualKey[], bool[], string> handler = actionType switch
        {
            ActionType.SendInput => SendInput,
            ActionType.KybdEvent => KybdEvent,
            ActionType.PostMessage => PostMessage,
            ActionType.SendMessage => SendMessage,
            _ => SendGameInput
        };
        handler(keys, states, windowTitle);
        await Task.CompletedTask;
    }
    public static async Task HandleSpecials(SpecialType[] specials, bool isDown, SizeInt32? screenSize = null, PointInt32? point = null, int durationMs = 0, int pixelsPerStepDelay = 23, int stepTime = 1)
    {
        foreach (var input in specials)
            switch (input)
            {
                case SpecialType.LeftButton:
                case SpecialType.RightButton:
                    MouseClick(input is SpecialType.LeftButton, isDown);
                    break;
                case SpecialType.MiddleButton:
                    MouseMiddleClick(true);
                    break;
                case SpecialType.MouseScrollUp:
                    MouseWheelDown(-screenSize!.Value.Height);
                    break;
                case SpecialType.MouseScrollDown:
                    MouseWheelDown(screenSize!.Value.Height);
                    break;
                case SpecialType.MouseNavigateUp:
                    MouseMoveRelative(0, screenSize!.Value.Height);
                    break;
                case SpecialType.MouseNavigateUpSmooth:
                    await MouseMoveRelativeSmooth(0, -screenSize!.Value.Width, screenSize!.Value.Height);
                    break;
                case SpecialType.MouseNavigateDown:
                    MouseMoveRelative(0, screenSize!.Value.Width);
                    break;
                case SpecialType.MouseNavigateDownSmooth:
                    await MouseMoveRelativeSmooth(0, screenSize!.Value.Width, screenSize!.Value.Height);
                    break;
                case SpecialType.MouseNavigateLeft:
                    MouseMoveRelative(-screenSize!.Value.Width, 0);
                    break;
                case SpecialType.MouseNavigateLeftSmooth:
                    await MouseMoveRelativeSmooth(-screenSize!.Value.Width, 0, screenSize!.Value.Height);
                    break;
                case SpecialType.MouseNavigateRight:
                    MouseMoveRelative(screenSize!.Value.Width, 0);
                    break;
                case SpecialType.MouseNavigateRightSmooth:
                    await MouseMoveRelativeSmooth(screenSize!.Value.Width, 0, screenSize!.Value.Height);
                    break;
                case SpecialType.MouseNavigateToXY:
                    MouseMoveTo(screenSize!.Value, point!.Value.X, point!.Value.Y);
                    break;
                case SpecialType.MouseNavigateToXYSmooth:
                    await MouseMoveToSmooth(screenSize!.Value, point!.Value.X, point.Value.X, durationMs, pixelsPerStepDelay, stepTime);
                    break;
            }
    }
    public static async Task HybridDelay(int delayMs, int spinAheadMs, CancellationToken token)
    {
        var sw = Stopwatch.StartNew();
        int delayTime = delayMs - Math.Min(spinAheadMs, delayMs);
        if (delayTime > 0)
            try { await Task.Delay(delayTime, token); } catch (TaskCanceledException) { return; }
        while (sw.Elapsed.TotalMilliseconds < delayMs && !token.IsCancellationRequested)
            Thread.SpinWait(ProcessorCountX2);
    }
    public static async Task PressKey(VirtualKey[] keys, int delay, int spinAheadMs, CancellationToken token, ActionType actionType = ActionType.SendGameKey, string windowTitle = "")
    {
        var states = new bool[keys.Length];
        Array.Fill(states, true);
        await HandleKeys(keys, states, actionType, windowTitle);
        await HybridDelay(delay, spinAheadMs, token);
        Array.Fill(states, false);
        await HandleKeys(keys, states, actionType, windowTitle);
    }
    public static VirtualKey ToVirtualKey(this InputType input) =>
    input switch
    {
        InputType.LeftButton => VirtualKey.LeftButton,
        InputType.RightButton => VirtualKey.RightButton,
        InputType.Cancel => VirtualKey.Cancel,
        InputType.MiddleButton => VirtualKey.MiddleButton,
        InputType.XButton1 => VirtualKey.XButton1,
        InputType.XButton2 => VirtualKey.XButton2,
        InputType.MouseScrollUp => VirtualKey.NavigationAccept,
        InputType.MouseScrollDown => VirtualKey.NavigationCancel,
        InputType.MouseNavigateUp => VirtualKey.NavigationUp,
        InputType.MouseNavigateUpSmooth => VirtualKey.GamepadDPadUp,
        InputType.MouseNavigateDown => VirtualKey.NavigationDown,
        InputType.MouseNavigateDownSmooth => VirtualKey.GamepadDPadDown,
        InputType.MouseNavigateLeft => VirtualKey.NavigationLeft,
        InputType.MouseNavigateLeftSmooth => VirtualKey.GamepadDPadLeft,
        InputType.MouseNavigateRight => VirtualKey.NavigationRight,
        InputType.MouseNavigateRightSmooth => VirtualKey.GamepadDPadRight,
        InputType.MouseNavigateToXY => VirtualKey.NavigationView,
        InputType.MouseNavigateToXYSmooth => VirtualKey.NavigationMenu,
        InputType.Back => VirtualKey.Back,
        InputType.Tab => VirtualKey.Tab,
        InputType.Escape => VirtualKey.Escape,
        InputType.Clear => VirtualKey.Clear,
        InputType.Enter => VirtualKey.Enter,
        InputType.Shift => VirtualKey.Shift,
        InputType.LeftShift => VirtualKey.LeftShift,
        InputType.RightShift => VirtualKey.RightShift,
        InputType.Control => VirtualKey.Control,
        InputType.LeftControl => VirtualKey.LeftControl,
        InputType.RightControl => VirtualKey.RightControl,
        InputType.Menu => VirtualKey.Menu,
        InputType.LeftMenu => VirtualKey.LeftMenu,
        InputType.RightMenu => VirtualKey.RightMenu,
        InputType.LeftWindows => VirtualKey.LeftWindows,
        InputType.RightWindows => VirtualKey.RightWindows,
        InputType.Pause => VirtualKey.Pause,
        InputType.CapitalLock => VirtualKey.CapitalLock,
        InputType.Kana => VirtualKey.Kana,
        InputType.Hanja => VirtualKey.Hanja,
        InputType.Convert => VirtualKey.Convert,
        InputType.NonConvert => VirtualKey.NonConvert,
        InputType.Accept => VirtualKey.Accept,
        InputType.ModeChange => VirtualKey.ModeChange,
        InputType.Space => VirtualKey.Space,
        InputType.PageUp => VirtualKey.PageUp,
        InputType.PageDown => VirtualKey.PageDown,
        InputType.End => VirtualKey.End,
        InputType.Home => VirtualKey.Home,
        InputType.Left => VirtualKey.Left,
        InputType.Up => VirtualKey.Up,
        InputType.Right => VirtualKey.Right,
        InputType.Down => VirtualKey.Down,
        InputType.Select => VirtualKey.Select,
        InputType.PrintScreen => VirtualKey.Print,
        InputType.Execute => VirtualKey.Execute,
        InputType.Snapshot => VirtualKey.Snapshot,
        InputType.Insert => VirtualKey.Insert,
        InputType.Delete => VirtualKey.Delete,
        InputType.Help => VirtualKey.Help,
        InputType.Number0 => VirtualKey.Number0,
        InputType.Number1 => VirtualKey.Number1,
        InputType.Number2 => VirtualKey.Number2,
        InputType.Number3 => VirtualKey.Number3,
        InputType.Number4 => VirtualKey.Number4,
        InputType.Number5 => VirtualKey.Number5,
        InputType.Number6 => VirtualKey.Number6,
        InputType.Number7 => VirtualKey.Number7,
        InputType.Number8 => VirtualKey.Number8,
        InputType.Number9 => VirtualKey.Number9,
        InputType.A => VirtualKey.A,
        InputType.B => VirtualKey.B,
        InputType.C => VirtualKey.C,
        InputType.D => VirtualKey.D,
        InputType.E => VirtualKey.E,
        InputType.F => VirtualKey.F,
        InputType.G => VirtualKey.G,
        InputType.H => VirtualKey.H,
        InputType.I => VirtualKey.I,
        InputType.J => VirtualKey.J,
        InputType.K => VirtualKey.K,
        InputType.L => VirtualKey.L,
        InputType.M => VirtualKey.M,
        InputType.N => VirtualKey.N,
        InputType.O => VirtualKey.O,
        InputType.P => VirtualKey.P,
        InputType.Q => VirtualKey.Q,
        InputType.R => VirtualKey.R,
        InputType.S => VirtualKey.S,
        InputType.T => VirtualKey.T,
        InputType.U => VirtualKey.U,
        InputType.V => VirtualKey.V,
        InputType.W => VirtualKey.W,
        InputType.X => VirtualKey.X,
        InputType.Y => VirtualKey.Y,
        InputType.Z => VirtualKey.Z,
        InputType.Application => VirtualKey.Application,
        InputType.Sleep => VirtualKey.Sleep,
        InputType.NumberPad0 => VirtualKey.NumberPad0,
        InputType.NumberPad1 => VirtualKey.NumberPad1,
        InputType.NumberPad2 => VirtualKey.NumberPad2,
        InputType.NumberPad3 => VirtualKey.NumberPad3,
        InputType.NumberPad4 => VirtualKey.NumberPad4,
        InputType.NumberPad5 => VirtualKey.NumberPad5,
        InputType.NumberPad6 => VirtualKey.NumberPad6,
        InputType.NumberPad7 => VirtualKey.NumberPad7,
        InputType.NumberPad8 => VirtualKey.NumberPad8,
        InputType.NumberPad9 => VirtualKey.NumberPad9,
        InputType.NumberKeyLock => VirtualKey.NumberKeyLock,
        InputType.Multiply => VirtualKey.Multiply,
        InputType.Add => VirtualKey.Add,
        InputType.Separator => VirtualKey.Separator,
        InputType.Subtract => VirtualKey.Subtract,
        InputType.Decimal => VirtualKey.Decimal,
        InputType.Divide => VirtualKey.Divide,
        InputType.F1 => VirtualKey.F1,
        InputType.F2 => VirtualKey.F2,
        InputType.F3 => VirtualKey.F3,
        InputType.F4 => VirtualKey.F4,
        InputType.F5 => VirtualKey.F5,
        InputType.F6 => VirtualKey.F6,
        InputType.F7 => VirtualKey.F7,
        InputType.F8 => VirtualKey.F8,
        InputType.F9 => VirtualKey.F9,
        InputType.F10 => VirtualKey.F10,
        InputType.F11 => VirtualKey.F11,
        InputType.F12 => VirtualKey.F12,
        InputType.F13 => VirtualKey.F13,
        InputType.F14 => VirtualKey.F14,
        InputType.F15 => VirtualKey.F15,
        InputType.F16 => VirtualKey.F16,
        InputType.F17 => VirtualKey.F17,
        InputType.F18 => VirtualKey.F18,
        InputType.F19 => VirtualKey.F19,
        InputType.F20 => VirtualKey.F20,
        InputType.F21 => VirtualKey.F21,
        InputType.F22 => VirtualKey.F22,
        InputType.F23 => VirtualKey.F23,
        InputType.F24 => VirtualKey.F24,
        InputType.Scroll => VirtualKey.Scroll,
        InputType.Oem1 => (VirtualKey)0xBA,
        InputType.OemPlus => (VirtualKey)0xBB,
        InputType.OemComma => (VirtualKey)0xBC,
        InputType.OemMinus => (VirtualKey)0xBD,
        InputType.OemPeriod => (VirtualKey)0xBE,
        InputType.Oem2 => (VirtualKey)0xBF,
        InputType.Oem3 => (VirtualKey)0xC0,
        InputType.Oem4 => (VirtualKey)0xDB,
        InputType.Oem5 => (VirtualKey)0xDC,
        InputType.Oem6 => (VirtualKey)0xDD,
        InputType.Oem7 => (VirtualKey)0xDE,
        InputType.Oem8 => (VirtualKey)0xDF,
        InputType.Oem102 => (VirtualKey)0xE2,
        InputType.ProcessKey => (VirtualKey)0xE5,
        InputType.Packet => (VirtualKey)0xE7,
        InputType.Attn => (VirtualKey)0xF6,
        InputType.CrSel => (VirtualKey)0xF7,
        InputType.ExSel => (VirtualKey)0xF8,
        InputType.EraseEof => (VirtualKey)0xF9,
        InputType.Play => (VirtualKey)0xFA,
        InputType.Zoom => (VirtualKey)0xFB,
        InputType.NoName => (VirtualKey)0xFC,
        InputType.Pa1 => (VirtualKey)0xFD,
        InputType.OemClear => (VirtualKey)0xFE,
        _ => VirtualKey.None
    };
    public static SpecialType ToSpecial(this InputType input) => input switch
    {
        InputType.LeftButton => SpecialType.LeftButton,
        InputType.RightButton => SpecialType.RightButton,
        InputType.MiddleButton => SpecialType.MiddleButton,
        InputType.XButton1 => SpecialType.XButton1,
        InputType.XButton2 => SpecialType.XButton2,
        InputType.MouseScrollUp => SpecialType.MouseScrollUp,
        InputType.MouseScrollDown => SpecialType.MouseScrollDown,
        InputType.MouseNavigateUp => SpecialType.MouseNavigateUp,
        InputType.MouseNavigateUpSmooth => SpecialType.MouseNavigateUpSmooth,
        InputType.MouseNavigateDown => SpecialType.MouseNavigateDown,
        InputType.MouseNavigateDownSmooth => SpecialType.MouseNavigateDownSmooth,
        InputType.MouseNavigateLeft => SpecialType.MouseNavigateLeft,
        InputType.MouseNavigateLeftSmooth => SpecialType.MouseNavigateLeftSmooth,
        InputType.MouseNavigateRight => SpecialType.MouseNavigateRight,
        InputType.MouseNavigateRightSmooth => SpecialType.MouseNavigateRightSmooth,
        InputType.MouseNavigateToXY => SpecialType.MouseNavigateToXY,
        InputType.MouseNavigateToXYSmooth => SpecialType.MouseNavigateToXYSmooth,
        _ => throw new InvalidOperationException($"{input} is not Special Input")
    };
    public static VirtualKey[] ToInputs(string sentence)
    {
        List<VirtualKey> keys = [];
        foreach (char c in sentence)
            if (char.IsLetter(c))
            {
                if (char.IsUpper(c)) keys.Add(VirtualKey.LeftShift);
                keys.Add((VirtualKey)((int)VirtualKey.A + (char.ToUpper(c) - 'A')));
            }
            else if (char.IsDigit(c))
                keys.Add((VirtualKey)((int)VirtualKey.Number0 + (c - '0')));
            else
                switch (c)
                {
                    case ' ': keys.Add(VirtualKey.Space); break;
                    case '.': keys.Add(VirtualKey.Decimal); break;
                    case '+': keys.Add(VirtualKey.Add); break;
                    case '-': keys.Add(VirtualKey.Subtract); break;
                    case '*': keys.Add(VirtualKey.Multiply); break;
                    case '/': keys.Add(VirtualKey.Divide); break;
                    case ';': keys.Add((VirtualKey)0xBA); break;
                    case '=': keys.Add((VirtualKey)0xBB); break;
                    case ',': keys.Add((VirtualKey)0xBC); break;
                    case '_': keys.Add(VirtualKey.LeftShift); keys.Add((VirtualKey)0xBD); break;
                    case '?': keys.Add(VirtualKey.LeftShift); keys.Add((VirtualKey)0xBF); break;
                    case '`': keys.Add((VirtualKey)0xC0); break;
                    case '[': keys.Add((VirtualKey)0xDB); break;
                    case '\\': keys.Add((VirtualKey)0xDC); break;
                    case ']': keys.Add((VirtualKey)0xDD); break;
                    case '\'': keys.Add((VirtualKey)0xDE); break;
                    case '<': keys.Add(VirtualKey.LeftShift); keys.Add((VirtualKey)0xBC); break;
                    case '>': keys.Add(VirtualKey.LeftShift); keys.Add((VirtualKey)0xBE); break;
                    case '"': keys.Add(VirtualKey.LeftShift); keys.Add((VirtualKey)0xDE); break;
                    default: break;
                }
        return [.. keys];
    }
    public static string ToSentence(VirtualKey[] keys)
    {
        bool capsLockState = false;
        bool shiftActive;
        StringBuilder sb = new();
        foreach (var key in keys)
        {
            if (key is VirtualKey.LeftShift or VirtualKey.RightShift or VirtualKey.Shift)
            {
                shiftActive = true;
                continue;
            }
            if (key is VirtualKey.CapitalLock)
            {
                capsLockState = !capsLockState;
                continue;
            }
            shiftActive = keys.Any(k => k is VirtualKey.LeftShift or VirtualKey.RightShift or VirtualKey.Shift);
            var c = key.ToChar();
            if (c is null)
                continue;
            char ch = c.Value;
            if (char.IsLetter(ch))
            {
                bool makeUpper = (capsLockState && !shiftActive) || (!capsLockState && shiftActive);
                ch = makeUpper ? char.ToUpper(ch) : char.ToLower(ch);
            }
            else if (shiftActive)
                ch = ApplyShiftToNonLetter(ch);
            sb.Append(ch);
        }
        return sb.ToString();
    }
    public static char? ToChar(this VirtualKey key)
    {
        if (key >= VirtualKey.A && key <= VirtualKey.Z)
            return (char)('a' + (key - VirtualKey.A));
        if (key >= VirtualKey.Number0 && key <= VirtualKey.Number9)
            return (char)('0' + (key - VirtualKey.Number0));
        if (key >= VirtualKey.NumberPad0 && key <= VirtualKey.NumberPad9)
            return (char)('0' + (key - VirtualKey.NumberPad0));
        if (key >= VirtualKey.F1 && key <= VirtualKey.F24)
            return (char)('F' + (key - VirtualKey.F1));
        return key switch
        {
            VirtualKey.Space => ' ',
            VirtualKey.Decimal => '.',
            VirtualKey.Add => '+',
            VirtualKey.Subtract => '-',
            VirtualKey.Multiply => '*',
            VirtualKey.Divide => '/',
            (VirtualKey)0xBA => ';',
            (VirtualKey)0xBB => '=',
            (VirtualKey)0xBC => ',',
            (VirtualKey)0xBD => '-',
            (VirtualKey)0xBE => '.',
            (VirtualKey)0xBF => '/',
            (VirtualKey)0xC0 => '`',
            (VirtualKey)0xDB => '[',
            (VirtualKey)0xDC => '\\',
            (VirtualKey)0xDD => ']',
            (VirtualKey)0xDE => '\'',
            (VirtualKey)0xE2 => '<',
            _ => null
        };
    }
    private static char ApplyShiftToNonLetter(char c) => c switch
    {
        '1' => '!',
        '2' => '@',
        '3' => '#',
        '4' => '$',
        '5' => '%',
        '6' => '^',
        '7' => '&',
        '8' => '*',
        '9' => '(',
        '0' => ')',
        '-' => '_',
        '=' => '+',
        ';' => ':',
        ',' => '<',
        '.' => '>',
        '/' => '?',
        '`' => '~',
        '[' => '{',
        ']' => '}',
        '\\' => '|',
        '\'' => '"',
        _ => c
    };
    public static void SendGameInput(VirtualKey[] keys, bool[] states, string? windowTitle = null)
    {
        var inputs = new INPUT[keys.Length];
        for (int i = 0; i < keys.Length; i++)
            inputs[i] = new INPUT
            {
                type = INPUT_KEYBOARD,
                u = new INPUTUNION
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = 0,
                        wScan = (ushort)MapVirtualKey((uint)keys[i], 0),
                        dwFlags = states[i] ? KEYEVENTF_SCANCODE : (KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP),
                        time = 0,
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            };
        _ = SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());
    }
    public static void SendInput(VirtualKey[] keys, bool[] states, string? windowTitle = null)
    {
        var inputs = new INPUT[keys.Length];
        for (int i = 0; i < keys.Length; i++)
            inputs[i] = new INPUT
            {
                type = INPUT_KEYBOARD,
                u = new INPUTUNION
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = (ushort)keys[i],
                        dwFlags = states[i] ? 0 : KEYEVENTF_KEYUP,
                        time = 0,
                        dwExtraInfo = nint.Zero
                    }
                }
            };
        _ = SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());
    }
    public static void KybdEvent(VirtualKey[] keys, bool[] states, string? windowTitle = null)
    {
        for (int i = 0; i < keys.Length; i++)
            if (states[i])
                keybd_event((byte)keys[i], 0, KEYEVENTF_EXTENDEDKEY, 0);
            else
                keybd_event((byte)keys[i], 0, KEYEVENTF_KEYUP | KEYEVENTF_EXTENDEDKEY, 0);
    }
    public static void PostMessage(VirtualKey[] keys, bool[] states, string windowTitle)
    {
        nint hWnd;
        if (string.IsNullOrWhiteSpace(windowTitle))
            hWnd = GetForegroundWindow();
        else
            hWnd = FindWindow(null, windowTitle);
        if (hWnd == nint.Zero) throw new Exception($"Window{windowTitle} is doesn't exit");
        for (int i = 0; i < keys.Length; i++)
        {
            int msg = states[i] ? WM_KEYDOWN : WM_KEYUP;
            _ = PostMessage(hWnd, (uint)msg, (ushort)keys[i], nint.Zero);
        }
    }
    public static void SendMessage(VirtualKey[] keys, bool[] states, string windowTitle)
    {
        nint hWnd;
        if (string.IsNullOrWhiteSpace(windowTitle))
            hWnd = GetForegroundWindow();
        else
            hWnd = FindWindow(null, windowTitle);
        if (hWnd == nint.Zero) throw new Exception($"Window{windowTitle} is doesn't exit");
        for (int i = 0; i < keys.Length; i++)
        {
            int msg = states[i] ? WM_KEYDOWN : WM_KEYUP;
            _ = SendMessage(hWnd, (uint)msg, (ushort)keys[i], nint.Zero);
        }
    }
    private static void MouseClick(bool isLeftClick, bool isDown)
    {
        INPUT input = new()
        {
            type = INPUT_MOUSE,
            u = new INPUTUNION
            {
                mi = new MOUSEINPUT
                {
                    dwFlags = isLeftClick
                        ? (isDown ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_LEFTUP)
                        : (isDown ? MOUSEEVENTF_RIGHTDOWN : MOUSEEVENTF_RIGHTUP),
                    time = 0,
                    dwExtraInfo = nint.Zero
                }
            }
        };
        _ = SendInput(1, [input], Marshal.SizeOf<INPUT>());
    }
    private static void MouseMiddleClick(bool isDown)
    {
        INPUT input = isDown
            ? new INPUT { type = INPUT_MOUSE, u = new INPUTUNION { mi = new MOUSEINPUT { dwFlags = MOUSEEVENTF_MIDDLEDOWN } } }
            : new INPUT { type = INPUT_MOUSE, u = new INPUTUNION { mi = new MOUSEINPUT { dwFlags = MOUSEEVENTF_MIDDLEUP } } };
        _ = SendInput(1, [input], Marshal.SizeOf<INPUT>());
    }
    private static void MouseXButtonClick(bool isXButton1, bool isDown)
    {
        INPUT input = new()
        {
            type = INPUT_MOUSE,
            u = new INPUTUNION
            {
                mi = new MOUSEINPUT
                {
                    mouseData = isXButton1 ? XBUTTON1 : XBUTTON2,
                    dwFlags = isDown ? MOUSEEVENTF_XDOWN : MOUSEEVENTF_XUP,
                    time = 0,
                    dwExtraInfo = nint.Zero
                }
            }
        };
        _ = SendInput(1, [input], Marshal.SizeOf<INPUT>());
    }
    private static void MouseWheelDown(int scrollAmount)
    {
        INPUT input = new()
        {
            type = INPUT_MOUSE,
            u = new INPUTUNION
            {
                mi = new MOUSEINPUT
                {
                    mouseData = WHEEL_DELTA * scrollAmount,
                    dwFlags = MOUSEEVENTF_WHEEL,
                    time = 0,
                    dwExtraInfo = nint.Zero
                }
            }
        };
        _ = SendInput(1, [input], Marshal.SizeOf<INPUT>());
    }
    private static void MouseMoveTo(SizeInt32 screenSize, int x, int y)
    {
        INPUT input = new()
        {
            type = INPUT_MOUSE,
            u = new INPUTUNION
            {
                mi = new MOUSEINPUT
                {
                    dx = x * 65535 / screenSize.Width,
                    dy = y * 65535 / screenSize.Height,
                    dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE,
                    time = 0,
                    dwExtraInfo = nint.Zero
                }
            }
        };
        _ = SendInput(1, [input], Marshal.SizeOf<INPUT>());
    }
    private static async Task MouseMoveToSmooth(SizeInt32 screenSize, int targetX, int targetY, int durationMs, int pixelsPerStepDelay = 23, int stepTime = 1)
    {
        if (!GetCursorPos(out var startPos))
            return;
        float currentX = startPos.X, currentY = startPos.Y,
              deltaX = targetX - currentX, deltaY = targetY - currentY,
              distance = MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
        int steps = Math.Max(1, Math.Min(durationMs, (int)(distance / Math.Max(1, pixelsPerStepDelay)))),
            absX, absY, lastX = -1, lastY = -1;
        float stepX = deltaX / steps,
              stepY = deltaY / steps;
        for (int i = 0; i < steps; i++)
        {
            currentX += stepX;
            currentY += stepY;
            absX = (int)(currentX * 65535 / screenSize.Width);
            absY = (int)(currentY * 65535 / screenSize.Height);
            if (absX == lastX && absY == lastY)
                continue;
            lastX = absX; lastY = absY;
            INPUT input = new()
            {
                type = INPUT_MOUSE,
                u = new INPUTUNION
                {
                    mi = new MOUSEINPUT
                    {
                        dx = absX,
                        dy = absY,
                        dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE,
                        time = 0,
                        dwExtraInfo = nint.Zero
                    }
                }
            };
            _ = SendInput(1, [input], Marshal.SizeOf<INPUT>());
            await Task.Delay(stepTime);
        }
    }
    private static void MouseMoveRelative(int deltaX, int deltaY)
    {
        INPUT input = new()
        {
            type = INPUT_MOUSE,
            u = new INPUTUNION
            {
                mi = new MOUSEINPUT
                {
                    dx = deltaX,
                    dy = deltaY,
                    dwFlags = MOUSEEVENTF_MOVE,
                    time = 0,
                    dwExtraInfo = nint.Zero
                }
            }
        };
        _ = SendInput(1, [input], Marshal.SizeOf<INPUT>());
    }
    private static async Task MouseMoveRelativeSmooth(int deltaX, int deltaY, int durationMs, int pixelsPerStepDelay = 23, int stepTime = 1)
    {
        float distance = MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
        int steps = Math.Max(1, Math.Min(durationMs / stepTime, (int)(distance / Math.Max(1, pixelsPerStepDelay)))),
            moveX, moveY;
        float stepX = (float)deltaX / steps,
              stepY = (float)deltaY / steps,
              accX = 0, accY = 0;
        for (int i = 0; i < steps; i++)
        {
            accX += stepX;
            accY += stepY;
            moveX = (int)Math.Round(accX);
            moveY = (int)Math.Round(accY);
            accX -= moveX;
            accY -= moveY;
            if (moveX != 0 || moveY != 0)
                MouseMoveRelative(moveX, moveY);
            await Task.Delay(stepTime);
        }
    }
}