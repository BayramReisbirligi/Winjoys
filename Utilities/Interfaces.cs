using ReisProduction.Winjoys.Utilities.Structs;
using ReisProduction.Winjoys.Utilities.Enums;
using Windows.Gaming.Input;
using Windows.Graphics;
using Windows.UI.Input.Preview.Injection;
namespace ReisProduction.Winjoys.Utilities;
public interface IInputAction { }
public interface IPressAction : IInputAction
{
    bool[] States { get; }
    nint WindowhWnd { get; }
    string WindowTitle { get; }
}
public interface IKybdAction<T> : IPressAction
{
    T[] Keys { get; }
}
public interface IMouseButton : IPressAction
{
    ButtonType[] Buttons { get; }
}
public interface IScrollAction : IInputAction
{
    ScrollType[] ScrollTypes { get; }
    int[] ScrollAmount { get; }
}
public interface IMoveAction : IInputAction
{
    MoveType[] Moves { get; }
    PointInt32[] CursorPoints { get; }
}
public interface IGamepadAction : IInputAction
{
    GamepadButtons Buttons { get; }
    byte LeftTrigger { get; }
    byte RightTrigger { get; }
    short LeftThumbstickX { get; }
    short LeftThumbstickY { get; }
    short RightThumbstickX { get; }
    short RightThumbstickY { get; }
}
public interface IPenAction : IInputAction
{
    InjectedInputPointerOptions Options { get; }
    InjectedInputPoint Point { get; }
    int Pressure { get; }
}
public interface ITouchAction : IInputAction
{
    InjectedInputPointerOptions Options { get; }
    InjectedInputPoint Point { get; }
}