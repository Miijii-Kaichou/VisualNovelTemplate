using System;
using UnityEngine;

/// <summary>
/// A list of different commands
/// </summary>
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