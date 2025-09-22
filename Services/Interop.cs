using ReisProduction.Winjoys.Utilities.Structs;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static ReisProduction.Winjoys.Utilities.Constants;
namespace ReisProduction.Winjoys.Services;
public static class Interop
{
    public static void BringToFrontNameOrHwnd(nint hWnd = 0, string? windowTitle = null)
    {
        if (hWnd is 0)
            if (string.IsNullOrWhiteSpace(windowTitle))
                throw new ArgumentException("Window title cannot be null or empty when hWnd is 0.");
            else
                hWnd = FindWindow(0, windowTitle);
        ShowWindow(hWnd, SW_SHOW);
        SetForegroundWindow(hWnd);
    }
    public static POINT GetCursorPos() => GetCursorPos(out POINT point) ? point :
        throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to get cursor position");
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool GetCursorPos(out POINT lpPoint);
    public static nint FindWindow(nint windowhWnd = 0, string? windowTitle = null, bool GetForeground = false)
    {
        if (windowhWnd is 0)
            if (!string.IsNullOrWhiteSpace(windowTitle))
                windowhWnd = FindWindow(null, windowTitle);
            else if (GetForeground)
                windowhWnd = GetForegroundWindow();
        if (windowhWnd is 0 && GetForeground) throw new Exception($"Window= \"{windowTitle}\" or hWnd= \"{windowhWnd}\" is doesn't exit");
        return windowhWnd;
    }
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool PostMessage(nint hWnd, uint Msg, nint wParam, nint lParam);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    internal static extern nint SendMessage(nint hWnd, uint Msg, nint wParam, nint lParam);
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern uint MapVirtualKey(uint uCode, uint uMapType);
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern nint GetMessageExtraInfo();
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern nint GetForegroundWindow();
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool SetForegroundWindow(nint hWnd);
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool ShowWindow(nint hWnd, int nCmdShow);
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern nint FindWindow(string? lpClassName, string lpWindowName);
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool CloseHandle(nint hObject);
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern nint CreateWaitableTimer(nint lpTimerAttributes, bool bManualReset, string? lpTimerName);
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool SetWaitableTimer(nint hTimer, ref long pDueTime, int lPeriod, nint pfnCompletionRoutine, nint lpArgToCompletionRoutine, bool fResume);
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern uint WaitForSingleObject(nint hHandle, uint dwMilliseconds);
    [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern uint timeBeginPeriod(uint uMilliseconds);
    [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern uint timeEndPeriod(uint uMilliseconds);
}