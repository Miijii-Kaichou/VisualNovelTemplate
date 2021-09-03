using UnityEngine;

public class PromptSystem : MonoBehaviour
{
    [SerializeField]
    ResponseButton[] answers;

    Prompt currentPrompt;

    internal void Insert(Prompt prompt)
    {
        currentPrompt = prompt;
        Display();
    }

    private void Display()
    {
        int indexMax = currentPrompt.answers.Length;
        for (int i = 0; i < indexMax; i++)
        {
            ResponseButton answer = answers[i];
            if (!answer.gameObject.activeInHierarchy)
            {
                answer.gameObject.SetActive(true);

                answer.content.text = currentPrompt.answers[i].choices;

                Dialogue responseDialouge = currentPrompt.answers[i].responseDialogue;
                string eventCode = currentPrompt.answers[i].eventCode;
                answer.button.onClick.AddListener(() =>
                {
                    EventManager.TriggerEvent(eventCode);
                    GotoDialogue(responseDialouge);
                });
            }
        }
    }

    internal void GotoDialogue(Dialogue response)
    {
        if (response == null) return;

        int indexMax = currentPrompt.answers.Length;
        for (int i = 0; i < indexMax; i++)
        {
            ResponseButton answer = answers[i];
            answer.content.text = currentPrompt.answers[i].choices;
            answer.button.onClick.RemoveAllListeners();
            answer.gameObject.SetActive(false);
        }
        DialogueSystem.Stage(response);
        DialogueSystem.Run();
    }
}
