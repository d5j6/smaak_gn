using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using Cutscene = Slate.Cutscene;
using Section = Slate.Section;

namespace NodeCanvas.Tasks.Slate{

	[Category("SLATE")]
	[Icon("SLATE")]
	[Description("Play only a specific section of the cutscene")]
	public class PlaySection : ActionTask{

		[RequiredField]
		public BBParameter<Cutscene> cutscene;
		public Cutscene.WrapMode wrapMode;
		[SerializeField]
		private string sectionUID;

		protected override string info{
			get
			{
				if (cutscene.isNull || string.IsNullOrEmpty(sectionUID)){
					return "No Section Selected";
				}
				return string.Format("Play Section '{0}'", cutscene.value.GetSectionByUID(sectionUID));
			}
		}

		protected override void OnExecute(){
			cutscene.value.PlaySection(cutscene.value.GetSectionByUID(sectionUID), wrapMode, ()=>{ EndAction(); } );
		}

		protected override void OnStop(){
			cutscene.value.Stop();
		}

		
		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
			
		protected override void OnTaskInspectorGUI(){
			base.OnTaskInspectorGUI();
			if (!cutscene.isNull){
				var section = cutscene.value.GetSectionByUID(sectionUID);
				section = EditorUtils.Popup<Section>("Section", section, cutscene.value.GetSections().ToList() );
				sectionUID = section != null? section.UID : null;
			}
		}

		#endif
	}
}