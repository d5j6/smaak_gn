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

    [SerializeField]
    private string m_Filepath;

    [SerializeField]
    private ScoreData m_Data;

    private int[] m_Scores;

    protected override void Awake()
    {
        base.Awake();
        m_Scores = new int[Enum.GetNames(typeof(ScoreType)).Length];
    }
    
    public void Initialize()
    {
        Deserialize();
    }

    //Mutators
    public void AddScore(string key)
    {
        List<int> scores = m_Data.GetScores(key);
        if (scores == null)
            return;

        for (int i = 0; i < m_Scores.Length; ++i)
        {
            AddScore((ScoreType)i, scores[i]);
        }
    }

    public void AddScore(ScoreType scoreType, int value)
    {
        if (value == 0)
            return;

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

    //Accessors
    public string GetScoresText(string key)
    {
        if (m_Data == null)
            return "DATA not yet assigned";

        return m_Data.GetScoresText(key);
    }

    //Serialization
    private void Serialize()
    {
        //TODO
    }

    private bool Deserialize()
    {
        bool success = m_Data.Deserialize(m_Filepath);

        #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(m_Data);
        #endif

        return success;
    }
}
