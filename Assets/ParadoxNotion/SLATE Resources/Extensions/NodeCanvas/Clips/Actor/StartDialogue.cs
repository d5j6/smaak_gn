using UnityEngine;
using System.Collections;
using NodeCanvas.Framework;
using NodeCanvas.DialogueTrees;

//Stijn: custom extention
namespace Slate.ActionClips.NodeCanvas{

	[Category("NodeCanvas")]
	[Description("Starts the dialogue tree attached to this Game Object")]
    public class StartDialogue : ActorActionClip<DialogueTreeController>
    {
		public override string info{
			get {return string.Format("Start dialogue"); }
		}

		protected override void OnEnter(){
			if (Application.isPlaying){
                actor.StartDialogue();
			}
		}
	}
}