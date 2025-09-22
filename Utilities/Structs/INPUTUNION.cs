using System.Runtime.InteropServices;
namespace ReisProduction.Winjoys.Utilities.Structs;
[StructLayout(LayoutKind.Explicit)]
internal struct INPUTUNION
{
    [FieldOffset(0)] internal MOUSEINPUT mi;
    [FieldOffset(0)] internal KEYBDINPUT ki;
    [FieldOffset(0)] internal HARDWAREINPUT hi;
}