using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using Cutscene = Slate.Cutscene;
using Section = Slate.Section;

namespace NodeCanvas.Tasks.Slate{

	[Category("SLATE")]
	[Icon("SLATE")]
	[Description("Jump the current time of the cutscene to the speficied section, considering the cutscene is playing in the first place.")]
	public class JumpToSection : ActionTask{

		[RequiredField]
		public BBParameter<Cutscene> cutscene;
		[SerializeField]
		private string sectionUID;

		protected override string info{
			get
			{
				if (cutscene.isNull || string.IsNullOrEmpty(sectionUID)){
					return "No Section Selected";
				}
				return string.Format("Jump To Section '{0}'", cutscene.value.GetSectionByUID(sectionUID));
			}
		}

		protected override void OnExecute(){
			EndAction(  cutscene.value.JumpToSection(cutscene.value.GetSectionByUID(sectionUID))  );
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