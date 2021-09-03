using System;
using UnityEngine;

[Serializable]
public class Expression
{
    [SerializeField]
    public string expressionName
    {
        get
        {
            return expression.name;
        }
    }
    public Texture2D expression;

    public Texture2D GetExpression => expression;
}