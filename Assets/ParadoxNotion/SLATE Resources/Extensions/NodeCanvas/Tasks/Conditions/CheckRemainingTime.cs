using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using ParadoxNotion;
using Cutscene = Slate.Cutscene;

namespace NodeCanvas.Tasks.Slate{

	[Category("SLATE")]
	[Icon("SLATE")]
	public class CheckRemainingTime : ConditionTask{

		[RequiredField]
		public BBParameter<Cutscene> cutscene;
		public CompareMethod checkType = CompareMethod.LessThan;
		public BBParameter<float> value;

		protected override string info{
			get	{return string.Format("{0} Remaining Time{1}{2}", cutscene, OperationTools.GetCompareString(checkType), value );}
		}

		protected override bool OnCheck(){
			var time = cutscene.value.remainingTime;
			return OperationTools.Compare((float)time, (float)value.value, checkType, 0.1f);
		}
	}
}