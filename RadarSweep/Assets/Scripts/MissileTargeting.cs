using UnityEngine;

public class MissileTargeting : MonoBehaviour
{
    [SerializeField] float rotSpeedValue = 400;
    [SerializeField] float missileSpeed = 500;
    Rigidbody2D missilePhys;
    Rigidbody2D targetPhys;
    float rotSpeed = 0;
    bool lookEnable = false;

    public GameObject target;

    /*
     * Intended target
     * 0 = nothing
     * 1 = Player
     * 2 = Bandit
     */
    public int targetIndex;

    void Start()
    {
        Invoke("LookEnable", 0.5f);
        missilePhys = GetComponent<Rigidbody2D>();
        targetPhys = target.GetComponent<Rigidbody2D>();
        Destroy(gameObject, 60f);
    }

    private void LookEnable()
    {
        lookEnable = true;
    }

    private void Update()
    {
        if (target.Equals(null))
        {
            Debug.Log("Null missile target (SD)");
            Destroy(gameObject);
        }

        float targetAngle = MissileTargetingAlgorithm();
        RotCtrl(targetAngle);

        Vector2 tmp = new Vector2(missileSpeed * Time.deltaTime, missileSpeed * Time.deltaTime);
        missilePhys.AddForce(tmp * transform.up);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetIndex == 1)
        {
            if (collision.CompareTag("Blue") || collision.CompareTag("BlueAi"))
            {
                Debug.Log("Kill Player");
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
        if (targetIndex == 2)
        {
            if (collision.CompareTag("Bandit") || collision.CompareTag("BanditAi"))
            {
                Debug.Log("Kill Bandit");
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void RotCtrl(float targetAngle)
    {
        rotSpeed = 0;

        if (targetAngle > 1)
        {
            rotSpeed = rotSpeedValue;
        }

        if (targetAngle < 1)
        {
            rotSpeed = -rotSpeedValue;
        }

        missilePhys.AddTorque(rotSpeed * Time.deltaTime);
    }

    private float MissileTargetingAlgorithm()
    {
        Vector2 chaffNoise = new Vector2(0, 0);

        /*
        SpriteRenderer targetSpriteRenderer = target.GetComponent<SpriteRenderer>();
        string sortOrderType = targetSpriteRenderer.sortingLayerName;

        if (sortOrderType == "Plane")
        {
            PlaneMissileHandler PlaneMissileHandlerScript = target.GetComponent<PlaneMissileHandler>();

            if (PlaneMissileHandlerScript.isChaff)
            {
                chaffNoise = new Vector2(Random.Range(0, 50), Random.Range(0, 50));
            }
        }
        */
        if (targetPhys != null)
        {
            Vector2 distanceToTarget = (targetPhys.position + chaffNoise) - missilePhys.position;
            Vector2 relativeVelocity = targetPhys.velocity - missilePhys.velocity;

            if (relativeVelocity.sqrMagnitude < 0.0001f)
            {
                Debug.LogError("Relative velocity is too small. Division by zero avoided.");
                return 0f;
            }

            float timeToIntercept = distanceToTarget.magnitude / relativeVelocity.magnitude;
            Vector2 interceptPoint = targetPhys.position + targetPhys.velocity * timeToIntercept;
            Vector2 directionToTarget = interceptPoint - missilePhys.position;

            float targetAngle = Vector2.SignedAngle(missilePhys.transform.up, directionToTarget);

            if (lookEnable == true)
            {
                if (targetAngle > 90 || targetAngle < -90)
                {
                    Debug.Log("Target lost, " + targetAngle + " (SD)");
                    Destroy(gameObject);
                }
            }
            return targetAngle;
        }
        return 0;
    }
}