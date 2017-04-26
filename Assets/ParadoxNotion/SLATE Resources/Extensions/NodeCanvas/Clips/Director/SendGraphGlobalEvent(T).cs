using UnityEngine;
using System.Collections;
using NodeCanvas.Framework;

namespace Slate.ActionClips.NodeCanvas{

	[Category("NodeCanvas")]
	[Description("Send a global NodeCanvas event to all active GraphOwner graphs in the scene.")]
	abstract public class SendGraphGlobalEvent<T> : DirectorActionClip {

		public string eventName;
		public T eventValue;

		public override string info{
			get {return string.Format("Send Graph Global Event\n'{0}' ({1})", eventName, eventValue != null? eventValue.ToString() : "null"); }
		}

		protected override void OnEnter(){
			if (Application.isPlaying){
				Graph.SendGlobalEvent<T>(eventName, eventValue);
			}
		}
	}
}