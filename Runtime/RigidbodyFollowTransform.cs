using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Rigidbody) )]
public class RigidbodyFollowTransform : MonoBehaviour {
    [Header("Inscribed")]
    public Transform transformToFollow;
    public Vector3   offset = Vector3.zero;
    
    private Rigidbody rigid;
    
    /// <summary>
    /// Automatically sets the required settings on the Rigidbody,
    /// though the reference to the Rigidbody is not serialized.
    /// </summary>
    void OnValidate() {
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;
        rigid.isKinematic = true;
    }

    void Awake() {
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;
        rigid.isKinematic = true;
    }

    private void Update() {
        if ( transformToFollow == null ) return;
        Vector3 rotatedOffset = transformToFollow.rotation * offset;
        rigid.Move(transformToFollow.position + rotatedOffset, transformToFollow.rotation);
    }
}
