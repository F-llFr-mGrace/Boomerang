using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playermovement : MonoBehaviour
{
    Rigidbody2D planePhys;
    Vector2 moveInput;
    [SerializeField] float rotSpeedValue = 400;
    [SerializeField] float planeSpeed = 500;
    [SerializeField] float planeSpeedboostValue = 2;
    float rotSpeed = 0;
    float planeSpeedboost = 1;

    private void Start()
    {
        planePhys = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        planePhys.AddTorque(rotSpeed * Time.deltaTime);

        Vector2 tmp = new Vector2(planeSpeed * Time.deltaTime, planeSpeed * Time.deltaTime);
        planePhys.AddForce(tmp * transform.up * planeSpeedboost);
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        rotSpeed = 0;

        if (moveInput.x > float.Epsilon)
        {
            rotSpeed = -rotSpeedValue;
        }

        if (moveInput.x < -float.Epsilon)
        {
            rotSpeed = rotSpeedValue;
        }

        planeSpeedboost = 1;

        if (moveInput.y > float.Epsilon)
        {
            planeSpeedboost = planeSpeedboostValue;
        }
    }
}