using UnityEngine;
using System.Collections;
using NodeCanvas.Framework;

namespace Slate.ActionClips.NodeCanvas{

	[Category("NodeCanvas")]
	[Description("Sends a value event to the GraphOwner(BT, FSM) attached on the actor")]
	abstract public class SendGraphEvent<T> : ActorActionClip<GraphOwner> {

		public string eventName;
		public T eventValue;

		public override string info{
			get {return string.Format("Send Graph Event\n'{0}' ({1})", eventName, eventValue != null? eventValue.ToString() : "null"); }
		}

		protected override void OnEnter(){
			if (Application.isPlaying){
				actor.SendEvent<T>(eventName, eventValue);
			}
		}
	}
}