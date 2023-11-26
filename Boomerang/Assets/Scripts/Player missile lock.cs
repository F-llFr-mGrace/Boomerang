using UnityEngine;
using UnityEngine.UIElements;

public class Playermissilelock : MonoBehaviour
{
    [SerializeField] GameObject missile;
    GameObject missileInstance;
    GameObject lockTarget = null;
    GameObject closestTarget = null;
    Transform planeTran;
    PolygonCollider2D radarScope;
    bool isBandit = false;

    void Start()
    {
        planeTran = GetComponent<Transform>();
        radarScope = GetComponentInChildren<PolygonCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bandit"))
        {
            isBandit = true;

            if (radarScope.IsTouching(collision.GetComponent<Collider2D>()))
            {
                lockTarget = collision.gameObject;

                if (lockTarget != null)
                {
                    if (closestTarget == null)
                    {
                        closestTarget = lockTarget;
                    }
                    else if (closestTarget != null)
                    {
                        Rigidbody2D planePhys = GetComponent<Rigidbody2D>();
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == closestTarget)
        {
            closestTarget = null;
            isBandit = false;
        }
    }

    private void OnFire()
    {
        if (isBandit)
        {
            missileInstance = Instantiate(missile, planeTran.position, planeTran.rotation);

            Missiletargeting missileInstanceScript = missileInstance.GetComponent<Missiletargeting>();
            missileInstanceScript.target = closestTarget;

            SpriteRenderer missileInstanceColor = missileInstance.GetComponent<SpriteRenderer>();
            missileInstanceColor.color = Color.green;

            closestTarget = null;
            isBandit = false;
        }
    }
}
