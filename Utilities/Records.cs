using ReisProduction.Winjoys.Utilities.Enums;
using ReisProduction.Windelay.Utilities;
using Windows.Graphics;
using Windows.System;
namespace ReisProduction.Winjoys.Utilities;
public record InputSequence(IReadOnlyList<InputStep> Steps);
public record InputStep(
    IInputAction Action,
    DelayAction? Delay = default
);
public record KybdAction(
    bool[] States,
    VirtualKey[] Keys,
    string? WindowTitle = null,
    nint WindowhWnd = 0
) : IKybdAction;
public record ButtonAction(
    bool[] States,
    ButtonType[] Buttons
) : IMouseButton;
public record ScrollAction(
    ScrollType[] ScrollTypes,
    int[] ScrollAmount
) : IScrollAction;
public record MoveAction(
    MoveType[] Moves,
    PointInt32[] CursorPoints
) : IMoveAction;