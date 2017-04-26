using UnityEngine;
using System.Collections;
using NodeCanvas.Framework;

namespace Slate.ActionClips.NodeCanvas{

	[Category("NodeCanvas")]
	[Description("Set a variable of the Blackboard attached on the actor, to a new value.")]
	abstract public class SetBlackboardVariable<T> : ActorActionClip<Blackboard>{

		public string targetVariableName;
		public T value;

		public override bool isValid{
			get {return base.isValid && !string.IsNullOrEmpty(targetVariableName);}
		}

		public override string info{
			get {return string.Format("Set Variable '{0}'\nTo '{1}'", targetVariableName, value != null? value.ToString() : "null");}
		}

		protected override void OnEnter(){
			if (Application.isPlaying){
				actor.SetValue(targetVariableName, value);
			}
		}
	}
}