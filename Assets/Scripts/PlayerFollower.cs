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

    private void Awake()
    {
        TryGetComponent(out spring);
    }

    private void FixedUpdate()
    {
        Vector3 anchor = playerTransform.position;
        anchor.y += heightTarget;
        spring.anchor = anchor;
    }
}
