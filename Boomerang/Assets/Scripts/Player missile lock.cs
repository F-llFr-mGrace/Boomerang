using UnityEngine;

public class Playermissilelock : MonoBehaviour
{
    [SerializeField] GameObject missile;
    GameObject missileInstance;
    GameObject lockTarget;
    Transform planePhys;
    PolygonCollider2D radarScope;

    void Start()
    {
        planePhys = GetComponent<Transform>();
        radarScope = GetComponentInChildren<PolygonCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bandit"))
        {
            if (radarScope.IsTouching(collision.GetComponent<Collider2D>()))
            {
                lockTarget = collision.gameObject;
                Debug.Log("Bandit Name: " + collision.gameObject.name);
            }
        }
    }

    private void OnFire()
    {
        missileInstance = Instantiate(missile, planePhys.position, planePhys.rotation);
        Missiletargeting missileInstanceScript = missileInstance.GetComponent<Missiletargeting>();
        missileInstanceScript.target = lockTarget;
    }
}
