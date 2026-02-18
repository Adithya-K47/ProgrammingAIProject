using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {


	public class SetMaterialAT : ActionTask {


		public BBParameter<Renderer> targetRenderer;
		

		public BBParameter<Material> newMaterial;

		protected override string OnInit() {
			return null;
		}

		protected override void OnExecute() {
			if (targetRenderer.value != null && newMaterial.value != null) {
				targetRenderer.value.material = newMaterial.value;
			} else {
				Debug.LogWarning("SetMaterialAT: Missing targetRenderer or newMaterial.");
			}
			EndAction(true);
		}
	}
}
