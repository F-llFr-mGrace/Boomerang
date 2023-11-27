using UnityEngine;

public class TargetingRangeHandler : MonoBehaviour
{
    [SerializeField] CircleCollider2D detectionRange;
    [SerializeField] GameObject AiSelf;
    Rigidbody2D otherRigidbody;
    bool isTarget = false;

    public Rigidbody2D targetToGoTo;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger enter");
        if (other.CompareTag("Blue") || other.CompareTag("BlueAi") || other.CompareTag("Bandit") || other.CompareTag("BanditAi"))
        {
            if (!isTarget)
            {
                Debug.Log("!isTarget");
                AiPlaneHandling AiPlaneHandlingScript = AiSelf.GetComponent<AiPlaneHandling>();

                otherRigidbody = other.gameObject.GetComponent<Rigidbody2D>();
                targetToGoTo = otherRigidbody;
                isTarget = true;
                AiPlaneHandlingScript.aiStateIndex = 2;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger exit");
        if (collision.gameObject == targetToGoTo)
        {
            Debug.Log("collision.gameObject == targetToGoTo");
            AiPlaneHandling AiPlaneHandlingScript = AiSelf.GetComponent<AiPlaneHandling>();
            AiPlaneHandlingScript.aiStateIndex = 0;
            targetToGoTo = null;
            isTarget = false;
        }
    }
}
