using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;


namespace NodeCanvas.Tasks.Actions {

	public class HopAT : ActionTask {
		public BBParameter<Transform> targetPerch;
		public float hopSpeed = 4f;
		public float hopHeight = 2f;
		public float landingOffset = 0.3f; // How far above the perch pivot to land

		private Vector3 startPosition;
		private float hopProgress = 0f;
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
			if (targetPerch == null || targetPerch.value == null) {
				EndAction(false);
				return;
			}

			// Disable NavMeshAgent so it doesn't fight transform movement
			if (navAgent != null) navAgent.enabled = false;

			startPosition = agent.transform.position;
			hopProgress = 0f;
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() {
			if (targetPerch == null || targetPerch.value == null) {
				EndAction(false);
				return;
			}

			float distance = Vector3.Distance(startPosition, targetPerch.value.position);
			hopProgress += (hopSpeed / distance) * Time.deltaTime;

			if (hopProgress >= 1f) {
				// Land on top of perch, not at its centre pivot
				Vector3 landPos = targetPerch.value.position + Vector3.up * landingOffset;
				agent.transform.position = landPos;
				EndAction(true);
				return;
			}

			//Calculate parabolic arc using Lerp and Sin
			Vector3 targetPos = targetPerch.value.position + Vector3.up * landingOffset;
			Vector3 currentPos = Vector3.Lerp(startPosition, targetPos, hopProgress);
			float heightOffset = Mathf.Sin(hopProgress * Mathf.PI) * hopHeight;
			currentPos.y += heightOffset;

			agent.transform.position = currentPos;
		}

		//Called when the task is disabled.
		protected override void OnStop() {
			// NavMeshAgent is re-enabled by the next IdleAT (with disableNavAgent=true)
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}
