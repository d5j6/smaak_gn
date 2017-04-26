using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Cutscene = Slate.Cutscene;

namespace NodeCanvas.Tasks.Slate{

	[Category("SLATE")]
	[Icon("SLATE")]
	[Description("Play a Cutscene overriding it's default parameters. You can optionaly also replace one or more specific Actor Group target game objects dynamicaly. Check CutsceneIsPrefab to true if you want to instantiate the cutscene.")]
	public class PlayCutsceneAdvanced : ActionTask, ISubParametersContainer{

		public BBParameter[] GetIncludeParseParameters(){
			return replacementActors.Select(a => a.newActor).ToArray();
		}

		[System.Serializable]
		public class ReplacementInfo{
			public string groupName;
			public BBParameter<GameObject> newActor = new BBParameter<GameObject>();
		}

		[RequiredField]
		public BBParameter<Cutscene> cutscene;
		public BBParameter<float> startTime = 0;
		public Cutscene.WrapMode wrapMode;
		public Cutscene.StopMode stopMode;
		public bool cutsceneIsPrefab;
		public bool waitActionFinish = true;
		public List<ReplacementInfo> replacementActors = new List<ReplacementInfo>();

		private Cutscene cutsceneInstance;

		protected override string info{
			get {return "Play Cutscene " + cutscene;}
		}

		protected override void OnExecute(){
			
			if (cutscene.value.isActive){
				EndAction();
				return;
			}

			cutsceneInstance = cutsceneIsPrefab? Object.Instantiate(cutscene.value) : cutscene.value;

			foreach(var replacement in replacementActors){
				if (!string.IsNullOrEmpty(replacement.groupName) && replacement.newActor.value != null){
					cutsceneInstance.SetGroupActorOfName(replacement.groupName, replacement.newActor.value);
				}
			}

			cutsceneInstance.defaultStopMode = stopMode;
			if (waitActionFinish){
				cutsceneInstance.Play(startTime.value, cutsceneInstance.length, wrapMode, ()=>{ FinalizeCutscene(); EndAction(); } );
			} else {
				cutsceneInstance.Play(startTime.value, cutsceneInstance.length, wrapMode, ()=>{ FinalizeCutscene(); });
				EndAction();
			}
		}

		void FinalizeCutscene(){
			if (cutsceneIsPrefab){
				Object.Destroy(cutsceneInstance.gameObject);
			}
		}

		protected override void OnStop(){
			if (waitActionFinish && cutscene.value.isActive){
				cutsceneInstance.Stop(stopMode);
			}
		}


		///EDITOR
		#if UNITY_EDITOR

		protected override void OnTaskInspectorGUI(){
			base.OnTaskInspectorGUI();
			if (GUI.changed){
				BBParameter.SetBBFields(this, blackboard);
			}
		}

		#endif
	}
}