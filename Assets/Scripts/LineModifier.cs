using System;
using UnityEngine;

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

    [SerializeField]
    public string[] parameter;
}