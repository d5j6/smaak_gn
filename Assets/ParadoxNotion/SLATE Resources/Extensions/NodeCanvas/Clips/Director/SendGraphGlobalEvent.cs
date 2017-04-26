using UnityEngine;
using System.Collections;
using NodeCanvas.Framework;

namespace Slate.ActionClips.NodeCanvas{

	[Category("NodeCanvas")]
	[Description("Send a global NodeCanvas event to all active GraphOwner graphs in the scene.")]
	public class SendGraphGlobalEvent : DirectorActionClip {

		public string eventName;

		public override string info{
			get {return string.Format("Send Graph Global Event '{0}'", eventName); }
		}

		protected override void OnEnter(){
			if (Application.isPlaying){
				Graph.SendGlobalEvent(eventName);
			}
		}
	}
}