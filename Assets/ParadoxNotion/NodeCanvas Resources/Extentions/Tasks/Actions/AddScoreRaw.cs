using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Flavour
{
    [Category("Greenberg Nielsen")]
    [Icon("Flavour")]
    [Description("Add score raw")]
    public class AddScoreRaw : ActionTask
    {
        [RequiredField]
        public BBParameter<ScoreManager.ScoreType> ScoreType; //No m_ as NodeCanvas doesn't filter it out as the unity inspector does :(

        [RequiredField]
        public BBParameter<int> Value;

        protected override string info
        {
            get { return "Add a " + ScoreType.value.ToString() + " score of " + Value.value; }
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
                scoreManager.AddScore(ScoreType.value, Value.value);
            }

            EndAction();
        }
    }
}