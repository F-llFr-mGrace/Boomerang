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
        if (collision.CompareTag("Blue") || collision.CompareTag("BlueAi") || collision.CompareTag("Bandit") || collision.CompareTag("BanditAi"))
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        isHere = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
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
