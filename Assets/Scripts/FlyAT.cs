using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


namespace NodeCanvas.Tasks.Actions {

	public class FlyAT : ActionTask {

		public float flyHeight     = 5f;   // Target Y when airborne
		public float riseSpeed     = 2f;   // Units/sec rising and descending
		public float wanderRadius  = 4f;   // Horizontal wander radius while airborne
		public float wanderSpeed   = 2f;   // Horizontal movement speed while airborne
		public float bobAmplitude  = 0.2f; // ±Y bob range (wing flap effect)
		public float bobFrequency  = 3f;   // How fast the bob oscillates

		private NavMeshAgent navAgent;
		private Vector3 wanderTarget;
		private float bobTime = 0f;
		private bool descending = false;
		private float groundY;

		// Phases: Rising → Airborne → Descending
		private enum FlyPhase { Rising, Airborne, Descending }
		private FlyPhase phase;

		protected override string OnInit() {
			navAgent = agent.GetComponent<NavMeshAgent>();
			return null;
		}

		protected override void OnExecute() {
			ParrotState.Current = "Flying";
			// Disable NavMeshAgent — we move via transform directly
			if (navAgent != null) navAgent.enabled = false;

			groundY = agent.transform.position.y;
			bobTime = 0f;
			descending = false;
			phase = FlyPhase.Rising;

			// Pick an initial wander target at fly height
			PickNewWanderTarget();
		}

		protected override void OnUpdate() {
			bobTime += Time.deltaTime;

			// F key pressed while airborne → begin descent
			if (phase == FlyPhase.Airborne && Keyboard.current.fKey.wasPressedThisFrame) {
				phase = FlyPhase.Descending;
			}

			switch (phase) {
				case FlyPhase.Rising:
					Rise();
					break;
				case FlyPhase.Airborne:
					WanderAirborne();
					break;
				case FlyPhase.Descending:
					Descend();
					break;
			}
		}

		private void Rise() {
			Vector3 pos = agent.transform.position;
			float targetY = flyHeight;
			pos.y = Mathf.MoveTowards(pos.y, targetY, riseSpeed * Time.deltaTime);
			agent.transform.position = pos;

			if (Mathf.Abs(pos.y - targetY) < 0.05f) {
				phase = FlyPhase.Airborne;
			}
		}

		private void WanderAirborne() {
			Vector3 pos = agent.transform.position;

			// Move horizontally toward wander target
			Vector3 flatTarget = new Vector3(wanderTarget.x, pos.y, wanderTarget.z);
			pos = Vector3.MoveTowards(pos, flatTarget, wanderSpeed * Time.deltaTime);

			// Bob up and down (wing flap)
			float bob = Mathf.Sin(bobTime * bobFrequency) * bobAmplitude;
			pos.y = flyHeight + bob;

			agent.transform.position = pos;

			// Pick new wander target when close
			if (Vector3.Distance(new Vector3(pos.x, 0, pos.z), new Vector3(wanderTarget.x, 0, wanderTarget.z)) < 0.5f) {
				PickNewWanderTarget();
			}
		}

		private void Descend() {
			Vector3 pos = agent.transform.position;
			pos.y = Mathf.MoveTowards(pos.y, groundY, riseSpeed * Time.deltaTime);
			agent.transform.position = pos;

			if (Mathf.Abs(pos.y - groundY) < 0.05f) {
				pos.y = groundY;
				agent.transform.position = pos;
				EndAction(true);
			}
		}

		private void PickNewWanderTarget() {
			Vector2 randomCircle = Random.insideUnitCircle.normalized * wanderRadius;
			wanderTarget = agent.transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
		}

		protected override void OnStop() {
			// Re-enable NavMeshAgent when returning to ground
			if (navAgent != null) navAgent.enabled = true;
		}

		protected override void OnPause() { }
	}
}
