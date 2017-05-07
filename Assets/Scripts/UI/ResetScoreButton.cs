using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScoreButton : MonoBehaviour
{
    public void ResetScore()
    {
        ScoreManager.Instance.ResetScore();
    }
}
