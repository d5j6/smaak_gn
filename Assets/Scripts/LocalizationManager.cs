using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class GreenbergNielsenMenuItem
{
    [MenuItem("Greenberg Nielsen/Parsing/Parse All")]
    public static void ParseAllDatabases()
    {
        ParseLocalizationDatabase();
        ParseScoreDatabase();
    }

    [MenuItem("Greenberg Nielsen/Parsing/Parse Localization Database")]
    public static void ParseLocalizationDatabase()
    {
        LocalizationManager.Instance.Initialize();
    }

    [MenuItem("Greenberg Nielsen/Parsing/Parse Score Database")]
    public static void ParseScoreDatabase()
    {
        ScoreManager.Instance.Initialize();
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
        {
            #if UNITY_EDITOR
                Initialize();
            #else
                return "DATA NOT READ: Please parse the database first.";
            #endif
        }

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
}
