using System;
using System.Collections.Generic;
using UnityEngine;

//Variation on SerializableDictionary

[Serializable]
public class KeyAndValueListDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>, ISerializationCallbackReceiver
{
    [Serializable]
    private class KeyAndValueListPair
    {
        [SerializeField]
        private TKey m_Key;
        public TKey Key
        {
            get { return m_Key; }
        }

        [SerializeField]
        private List<TValue> m_ValueList;
        public List<TValue> ValueList
        {
            get { return m_ValueList; }
        }

        public KeyAndValueListPair(TKey key, List<TValue> valueList)
        {
            m_Key = key;
            m_ValueList = valueList;
        }
    }

    [SerializeField]
    private List<KeyAndValueListPair> m_Dictionary = new List<KeyAndValueListPair>();

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        m_Dictionary.Clear();

        foreach (KeyValuePair<TKey, List<TValue>> pair in this)
        {
            KeyAndValueListPair temp = new KeyAndValueListPair(pair.Key, pair.Value);
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