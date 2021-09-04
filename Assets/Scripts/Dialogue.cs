using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    [SerializeField, Header("Scenery Image")]
    Sprite openingScenery;

    [SerializeField, Header("Transition Event Codes")]
    string transitionBackgroundEventCode, transitionForegroundEventCode;

    [Header("Transition Delay")]
    public int delay = 3;

    [Tooltip("Set up to 4 characters to any given slot.")]
    [SerializeField]
    CharacterModel[] activeCharacters = new CharacterModel[CHARACTER_CAPACITY_LIMIT];

    [SerializeField]
    Line[] lines;

    [SerializeField]
    Prompt prompt;

    internal Line[] GetLines() => lines;

    [SerializeField, Header("After Last Line")]
    internal Dialogue nextDialogue;

    internal Prompt GetPrompt() => prompt;

    const int CHARACTER_CAPACITY_LIMIT = 5;

    private void OnValidate()
    {
        if(activeCharacters.Length > CHARACTER_CAPACITY_LIMIT)
        {
            CharacterModel[] sameSizeArray = new CharacterModel[CHARACTER_CAPACITY_LIMIT];
            for(int i = 0; i < CHARACTER_CAPACITY_LIMIT; i++)
            {
                sameSizeArray[i] = activeCharacters[i];
            }

            activeCharacters = sameSizeArray;
#if UNITY_EDITOR
            Debug.LogWarning("Only 4 Active Character Models at a time.");
#endif //UNITY_EDITOR
            return;
        }
    }

    public void Open()
    {
        if (openingScenery != null)
           SceneryHandler.SetImage(openingScenery);

        if(transitionBackgroundEventCode != string.Empty)
            EventManager.TriggerEvent(transitionBackgroundEventCode);

        if(transitionForegroundEventCode != string.Empty)
            EventManager.TriggerEvent(transitionForegroundEventCode);
    }

    public CharacterModel[] GetCharacterModels() => activeCharacters;
}
