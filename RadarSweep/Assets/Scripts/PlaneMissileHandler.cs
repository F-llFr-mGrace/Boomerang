using TMPro;
using UnityEngine;

public class PlaneMissileHandler : MonoBehaviour
{
    [SerializeField] GameObject missile;
    [SerializeField] Rigidbody2D planePhys;
    [SerializeField] PolygonCollider2D radarScope;
    GameObject missileInstance;
    GameObject lockTarget = null;
    GameObject closestTarget = null;
    int intendedTarget = 0;
    bool isValidTarget = false;
    bool isReloading = false;
    Color missileColor = Color.white;

    public int missileCountMax = 6;
    public int missileCount = 6;
    public bool isChaff;

    private void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CompareTag("Blue") || CompareTag("BlueAi"))
        {
            if (collision.CompareTag("Bandit") || collision.CompareTag("BanditAi"))
            {
                InterceptLogic(collision);
            }
        }
        if (CompareTag("Bandit") || CompareTag("BanditAi"))
        {
            if (collision.CompareTag("Blue") || collision.CompareTag("BlueAi"))
            {
                InterceptLogic(collision);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (radarScope.IsTouching(collision.GetComponent<Collider2D>()))
        {
            if (CompareTag("BanditAi"))
            {
                if (collision.CompareTag("Blue") || collision.CompareTag("BlueAi"))
                {
                    Invoke("OnFire", Time.deltaTime);
                }
            }

            if (CompareTag("BlueAi"))
            {
                if (collision.CompareTag("Bandit") || collision.CompareTag("BanditAi"))
                {
                    Invoke("OnFire", Time.deltaTime);
                }
            }
        }
    }

    private void InterceptLogic(Collider2D collision)
    {
        isValidTarget = true;

        if (radarScope.IsTouching(collision.GetComponent<Collider2D>()))
        {
            lockTarget = collision.gameObject;

            if (lockTarget != null)
            {
                if (closestTarget == null)
                {
                    closestTarget = lockTarget;
                }
                if (closestTarget != null)
                {
                    Rigidbody2D lockTargetPhys = lockTarget.GetComponent<Rigidbody2D>();
                    Rigidbody2D closestTargetPhys = closestTarget.GetComponent<Rigidbody2D>();

                    float distanceToClosest = Vector2.Distance(planePhys.position, closestTargetPhys.position);
                    float distanceToNew = Vector2.Distance(planePhys.position, lockTargetPhys.position);

                    if (distanceToClosest > distanceToNew)
                    {
                        closestTarget = lockTarget;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == closestTarget)
        {
            closestTarget = null;
            isValidTarget = false;
        }
    }

    private void OnFire()
    {
        if (!isReloading)
        {
            Invoke("ReArmMissiles", 5f);
            isReloading = true;
        }

        if (CompareTag("Bandit") || CompareTag("BanditAi"))
        {
            intendedTarget = 1;
            missileColor = Color.red;
        }
        if (CompareTag("Blue") || CompareTag("BlueAi"))
        {
            intendedTarget = 2;
            missileColor = Color.green;
        }
        if (isValidTarget)
        {
            if (CompareTag("BlueAi") || CompareTag("BanditAi"))
            {
                AiPlaneHandling AiPlaneHandlingScript = GetComponent<AiPlaneHandling>();
                AiPlaneHandlingScript.aiStateIndex = 0;
            }

            if (missileCount > 0)
            {
                missileInstance = Instantiate(missile, transform.position, transform.rotation);

                MissileTargeting missileInstanceScript = missileInstance.GetComponent<MissileTargeting>();
                missileInstanceScript.target = closestTarget;
                missileInstanceScript.targetIndex = intendedTarget;

                SpriteRenderer missileInstanceColor = missileInstance.GetComponent<SpriteRenderer>();
                missileInstanceColor.color = missileColor;

                missileCount--;
                Debug.Log("Missile Fired at: " + closestTarget.name);
            }
        }
    }

    private void ReArmMissiles()
    {
        if (missileCount < missileCountMax)
        {
            missileCount++;
            Invoke("ReArmMissiles", 5f);
        }
        else
        {
            isReloading = false;
        }
    }

    private void onChaff()
    {
        isChaff = true;
    }
}