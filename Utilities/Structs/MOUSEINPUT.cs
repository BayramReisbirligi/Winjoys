using System.Runtime.InteropServices;
namespace ReisProduction.Winjoys.Utilities.Structs;
[StructLayout(LayoutKind.Sequential)]
internal struct MOUSEINPUT
{
    internal int
        dx, dy,
        mouseData,
        dwFlags,
        time;
    internal nint dwExtraInfo;
}