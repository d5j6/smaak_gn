using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class LocalizationManagerMenuItem
{
    [MenuItem("Greenberg Nielsen/Parse Localization Database")]
    public static void ParseLocalizationDatabase()
    {
        LocalizationManager.Instance.Initialize();
    }

    [MenuItem("Greenberg Nielsen/Language/Dutch")]
    public static void SetLanguageDutch()
    {
        LocalizationManager.Instance.SetLanguage(LocalizationManager.Language.Dutch);
    }

    [MenuItem("Greenberg Nielsen/Language/English")]
    public static void SetLanguageEnglish()
    {
        LocalizationManager.Instance.SetLanguage(LocalizationManager.Language.English);
    }

    [MenuItem("Greenberg Nielsen/Language/French")]
    public static void SetLanguageFrench()
    {
        LocalizationManager.Instance.SetLanguage(LocalizationManager.Language.French);
    }

    [MenuItem("Greenberg Nielsen/Language/German")]
    public static void SetLanguageGerman()
    {
        LocalizationManager.Instance.SetLanguage(LocalizationManager.Language.German);
    }
}

public class LocalizationManager : Singleton<LocalizationManager>
{
    public enum Language
    {
        Dutch,
        English,
        French,
        German
    }

    [SerializeField]
    private Language m_CurrentLanguage;

    [SerializeField]
    private string m_Filepath;
    private Dictionary<string, List<string>> m_Data; //Per key multiple language values

    private int m_NumberOfLanguages = 0;
    private bool m_IsInitialized = false;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        m_Data = new Dictionary<string, List<string>>();
        m_IsInitialized = Deserialize();
    }

    public string GetText(string key)
    {
        return GetText(key, m_CurrentLanguage);
    }

    public string GetText(string key, Language language)
    {
        //Initialization check
        if (m_Data == null || m_IsInitialized == false)
            return "DATA NOT READ: Please parse the database first.";

        //Key check
        if (key == "")
            return "Enter a key";

        if (m_Data.ContainsKey(key) == false)
            return "INVALID KEY: " + key + " does not exist.";

        //Language check
        int languageID = (int)language;
        if (languageID >= m_NumberOfLanguages)
            return "INVALID LANGUAGE: There are currently only " + m_NumberOfLanguages + " available.";

        string result = m_Data[key][languageID];

        if (result == "")
            result = "No " + language.ToString() + " translation for " + key + " yet!";

        return result;
    }

    public void SetLanguage(Language language)
    {
        m_CurrentLanguage = language;
    }

    //Serialization
    private void Serialize()
    {
        //TODO
    }

    private bool Deserialize()
    {
        string[,] parsedFile = ParseCSV(m_Filepath);

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
            List<string> translations = new List<string>();

            //Get all the translations
            for (int x = 1; x < parsedFile.GetLength(0); ++x)
            {
                translations.Add(parsedFile[x, y]);
            }

            //Add it to our data
            m_Data.Add(key, translations);
        }

        m_NumberOfLanguages = parsedFile.GetLength(1) - 1;
        Debug.Log("Localisation database parsed!");
        return true;
    }

    private string[,] ParseCSV(string filename)
    {
        string fileText = "";
        try
        {
            fileText = File.ReadAllText(filename);
        }
        catch (Exception e)
        {
            //The file was not found, but that shouldn't crash the game!
            Debug.LogError(e.Message);
            return null;
        }

        string[,] result = new string[0, 0];

        //Split the text in rows
        string[] srcRows = fileText.Split(new char[] { '\r', '\n' });
        List<string> rows = new List<string>(srcRows);
        rows.RemoveAll(rowName => rowName == "");

        //Split the rows in colmuns
        for (int y = 0; y < rows.Count; ++y)
        {
            string[] srcColumns = rows[y].Split(new char[] { ';' });

            //Create new 2 dimensional array if required (we only now know the size)
            if (result.Length == 0)
                result = new string[srcColumns.Length, rows.Count];

            for (int x = 0; x < srcColumns.Length; ++x)
            {
                //Pretty much impossible, just an extra safety net
                if (x >= result.GetLength(0))
                {
                    Debug.LogWarning("Row consists of more columns than the first row of the table! Source: " + rows[y]);
                    return null;
                }

                result[x, y] = srcColumns[x];
            }
        }

        return result;
    }
}
