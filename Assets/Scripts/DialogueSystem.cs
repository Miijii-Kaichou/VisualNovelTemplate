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

    static string EvaluatingString = string.Empty;

    public class CommandCallbacks
    {
        public int callBackPosition;
        public Command command;
    }

    int currentLine = 0;
    bool running = false;
    bool onHold = false;
    static List<CommandCallbacks> CommandCallbacksLog = new List<CommandCallbacks>();
    bool visible
    {
        get
        {
            return canvasGroup.alpha > 0 ? true : false;
        }
    }


    bool eligibleResponse = false;
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
            return currentLine > lineLength - 1;
        }
    }

    /// <summary>
    /// End of Current Line
    /// </summary>
    public static bool EOL
    {
        get
        {
            return ParseCharPos > EvaluatingString.Length - 1 ;
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

        DisplayCharacterName(characterName);

        running = true;

        while (running)
        {
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
        characterName = currentDialogue.GetLines()[currentLine].characterName;
        textToDisplay = ParseLine(currentDialogue.GetLines()[currentLine].content);
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
            DisplayChar();
        }
        else
        {
            eligibleResponse = true;
            StartCoroutine(ClickResponseCycle());
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
        currentLine++;

        ParseCharPos = -1;

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

        VisualCore.AddToHistory(currentDialogue.GetLines()[currentLine - 1]);

        FetchDialogueData();
        pageNum = 1;
        DisplayCharacterName(characterName);
        charPos = 0;
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
        Instance.charPos = 1;
        Instance.pageNum = 0;
        Instance.currentLine = 0;
        Instance.currentDialogue.Open();
        Instance.StartCoroutine(Instance.DialogueCycle());
    }

    /// <summary>
    /// Parse's a line of dialogue for any commands.
    /// </summary>
    /// <param name="targetString"></param>
    /// <returns></returns>
    internal static string ParseLine(string targetString)
    {
        EvaluatingString = targetString;
        char character = '\n';
        int commandIndex;
        
        while (!EOL)
        {
            
            if (character == '[')
            {
                CommandCallbacks newCallback = new CommandCallbacks();
                newCallback.callBackPosition = ParseCharPos;
                commandIndex = (int)char.GetNumericValue(Peek(0));
                newCallback.command = Instance.currentDialogue.GetLines()[Instance.currentLine].lineModifiers[commandIndex].command;
                CommandCallbacksLog.Add(newCallback);
            }
            character = Peek(0);
        }
        Debug.Log($"Command Length: {CommandCallbacksLog.Count}");
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

    /// <summary>
    /// Removes all occuring characters in a given string.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="characters"></param>
    /// <returns></returns>
    static string RemoveAllChars(string str, params char[] characters)
    {
        string parsedString = string.Empty;
        for (int k = 0; k < characters.Length; k++)
        {
            int j, count = 0, n = str.Length;
            char[] t = str.ToCharArray();
            for (int i = j = 0; i < n; i++)
            {
                if (str[i] != characters[k])
                    t[j++] = str[i];
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