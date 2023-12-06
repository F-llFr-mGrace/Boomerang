using UnityEngine;

public class TargetingRangeHandler : MonoBehaviour
{
    [SerializeField] CircleCollider2D detectionRange;
    [SerializeField] GameObject AiSelf;
    GameObject trackedTarget;
    Rigidbody2D otherRigidbody;
    Rigidbody2D otherotherRigidbody;
    bool isTarget = false;

    public Rigidbody2D targetToGoTo;
    public bool isHere = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger enter");
        if (AiSelf.CompareTag("Bandit") || AiSelf.CompareTag("BanditAi"))
        {
            Debug.Log("isBandit");
            if (collision.CompareTag("Blue") || collision.CompareTag("BlueAi"))
            {
                Debug.Log("isTargetBlue");
                if (!isTarget)
                {
                    Debug.Log("!isTarget");
                    trackedTarget = collision.gameObject;
                    AiPlaneHandling AiPlaneHandlingScript = AiSelf.GetComponent<AiPlaneHandling>();

                    otherRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
                    targetToGoTo = otherRigidbody;
                    isTarget = true;
                    AiPlaneHandlingScript.aiStateIndex = 2;
                }
            }
        }
        if (AiSelf.CompareTag("Blue") || AiSelf.CompareTag("BlueAi"))
        {
            if (collision.CompareTag("Bandit") || collision.CompareTag("BanditAi"))
            {
                if (!isTarget)
                {
                    trackedTarget = collision.gameObject;
                    AiPlaneHandling AiPlaneHandlingScript = AiSelf.GetComponent<AiPlaneHandling>();

                    otherRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
                    targetToGoTo = otherRigidbody;
                    isTarget = true;
                    AiPlaneHandlingScript.aiStateIndex = 2;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Trigger stay");
        if (AiSelf.CompareTag("Bandit") || AiSelf.CompareTag("BanditAi"))
        {
            if (collision.CompareTag("Blue") || collision.CompareTag("BlueAi"))
            {
                isHere = true;
            }
        }
        if (AiSelf.CompareTag("Blue") || AiSelf.CompareTag("BlueAi"))
        {
            if (collision.CompareTag("Bandit") || collision.CompareTag("BanditAi"))
            {
                isHere = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger exit");
        if (AiSelf.CompareTag("Bandit") || AiSelf.CompareTag("BanditAi"))
        {
            if (collision.CompareTag("Blue") || collision.CompareTag("BlueAi"))
            {
                otherRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
                otherotherRigidbody = trackedTarget.GetComponent<Rigidbody2D>();

                if (otherRigidbody == targetToGoTo || trackedTarget == targetToGoTo)
                {
                    AiPlaneHandling AiPlaneHandlingScript = AiSelf.GetComponent<AiPlaneHandling>();
                    AiPlaneHandlingScript.aiStateIndex = 0;
                    isTarget = false;
                    isHere = false;
                }
            }
            if (AiSelf.CompareTag("Blue") || AiSelf.CompareTag("BlueAi"))
            {
                if (collision.CompareTag("Bandit") || collision.CompareTag("BanditAi"))
                {
                    otherRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
                    otherotherRigidbody = trackedTarget.GetComponent<Rigidbody2D>();

                    if (otherRigidbody == targetToGoTo || trackedTarget == targetToGoTo)
                    {
                        AiPlaneHandling AiPlaneHandlingScript = AiSelf.GetComponent<AiPlaneHandling>();
                        AiPlaneHandlingScript.aiStateIndex = 0;
                        isTarget = false;
                        isHere = false;
                    }
                }
            }

        }
    }
}
