using UnityEngine;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Serialization;
using Status = NodeCanvas.Status;

namespace Slate.ActionClips.NodeCanvas{

	[Category("NodeCanvas")]
	[Description("Triggers a list of NodeCanvas actions. Note that not all of the actions available make sense in the context of a cutscene.\nIMPORTANT: If you specify a length for the clip, the actions will repeatedly be triggered for the duration of the clip. If you specify a length of 0, then the action will take as long as it requires to finish.")]
	public class TriggerActionTask : ActorActionClip, ITaskSystem, ISerializationCallbackReceiver {

		public enum BlackboardSourceTarget{
			DontUse,
			GetFromActor,
			GetFromCutscene
		}

		[SerializeField] [HideInInspector]
		private List<Object> _objectReferences;
		[SerializeField] [HideInInspector]
		private string _serializedList;

		public void OnBeforeSerialize(){
			_objectReferences = new List<Object>();
			_serializedList = JSONSerializer.Serialize(typeof(ActionList), actionList, false, _objectReferences);
		}

		public void OnAfterDeserialize(){
			actionList = JSONSerializer.Deserialize<ActionList>(_serializedList, _objectReferences);
			if (actionList == null) actionList = (ActionList)Task.Create(typeof(ActionList), this);
		}

		[SerializeField] [HideInInspector]
		private float _length;
		[SerializeField] [HideInInspector]
		private BlackboardSourceTarget _blackboardSourceTarget = BlackboardSourceTarget.GetFromActor;

		[System.NonSerialized]
		private ActionList _actionList;
		[System.NonSerialized]
		private Blackboard _blackboard;

		protected override void OnCreate(){
			_actionList = (ActionList)Task.Create(typeof(ActionList), this);
			ResolveBlackboard();
			SendTaskOwnerDefaults();
		}

		protected override void OnAfterValidate(){
			ResolveBlackboard();
			SendTaskOwnerDefaults();
		}

		void ResolveBlackboard(){
			if (_blackboardSourceTarget == BlackboardSourceTarget.DontUse){
				_blackboard = null;
			}
			if (_blackboardSourceTarget == BlackboardSourceTarget.GetFromActor){
				_blackboard = actor != null? actor.GetComponent<Blackboard>() : null;
			}
			if (_blackboardSourceTarget == BlackboardSourceTarget.GetFromCutscene){
				var cutscene = root as Cutscene;
				_blackboard = cutscene != null? cutscene.GetComponent<Blackboard>() : null;
			}			
		}

		////////
		////////

		public ActionList actionList{
			get {return _actionList;}
			set {_actionList = value;}
		}

		public BlackboardSourceTarget blackboardSourceTarget{
			get {return _blackboardSourceTarget;}
			set
			{
				if (_blackboardSourceTarget != value){
					_blackboardSourceTarget = value;
					ResolveBlackboard();
					SendTaskOwnerDefaults();
				}
			}
		}


		//ITaskSystem
		public Component agent{ get {return actor != null? actor.transform : null;} }
		public IBlackboard blackboard{ get {return _blackboard;} }
		public float elapsedTime{ get {return actionList.elapsedTime;} }
		public Object contextObject{ get {return this;} }
		void ITaskSystem.SendEvent(ParadoxNotion.EventData eventData){}
		public void SendTaskOwnerDefaults(){
			actionList.SetOwnerSystem(this);
			foreach(var a in actionList.actions){
				a.SetOwnerSystem(this);
			}
		}
		//...


		public override string info{
			get {return actionList != null? actionList.summaryInfo : null;}
		}

		public override float length{
			get { return _length == 0 && actionList != null? actionList.elapsedTime : _length ; }
			set { if (!Application.isPlaying) {_length = value;} }
		}

		protected override void OnEnter(){
			if (Application.isPlaying){
				if (length == 0){
					actionList.ExecuteAction(agent, blackboard, null);
				}
			}
		}

		protected override void OnUpdate(float time, float previousTime){
			if (Application.isPlaying && time > previousTime){
				if (length > 0){
					actionList.ExecuteAction(agent, blackboard);
				}
			}
		}

		protected override void OnExit(){
			if (Application.isPlaying){
				if (length > 0){
					actionList.EndAction(null);
				}
			}
		}
	}
}