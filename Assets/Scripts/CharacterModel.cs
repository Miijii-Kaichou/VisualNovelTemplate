using UnityEngine;

[CreateAssetMenu(fileName = "New Character Model", menuName = "VN Character Model", order = 2)]
public class CharacterModel : ScriptableObject
{
    public string characterName;
    public Voice[] voices;
    public Expression[] expressions;
}