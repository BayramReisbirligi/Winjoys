using System.Runtime.InteropServices;
namespace ReisProduction.Winjoys.Utilities.Structs;
[StructLayout(LayoutKind.Sequential)]
public struct INPUT
{
    internal int type;
    internal INPUTUNION u;
}