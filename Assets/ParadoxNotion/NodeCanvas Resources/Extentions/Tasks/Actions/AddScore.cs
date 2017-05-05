using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Flavour
{
    [Category("Greenberg Nielsen")]
    [Icon("Flavour")]
    [Description("Add score")]
    public class AddScore : ActionTask
    {
        [RequiredField]
        public BBParameter<string> Key;

        protected override string info
        {
            get { return ScoreManager.Instance.GetScoresText(Key.value); }
        }

        protected override void OnExecute()
        {
            ScoreManager scoreManager = ScoreManager.Instance;

            if (scoreManager == null)
            {
                Debug.LogError("No score manager found: Cannot add score from NodeCanvas");
            }
            else
            {
                scoreManager.AddScore(Key.value);
            }

            EndAction();
        }
    }
}