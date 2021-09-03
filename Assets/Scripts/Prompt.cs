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

        [SerializeField]
        public string eventCode;
    }

    public Answer[] answers;
}