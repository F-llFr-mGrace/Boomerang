using TMPro;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] TextMeshProUGUI missileCountText;
    [SerializeField] Canvas gameCanvas;

    private void Update()
    {
        PlaneMissileHandler PlaneMissileHandlerScript = player.GetComponent<PlaneMissileHandler>();
        missileCountText.text = ("Missile Count : " + PlaneMissileHandlerScript.missileCount);
    }
}
