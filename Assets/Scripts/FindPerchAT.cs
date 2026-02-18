using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class FindPerchAT : ActionTask {
		public LayerMask perchLayerMask;
		public float detectionRadius = 15f;
		public BBParameter<Transform> targetPerch;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit() {
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {
			Collider[] detectedColliders = Physics.OverlapSphere(agent.transform.position, detectionRadius, perchLayerMask);
			
			if (detectedColliders.Length > 0) {
				Transform nearestPerch = null;
				float nearestDistance = Mathf.Infinity;

				foreach (Collider perchCollider in detectedColliders) {
					float distance = Vector3.Distance(agent.transform.position, perchCollider.transform.position);
					if (distance < nearestDistance) {
						nearestDistance = distance;
						nearestPerch = perchCollider.transform;
					}
				}

				targetPerch.value = nearestPerch;
				EndAction(true);
			} else {
				targetPerch.value = null;
				EndAction(false);
			}
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() {
			
		}

		//Called when the task is disabled.
		protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}
