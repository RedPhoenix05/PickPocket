using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SpringJoint))]
public class PlayerFollower : MonoBehaviour
{
    public Transform playerTransform;
    public float heightTarget = 5f;
    SpringJoint spring;
    Rigidbody body;

    bool valid = false;

    private void Awake()
    {
        TryGetComponent(out spring);
        TryGetComponent(out body);

        if (playerTransform && spring && body)
        {
            valid = true;
        }

        if (valid)
        {
            spring.autoConfigureConnectedAnchor = false;
            transform.position = playerTransform.position;
            transform.position += new Vector3(0f, heightTarget, 0f);
            body.WakeUp();
        }
    }

    private void Update()
    {
        if (valid)
        {
            transform.localEulerAngles = new(0f, playerTransform.localEulerAngles.y, 0f);
        }
    }

    private void FixedUpdate()
    {
        if (valid)
        {
            body.WakeUp();
            Vector3 anchor = playerTransform.position;
            anchor.y += heightTarget;
            spring.connectedAnchor = anchor;
        }
    }
}
