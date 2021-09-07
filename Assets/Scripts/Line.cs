using System;
using UnityEngine;

public enum ShowOptions
{
    None,
    JustThisOne,
    AllCharacters,
    AllButThisCharacter
}

[Serializable]
public class Line
{
    public string characterName;
    public string voiceName;
    public string expressionName;

    [Range(-1,3), Tooltip("Use this value to reference a specific character model. Especially" +
        "handy when using multiple of the same character model. -1 references strictly on character name" +
        "while 0 - 3 points to a specific character model to be the active actor.")]
    public int referencePointer = -1;

    public bool showCharacters = true;

    public ShowOptions showOptions = ShowOptions.JustThisOne;

    //This is an array of strings to change the expression of other characters based on the ShowOptions
    public string[] othersExpression;

    [TextArea(5, 10)]
    public string content;

    public LineModifier[] lineModifiers;

}