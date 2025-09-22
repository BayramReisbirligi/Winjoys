using System.Runtime.InteropServices;
namespace ReisProduction.Winjoys.Utilities.Structs;
[StructLayout(LayoutKind.Sequential)]
internal struct KEYBDINPUT
{
    internal ushort
        wVk, wScan;
    internal int
        dwFlags, time;
    internal nint dwExtraInfo;
}