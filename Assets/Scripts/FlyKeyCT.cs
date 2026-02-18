using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.InputSystem;


namespace NodeCanvas.Tasks.Conditions {

	public class FlyKeyCT : ConditionTask {

		protected override string OnInit() {
			return null;
		}

		protected override void OnEnable() { }

		protected override void OnDisable() { }

		// Returns true the frame F is pressed — triggers transition into Flying state
		protected override bool OnCheck() {
			return Keyboard.current.fKey.wasPressedThisFrame;
		}
	}
}
