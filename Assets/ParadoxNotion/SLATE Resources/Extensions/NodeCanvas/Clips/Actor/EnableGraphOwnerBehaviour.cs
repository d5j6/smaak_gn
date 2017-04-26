using UnityEngine;
using System.Collections;
using NodeCanvas.Framework;

namespace Slate.ActionClips.NodeCanvas{

	[Category("NodeCanvas")]
	[Description("This clip works with a GraphOwner(BT, FSM) attached on the actor. The owner's behaviour will be enabled (if not already) for the duration of the clip and then disabled.")]
	public class EnableGraphOwnerBehaviour : ActorActionClip<GraphOwner> {

		[SerializeField] [HideInInspector]
		private float _length = 5f;

		public override float length{
			get {return _length;}
			set {_length = value;}
		}

		protected override void OnEnter(){
			if (Application.isPlaying){
				actor.StartBehaviour();
			}
		}

		protected override void OnExit(){
			if (Application.isPlaying){
				actor.StopBehaviour();
			}
		}
	}
}