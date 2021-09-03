using System;

public enum Command
{
    Halt,
    ChangeSpeed,
    Bold,
    Italize,
    Underline,
    SetColor,
    ChangeExpression
}

[Serializable]
public class LineModifier
{
    public Command command;
    public string[] parameter;
}