using System;
using UnityEngine;

public enum Command
{
    Halt,
    ChangeSpeed,
    ChangeExpression,
    InsertCharacterModel
}

[Serializable]
public class LineModifier
{
    public Command command;

    [SerializeField]
    public string[] parameter;
}