using System;
using System.Collections.Generic;
using UnityEngine;

//Variation on SerializableDictionary

[Serializable]
public class StringAndIntListDictionary : Dictionary<string, List<int>>, ISerializationCallbackReceiver
{
    [Serializable]
    private class StringAndStringListPair
    {
        [SerializeField]
        private string m_Key;
        public string Key
        {
            get { return m_Key; }
        }

        [SerializeField]
        private List<int> m_ValueList;
        public List<int> ValueList
        {
            get { return m_ValueList; }
        }

        public StringAndStringListPair(string key, List<int> valueList)
        {
            m_Key = key;
            m_ValueList = valueList;
        }
    }

    [SerializeField]
    private List<StringAndStringListPair> m_Dictionary = new List<StringAndStringListPair>();

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        m_Dictionary.Clear();

        foreach (KeyValuePair<string, List<int>> pair in this)
        {
            StringAndStringListPair temp = new StringAndStringListPair(pair.Key, pair.Value);
            m_Dictionary.Add(temp);
        }
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        for (int i = 0; i < m_Dictionary.Count; i++)
        {
            this.Add(m_Dictionary[i].Key, m_Dictionary[i].ValueList);
        }
    }
}