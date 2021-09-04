using UnityEngine;

public class ActiveCharacterSelector : Singleton<ActiveCharacterSelector>
{
    [SerializeField]
    CharacterModelHandler[] characterModelHandlers;

    public static void LoadInCharacters(Dialogue targetDialogue)
    {
        int index = 0;
        foreach(CharacterModelHandler handler in Instance.characterModelHandlers)
        {
            handler.InsertCharacterModel(targetDialogue.GetCharacterModels()[index]);
            index++;
        }
    }

    public static void SelectActiveSpeaker()
    {

    }
}