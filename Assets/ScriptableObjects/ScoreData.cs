using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Greenberg Nielsen/Score Data")]
public class ScoreData : ScriptableObject
{
    [SerializeField]
    private StringAndIntListDictionary m_Data; //Per key multiple language values

    public List<int> GetScores(string key)
    {
        string error = KeyCheck(key);
        if (error != "")
            return null;

        return m_Data[key];
    }

    public string GetScoresText(string key)
    {
        string error = KeyCheck(key);
        if (error != "")
            return error;

        string result = "";
        for (int i = 0; i < Enum.GetNames(typeof(ScoreManager.ScoreType)).Length; ++i)
        {
            int value = m_Data[key][i];

            if (value != 0)
                result += "Add a " + ((ScoreManager.ScoreType)i).ToString() + " score of " + m_Data[key][i].ToString() + "\n";
        }

        return result.Remove(result.Length - 1); //Remove the last \n
    }

    private string KeyCheck(string key)
    {
        //Initialization check
        if (m_Data == null)
        {
            return "SCORE DATA NOT READ: Please parse the database first.";
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

    public bool Deserialize(string filePath)
    {
        m_Data = new StringAndIntListDictionary();

        string[,] parsedFile = ExtentionMethods.ParseCSV(filePath);

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
