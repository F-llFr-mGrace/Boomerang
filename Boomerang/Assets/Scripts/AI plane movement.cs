using UnityEngine;
using UnityEngine.InputSystem;

public class AIplanemovement : MonoBehaviour
{
    //Flight characteristics
    [SerializeField] float rotSpeedValue = 400;
    [SerializeField] float planeSpeed = 500;
    [SerializeField] float planeSpeedboostValue = 2;
    [SerializeField] float planeBrakeValue = 2;
    Rigidbody2D planePhys;
    SpriteRenderer spriteRenderer;
    Vector2 moveInput;
    Vector2 loiterPos;
    float rotSpeed = 0;
    float planeSpeedboost = 1;

    //AI parameters
    /*
     * 1 = Loiter
     * 2 = Seeking
     * 3 = RTB
     */
    int aiStateIndex = 1;

    /*
     * 0 = null (Destroy)
     * 1 = Player
     * 2 = Bandit
     */
    int aiTeam = 0;


    private void Start()
    {
        planePhys = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        CullNoTeam();

        loiterPos = planePhys.position;
    }

    private void CullNoTeam()
    {
        if (CompareTag("Player"))
        {
            spriteRenderer.color = Color.blue;
        }

        else if (CompareTag("Bandit"))
        {
            spriteRenderer.color = Color.red;
        }

        else
        {
            Debug.Log(gameObject.name + " Does not have a team");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        planePhys.AddTorque(rotSpeed * Time.deltaTime);

        Vector2 tmp = new Vector2(planeSpeed * Time.deltaTime, planeSpeed * Time.deltaTime);
        planePhys.AddForce(tmp * transform.up * planeSpeedboost);

        if (aiStateIndex == 1)
        {
            Vector2 directionToTarget = loiterPos - planePhys.position;

            float targetAngle = Vector2.SignedAngle(planePhys.transform.up, directionToTarget);
            RotCtrl(targetAngle);
        }
    }

    private void SpeedControl()
    {
        planeSpeedboost = 1;

        if (aiStateIndex == 1)
        {
            planeSpeedboost = planeBrakeValue;
        }

        if (aiStateIndex == 0 || aiStateIndex == 3)
        {
            planeSpeedboost = planeSpeedboostValue;
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

        planePhys.AddTorque(rotSpeed * Time.deltaTime);
    }
}
