using System;

[Flags]
public enum UISoundFlag
{
    None = 0,
    Hover = 1 << 0,
    Click = 1 << 1
}
