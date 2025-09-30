using ReisProduction.Winjoys.Utilities.Enums;
using Windows.Graphics;
using Windows.System;
namespace ReisProduction.Winjoys.Utilities;
public interface IInputAction { }
public interface IPressAction : IInputAction
{
    bool[] States { get; }
}
public interface IKybdAction : IPressAction
{
    VirtualKey[] Keys { get; }
    string? WindowTitle { get; }
    nint WindowhWnd { get; }
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