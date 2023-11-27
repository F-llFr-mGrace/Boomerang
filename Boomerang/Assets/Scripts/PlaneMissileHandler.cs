using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlaneMissileHandler : MonoBehaviour
{
    [SerializeField] GameObject missile;
    [SerializeField] GameObject targetingRangeChild;
    [SerializeField] int missileCount = 5;
    [SerializeField] TextMeshProUGUI missileCountText;
    GameObject missileInstance;
    GameObject lockTarget = null;
    GameObject closestTarget = null;
    Transform planeTran;
    Transform targetingRangeChildTrans;
    PolygonCollider2D radarScope;
    CircleCollider2D targetingRange;
    Rigidbody2D planePhys;
    int intendedTarget = 0;
    bool isValidTarget = false;
    bool isBanditAi = false;
    Color missileColor = Color.white;


    public Canvas gameCanvas;
    public bool isChaff;

    void Start()
    {
        if (CompareTag("BanditAi"))
        {
            isBanditAi = true;
        }

        planePhys = GetComponent<Rigidbody2D>();
        planeTran = GetComponent<Transform>();
        if (CompareTag("BlueAi") || CompareTag("BanditAi"))
        {
            targetingRangeChildTrans = targetingRangeChild.GetComponent<Transform>();
        }
        radarScope = GetComponentInChildren<PolygonCollider2D>();

        if (CompareTag("BlueAi") || CompareTag("BanditAi"))
        {
            targetingRange = GetComponentInChildren<CircleCollider2D>();
        }

        if (CompareTag("Blue"))
        {
            missileCountText.text = ("Missile Count : " + missileCount);
        }
    }

    private void Update()
    {
        if (CompareTag("BlueAi") || CompareTag("BanditAi"))
        {
            targetingRangeChildTrans.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
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

                if (CompareTag("Blue"))
                {
                    missileCountText.text = ("Missile Count : " + missileCount);
                }
            }
        }
    }

    private void onChaff()
    {
        isChaff = true;
    }
}