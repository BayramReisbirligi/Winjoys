using Injector = Windows.UI.Input.Preview.Injection.InputInjector;
using static ReisProduction.Winjoys.Utilities.Constants;
using static ReisProduction.Winjoys.Services.Interop;
using ReisProduction.Winjoys.Utilities.Structs;
using ReisProduction.Winjoys.Utilities.Enums;
using Windows.UI.Input.Preview.Injection;
using ReisProduction.Winjoys.Utilities;
using System.Runtime.InteropServices;
using Windows.System;
namespace ReisProduction.Winjoys.Models;
public static partial class InputInjector
{
    public static void SendWinRTKeys(KybdAction<WinRTKey> kybdAction)
    {
        BringToFrontNameOrHwnd(kybdAction.WindowhWnd, kybdAction.WindowTitle);
        var injector = Injector.TryCreate();
        List<InjectedInputKeyboardInfo> inputList = [];
        for (int i = 0; i < kybdAction.Keys.Length; i++)
            inputList.Add(new()
            {
                VirtualKey = (ushort)kybdAction.Keys[i],
                KeyOptions = kybdAction.States[i]
                    ? InjectedInputKeyOptions.None
                    : InjectedInputKeyOptions.KeyUp
            });
        injector.InjectKeyboardInput(inputList);
    }
    public static void SendGameInput(KybdAction<VirtualKey> kybdAction)
    {
        BringToFrontNameOrHwnd(kybdAction.WindowhWnd, kybdAction.WindowTitle);
        var inputs = new INPUT[kybdAction.Keys.Length];
        for (int i = 0; i < kybdAction.Keys.Length; i++)
            inputs[i] = new INPUT
            {
                type = INPUT_KEYBOARD,
                u = new INPUTUNION
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = 0,
                        wScan = (ushort)MapVirtualKey((uint)kybdAction.Keys[i], 0),
                        dwFlags = kybdAction.States[i] ? KEYEVENTF_SCANCODE : (KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP),
                        time = 0,
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            };
        _ = Services.Interop.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());
    }
    public static void SendInput(KybdAction<VirtualKey> kybdAction)
    {
        BringToFrontNameOrHwnd(kybdAction.WindowhWnd, kybdAction.WindowTitle);
        var inputs = new INPUT[kybdAction.Keys.Length];
        for (int i = 0; i < kybdAction.Keys.Length; i++)
            inputs[i] = new INPUT
            {
                type = INPUT_KEYBOARD,
                u = new INPUTUNION
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = (ushort)kybdAction.Keys[i],
                        dwFlags = kybdAction.States[i] ? 0 : KEYEVENTF_KEYUP,
                        time = 0,
                        dwExtraInfo = nint.Zero
                    }
                }
            };
        _ = Services.Interop.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());
    }
    public static void KybdEvent(KybdAction<VirtualKey> kybdAction)
    {
        BringToFrontNameOrHwnd(kybdAction.WindowhWnd, kybdAction.WindowTitle);
        for (int i = 0; i < kybdAction.Keys.Length; i++)
            if (kybdAction.States[i])
                keybd_event((byte)kybdAction.Keys[i], 0, KEYEVENTF_EXTENDEDKEY, 0);
            else
                keybd_event((byte)kybdAction.Keys[i], 0, KEYEVENTF_KEYUP | KEYEVENTF_EXTENDEDKEY, 0);
    }
    public static void PostMessage(KybdAction<VirtualKey> kybdAction)
    {
        nint windowhWnd = FindWindow(kybdAction.WindowhWnd, kybdAction.WindowTitle, true);
        BringToFrontNameOrHwnd(windowhWnd, kybdAction.WindowTitle);
        for (int i = 0; i < kybdAction.Keys.Length; i++)
        {
            int msg = kybdAction.States[i] ? WM_KEYDOWN : WM_KEYUP;
            _ = Services.Interop.PostMessage(windowhWnd, (uint)msg, (ushort)kybdAction.Keys[i], nint.Zero);
        }
    }
    public static void SendMessage(KybdAction<VirtualKey> kybdAction)
    {
        nint windowhWnd = FindWindow(kybdAction.WindowhWnd, kybdAction.WindowTitle, true);
        BringToFrontNameOrHwnd(windowhWnd, kybdAction.WindowTitle);
        for (int i = 0; i < kybdAction.Keys.Length; i++)
        {
            int msg = kybdAction.States[i] ? WM_KEYDOWN : WM_KEYUP;
            _ = Services.Interop.SendMessage(windowhWnd, (uint)msg, (ushort)kybdAction.Keys[i], nint.Zero);
        }
    }
    public static void SendKeys(KybdAction<VirtualKey> kybdAction) => SendKeys(string.Concat(kybdAction.Keys.Select(k => k.ToKeyString())), kybdAction.WindowhWnd, kybdAction.WindowTitle);
    public static void SendKeys(string keys, nint windowhWnd = 0, string windowTitle = "")
    {
        BringToFrontNameOrHwnd(windowhWnd, windowTitle);
        System.Windows.Forms.SendKeys.Send(keys);
    }
    public static void SendWait(KybdAction<VirtualKey> kybdAction) => SendWait(string.Concat(kybdAction.Keys.Select(k => k.ToKeyString())), kybdAction.WindowhWnd, kybdAction.WindowTitle);
    public static void SendWait(string keys, nint windowhWnd = 0, string windowTitle = "")
    {
        BringToFrontNameOrHwnd(windowhWnd, windowTitle);
        System.Windows.Forms.SendKeys.SendWait(keys);
    }
    public static void SendGamepadInput(GamepadAction gamepadAction)
    {
        BringToFrontNameOrHwnd(gamepadAction.WindowhWnd, gamepadAction.WindowTitle);
        var injector = Injector.TryCreate();
        InjectedInputGamepadInfo input = new()
        {
            Buttons = gamepadAction.Buttons,
            LeftTrigger = gamepadAction.LeftTrigger,
            RightTrigger = gamepadAction.RightTrigger,
            LeftThumbstickX = gamepadAction.LeftThumbstickX,
            LeftThumbstickY = gamepadAction.LeftThumbstickY,
            RightThumbstickX = gamepadAction.RightThumbstickX,
            RightThumbstickY = gamepadAction.RightThumbstickY
        };
        injector.InjectGamepadInput(input);
    }
    public static void SendTouchInput(TouchAction touchAction)
    {
        var injector = Injector.TryCreate();
        injector.InjectTouchInput([new()
        {
            PointerInfo = new()
            {
                PointerId = TouchPointerId,
                PixelLocation = touchAction.Point,
                PointerOptions = touchAction.Options,
                PerformanceCount = PerformanceCount,
                TimeOffsetInMilliseconds = TimeOffsetInMilliseconds
            },
            TouchParameters = TouchParameters,
            Orientation = Orientation,
            Pressure = Pressure,
            Contact = Contact
        }]);
    }
    public static void SendPenInput(PenAction penAction)
    {
        var injector = Injector.TryCreate();
        injector.InjectPenInput(new()
        {
            PointerInfo = new()
            {
                PointerId = PenPointerId,
                PixelLocation = penAction.Point,
                PointerOptions = penAction.Options,
                PerformanceCount = PerformanceCount,
                TimeOffsetInMilliseconds = TimeOffsetInMilliseconds
            },
            Pressure = penAction.Pressure,
            TiltX = penAction.TiltX,
            TiltY = penAction.TiltY,
            Rotation = penAction.Rotation,
            PenButtons = penAction.PenButtons,
            PenParameters = penAction.PenParameters
        });
    }
}