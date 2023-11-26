using TMPro;
using UnityEngine;

public class PlaneMissileHandler : MonoBehaviour
{
    [SerializeField] GameObject missile;
    [SerializeField] int missileCount = 5;
    [SerializeField] TextMeshProUGUI missileCountText;
    GameObject missileInstance;
    GameObject lockTarget = null;
    GameObject closestTarget = null;
    Transform planeTran;
    PolygonCollider2D radarScope;
    Rigidbody2D planePhys;
    int intendedTarget = 0;
    bool isValidTarget = false;
    bool isAi = false;
    Color missileColor = Color.white;


    public Canvas gameCanvas;
    public bool isChaff;

    void Start()
    {
        if (gameObject.CompareTag("Bandit"))
        {
            isAi = true;
        }

        planePhys = GetComponent<Rigidbody2D>();
        planeTran = GetComponent<Transform>();
        radarScope = GetComponentInChildren<PolygonCollider2D>();

        if (!isAi)
        {
            missileCountText.text = ("Missile Count : " + missileCount);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CompareTag("Player"))
        {
            if (collision.CompareTag("Bandit"))
            {
                InterceptLogic(collision);
            }
        }
        if (CompareTag("Bandit"))
        {
            if (collision.CompareTag("Player"))
            {
                InterceptLogic(collision);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CompareTag("Bandit"))
        {
            if (collision.CompareTag("Player"))
            {
                Invoke("OnFire", 1 * Time.deltaTime);
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
        if (isAi)
        {
            intendedTarget = 1;
            missileColor = Color.red;
        }
        if (!isAi)
        {
            intendedTarget = 2;
            missileColor = Color.green;
        }
        if (missileCount > 0)
        {
            if (isValidTarget)
            {
                missileInstance = Instantiate(missile, planeTran.position, planeTran.rotation);

                MissileTargeting missileInstanceScript = missileInstance.GetComponent<MissileTargeting>();
                missileInstanceScript.target = closestTarget;
                missileInstanceScript.targetIndex = intendedTarget;

                SpriteRenderer missileInstanceColor = missileInstance.GetComponent<SpriteRenderer>();
                missileInstanceColor.color = missileColor;

                missileCount--;
                missileCountText.text = ("Missile Count : " + missileCount);
            }
        }
    }

    private void onChaff()
    {
        isChaff = true;
    }
}
