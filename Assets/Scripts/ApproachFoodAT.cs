using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;


namespace NodeCanvas.Tasks.Actions {

	public class ApproachFoodAT : ActionTask {
		public BBParameter<Transform> foodTarget;
		public float seekFrequency = 0.5f;

		private float timeSinceLastSeek = 0f;
		private NavMeshAgent navAgent;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit() {
			navAgent = agent.GetComponent<NavMeshAgent>();
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {
			if (!navAgent.enabled || !navAgent.isOnNavMesh) {
				EndAction(false);
				return;
			}
			if (foodTarget == null || foodTarget.value == null) {
				EndAction(false);
				return;
			}
			navAgent.SetDestination(foodTarget.value.position);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() {
			if (!navAgent.enabled || !navAgent.isOnNavMesh) return;

			if (foodTarget == null || foodTarget.value == null) {
				EndAction(false);
				return;
			}

			timeSinceLastSeek += Time.deltaTime;

			if (timeSinceLastSeek > seekFrequency) {
				navAgent.SetDestination(foodTarget.value.position);
				timeSinceLastSeek = 0f;
			}

			if (navAgent.remainingDistance < 0.5f && !navAgent.pathPending) {
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
