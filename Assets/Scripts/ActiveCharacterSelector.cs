using UnityEngine;
using System.Collections.Generic;

public class ActiveCharacterSelector : Singleton<ActiveCharacterSelector>
{
    [SerializeField]
    CharacterModelHandler[] characterModelHandlers;

    public static CharacterModelHandler[] CharacterModelHandlers => Instance.characterModelHandlers;

    const char UNDERSCORE = '_';

    /// <summary>
    /// Takes a dialogue, and assigns the characterModels of that dialogue
    /// to their respective CharacterModelHandlers (a gameObject container for CharacterModel)
    /// </summary>
    /// <param name="targetDialogue"></param>
    public static void SendCharacterModelsToHandlers(Dialogue targetDialogue)
    {
        if (targetDialogue == null) return;

        int index = 0;

        //Iterate throught each and every handler, and assign them a character model;
        foreach(CharacterModelHandler handler in CharacterModelHandlers)
        {
            handler.InsertCharacterModel(targetDialogue.GetCharacterModels()[index]);
            index++;
        }
    }

    /// <summary>
    /// Marks the character currently speaking.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="useReferenceIndex"></param>
    /// <param name="show"></param>
    public static void SelectActiveSpeaker(string name, Line line)
    {
        if (name == string.Empty || line == null) return;

        int count = 0;
        foreach(CharacterModelHandler handler in CharacterModelHandlers)
        {
            if(handler.HasCharacterModel && handler.AttachedCharacterModel.characterName.Equals(name))
            {
                SelectActiveSpeaker(count, line);
                return;
            }

            count++;
        }
    }

    /// <summary>
    /// Marks the character current speaking based 
    /// from their characterModel index
    /// </summary>
    /// <param name="index"></param>
    public static void SelectActiveSpeaker(int index, Line line)
    {
        if (line == null) return;

        CharacterModel[] characterModels = GetCharacterModels();
        for(int i = 0; i < characterModels.Length; i++)
        {
            if(i == index)
            {
                //TODO: Check if the character will be shown. If not, don't display anything
                switch (line.showOptions)
                {
                    case ShowOptions.None:
                        //We're not doing anything. Skip this, and just set the voice
                        break;

                    case ShowOptions.JustThisOne:
                        Instance.SetExpression(characterModels[i], line.expressionName, index);
                        break;

                    case ShowOptions.AllCharacters:
                        //TODO: Enable new fields if All Characters are selected, then set their respective expressions
                        break;

                    case ShowOptions.AllButThisCharacter:
                        //TODO: Enable new fields of this is selected. Omit this character
                        break;

                    default:
                        break;
                }

                
                Instance.SetVoice(characterModels[i], line.voiceName);
            }
        }
    }

    /// <summary>
    /// Grabs the character models from the selector's referenced CharacterModelHandlers
    /// </summary>
    /// <returns></returns>
    public static CharacterModel[] GetCharacterModels()
    {
        //Create a list of models to add from the iterating handlers.
        List<CharacterModel> models = new List<CharacterModel>();

        //Foreach handler we iterate through from ChracterModelHandlers,
        //check if that handler has an attached CharacterModel.
        //If it does, add it to our models list.
        foreach(CharacterModelHandler handler in CharacterModelHandlers)
        {
            if (handler.HasCharacterModel)
            {
                models.Add(handler.AttachedCharacterModel);
            }
        }

        //Return the character models that we have gathered from our CharacterModelHandlers
        //Be sure to convert it into an Array.
        return models.ToArray();
    }

    void SetExpression(CharacterModel model, string expressionName, int slotIndex)
    {
        int element = 0;

        if (model == null || expressionName == string.Empty) return;

        try
        {
            //Check the Naming Convention of a set expression
            //The file itself has to be <characterName>_<expressionName>
            //If these conditions aren't met, the request is invalid
            foreach (Expression exp in model.expressions)
            {
                string[] data = exp.expressionName.Split(UNDERSCORE);

                //Check if the first element is the charactername
                //Then check if the second element is the name expressions
                if (data[element++] == model.characterName && data[element] == expressionName)
                {
                    //TODO: Set the expression at the specified HandlerSlot
                    characterModelHandlers[slotIndex].LoadExpression(expressionName);
                    return;
                }
            }
        }
        catch
        {
            Debug.LogError("Naming Convention Verification Failed");
        }
    }

    void SetVoice(CharacterModel model, string voiceName)
    {
        int element = 0;

        if (model == null || voiceName == string.Empty) return;

        try
        {
            //Check the Naming Convention of a set expression
            //The file itself has to be <characterName>_<expressionName>
            //If these conditions aren't met, the request is invalid
            foreach (Voice voice in model.voices)
            {
                //This is where the voiceName passed through is 
                //parsed with the underscore delimiter.
                //If this fails, an exception is throw.
                string[] data = voice.voiceName.Split(UNDERSCORE);

                //Check if the first element is the charactername
                //Then check if the second element is the name expressions
                if (data[element++] == model.characterName && data[element] == voiceName)
                {
                    //TODO: Access the voice on the VoiceManager
                    VoiceManager.PlayVoice(voice);
                    return;
                }
            }
        }
        catch
        {
            //This exception was throw due to not splitting the voiceName
            //based on the underscore delimiter.
            Debug.LogError("Naming Convention Verification Failed");
        }
    }
}