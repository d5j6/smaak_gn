using UnityEngine;
using ParadoxNotion.Design;
using NodeCanvas.Framework;
using NodeCanvas.DialogueTrees;

namespace NodeCanvas.Tasks.Actions{

	[Category("Dialogue")]
	[Description("You can use a variable inline with the text by using brackets likeso: [myVarName] or [Global/myVarName].\nThe bracket will be replaced with the variable value ToString")]
	[Icon("Dialogue")]
	[AgentType(typeof(IDialogueActor))]
	public class Say : ActionTask {

		public Statement statement = new Statement();

		protected override string info{
			get { return string.Format("<i>' {0} '</i>", (statement.text.Length > 30? statement.text.Substring(0, 30) + "..." : statement.text) ); }
		}

		protected override void OnExecute(){
			var tempStatement = statement.BlackboardReplace(blackboard);
			DialogueTree.RequestSubtitles( new SubtitlesRequestInfo( (IDialogueActor)agent, tempStatement, EndAction ) );
		}


		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnTaskInspectorGUI(){
			statement.textKey = UnityEditor.EditorGUILayout.TextArea(statement.textKey, (GUIStyle)"textField", GUILayout.Height(100));
			statement.audio = (AudioClip)UnityEditor.EditorGUILayout.ObjectField("Audio Clip", statement.audio, typeof(AudioClip), false);
			statement.meta = UnityEditor.EditorGUILayout.TextField("Meta", statement.meta);
		}

		#endif
	}
}