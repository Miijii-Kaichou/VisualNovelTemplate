using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum TransitionAction
{
    FadeToBlack,
    FadeToScenery,
    FadeToBlackThenScenery,
    FadeInToBlackForeground,
    FadeInToWhiteForeground,
    FadeOutForeground
}

public class SceneryHandler : Singleton<SceneryHandler>
{
    [SerializeField]
    Image sceneryImageA, sceneryImageB, foreground;

    static Image[] sceneryLinks = new Image[2];

    static int sceneryIndex = A;
    static int sign = -1;

    const int A = 0, B = 1;

    static bool transitioning = false;

    static bool secondSceneTarget = false;

    static readonly Color Black = Color.black;

    /// <summary>
    /// Transition with whatever kind of transition you desire.
    /// </summary>
    /// <param name="action"></param>
    public void Transition(TransitionAction action)
    {
        transitioning = true;

        #region Basic Transition Commands
        switch (action)
        {
            case TransitionAction.FadeToBlack:
                Transition_Fade_To_Black();
                break;
            case TransitionAction.FadeToScenery:
                Transition_Fade_To_Next_Scenery();
                break;
            case TransitionAction.FadeToBlackThenScenery:
                Transition_Fade_To_Black_Into_Next_Scenery();
                break;
            case TransitionAction.FadeInToBlackForeground:
                Transition_Fade_To_Black_Foreground();
                break;
            case TransitionAction.FadeInToWhiteForeground:
                Transition_Fade_To_White_Foreground();
                break;
            case TransitionAction.FadeOutForeground:
                Transition_Fade_Out_From_Foreground();
                break;
            default:
                break;
        } 
        #endregion
    }

    /// <summary>
    /// Sets the image of the scenery
    /// </summary>
    /// <param name="sprite"></param>
    internal static void SetImage(Sprite sprite)
    {
        int index = 0;
        sceneryLinks[index++] = Instance.sceneryImageA;
        sceneryLinks[index++] = Instance.sceneryImageB;
        DetermineSceneryControl();
        sceneryLinks[sceneryIndex + sign].sprite = sprite;

    }

    #region Static Function Methods
    public static void Transition_Fade_To_Black()
    {
        Instance.StartCoroutine(FadeToBlack());
    }

    public static void Transition_Fade_To_Next_Scenery()
    {
        Instance.StartCoroutine(FadeIntoNextScenery());
    }

    public static void Transition_Fade_To_Black_Into_Next_Scenery()
    {
        Instance.StartCoroutine(FadeIntoBlackIntoNextScenery());
    }

    public static void Transition_Fade_To_Black_Foreground()
    {
        Instance.StartCoroutine(FadeToBlackForeground());
    }
    public static void Transition_Fade_To_White_Foreground()
    {
        Instance.StartCoroutine(FadeToWhiteForeground());
    }

    public static void Transition_Fade_Out_From_Foreground()
    {
        Instance.StartCoroutine(FadeOutFromForeground());
    }

    #endregion

    #region Transition Coroutines
    static IEnumerator FadeToBlack()
    {
        float alphaValue = 0f;
        DetermineSceneryControl();
        while (transitioning)
        {
            alphaValue++;

            sceneryLinks[sceneryIndex].color = new Color(255f, 255f, 255f, 1f - (alphaValue / 255f));
            sceneryLinks[sceneryIndex + sign].color = new Color(0f, 0f, 0f, 0f);

            yield return new WaitForEndOfFrame();
            if (alphaValue >= 255f)
                transitioning = false;
        }
    }

    static IEnumerator FadeIntoNextScenery()
    {
        float alphaValue = 0f;
        DetermineSceneryControl();
        while (transitioning)
        {
            alphaValue++;

            if (sceneryLinks[sceneryIndex].color.a < 255f)
                sceneryLinks[sceneryIndex].color = new Color(255f, 255f, 255f, alphaValue / 255f);

            if (sceneryLinks[sceneryIndex + sign].color.a > 0f)
                sceneryLinks[sceneryIndex + sign].color = new Color(0f, 0f, 0f, 1f - (alphaValue / 255f));

            yield return new WaitForEndOfFrame();

            if (alphaValue >= 255f)
                transitioning = false;
        }
    }

    static IEnumerator FadeIntoBlackIntoNextScenery()
    {
        float alphaValue = 255f;
        DetermineSceneryControl();
        while (transitioning)
        {

            sceneryLinks[sceneryIndex].color = new Color(alphaValue / 255f, alphaValue / 255f, alphaValue / 255f);
            sceneryLinks[sceneryIndex + sign].color = new Color(alphaValue / 255f, alphaValue / 255f, alphaValue / 255f);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1);

        DetermineSceneryControl();
        while (transitioning)
        {

            sceneryLinks[sceneryIndex].color = new Color(alphaValue / 255f, alphaValue / 255f, alphaValue / 255f);
            sceneryLinks[sceneryIndex + sign].color = new Color(1 - (alphaValue / 255), 1 - (alphaValue / 255), 1 - (alphaValue / 255));
            yield return new WaitForEndOfFrame();
        }
    }

    static IEnumerator FadeToBlackForeground()
    {
        float alphaValue = 0f;
        while (transitioning)
        {
            alphaValue++;
            Instance.foreground.color = new Color(0f, 0f, 0f, alphaValue / 255f);
            yield return new WaitForEndOfFrame();
            if (alphaValue >= 255f)
                transitioning = false;
        }
    }

    static IEnumerator FadeToWhiteForeground()
    {
        float alphaValue = 0;
        while (transitioning)
        {
            alphaValue++;

            Instance.foreground.color = new Color(255f, 255f, 255f, alphaValue / 255f);

            yield return new WaitForEndOfFrame();
            if (alphaValue >= 255f)
                transitioning = false;
        }
    }

    static IEnumerator FadeOutFromForeground()
    {
        float alphaValue = 255f;
        while (transitioning)
        {
            alphaValue--;

            Color oriColor = Instance.foreground.color;
            Instance.foreground.color = new Color(oriColor.r, oriColor.b, oriColor.g, alphaValue / 255f);

            yield return new WaitForEndOfFrame();
            if (alphaValue <= 0)
                transitioning = false;
        }
    }

    #endregion

    /// <summary>
    /// Switches what Image object will transition between the other Image object.
    /// </summary>
    static void DetermineSceneryControl()
    {
        secondSceneTarget = !secondSceneTarget;
        sign = secondSceneTarget ? -1 : 1;
        sceneryIndex = secondSceneTarget ? B : A;
    }
}
