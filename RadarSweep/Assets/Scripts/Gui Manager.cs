using TMPro;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] TextMeshProUGUI missileCountText;
    [SerializeField] Canvas gameCanvas;
    [SerializeField] PlaneMissileHandler PlaneMissileHandlerScript;

    private void Update()
    {
        if (PlaneMissileHandlerScript.missileCount > PlaneMissileHandlerScript.missileCountMax / 2)
        {
            missileCountText.color = Color.green;
        }
        if (PlaneMissileHandlerScript.missileCount <= PlaneMissileHandlerScript.missileCountMax / 2)
        {
            missileCountText.color = Color.yellow;
        }
        if (PlaneMissileHandlerScript.missileCount < PlaneMissileHandlerScript.missileCountMax / 4)
        {
            missileCountText.color = Color.red;
        }

        missileCountText.text = ("Missile Count : " + PlaneMissileHandlerScript.missileCount);
    }
}