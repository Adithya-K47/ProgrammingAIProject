using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class IdleAT : ActionTask {
		public BBParameter<Animator> animator;
		public string idleParameter = "IsIdle";
		public float minIdleDuration = 2f;
		public float maxIdleDuration = 5f;

		private float idleDuration;
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
			idleDuration = Random.Range(minIdleDuration, maxIdleDuration);
			elapsedTime = 0f;

			if (animator != null && animator.value != null) {
				animator.value.SetBool(idleParameter, true);
			}
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() {
			elapsedTime += Time.deltaTime;

			if (elapsedTime >= idleDuration) {
				EndAction(true);
			}
		}

		//Called when the task is disabled.
		protected override void OnStop() {
			if (animator != null && animator.value != null) {
				animator.value.SetBool(idleParameter, false);
			}
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}
