using System;
using UnityEngine;

[Serializable]
public class Line
{
    public string characterName;
    public string voiceName;
    public string expressionName;

    [TextArea(5, 10)]
    public string content;

    public LineModifier[] lineModifiers;
}