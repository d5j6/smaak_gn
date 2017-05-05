using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    private Dictionary<string, List<int>> m_Data; //Per key multiple score types

    private int[] m_Scores;
    private bool m_IsInitialized = false;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    public void Initialize()
    {
        m_Scores = new int[Enum.GetNames(typeof(ScoreType)).Length];
        m_Data = new Dictionary<string, List<int>>();
        m_IsInitialized = Deserialize();
    }

    //Mutators
    public void AddScore(string key)
    {
        string error = KeyCheck(key);
        if (error != "")
            return;

        for (int i = 0; i < m_Scores.Length; ++i)
        {
            AddScore((ScoreType)i, m_Data[key][i]);
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
    public string GetScores(string key)
    {
        string error = KeyCheck(key);
        if (error != "")
            return error;

        string result = "";
        for (int i = 0; i < m_Scores.Length; ++i)
        {
            int value = m_Data[key][i];

            if (value != 0)
                result += "Add a " + ((ScoreType)i).ToString() + " score of " + m_Data[key][i].ToString() + "\n";
        }

        return result.Remove(result.Length - 1); //Remove the last \n
    }

    private string KeyCheck(string key)
    {
        //Initialization check
        if (m_Data == null || m_IsInitialized == false)
        {
            #if UNITY_EDITOR
                Initialize();
            #else
                return "SCORE DATA NOT READ: Please parse the database first.";
            #endif
        }

        //Key check
        if (key == "")
        {
            return "Enter a key";
        }

        if (m_Data.ContainsKey(key) == false)
        {
            return "INVALID KEY: " + key + " does not exist.";
        }

        return "";
    }

    //Serialization
    private void Serialize()
    {
        //TODO
    }

    private bool Deserialize()
    {
        string[,] parsedFile = ExtentionMethods.ParseCSV(m_Filepath);

        if (parsedFile == null)
            return false;

        if (parsedFile.GetLength(1) < 2)
        {
            Debug.LogError("The localisation file does not contain any data!");
            return false;
        }

        //For every row
        for (int y = 1; y < parsedFile.GetLength(1); ++y)
        {
            //Get the key
            string key = parsedFile[0, y];
            List<int> scores = new List<int>();

            //Get all the translations
            for (int x = 1; x < parsedFile.GetLength(0); ++x)
            {
                int result = 0;
                bool success = int.TryParse(parsedFile[x, y], out result);

                if (success || parsedFile[x, y] == "")
                    scores.Add(result);
            }

            //Add it to our data
            m_Data.Add(key, scores);
        }

        Debug.Log("Score database parsed!");
        return true;
    }
}
