using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Cutscene = Slate.Cutscene;

namespace NodeCanvas.Tasks.Slate{

	[Category("SLATE")]
	[Icon("SLATE")]
	[Description("Play a cutscene from start to finish")]
	public class PlayCutscene : ActionTask{

		[RequiredField]
		public BBParameter<Cutscene> cutscene;

		protected override string info{
			get {return "Play Cutscene " + cutscene;}
		}

		protected override void OnExecute(){
			cutscene.value.Play(0, ()=>{ EndAction(); } );
		}

		protected override void OnStop(){
			cutscene.value.Stop();
		}
	}
}