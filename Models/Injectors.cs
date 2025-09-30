using static ReisProduction.Winjoys.Utilities.Constants;
using static ReisProduction.Winjoys.Services.Interop;
using ReisProduction.Winjoys.Utilities.Structs;
using System.Runtime.InteropServices;
using ReisProduction.Winjoys.Utilities;
namespace ReisProduction.Winjoys.Models;
public static partial class InputInjector
{
    // Winjoys'a using Windows.UI.Input.Preview.Injection; eklenecek
    public static void SendGameInput(KybdAction kybdAction)
    {
        nint hWnd = FindWindow(kybdAction.WindowhWnd, kybdAction.WindowTitle);
        if (hWnd is not 0)
            BringToFrontNameOrHwnd(hWnd);
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
                        dwFlags = kybdAction.States![i] ? KEYEVENTF_SCANCODE : (KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP),
                        time = 0,
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            };
        _ = Services.Interop.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());
    }
    public static void SendInput(KybdAction kybdAction)
    {
        nint hWnd = FindWindow(kybdAction.WindowhWnd, kybdAction.WindowTitle);
        if (hWnd is not 0)
            BringToFrontNameOrHwnd(hWnd);
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
                        dwFlags = kybdAction.States![i] ? 0 : KEYEVENTF_KEYUP,
                        time = 0,
                        dwExtraInfo = nint.Zero
                    }
                }
            };
        _ = Services.Interop.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());
    }
    public static void KybdEvent(KybdAction kybdAction)
    {
        nint hWnd = FindWindow(kybdAction.WindowhWnd, kybdAction.WindowTitle);
        if (hWnd is not 0)
            BringToFrontNameOrHwnd(hWnd);
        for (int i = 0; i < kybdAction.Keys.Length; i++)
            if (kybdAction.States![i])
                keybd_event((byte)kybdAction.Keys[i], 0, KEYEVENTF_EXTENDEDKEY, 0);
            else
                keybd_event((byte)kybdAction.Keys[i], 0, KEYEVENTF_KEYUP | KEYEVENTF_EXTENDEDKEY, 0);
    }
    public static void PostMessage(KybdAction kybdAction)
    {
        nint windowhWnd = FindWindow(kybdAction.WindowhWnd, kybdAction.WindowTitle, true);
        for (int i = 0; i < kybdAction.Keys.Length; i++)
        {
            int msg = kybdAction.States![i] ? WM_KEYDOWN : WM_KEYUP;
            _ = Services.Interop.PostMessage(windowhWnd, (uint)msg, (ushort)kybdAction.Keys[i], nint.Zero);
        }
    }
    public static void SendMessage(KybdAction kybdAction)
    {
        nint windowhWnd = FindWindow(kybdAction.WindowhWnd, kybdAction.WindowTitle, true);
        for (int i = 0; i < kybdAction.Keys.Length; i++)
        {
            int msg = kybdAction.States![i] ? WM_KEYDOWN : WM_KEYUP;
            _ = Services.Interop.SendMessage(windowhWnd, (uint)msg, (ushort)kybdAction.Keys[i], nint.Zero);
        }
    }
    public static void SendKeys(KybdAction kybdAction) => SendKeys(string.Concat(kybdAction.Keys.Select(k => k.ToKeyString())), kybdAction.WindowhWnd, kybdAction.WindowTitle);
    public static void SendKeys(string keys, nint windowhWnd = 0, string? windowTitle = null)
    {
        nint hWnd = FindWindow(windowhWnd, windowTitle);
        if (hWnd is not 0)
            BringToFrontNameOrHwnd(hWnd);
        System.Windows.Forms.SendKeys.Send(keys);
    }
    public static void SendWait(KybdAction kybdAction) => SendWait(string.Concat(kybdAction.Keys.Select(k => k.ToKeyString())), kybdAction.WindowhWnd, kybdAction.WindowTitle);
    public static void SendWait(string keys, nint windowhWnd = 0, string? windowTitle = null)
    {
        nint hWnd = FindWindow(windowhWnd, windowTitle);
        if (hWnd is not 0)
            BringToFrontNameOrHwnd(hWnd);
        System.Windows.Forms.SendKeys.SendWait(keys);
    }
}