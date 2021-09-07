using UnityEngine;

/// <summary>
/// A scriptable object used to construct dialogue of a different set
/// of character models.
/// </summary>
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

    /// <summary>
    /// Get all subsequent lines from this dialogue.
    /// </summary>
    /// <returns></returns>
    internal Line[] GetLines() => lines;

    /// <summary>
    /// The next dialogue to be read after this one.
    /// </summary>
    [SerializeField, Header("After Last Line")]
    internal Dialogue nextDialogue;

    /// <summary>
    /// Gets the attached prompt for this set of dialogue.
    /// </summary>
    /// <returns></returns>
    internal Prompt GetPrompt() => prompt;

    //The amount of characters allowed for dialogue
    const int CHARACTER_CAPACITY_LIMIT = 5;

    /// <summary>
    /// Is called for every values that changes. (Automatically called
    /// from Unity)
    /// </summary>
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

    /// <summary>
    /// Open the scene with an image, background transition, and
    /// foreground transition.
    /// </summary>
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
