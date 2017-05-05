using UnityEditor;
using UnityEngine;

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

    [SerializeField]
    private LocalizationData m_Data;

    public void Initialize()
    {
        Deserialize();
    }

    public string GetText(string key)
    {
        return GetText(key, m_CurrentLanguage);
    }

    public string GetText(string key, Language language)
    {
        return m_Data.GetText(key, language);
    }

    public void SetLanguage(Language language)
    {
        m_CurrentLanguage = language;
    }

    //Serialization
    private void Serialize()
    {
        //TODO(?)
    }

    private bool Deserialize()
    {
        bool success = m_Data.Deserialize(m_Filepath);
        EditorUtility.SetDirty(m_Data);

        return success;
    }
}
