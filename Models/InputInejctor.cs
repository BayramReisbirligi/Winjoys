using static ReisProduction.Windelay.Models.DelayExecutor;
using ReisProduction.Winjoys.Utilities.Enums;
using ReisProduction.Windelay.Utilities;
using ReisProduction.Winjoys.Utilities;
using Windows.System;
namespace ReisProduction.Winjoys.Models;
public static partial class InputInjector
{
    public static async Task HandleInputs(params IInputAction[] actions)
    {
        foreach (var action in actions)
            switch (action)
            {
                case IKybdAction<VirtualKey> kybd:
                    await HandleKeys((KybdAction<VirtualKey>)(object)kybd);
                    break;
                case IKybdAction<WinRTKey> kybd:
                    await HandleKeys((KybdAction<WinRTKey>)(object)kybd);
                    break;
                case ButtonAction btn:
                    await HandleButtons(btn);
                    break;
                case ScrollAction scr:
                    await HandleScrolls(scr);
                    break;
                case MoveAction move:
                    await HandleMoves(move);
                    break;
#if WINUI || WINDOWS_APP || WINRT
                case MouseAction mouse:
                    await HandleMouse(mouse);
                    break;
                case GamepadAction gp:
                    await HandleGamepad(gp);
                    break;
                case TouchAction touch:
                    await HandleTouch(touch);
                    break;
                case PenAction pen:
                    await HandlePen(pen);
                    break;
#endif
                default:
                    throw new NotSupportedException("The provided action type is not supported.");
            }
    }
    public static async Task HandleKeys<T>(KybdAction<T> kybdAction)
    {
        if (typeof(T) == typeof(VirtualKey))
            ActionHandler((KybdAction<VirtualKey>)(object)kybdAction);
#if WINUI || WINDOWS_APP || WINRT
        else if (typeof(T) == typeof(WinRTKey))
            SendWinRTKeys((KybdAction<WinRTKey>)(object)kybdAction);
#endif
        else
            throw new NotSupportedException("The provided key type is not supported.");
        await Task.CompletedTask;
    }
    public static async Task HandleButtons(ButtonAction buttonAction)
    {
        for (int i = 0; i < buttonAction.Buttons.Length; i++)
            switch (buttonAction.Buttons[i])
            {
                case ButtonType.LeftButton:
                    MouseClick(true, buttonAction.States[i]);
                    break;
                case ButtonType.RightButton:
                    MouseClick(false, buttonAction.States[i]);
                    break;
                case ButtonType.MiddleButton:
                    MouseMiddleClick(buttonAction.States[i]);
                    break;
                case ButtonType.XButton1:
                    MouseXButtonClick(true, buttonAction.States[i]);
                    break;
                case ButtonType.XButton2:
                    MouseXButtonClick(false, buttonAction.States[i]);
                    break;
            }
        await Task.CompletedTask;
    }
    public static async Task HandleScrolls(ScrollAction scrollAction)
    {
        for (int i = 0; i < scrollAction.ScrollTypes.Length; i++)
            switch (scrollAction.ScrollTypes[i])
            {
                case ScrollType.MouseScrollLeft:
                    MouseWheelRight(-scrollAction.ScrollAmount[i]);
                    break;
                case ScrollType.MouseScrollRight:
                    MouseWheelRight(scrollAction.ScrollAmount[i]);
                    break;
                case ScrollType.MouseScrollUp:
                    MouseWheelDown(-scrollAction.ScrollAmount[i]);
                    break;
                case ScrollType.MouseScrollDown:
                    MouseWheelDown(scrollAction.ScrollAmount[i]);
                    break;
            }
        await Task.CompletedTask;
    }
    public static async Task HandleMoves(MoveAction moveAction)
    {
        for (int i = 0; i < moveAction.Moves.Length; i++)
            switch (moveAction.Moves[i])
            {
                case MoveType.MouseNavigateUp:
                    MouseMoveRelative(0, moveAction.CursorPoints[i].X);
                    break;
                case MoveType.MouseNavigateUpSmooth:
                    await MouseMoveRelativeSmooth(0, -moveAction.CursorPoints[i].Y);
                    break;
                case MoveType.MouseNavigateDown:
                    MouseMoveRelative(0, moveAction.CursorPoints[i].Y);
                    break;
                case MoveType.MouseNavigateDownSmooth:
                    await MouseMoveRelativeSmooth(0, moveAction.CursorPoints[i].Y);
                    break;
                case MoveType.MouseNavigateLeft:
                    MouseMoveRelative(-moveAction.CursorPoints[i].X, 0);
                    break;
                case MoveType.MouseNavigateLeftSmooth:
                    await MouseMoveRelativeSmooth(-moveAction.CursorPoints[i].X, 0);
                    break;
                case MoveType.MouseNavigateRight:
                    MouseMoveRelative(moveAction.CursorPoints[i].X, 0);
                    break;
                case MoveType.MouseNavigateRightSmooth:
                    await MouseMoveRelativeSmooth(moveAction.CursorPoints[i].X, 0);
                    break;
                case MoveType.MouseNavigateToXY:
                    MouseMoveTo(moveAction.CursorPoints[i].X, moveAction.CursorPoints[i].Y);
                    break;
                case MoveType.MouseNavigateToXYSmooth:
                    await MouseMoveToSmooth(moveAction.CursorPoints[i].X, moveAction.CursorPoints[i].Y);
                    break;
            }
    }
#if WINUI || WINDOWS_APP || WINRT
    public static async Task HandleMouse(MouseAction mouseAction) => await Task.Run(() => SendMouseInput(mouseAction));
    public static async Task HandleGamepad(GamepadAction gamepadAction) => await Task.Run(() => SendGamepadInput(gamepadAction));
    public static async Task HandleTouch(TouchAction touchAction) => await Task.Run(() => SendTouchInput(touchAction));
    public static async Task HandlePen(PenAction penAction) => await Task.Run(() => SendPenInput(penAction));
#endif
    public static async Task PressKeys<T>(KybdAction<T> kybdAction, DelayAction delayAction)
    {
        var states = new bool[kybdAction.Keys.Length];
        Array.Fill(states, true);
        await HandleKeys(kybdAction);
        await HybridDelay(delayAction);
        Array.Fill(states, false);
        await HandleKeys(kybdAction);
    }
    public static async Task PressButtons(ButtonAction buttonAction, DelayAction delayAction)
    {
        Array.Fill(buttonAction.States, true);
        await HandleButtons(buttonAction);
        await HybridDelay(delayAction);
        Array.Fill(buttonAction.States, false);
        await HandleButtons(buttonAction);
    }
    public static async Task HandleInputSequence(InputSequence sequence)
    {
        foreach (var step in sequence.Steps)
            await HandleInputs(step.Action);
    }
}