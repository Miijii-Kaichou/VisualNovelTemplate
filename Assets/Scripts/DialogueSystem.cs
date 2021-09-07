using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System;

public class DialogueSystem : Singleton<DialogueSystem>
{
    [SerializeField, Header("Dialogue Box Canvas Group")]
    CanvasGroup canvasGroup;

    [SerializeField, Header("Text Display")]
    TextMeshProUGUI displayNameField;

    [SerializeField]
    TextMeshProUGUI dialogueContent;

    [SerializeField, Header("Initial Dialogue")]
    Dialogue initDialogue;

    [SerializeField, Header("Prompt System")]
    PromptSystem promptSystem;

    //For displaying actual dialouge
    int charPos = 0;

    //For parse a line before displaying it onto the screen
    static int ParseCharPos = -1;


    //The string data that is currently being parsed.
    static string EvaluatingString = string.Empty;


    /// <summary>
    /// A class that takes in the different commands that happens in 
    /// a line.
    /// </summary>
    public class CommandCallbacks
    {
        public int callBackPosition;
        public Command command;
        public string[] parameters;
        public delegate void CommandMethod();

        public CommandCallbacks()
        {
            parameters = new string[4];
        }

        /// <summary>
        /// The callback associated with a Command
        /// </summary>
        public CommandMethod GetCommandMethod()
        {
            int paramIndex = 0;
            switch (command)
            {
                case Command.Halt:
                    return () => Halt(Convert.ToInt32(parameters[paramIndex++]));

                case Command.ChangeSpeed:
                    return () => ChangeSpeed(Convert.ToSingle(parameters[paramIndex++]));

                case Command.ChangeExpression:
                    return () => ChangeExpression(parameters[paramIndex++]);

                case Command.InsertCharacterModel:
                   throw new NotImplementedException();

                default:
                   throw new NotImplementedException();
            }
        }
    }

    //The index value of the current line that we are on.
    int lineIndex = 0;

    Line currentLine = null;

    //If dialouge is currently running.
    bool running = false;

    //If a line will pause
    bool onHold = false;

    //How long a line is on pause.
    int holdDuration;

    //A list of commands parses from the current line.
    static List<CommandCallbacks> CommandCallbacksLog = new List<CommandCallbacks>();
    
    //If the Dialogue Box is visible or not.
    bool visible
    {
        get
        {
            return canvasGroup.alpha > 0 ? true : false;
        }
    }

    //If the player is allowed to click to the next dialogue
    bool eligibleResponse = false;

    //The chracter name, and their line to display
    string characterName, textToDisplay;
    int pageNum = 1;
    int textLength = 0;

    Dialogue currentDialogue;

    /// <summary>
    /// The last line of a staged dialogue
    /// </summary>
    public bool LastLine
    {
        get
        {
            int lineLength = currentDialogue.GetLines().Length;
            return lineIndex > lineLength - 1;
        }
    }

    /// <summary>
    /// End of Current Line
    /// </summary>
    public static bool EOL
    {
        get
        {
            return ParseCharPos > EvaluatingString.Length - 1;
        }
    }

    private void Start()
    {
        currentDialogue = initDialogue;
        Run();
    }

    /// <summary>
    /// Dialogue coroutine
    /// </summary>
    /// <returns></returns>
    IEnumerator DialogueCycle()
    {
        //If there's a delay duration set in the dialogue, wait for that amount of milliseconds.
        if (currentDialogue.delay != 0)
            yield return new WaitForSeconds(currentDialogue.delay / 1000);

        FetchDialogueData();

        ActiveCharacterSelector.SendCharacterModelsToHandlers(currentDialogue);

        DisplayCharacterName(characterName);

        SetUpCharacter();

        running = true;

        while (running)
        {
            #region Pausing Dialogue Cycle from HALT commmand
            if (onHold)
            {
                yield return new WaitForSeconds(holdDuration / 1000f);
                onHold = false;
            }
            #endregion

            yield return new WaitForSeconds(Mathf.Pow(VisualCore.TextSpeedRate, ((int)VisualCore.TextSpeed)));
            NextChar(textLength);
        }
        canvasGroup.alpha = 0;
        StopCoroutine(DialogueCycle());
    }

    /// <summary>
    /// Get the dialogue data already staged.
    /// </summary>
    private void FetchDialogueData()
    {
        currentLine = currentDialogue.GetLines()[lineIndex];
        characterName = currentLine.characterName;
        textToDisplay = ParseLine(currentLine.content);
        textLength = textToDisplay.Length;
    }

    /// <summary>
    /// Process to the next character position
    /// </summary>
    /// <param name="lengthReference"></param>
    void NextChar(int lengthReference)
    {
        if (charPos < lengthReference)
        {
            charPos++;

            //TODO: Check CommandPositions
            EvaluateCommandLog();

            DisplayChar();
        }
        else
        {
            eligibleResponse = true;
            StartCoroutine(ClickResponseCycle());
        }
    }

    /// <summary>
    /// Evaluate the saved commands from a parsed
    /// evaluated string.
    /// </summary>
    private void EvaluateCommandLog()
    {
        foreach (CommandCallbacks callback in CommandCallbacksLog)
        {
            if (charPos.Equals(callback.callBackPosition))
            {
                callback.GetCommandMethod().Invoke();
            }
        }
    }

    /// <summary>
    /// Coroutine for receiving a user response via mouse click
    /// </summary>
    /// <returns></returns>
    IEnumerator ClickResponseCycle()
    {
        while (eligibleResponse)
        {
            if (Input.GetMouseButtonDown(0))
            {
                NextLine();
                if (CheckForPrompt())
                {
                    InitPrompt();
                    running = false;
                    eligibleResponse = false;
                }
                else if (LastLine && currentDialogue.nextDialogue == null)
                {
                    running = false;
                    eligibleResponse = false;
                }
                eligibleResponse = false;
            };

            yield return null;
        }

        StopCoroutine(ClickResponseCycle());
    }

