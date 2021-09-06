using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TextSpeed
{
    Slow = 1,
    Normal = 2,
    Fast = 3
}

/// <summary>
/// The main core of the whole Visual Novel Engine / Mechanic / Feauture
/// </summary>
public class VisualCore : Singleton<VisualCore>
{
    static List<Line> _DialogueHistory = new List<Line>();

    [SerializeField]
    TextSpeed textSpeed = TextSpeed.Slow;

    [SerializeField]
    private List<Line> dialogueHistory;

    float textSpeedRate = 0.25f;

    public static TextSpeed TextSpeed
    {
        get
        {
            return Instance.textSpeed;
        }
    }

    public static float TextSpeedRate
    {
        get
        {
            return Instance.textSpeedRate;
        }
        set
        {
            Instance.textSpeedRate = value;
        }
    }

    public SceneryHandler sceneryHandler;
    public static SceneryHandler SceneryHandler => Instance.sceneryHandler;

    // Start is called before the first frame update
    void OnEnable()
    {
        VNEventCodeLogger.BirthAllEvents();
    }

    public static void AddToHistory(Line newLine)
    {
        _DialogueHistory.Add(newLine);
        Instance.dialogueHistory = _DialogueHistory;
    }
}
