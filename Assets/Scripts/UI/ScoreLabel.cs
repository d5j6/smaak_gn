using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreLabel : MonoBehaviour
{
    [SerializeField]
    private ScoreManager.ScoreType m_ScoreType;
    private Text m_Text;

    private void Awake()
    {
        m_Text = GetComponent<Text>();
    }

    private void Start()
    {
        ScoreManager.Instance.ScoreChangedEvent += OnScoreChanged;
        OnScoreChanged(m_ScoreType, 0);
    }

    private void OnDestroy()
    {
        ScoreManager scoreManager = ScoreManager.Instance;

        if (scoreManager != null)
            scoreManager.ScoreChangedEvent -= OnScoreChanged;
    }

    private void OnScoreChanged(ScoreManager.ScoreType scoreType, int i)
    {
        if (scoreType == m_ScoreType)
            m_Text.text = scoreType.ToString() + ": " + i;
    }
}