    /// <summary>
    /// Check if this dialogue has an upcoming prompt
    /// </summary>
    /// <returns></returns>
    bool CheckForPrompt()
    {
        return LastLine && (currentDialogue.GetPrompt().answers != null);
    }

    /// <summary>
    /// Display the character name on to the screen.
    /// </summary>
    /// <param name="str"></param>
    void DisplayCharacterName(string str)
    {
        displayNameField.text = str;
        if (charPos > displayNameField.maxVisibleCharacters)
        {
            pageNum++;
            displayNameField.pageToDisplay = pageNum;
        }
    }

    /// <summary>
    /// Display the character at the current character position
    /// </summary>
    void DisplayChar()
    {
        dialogueContent.text = textToDisplay.Substring(0, charPos);
    }

    /// <summary>
    /// Proceed to the next line in the dialogue.
    /// </summary>
    void NextLine()
    {
        lineIndex++;

        ParseCharPos = -1;

        CommandCallbacksLog.Clear();

        if (LastLine)
        {
            int lastLine = currentDialogue.GetLines().Length - 1;
            VisualCore.AddToHistory(currentDialogue.GetLines()[lastLine]);

            if (currentDialogue.nextDialogue != null)
            {
                running = currentDialogue.nextDialogue != null;
                Stage(currentDialogue.nextDialogue, true);
            }
            return;
        }

        VisualCore.AddToHistory(currentDialogue.GetLines()[lineIndex - 1]);

        FetchDialogueData();
        pageNum = 1;
        DisplayCharacterName(characterName);
        charPos = 0;

        SetUpCharacter();
    }

    void SetUpCharacter()
    {
        if (currentLine.referencePointer == -1)
        {
            ActiveCharacterSelector.SelectActiveSpeaker(characterName, currentLine);
        }
        else
        {
            ActiveCharacterSelector.SelectActiveSpeaker(currentLine.referencePointer, currentLine);
        }
    }

    /// <summary>
    /// Initialize a prompt
    /// </summary>
    void InitPrompt()
    {
        promptSystem.Insert(currentDialogue.GetPrompt());
    }

    /// <summary>
    /// Prepare to use Dialogue after a given response.
    /// </summary>
    /// <param name="response"></param>
    /// <param name="runImmediately"></param>
    internal static void Stage(Dialogue response, bool runImmediately = false)
    {
        Instance.currentDialogue = response;

        if (runImmediately)
        {
            Run();
        }
    }

    /// <summary>
    /// Run's a staged dialogue object
    /// </summary>
    internal static void Run()
    {
        Instance.StopAllCoroutines();
        Instance.canvasGroup.alpha = 1;
        Instance.charPos = 0;
        Instance.pageNum = 0;
        Instance.lineIndex = 0;
        Instance.currentDialogue.Open();
        Instance.StartCoroutine(Instance.DialogueCycle());
    }

    /// <summary>
    /// Parse's a line of dialogue for any commands.
    /// </summary>
    /// <param name="targetString"></param>
    /// <returns></returns>
    public static string ParseLine(string targetString)
    {
        EvaluatingString = targetString;
        char character = '\n';
        int commandIndex;
        int count = 0;
        while (!EOL)
        {

            if (character == '[')
            {
                CommandCallbacks newCallback = new CommandCallbacks();
                newCallback.callBackPosition = ParseCharPos - count;
                char numberChar = Peek(0);
                EvaluatingString = EvaluatingString.Remove(ParseCharPos, 1);
                commandIndex = (int)char.GetNumericValue(numberChar);

                LineModifier modifier = Instance.currentLine.lineModifiers[commandIndex];
                newCallback.command = modifier.command;
                newCallback.parameters = modifier.parameter;

                CommandCallbacksLog.Add(newCallback);
                count+=2;
            }
            character = Peek(0);
        }
        targetString = RemoveAllChars(EvaluatingString, '[', ']');
        return targetString;
    }

    /// <summary>
    /// Peek at a character position
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    static char Peek(int offset)
    {
        ParseCharPos++;
        if (EOL)
        {
            return '\0';
        }

        return EvaluatingString[ParseCharPos + offset];
    }

    #region Dialogue Commands
    static void Halt(int milliseconds)
    {
        Instance.onHold = true;
        Instance.holdDuration = milliseconds;

    }

    static void ChangeSpeed(float textRate)
    {
        VisualCore.TextSpeedRate = textRate;
    }

    static void SetColor(float r, float g, float b, float a, int length = 1)
    {

    }

    static void ChangeExpression(string expressionName)
    {

    }

    static void InsertCharacterModel(string name)
    {

    }
    #endregion

    static void ChangeCharacterImage(int slotPosition)
    {

    }

    /// <summary>
    /// Removes all occuring characters in a given string.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="characters"></param>
    /// <returns></returns>
    static string RemoveAllChars(string str, params char[] characters)
    {
        string parsedString = str;

        for (int k = 0; k < characters.Length; k++)
        {
            int j, count = 0, n = parsedString.Length;

            char[] t = parsedString.ToCharArray();

            for (int i = j = 0; i < n; i++)
            {
                if (parsedString[i] != characters[k])
                    t[j++] = parsedString[i];

                else count++;
            }

            while (count > 0)
            {
                t[j++] = '\0';
                count--;
            }

            parsedString = t.ArrayToString();
        }

        return parsedString;
    }
}
