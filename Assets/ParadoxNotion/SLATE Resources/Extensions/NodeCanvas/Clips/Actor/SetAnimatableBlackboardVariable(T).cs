using UnityEngine;
using System.Collections;
using NodeCanvas.Framework;

namespace Slate.ActionClips.NodeCanvas{

	[Category("NodeCanvas")]
	[Description("Set or animate the target Blackboard variable value")]
	abstract public class SetAnimatableBlackboardVariable<T> : ActorActionClip<Blackboard>{

		[SerializeField] [HideInInspector]
		private float _length = 1f;

		public string targetVariableName;
		[AnimatableParameter]
		public T value;

		public override float length{
			get {return _length;}
			set {_length = value;}
		}

		public override bool isValid{
			get {return base.isValid && !string.IsNullOrEmpty(targetVariableName);}
		}

		public override string info{
			get {return string.Format("Set Variable '{0}'\n{1}", targetVariableName, length == 0? value.ToString() : "");}
		}

		protected override void OnUpdate(float time){
			if (Application.isPlaying){
				actor.SetValue(targetVariableName, value);
			}
		}
	}
}