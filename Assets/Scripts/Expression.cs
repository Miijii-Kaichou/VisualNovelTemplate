using System;
using UnityEngine;

[Serializable]
public class Expression
{
    [SerializeField]
    //Return the expression name from the texture's name
    //If the texture name is not found, an "ND" (Not Determined) is returned
    public string expressionName
    {
        get
        {
            return texture.name ?? "ND";
        }
    }
    public Texture2D texture;

    public Texture2D GetExpression => texture;

    public float pixelsPerUnit = 100;
}