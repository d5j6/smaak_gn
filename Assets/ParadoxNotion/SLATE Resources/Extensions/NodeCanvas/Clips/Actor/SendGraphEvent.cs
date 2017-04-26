using UnityEngine;
using System.Collections;
using NodeCanvas.Framework;

namespace Slate.ActionClips.NodeCanvas{

	[Category("NodeCanvas")]
	[Description("Sends an event to the GraphOwner(BT, FSM) attached on the actor")]
	public class SendGraphEvent : ActorActionClip<GraphOwner> {

		public string eventName;

		public override string info{
			get {return string.Format("Send Graph Event\n'{0}'", eventName); }
		}

		protected override void OnEnter(){
			if (Application.isPlaying){
				actor.SendEvent(eventName);
			}
		}
	}
}