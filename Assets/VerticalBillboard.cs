using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalBillboard : MonoBehaviour
{
    public Transform target;
    public Transform target1;
    public float offsetDistance = -0.13f; // Distance for the left offset

    void Update()
    {
        // Calculate the offset direction (left relative to the target's forward direction)
        Vector3 offsetDirection = -target1.right;

        // Apply the offset to the position
        Vector3 offsetPosition = target1.position + offsetDirection * offsetDistance;
        transform.position = offsetPosition;

        // Look at the target
        transform.LookAt(target, Vector3.up);
    }
}
