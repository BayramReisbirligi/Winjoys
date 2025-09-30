using System.Runtime.InteropServices;
namespace ReisProduction.Winjoys.Utilities.Structs;
[StructLayout(LayoutKind.Sequential)]
public struct POINT(int x, int y)
{
    public int
        X = x,
        Y = y;
}