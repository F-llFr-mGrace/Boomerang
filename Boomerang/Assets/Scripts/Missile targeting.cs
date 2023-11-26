using UnityEngine;

public class Missiletargeting : MonoBehaviour
{
    [SerializeField] float rotSpeedValue = 400;
    [SerializeField] float missileSpeed = 500;
    Rigidbody2D missilePhys;
    Rigidbody2D targetPhys;
    float rotSpeed = 0;
    public GameObject target;

    void Start()
    {
        missilePhys = GetComponent<Rigidbody2D>();
        targetPhys = target.GetComponent<Rigidbody2D>();
        Destroy(gameObject, 60f);
    }

    private void Update()
    {
        float targetAngle = MissileTargeting();
        RotCtrl(targetAngle);

        Vector2 tmp = new Vector2(missileSpeed * Time.deltaTime, missileSpeed * Time.deltaTime);
        missilePhys.AddForce(tmp * transform.up);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Kill Player");
        }
        if (collision.CompareTag("Bandit"))
        {
            Debug.Log("Kill Bandit");
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

    private float MissileTargeting()
    {
        Vector2 distanceToTarget = targetPhys.position - missilePhys.position;
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

        if (targetAngle > 45 || targetAngle < -45)
        {
            Destroy(gameObject);
        }

        return targetAngle;
    }
}