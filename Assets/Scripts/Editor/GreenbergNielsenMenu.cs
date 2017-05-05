using System.Collections;
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
