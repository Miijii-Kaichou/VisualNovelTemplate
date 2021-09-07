using UnityEngine;

[CreateAssetMenu(fileName = "New Character Model", menuName = "VN Character Model", order = 2)]
public class CharacterModel : ScriptableObject
{
    /// <summary>
    /// Character's Name
    /// </summary>
    public string characterName;

    /// <summary>
    /// Voices used for the character
    /// </summary>
    public Voice[] voices;

    /// <summary>
    /// Expression that the character will make
    /// </summary>
    public Expression[] expressions;

    /// <summary>
    /// The PPU (Pixels Per Unit) of all expressions
    /// </summary>
    public float expressionPPU = 100;
}