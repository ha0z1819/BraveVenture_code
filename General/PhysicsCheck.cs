using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("状态")]
    public bool isGround;
    [Header("检测参数")]
    public Vector2 bottomOffset;
    public float checkRaduis;
    public LayerMask groundLayer;
    // Update is called once per frame
    private void Update()
    {
        Check();
    }

    private void Check()
    {
        isGround= Physics2D.OverlapCircle((Vector2)transform.position+bottomOffset,checkRaduis,groundLayer);
    }
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere((Vector2)transform.position+bottomOffset,checkRaduis);
    }
}
