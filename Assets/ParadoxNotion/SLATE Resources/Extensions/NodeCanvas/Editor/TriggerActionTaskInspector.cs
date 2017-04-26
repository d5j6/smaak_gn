#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Slate.ActionClips.NodeCanvas;

namespace Slate{

	[CustomEditor(typeof(TriggerActionTask))]
	public class TriggerActionTaskInspector : ActionClipInspector<TriggerActionTask> {
		public override void OnInspectorGUI(){
			
			base.OnInspectorGUI();
			GUI.skin.label.richText = true;
			
			action.blackboardSourceTarget = (TriggerActionTask.BlackboardSourceTarget)EditorGUILayout.EnumPopup("Blackboard Source", action.blackboardSourceTarget);
			
			if (action.blackboardSourceTarget == TriggerActionTask.BlackboardSourceTarget.GetFromActor){
				if (action.actor == null || action.actor.GetComponent<Blackboard>() == null){
					EditorGUILayout.HelpBox("Actor does not have a Blackboard Component attached.", MessageType.Warning);
				}
			}

			if (action.blackboardSourceTarget == TriggerActionTask.BlackboardSourceTarget.GetFromCutscene){
				if (action.root.context.GetComponent<Blackboard>() == null){
					EditorGUILayout.HelpBox("Cutscene does not have a Blackboard Component attached.", MessageType.Warning);
				}
			}

			Task.ShowTaskInspectorGUI( action.actionList, null, false);
		}
	}
}

#endif