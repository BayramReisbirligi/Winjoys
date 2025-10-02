using ReisProduction.Winjoys.Utilities;
using Windows.Graphics;
using Windows.System;
namespace ReisProduction.Winjoys.Models;
public static partial class InputInjector
{
    /// <summary>
    /// Action handler for keyboard actions. Default is Send Game Input.
    /// </summary>
    public static Action<KybdAction<VirtualKey>> ActionHandler { get; set; } = SendGameInput;
    /// <summary>
    /// Screen size for relative mouse movements. Default is 1920x1080.
    /// </summary>
    public static SizeInt32 ScreenSize { get; set; } = new(1920, 1080);
    /// <summary>
    /// Bring target window to front before sending input. Default is false.
    /// </summary>
    public static bool BringToFrontWindow { get; set; } = false;
    /// <summary>
    /// Pixels per step delay for smooth mouse movements. Default is 23.
    /// </summary>
    public static int PixelsPerStepDelay { get; set; } = 23;
    /// <summary>
    /// Per step time delay in milliseconds for smooth mouse movements. Default is 1.
    /// </summary>
    public static int PerStepTimeDelay { get; set; } = 1;
    /// <summary>
    /// Smooth movement duration in milliseconds. Default is 9.
    /// </summary>
    public static int SmoothDurationMs { get; set; } = 9;
}