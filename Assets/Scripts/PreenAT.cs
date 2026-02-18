using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class PreenAT : ActionTask {
		public BBParameter<float> preenTimer;
		public BBParameter<Animator> animator;
		public string preenParameter = "Preen";
		public float preenDuration = 3f;

		private float elapsedTime = 0f;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit() {
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {
			ParrotState.Current = "Preen";
			elapsedTime = 0f;
			preenTimer.value = 0f;

			if (animator != null && animator.value != null) {
				animator.value.SetTrigger(preenParameter);
			}
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() {
			elapsedTime += Time.deltaTime;

			if (elapsedTime >= preenDuration) {
				EndAction(true);
			}
		}

		//Called when the task is disabled.
		protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}
