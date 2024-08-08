using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    protected Animator anim;
    PhysicsCheck physicsCheck;

    [Header("基本参数")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;
    public Transform attacker;

    [Header("计时器")]

    public float waitTime;
    public float waitTimeCounter;
    public bool wait;


    // ******************方法***********************//

    private void Awake(){
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        physicsCheck=GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        waitTimeCounter=waitTime;
    }

    private void Update(){
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        // 碰到墙壁则反转
        if ((physicsCheck.touchLeftWall&&faceDir.x<0)||(physicsCheck.touchRightWall&&faceDir.x>0))
        {
            wait=true;
        }
        TimeCounter();
    }
    
    public void FixedUpdate(){
        Move();
    }
    public virtual void Move(){
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }
    /// <summary>
    /// 计时器
    /// </summary>
    public void TimeCounter(){
        if(wait){
            waitTimeCounter-=Time.deltaTime;
            if(waitTimeCounter < 0)
            {
                wait=false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
    }

    public void OnTakeDamage(Transform attackrans) 
    {
        attacker = attackrans;
        Debug.Log(attackrans.position.x-transform.position.x);
        if (attackrans.position.x-transform.position.x>=0)
        {
            Debug.Log("1111");
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attackrans.position.x-transform.position.x<0)
        {
            Debug.Log("2222");
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
