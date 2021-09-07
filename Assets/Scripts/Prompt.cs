using UnityEngine;
using System;
using VN.Events;

[Serializable]
public class Prompt
{
    [Serializable]
    public class Answer
    {
        public string choices;

        public Dialogue responseDialogue;

        public string eventCode;

        public string parameter;
    }

    public Answer[] answers;
}