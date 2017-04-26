using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using Cutscene = Slate.Cutscene;
using Section = Slate.Section;

namespace NodeCanvas.Tasks.Slate{

	[Category("SLATE")]
	[Icon("SLATE")]
	[Description("Skip the cutscene to the next section immediately")]
	public class SkipNext : ActionTask{

		[RequiredField]
		public BBParameter<Cutscene> cutscene;

		protected override void OnExecute(){
			cutscene.value.Skip();
			EndAction();
		}
	}
}