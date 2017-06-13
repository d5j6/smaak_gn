using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Stijn: custom extention
namespace Slate.ActionClips.NodeCanvas
{
    [Category("Animation")]
    [Description("Change the attached animator to the requested state.")]
    public class PlayAnimation : ActorActionClip<Animator>
    {
        [SerializeField]
        public string m_AnimationName;

        public override string info
        {
            get { return string.Format("Play animation"); }
        }

        protected override void OnEnter()
        {
            if (Application.isPlaying)
            {
                actor.Play(m_AnimationName);
            }
        }
    }
}