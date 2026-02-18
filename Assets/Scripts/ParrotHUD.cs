using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;



public class ParrotHUD : MonoBehaviour {

    public TMP_Text hudText;

    private void Update() {
        if (hudText == null) return;

        string state = ParrotState.Current;
        string hex = state switch {
            "Idle"        => "#44ff88",
            "Wander"      => "#44aaff",
            "Preen"       => "#ffdd44",
            "Flying"      => "#ff88ff",
            "PerchHop"    => "#ff8844",
            "EatSequence" => "#ff6666",
            _             => "#ffffff"
        };

        hudText.text =
            $"<b><color={hex}>● {state}</color></b>\n" +
            "PRESS SPACE TO HOP TO PERCH\n" +
            "PRESS F TO FLY (WHEN WANDERING)";
    }
}
