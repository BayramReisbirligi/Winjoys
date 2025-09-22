namespace ReisProduction.Winjoys.Utilities.Enums;
public enum ActionType : ushort
{
    SendGameKey = 0x01,
    SendInput = 0x02,
    KybdEvent = 0x03,
    PostMessage = 0x04,
    SendMessage = 0x05,
    SendKeys = 0x06,
    SendWait = 0x07,
}