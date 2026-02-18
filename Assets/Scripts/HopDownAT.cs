using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;


namespace NodeCanvas.Tasks.Actions {

	/// <summary>
	/// Hops the parrot from its current position (e.g. a perch) back down to the ground.
	/// Uses a NavMesh raycast to find the ground directly below, then arcs down to it.
	/// Re-enables the NavMeshAgent on landing.
	/// </summary>
	public class HopDownAT : ActionTask {

		public float hopSpeed  = 4f;
		public float hopHeight = 1.5f; // Arc height on the way down

		private Vector3 startPosition;
		private Vector3 groundPosition;
		private float hopProgress = 0f;
		private NavMeshAgent navAgent;

		protected override string OnInit() {
			navAgent = agent.GetComponent<NavMeshAgent>();
			return null;
		}

		protected override void OnExecute() {
			startPosition = agent.transform.position;
			hopProgress   = 0f;

			// Find the nearest NavMesh point directly below the parrot
			NavMeshHit hit;
			if (NavMesh.SamplePosition(new Vector3(startPosition.x, 0f, startPosition.z), out hit, 10f, NavMesh.AllAreas)) {
				groundPosition = hit.position;
			} else {
				// Fallback: just drop straight down to y=0
				groundPosition = new Vector3(startPosition.x, 0f, startPosition.z);
			}
		}

		protected override void OnUpdate() {
			float distance = Vector3.Distance(startPosition, groundPosition);
			if (distance < 0.01f) {
				Land();
				return;
			}

			hopProgress += (hopSpeed / distance) * Time.deltaTime;

			if (hopProgress >= 1f) {
				Land();
				return;
			}

			// Parabolic arc downward
			Vector3 currentPos = Vector3.Lerp(startPosition, groundPosition, hopProgress);
			float heightOffset = Mathf.Sin(hopProgress * Mathf.PI) * hopHeight;
			currentPos.y += heightOffset;

			agent.transform.position = currentPos;
		}

		private void Land() {
			agent.transform.position = groundPosition;

			// Re-enable NavMeshAgent now that we're back on the NavMesh
			if (navAgent != null) {
				navAgent.enabled = true;
			}

			EndAction(true);
		}

		protected override void OnStop() { }

		protected override void OnPause() { }
	}
}
