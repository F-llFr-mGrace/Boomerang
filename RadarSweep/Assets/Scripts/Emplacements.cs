using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emplacements : MonoBehaviour
{
    [SerializeField] SpriteRenderer emplacementsSpriteRenderer;

    void Start()
    {
        CullNoTeam();
    }
    private void CullNoTeam()
    {
        if (CompareTag("BlueAi"))
        {
            emplacementsSpriteRenderer.color = Color.blue;
        }

        else if (CompareTag("BanditAi"))
        {
            emplacementsSpriteRenderer.color = Color.red;
        }

        else
        {
            Debug.Log(gameObject.name + " Does not have a team");
            Destroy(gameObject);
        }
    }
}
