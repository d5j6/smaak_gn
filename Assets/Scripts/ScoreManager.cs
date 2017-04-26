using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Mainly a singleton so we don't need to assign it constantly in NodeCanvas (the dialogues)
public class ScoreManager : Singleton<ScoreManager>
{
    public enum ScoreType
    {
        Power,
        Brains,
        SocialEffectiveness
    }

    private int[] m_Scores;

    protected override void Awake()
    {
        base.Awake();
        m_Scores = new int[Enum.GetNames(typeof(ScoreType)).Length];
    }

    public void AddScore(ScoreType scoreType, int value)
    {
        m_Scores[(int)scoreType] += value;
        Debug.Log("Added a " + scoreType.ToString() + " score of " + value +
                  ". Total " + scoreType.ToString() + " score is now " + m_Scores[(int)scoreType]);
    }

    public void AddPowerScore(int value)
    {
        AddScore(ScoreType.Power, value);
    }

    public void AddBrainScore(int value)
    {
        AddScore(ScoreType.Brains, value);
    }

    public void AddSEScore(int value)
    {
        AddScore(ScoreType.SocialEffectiveness, value);
    }

    private void Serialize()
    {

    }

    private void Deserialize()
    {

    }
}
