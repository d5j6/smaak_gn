using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using Cutscene = Slate.Cutscene;
using Section = Slate.Section;

namespace NodeCanvas.Tasks.Slate{

	[Category("SLATE")]
	[Icon("SLATE")]
	[Description("Checks and returns true if the time of the cutscene is greater or equal than the selected section time, thus possible to check section changes.")]
	public class CheckSectionReached : ConditionTask{

		[RequiredField]
		public BBParameter<Cutscene> cutscene;
		[SerializeField]
		private string sectionUID;
		private Section section;

		protected override string info{
			get
			{
				if (cutscene.isNull || string.IsNullOrEmpty(sectionUID)){
					return "No Section Selected";
				}
				return string.Format("Section Reached '{0}'", cutscene.value.GetSectionByUID(sectionUID));
			}
		}

		protected override string OnInit(){
			section = cutscene.value.GetSectionByUID(sectionUID);
			if (section == null){
				return "Section is not found";
			}
			return null;
		}

		protected override bool OnCheck(){
			return cutscene.value.currentTime >= section.time;
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