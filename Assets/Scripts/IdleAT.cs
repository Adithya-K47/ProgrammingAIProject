using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;


namespace NodeCanvas.Tasks.Actions {

	public class IdleAT : ActionTask {
		public BBParameter<Animator> animator;
		public string idleParameter = "IsIdle";
		public float minIdleDuration = 2f;
		public float maxIdleDuration = 5f;

		public BBParameter<Transform> capsuleTransform; // Assign the Capsule child
		public float lookSpeed = 1f;      // How fast it looks left/right
		public float lookAngle = 30f;     // Max degrees left/right
		public float tiltAngle = 15f;     // Degrees tilted downward (X axis)
		public bool disableNavAgent = false; // Set true for PerchIdle to keep agent disabled

		private float idleDuration;
		private float elapsedTime = 0f;
		private Quaternion originalRotation;
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
			ParrotState.Current = disableNavAgent ? "PerchIdle" : "Idle";
			idleDuration = Random.Range(minIdleDuration, maxIdleDuration);
			elapsedTime = 0f;

			if (animator != null && animator.value != null) {
				animator.value.SetBool(idleParameter, true);
			}

			// Keep NavMeshAgent disabled while on perch
			if (disableNavAgent && navAgent != null) navAgent.enabled = false;

			// Store original rotation and apply downward tilt
			if (capsuleTransform != null && capsuleTransform.value != null) {
				originalRotation = capsuleTransform.value.localRotation;
				capsuleTransform.value.localRotation = Quaternion.Euler(tiltAngle, 0f, 0f);
			}
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() {
			elapsedTime += Time.deltaTime;

			// Oscillate Y rotation to simulate looking left/right
			if (capsuleTransform != null && capsuleTransform.value != null) {
				float yaw = Mathf.Sin(elapsedTime * lookSpeed) * lookAngle;
				capsuleTransform.value.localRotation = Quaternion.Euler(tiltAngle, yaw, 0f);
			}

			if (elapsedTime >= idleDuration) {
				EndAction(true);
			}
		}

		//Called when the task is disabled.
		protected override void OnStop() {
			if (animator != null && animator.value != null) {
				animator.value.SetBool(idleParameter, false);
			}

			// NavMeshAgent is re-enabled by HopDownAT when the parrot lands back on the ground

			// Reset capsule rotation when leaving idle
			if (capsuleTransform != null && capsuleTransform.value != null) {
				capsuleTransform.value.localRotation = originalRotation;
			}
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}
