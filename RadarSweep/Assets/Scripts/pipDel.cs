using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipDel : MonoBehaviour
{
    void Start()
    {
        Invoke("RemoveSelf", 20);
    }

    private void RemoveSelf()
    {
        Destroy(gameObject);
    }
}
