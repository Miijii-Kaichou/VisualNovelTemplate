using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public static class VNEventCodeLogger
{
    static bool conceived = false;

    /*********************************************************************************************
     * This method is called after VisualCore has initialized                                    *
     * You are free to add any EventCodes to this log.                                           *
     * You can choose to give a fixed number for the UniqueID of an event,                       *
     * or you can use a number that is free by using EventManager.FreeID.                        *
     * The main take from this class is to be able to use when creating your visual novel.       *
     * If you need an event to happen at an end of a dialogue, those events will be created here,*
     * and called whereever you want (preferably with prompts for each Dialogue Object.          *
     ********************************************************************************************/

    public static string SetParam;

    public static void BirthAllEvents()
    {
        if (conceived) return;

        //Add all defined events in here for prompts or custom commands.
        EventManager.AddEvent(EventManager.FreeID, "QuitGame", () =>
        {
#if UNITY_EDITOR
            if (Application.isEditor)
            {
                EditorApplication.ExitPlaymode();
                return;
            }
#endif 
            Application.Quit();
        });

        #region Scenery Transitioning
        EventManager.AddEvent(EventManager.FreeID, "FadeToBlack", () =>
        {
            VisualCore.SceneryHandler.Transition(TransitionAction.FadeToBlack);
        });

        EventManager.AddEvent(EventManager.FreeID, "FadeToScenery", () =>
        {
            VisualCore.SceneryHandler.Transition(TransitionAction.FadeToScenery);
        });

        EventManager.AddEvent(EventManager.FreeID, "FadeToBlackThenScenery", () =>
        {
            VisualCore.SceneryHandler.Transition(TransitionAction.FadeToBlackThenScenery);
        });

        EventManager.AddEvent(EventManager.FreeID, "FadeToBlackForeground", () =>
        {
            VisualCore.SceneryHandler.Transition(TransitionAction.FadeInToBlackForeground);
        });

        EventManager.AddEvent(EventManager.FreeID, "FadeToWhiteForeground", () =>
        {
            VisualCore.SceneryHandler.Transition(TransitionAction.FadeInToWhiteForeground);
        });

        EventManager.AddEvent(EventManager.FreeID, "FadeOutFromForeground", () =>
        {
            VisualCore.SceneryHandler.Transition(TransitionAction.FadeOutForeground);
        });

        EventManager.AddEvent(EventManager.FreeID, "AddLove", () =>
        {
            int index = 0;
            if(SetParam != string.Empty)
            {

            }
        });
        #endregion

        //Leave this here. This prevents putting in double the amount of events we have.
        conceived = true;
    }
}
