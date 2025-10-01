using ReisProduction.Winjoys.Utilities.Structs;
using ReisProduction.Winjoys.Utilities.Enums;
using Windows.UI.Input.Preview.Injection;
using ReisProduction.Windelay.Utilities;
using Windows.Gaming.Input;
using Windows.Graphics;
namespace ReisProduction.Winjoys.Utilities;
public record InputSequence(IReadOnlyList<InputStep> Steps);
public record InputStep(
    IInputAction Action,
    DelayAction? Delay = default
);
public record KybdAction<T>(
    T[] Keys,
    bool[] States,
    nint WindowhWnd = 0,
    string WindowTitle = ""
) : IKybdAction<T>;
public record ButtonAction(
    bool[] States,
    ButtonType[] Buttons,
    nint WindowhWnd = 0,
    string WindowTitle = ""
) : IMouseButton;
public record ScrollAction(
    ScrollType[] ScrollTypes,
    int[] ScrollAmount,
    nint WindowhWnd = 0,
    string WindowTitle = ""
) : IScrollAction;
public record MoveAction(
    MoveType[] Moves,
    PointInt32[] CursorPoints
) : IMoveAction;
public record GamepadAction(
    GamepadButtons Buttons,
    byte LeftTrigger = 0,
    byte RightTrigger = 0,
    short LeftThumbstickX = 0,
    short LeftThumbstickY = 0,
    short RightThumbstickX = 0,
    short RightThumbstickY = 0,
    nint WindowhWnd = 0,
    string WindowTitle = ""
) : IGamepadAction;
public record PenAction(
    InjectedInputPointerOptions Options,
    InjectedInputPoint Point,
    int Pressure = 2000,
    int TiltX = 0,
    int TiltY = 0,
    double Rotation = 0,
    InjectedInputPenButtons PenButtons = InjectedInputPenButtons.None,
    InjectedInputPenParameters PenParameters = InjectedInputPenParameters.Pressure |
    InjectedInputPenParameters.Rotation | InjectedInputPenParameters.TiltX | InjectedInputPenParameters.TiltY,
    nint WindowhWnd = 0,
    string WindowTitle = ""
) : IPenAction;
public record TouchAction(
    InjectedInputPointerOptions Options,
    InjectedInputPoint Point,
    nint WindowhWnd = 0,
    string WindowTitle = ""
) : ITouchAction;