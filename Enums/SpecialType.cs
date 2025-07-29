namespace ReisProduction.Winjoys.Enums;
public enum SpecialType : ushort
{
    LeftButton = 0x01,
    RightButton = 0x02,
    MiddleButton = 0x04,
    XButton1 = 0x05,
    XButton2 = 0x06,
    MouseScrollUp = 0xFF00,
    MouseScrollDown = 0xFF01,
    MouseNavigateUp = 0xFF02,
    MouseNavigateUpSmooth = 0xFF03,
    MouseNavigateDown = 0xFF04,
    MouseNavigateDownSmooth = 0xFF05,
    MouseNavigateLeft = 0xFF06,
    MouseNavigateLeftSmooth = 0xFF07,
    MouseNavigateRight = 0xFF08,
    MouseNavigateRightSmooth = 0xFF09,
    MouseNavigateToXY = 0xFF0A,
    MouseNavigateToXYSmooth = 0xFF0B,
}