using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{

    private CapsuleCollider2D coll;

    [Header("状态")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;

    [Header("检测参数")]
    public bool manual;
    public Vector2 bottomOffset;
    public float checkRaduis;
    public LayerMask groundLayer;
    public Vector2 leftOffset;
    public Vector2 rightOffset;



    /*****************************方法*****************************************/
    private void Awake(){
        coll = GetComponent<CapsuleCollider2D> ();
        if (!manual)
        {
            rightOffset=new Vector2((coll.bounds.size.x+coll.offset.x)/2,coll.bounds.size.y/2);
            leftOffset=new Vector2(-rightOffset.x,rightOffset.y);
        }
    }
    private void Update()
    {
        Check();
    }

    private void Check()
    {
       //检测地面
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis, groundLayer);

        //墙体判断
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), checkRaduis, groundLayer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), checkRaduis);
    }
}
