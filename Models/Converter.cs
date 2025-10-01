using static ReisProduction.Winjoys.Utilities.Constants;
using static ReisProduction.Winjoys.Services.Interop;
using ReisProduction.Winjoys.Utilities.Enums;
using Windows.System;
using System.Text;
namespace ReisProduction.Winjoys.Models;
public static partial class InputInjector
{
    public static WinRTKey ToWinRTKey(this VirtualKey key) => (WinRTKey)(ushort)MapVirtualKey((uint)key, MAPVK_VK_TO_WK);
    public static VirtualKey ToVirtualKey(this WinRTKey scan) => (VirtualKey)MapVirtualKey((uint)scan, MAPVK_WK_TO_VK);
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
    public static ButtonType ToButton(this InputType input) => input switch
    {
        InputType.LeftButton => ButtonType.LeftButton,
        InputType.RightButton => ButtonType.RightButton,
        InputType.MiddleButton => ButtonType.MiddleButton,
        InputType.XButton1 => ButtonType.XButton1,
        InputType.XButton2 => ButtonType.XButton2,
        _ => ButtonType.None
    };
    public static ScrollType ToScroll(this InputType input) => input switch
    {
        InputType.MouseScrollLeft => ScrollType.MouseScrollLeft,
        InputType.MouseScrollRight => ScrollType.MouseScrollRight,
        InputType.MouseScrollUp => ScrollType.MouseScrollUp,
        InputType.MouseScrollDown => ScrollType.MouseScrollDown,
        _ => ScrollType.None
    };
    public static MoveType ToMove(this InputType input) => input switch
    {
        InputType.MouseNavigateUp => MoveType.MouseNavigateUp,
        InputType.MouseNavigateUpSmooth => MoveType.MouseNavigateUpSmooth,
        InputType.MouseNavigateDown => MoveType.MouseNavigateDown,
        InputType.MouseNavigateDownSmooth => MoveType.MouseNavigateDownSmooth,
        InputType.MouseNavigateLeft => MoveType.MouseNavigateLeft,
        InputType.MouseNavigateLeftSmooth => MoveType.MouseNavigateLeftSmooth,
        InputType.MouseNavigateRight => MoveType.MouseNavigateRight,
        InputType.MouseNavigateRightSmooth => MoveType.MouseNavigateRightSmooth,
        InputType.MouseNavigateToXY => MoveType.MouseNavigateToXY,
        InputType.MouseNavigateToXYSmooth => MoveType.MouseNavigateToXYSmooth,
        _ => MoveType.None
    };
    public static InputType ToInput(this WinRTKey key) => ToInput(key.ToVirtualKey());
    public static InputType ToInput(this VirtualKey key) => key switch
    {
        VirtualKey.LeftButton => InputType.LeftButton,
        VirtualKey.RightButton => InputType.RightButton,
        VirtualKey.Cancel => InputType.Cancel,
        VirtualKey.MiddleButton => InputType.MiddleButton,
        VirtualKey.XButton1 => InputType.XButton1,
        VirtualKey.XButton2 => InputType.XButton2,
        VirtualKey.NavigationAccept => InputType.MouseScrollUp,
        VirtualKey.NavigationCancel => InputType.MouseScrollDown,
        VirtualKey.NavigationUp => InputType.MouseNavigateUp,
        VirtualKey.GamepadDPadUp => InputType.MouseNavigateUpSmooth,
        VirtualKey.NavigationDown => InputType.MouseNavigateDown,
        VirtualKey.GamepadDPadDown => InputType.MouseNavigateDownSmooth,
        VirtualKey.NavigationLeft => InputType.MouseNavigateLeft,
        VirtualKey.GamepadDPadLeft => InputType.MouseNavigateLeftSmooth,
        VirtualKey.NavigationRight => InputType.MouseNavigateRight,
        VirtualKey.GamepadDPadRight => InputType.MouseNavigateRightSmooth,
        VirtualKey.NavigationView => InputType.MouseNavigateToXY,
        VirtualKey.NavigationMenu => InputType.MouseNavigateToXYSmooth,
        VirtualKey.Back => InputType.Back,
        VirtualKey.Tab => InputType.Tab,
        VirtualKey.Escape => InputType.Escape,
        VirtualKey.Clear => InputType.Clear,
        VirtualKey.Enter => InputType.Enter,
        VirtualKey.Shift => InputType.Shift,
        VirtualKey.LeftShift => InputType.LeftShift,
        VirtualKey.RightShift => InputType.RightShift,
        VirtualKey.Control => InputType.Control,
        VirtualKey.LeftControl => InputType.LeftControl,
        VirtualKey.RightControl => InputType.RightControl,
        VirtualKey.Menu => InputType.Menu,
        VirtualKey.LeftMenu => InputType.LeftMenu,
        VirtualKey.RightMenu => InputType.RightMenu,
        VirtualKey.LeftWindows => InputType.LeftWindows,
        VirtualKey.RightWindows => InputType.RightWindows,
        VirtualKey.Pause => InputType.Pause,
        VirtualKey.CapitalLock => InputType.CapitalLock,
        VirtualKey.Kana => InputType.Kana,
        VirtualKey.Hanja => InputType.Hanja,
        VirtualKey.Convert => InputType.Convert,
        VirtualKey.NonConvert => InputType.NonConvert,
        VirtualKey.Accept => InputType.Accept,
        VirtualKey.ModeChange => InputType.ModeChange,
        VirtualKey.Space => InputType.Space,
        VirtualKey.PageUp => InputType.PageUp,
        VirtualKey.PageDown => InputType.PageDown,
        VirtualKey.End => InputType.End,
        VirtualKey.Home => InputType.Home,
        VirtualKey.Left => InputType.Left,
        VirtualKey.Up => InputType.Up,
        VirtualKey.Right => InputType.Right,
        VirtualKey.Down => InputType.Down,
        VirtualKey.Select => InputType.Select,
        VirtualKey.Print => InputType.PrintScreen,
        VirtualKey.Execute => InputType.Execute,
        VirtualKey.Snapshot => InputType.Snapshot,
        VirtualKey.Insert => InputType.Insert,
        VirtualKey.Delete => InputType.Delete,
        VirtualKey.Help => InputType.Help,
        VirtualKey.Number0 => InputType.Number0,
        VirtualKey.Number1 => InputType.Number1,
        VirtualKey.Number2 => InputType.Number2,
        VirtualKey.Number3 => InputType.Number3,
        VirtualKey.Number4 => InputType.Number4,
        VirtualKey.Number5 => InputType.Number5,
        VirtualKey.Number6 => InputType.Number6,
        VirtualKey.Number7 => InputType.Number7,
        VirtualKey.Number8 => InputType.Number8,
        VirtualKey.Number9 => InputType.Number9,
        VirtualKey.A => InputType.A,
        VirtualKey.B => InputType.B,
        VirtualKey.C => InputType.C,
        VirtualKey.D => InputType.D,
        VirtualKey.E => InputType.E,
        VirtualKey.F => InputType.F,
        VirtualKey.G => InputType.G,
        VirtualKey.H => InputType.H,
        VirtualKey.I => InputType.I,
        VirtualKey.J => InputType.J,
        VirtualKey.K => InputType.K,
        VirtualKey.L => InputType.L,
        VirtualKey.M => InputType.M,
        VirtualKey.N => InputType.N,
        VirtualKey.O => InputType.O,
        VirtualKey.P => InputType.P,
        VirtualKey.Q => InputType.Q,
        VirtualKey.R => InputType.R,
        VirtualKey.S => InputType.S,
        VirtualKey.T => InputType.T,
        VirtualKey.U => InputType.U,
        VirtualKey.V => InputType.V,
        VirtualKey.W => InputType.W,
        VirtualKey.X => InputType.X,
        VirtualKey.Y => InputType.Y,
        VirtualKey.Z => InputType.Z,
        VirtualKey.Application => InputType.Application,
        VirtualKey.Sleep => InputType.Sleep,
        VirtualKey.NumberPad0 => InputType.NumberPad0,
        VirtualKey.NumberPad1 => InputType.NumberPad1,
        VirtualKey.NumberPad2 => InputType.NumberPad2,
        VirtualKey.NumberPad3 => InputType.NumberPad3,
        VirtualKey.NumberPad4 => InputType.NumberPad4,
        VirtualKey.NumberPad5 => InputType.NumberPad5,
        VirtualKey.NumberPad6 => InputType.NumberPad6,
        VirtualKey.NumberPad7 => InputType.NumberPad7,
        VirtualKey.NumberPad8 => InputType.NumberPad8,
        VirtualKey.NumberPad9 => InputType.NumberPad9,
        VirtualKey.NumberKeyLock => InputType.NumberKeyLock,
        VirtualKey.Multiply => InputType.Multiply,
        VirtualKey.Add => InputType.Add,
        VirtualKey.Separator => InputType.Separator,
        VirtualKey.Subtract => InputType.Subtract,
        VirtualKey.Decimal => InputType.Decimal,
        VirtualKey.Divide => InputType.Divide,
        VirtualKey.F1 => InputType.F1,
        VirtualKey.F2 => InputType.F2,
        VirtualKey.F3 => InputType.F3,
        VirtualKey.F4 => InputType.F4,
        VirtualKey.F5 => InputType.F5,
        VirtualKey.F6 => InputType.F6,
        VirtualKey.F7 => InputType.F7,
        VirtualKey.F8 => InputType.F8,
        VirtualKey.F9 => InputType.F9,
        VirtualKey.F10 => InputType.F10,
        VirtualKey.F11 => InputType.F11,
        VirtualKey.F12 => InputType.F12,
        VirtualKey.F13 => InputType.F13,
        VirtualKey.F14 => InputType.F14,
        VirtualKey.F15 => InputType.F15,
        VirtualKey.F16 => InputType.F16,
        VirtualKey.F17 => InputType.F17,
        VirtualKey.F18 => InputType.F18,
        VirtualKey.F19 => InputType.F19,
        VirtualKey.F20 => InputType.F20,
        VirtualKey.F21 => InputType.F21,
        VirtualKey.F22 => InputType.F22,
        VirtualKey.F23 => InputType.F23,
        VirtualKey.F24 => InputType.F24,
        VirtualKey.Scroll => InputType.Scroll,
        (VirtualKey)0xBA => InputType.Oem1,
        (VirtualKey)0xBB => InputType.OemPlus,
        (VirtualKey)0xBC => InputType.OemComma,
        (VirtualKey)0xBD => InputType.OemMinus,
        (VirtualKey)0xBE => InputType.OemPeriod,
        (VirtualKey)0xBF => InputType.Oem2,
        (VirtualKey)0xC0 => InputType.Oem3,
        (VirtualKey)0xDB => InputType.Oem4,
        (VirtualKey)0xDC => InputType.Oem5,
        (VirtualKey)0xDD => InputType.Oem6,
        (VirtualKey)0xDE => InputType.Oem7,
        (VirtualKey)0xDF => InputType.Oem8,
        (VirtualKey)0xE2 => InputType.Oem102,
        (VirtualKey)0xE5 => InputType.ProcessKey,
        (VirtualKey)0xE7 => InputType.Packet,
        (VirtualKey)0xF6 => InputType.Attn,
        (VirtualKey)0xF7 => InputType.CrSel,
        (VirtualKey)0xF8 => InputType.ExSel,
        (VirtualKey)0xF9 => InputType.EraseEof,
        (VirtualKey)0xFA => InputType.Play,
        (VirtualKey)0xFB => InputType.Zoom,
        (VirtualKey)0xFC => InputType.NoName,
        (VirtualKey)0xFD => InputType.Pa1,
        (VirtualKey)0xFE => InputType.OemClear,
        _ => InputType.None
    };
    public static InputType ToInput(this ButtonType button) => button switch
    {
        ButtonType.LeftButton => InputType.LeftButton,
        ButtonType.RightButton => InputType.RightButton,
        ButtonType.MiddleButton => InputType.MiddleButton,
        ButtonType.XButton1 => InputType.XButton1,
        ButtonType.XButton2 => InputType.XButton2,
        _ => InputType.None
    };
    public static InputType ToInput(this ScrollType scroll) => scroll switch
    {
        ScrollType.MouseScrollLeft => InputType.MouseScrollLeft,
        ScrollType.MouseScrollRight => InputType.MouseScrollRight,
        ScrollType.MouseScrollUp => InputType.MouseScrollUp,
        ScrollType.MouseScrollDown => InputType.MouseScrollDown,
        _ => InputType.None
    };
    public static InputType ToInput(this MoveType move) => move switch
    {
        MoveType.MouseNavigateUp => InputType.MouseNavigateUp,
        MoveType.MouseNavigateUpSmooth => InputType.MouseNavigateUpSmooth,
        MoveType.MouseNavigateDown => InputType.MouseNavigateDown,
        MoveType.MouseNavigateDownSmooth => InputType.MouseNavigateDownSmooth,
        MoveType.MouseNavigateLeft => InputType.MouseNavigateLeft,
        MoveType.MouseNavigateLeftSmooth => InputType.MouseNavigateLeftSmooth,
        MoveType.MouseNavigateRight => InputType.MouseNavigateRight,
        MoveType.MouseNavigateRightSmooth => InputType.MouseNavigateRightSmooth,
        MoveType.MouseNavigateToXY => InputType.MouseNavigateToXY,
        MoveType.MouseNavigateToXYSmooth => InputType.MouseNavigateToXYSmooth,
        _ => InputType.None
    };
    public static string ToKeyString(this WinRTKey key) => ToKeyString(key.ToVirtualKey());
    public static string ToKeyString(this VirtualKey key) => key switch
    {
        VirtualKey.Back => "{BACKSPACE}",
        VirtualKey.Tab => "{TAB}",
        VirtualKey.Clear => "{CLEAR}",
        VirtualKey.Enter => "{ENTER}",
        VirtualKey.Pause => "{PAUSE}",
        VirtualKey.Escape => "{ESC}",
        VirtualKey.Space => " ",
        VirtualKey.PageUp => "{PGUP}",
        VirtualKey.PageDown => "{PGDN}",
        VirtualKey.End => "{END}",
        VirtualKey.Home => "{HOME}",
        VirtualKey.Left => "{LEFT}",
        VirtualKey.Up => "{UP}",
        VirtualKey.Right => "{RIGHT}",
        VirtualKey.Down => "{DOWN}",
        VirtualKey.Select => "{SELECT}",
        VirtualKey.Print => "{PRINTSCREEN}",
        VirtualKey.Execute => "{EXECUTE}",
        VirtualKey.Snapshot => "{SNAPSHOT}",
        VirtualKey.Insert => "{INSERT}",
        VirtualKey.Delete => "{DELETE}",
        VirtualKey.Help => "{HELP}",
        VirtualKey.F1 => "{F1}",
        VirtualKey.F2 => "{F2}",
        VirtualKey.F3 => "{F3}",
        VirtualKey.F4 => "{F4}",
        VirtualKey.F5 => "{F5}",
        VirtualKey.F6 => "{F6}",
        VirtualKey.F7 => "{F7}",
        VirtualKey.F8 => "{F8}",
        VirtualKey.F9 => "{F9}",
        VirtualKey.F10 => "{F10}",
        VirtualKey.F11 => "{F11}",
        VirtualKey.F12 => "{F12}",
        VirtualKey.Number0 => "0",
        VirtualKey.Number1 => "1",
        VirtualKey.Number2 => "2",
        VirtualKey.Number3 => "3",
        VirtualKey.Number4 => "4",
        VirtualKey.Number5 => "5",
        VirtualKey.Number6 => "6",
        VirtualKey.Number7 => "7",
        VirtualKey.Number8 => "8",
        VirtualKey.Number9 => "9",
        VirtualKey.A => "A",
        VirtualKey.B => "B",
        VirtualKey.C => "C",
        VirtualKey.D => "D",
        VirtualKey.E => "E",
        VirtualKey.F => "F",
        VirtualKey.G => "G",
        VirtualKey.H => "H",
        VirtualKey.I => "I",
        VirtualKey.J => "J",
        VirtualKey.K => "K",
        VirtualKey.L => "L",
        VirtualKey.M => "M",
        VirtualKey.N => "N",
        VirtualKey.O => "O",
        VirtualKey.P => "P",
        VirtualKey.Q => "Q",
        VirtualKey.R => "R",
        VirtualKey.S => "S",
        VirtualKey.T => "T",
        VirtualKey.U => "U",
        VirtualKey.V => "V",
        VirtualKey.W => "W",
        VirtualKey.X => "X",
        VirtualKey.Y => "Y",
        VirtualKey.Z => "Z",
        VirtualKey.Add => "{ADD}",
        VirtualKey.Subtract => "{SUBTRACT}",
        VirtualKey.Multiply => "{MULTIPLY}",
        VirtualKey.Divide => "{DIVIDE}",
        VirtualKey.Decimal => "{DECIMAL}",
        VirtualKey.NumberPad0 => "{NUMPAD0}",
        VirtualKey.NumberPad1 => "{NUMPAD1}",
        VirtualKey.NumberPad2 => "{NUMPAD2}",
        VirtualKey.NumberPad3 => "{NUMPAD3}",
        VirtualKey.NumberPad4 => "{NUMPAD4}",
        VirtualKey.NumberPad5 => "{NUMPAD5}",
        VirtualKey.NumberPad6 => "{NUMPAD6}",
        VirtualKey.NumberPad7 => "{NUMPAD7}",
        VirtualKey.NumberPad8 => "{NUMPAD8}",
        VirtualKey.NumberPad9 => "{NUMPAD9}",
        VirtualKey.LeftWindows or VirtualKey.RightWindows => "^{ESC}", // örnek
        VirtualKey.LeftShift or VirtualKey.RightShift or VirtualKey.Shift => "+",
        VirtualKey.LeftControl or VirtualKey.RightControl or VirtualKey.Control => "^",
        VirtualKey.LeftMenu or VirtualKey.RightMenu or VirtualKey.Menu => "%",
        _ => ""
    };
    public static WinRTKey[] ToWinRTKeys(string sentence) => [.. ToVirtualKeys(sentence).Select(k => k.ToWinRTKey())];
    public static VirtualKey[] ToVirtualKeys(string sentence)
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
    public static string ToSentence(this WinRTKey[] keys) => ToSentence(keys.Select(k => k.ToVirtualKey()).ToArray());
    public static string ToSentence(this VirtualKey[] keys)
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
    public static char? ToChar(this WinRTKey key) => ToChar(key.ToVirtualKey());
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
}