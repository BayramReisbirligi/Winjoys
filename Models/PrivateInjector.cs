using static ReisProduction.Winjoys.Utilities.Constants;
using static ReisProduction.Winjoys.Services.Interop;
using ReisProduction.Winjoys.Utilities.Structs;
using Windows.UI.Input.Preview.Injection;
using ReisProduction.Winjoys.Utilities;
using System.Runtime.InteropServices;
using Windows.Graphics;
using Windows.System;
namespace ReisProduction.Winjoys.Models;
public static partial class InputInjector
{
    public static Action<KybdAction<VirtualKey>> ActionHandler { get; set; } = SendGameInput;
    public static SizeInt32 ScreenSize { get; set; } = new(1920, 1080);
    public static bool BringToFrontWindow { get; set; } = false;
    public static uint TimeOffsetInMilliseconds { get; set; } = 0;
    public static ulong PerformanceCount { get; set; } = 1;
    public static int PixelsPerStepDelay { get; set; } = 23;
    public static int PerStepTimeDelay { get; set; } = 1;
    public static int SmoothDurationMs { get; set; } = 9;
    public static int Pressure { get; set; } = 32000;
    public static int Orientation { get; set; } = 90;
    public static uint TouchPointerId { get; set; } = 0;
    public static uint PenPointerId { get; set; } = 0;
    public static InjectedInputTouchParameters TouchParameters =>
        InjectedInputTouchParameters.Contact |
        InjectedInputTouchParameters.Orientation |
        InjectedInputTouchParameters.Pressure;
    public static InjectedInputRectangle Contact =>
    new()
    {
        Top = -2,
        Bottom = 2,
        Left = -2,
        Right = 2
    };
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
        _ = Services.Interop.SendInput(1, [input], Marshal.SizeOf<INPUT>());
    }
    private static void MouseMiddleClick(bool isDown)
    {
        INPUT input = isDown
            ? new INPUT { type = INPUT_MOUSE, u = new INPUTUNION { mi = new MOUSEINPUT { dwFlags = MOUSEEVENTF_MIDDLEDOWN } } }
            : new INPUT { type = INPUT_MOUSE, u = new INPUTUNION { mi = new MOUSEINPUT { dwFlags = MOUSEEVENTF_MIDDLEUP } } };
        _ = Services.Interop.SendInput(1, [input], Marshal.SizeOf<INPUT>());
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
        _ = Services.Interop.SendInput(1, [input], Marshal.SizeOf<INPUT>());
    }
    private static void MouseWheelRight(int scrollAmount)
    {
        INPUT input = new()
        {
            type = INPUT_MOUSE,
            u = new INPUTUNION
            {
                mi = new MOUSEINPUT
                {
                    mouseData = WHEEL_DELTA * scrollAmount,
                    dwFlags = MOUSEEVENTF_HWHEEL,
                    time = 0,
                    dwExtraInfo = nint.Zero
                }
            }
        };
        _ = Services.Interop.SendInput(1, [input], Marshal.SizeOf<INPUT>());
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
        _ = Services.Interop.SendInput(1, [input], Marshal.SizeOf<INPUT>());
    }
    private static void MouseMoveTo(int x, int y)
    {
        INPUT input = new()
        {
            type = INPUT_MOUSE,
            u = new INPUTUNION
            {
                mi = new MOUSEINPUT
                {
                    dx = x * 65535 / ScreenSize.Width,
                    dy = y * 65535 / ScreenSize.Height,
                    dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE,
                    time = 0,
                    dwExtraInfo = nint.Zero
                }
            }
        };
        _ = Services.Interop.SendInput(1, [input], Marshal.SizeOf<INPUT>());
    }
    private static async Task MouseMoveToSmooth(int x, int y)
    {
        var startPos = GetCursorPos();
        float currentX = startPos.X, currentY = startPos.Y,
              deltaX = x - currentX, deltaY = y - currentY,
              distance = MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
        int steps = Math.Max(1, Math.Min(SmoothDurationMs / PerStepTimeDelay, (int)(distance / Math.Max(1, PixelsPerStepDelay)))),
            absX, absY, lastX = -1, lastY = -1;
        float stepX = deltaX / steps,
              stepY = deltaY / steps;
        for (int i = 0; i < steps; i++)
        {
            currentX += stepX;
            currentY += stepY;
            absX = (int)(currentX * 65535 / ScreenSize.Width);
            absY = (int)(currentY * 65535 / ScreenSize.Height);
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
            _ = Services.Interop.SendInput(1, [input], Marshal.SizeOf<INPUT>());
            await Task.Delay(PerStepTimeDelay);
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
        _ = Services.Interop.SendInput(1, [input], Marshal.SizeOf<INPUT>());
    }
    private static async Task MouseMoveRelativeSmooth(int deltaX, int deltaY)
    {
        float distance = MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
        int steps = Math.Max(1, Math.Min(SmoothDurationMs / PerStepTimeDelay, (int)(distance / Math.Max(1, PixelsPerStepDelay)))),
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
            await Task.Delay(PerStepTimeDelay);
        }
    }}