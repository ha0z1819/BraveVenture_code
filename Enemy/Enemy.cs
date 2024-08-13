using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D),typeof(Animator),typeof(PhysicsCheck))]
public class Enemy : MonoBehaviour
{
    [HideInInspector]public  Rigidbody2D rb;
    [HideInInspector]public Animator anim;
    [HideInInspector]public PhysicsCheck physicsCheck;

    [Header("基本参数")]
    public float normalSpeed;
    public float chaseSpeed;     
    public float currentSpeed;
    public Vector3 faceDir;
    public float hurtForce;
    public Transform attacker;
    public Vector3 spwanPoint;

    [Header("检测")]
    public Vector2 centorOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attaclLayer;


    [Header("计时器")]

    public float waitTime;
    public float waitTimeCounter;
    public bool wait;
    public float lostTime;
    public float lostTimeCounter;

    [Header("状态")]

    public bool isHurt;
    public bool isDead;
    protected BaseState patrolState;
    protected BaseState chaseState;
    protected BaseState currentState;
    protected BaseState skillState;

    
    // ******************方法***********************//

    protected virtual void Awake(){
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        physicsCheck=GetComponent<PhysicsCheck>();

        currentSpeed = normalSpeed;
        waitTimeCounter=waitTime;
        spwanPoint = transform.position;

    }

    public void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }
    
    private void Update(){
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        
        currentState.LogicUpdate();

        TimeCounter();
    }
    
    public void FixedUpdate(){

        currentState.PhysicsUpdate();

        if(!isHurt && !isDead && !wait)
            Move();
    }

    public void OnDisable()
    {
        currentState.OnExit();
    }

    public virtual void Move(){
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("PreMove")&&!anim.GetCurrentAnimatorStateInfo(0).IsName("snailRecover"))
            rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }

    /// <summary>
    /// 计时器
    /// </summary>
    public void TimeCounter(){
        if(wait){
            waitTimeCounter-=Time.deltaTime;
            if(waitTimeCounter <=0)
            {
                wait=false;
                Debug.Log("计时结束");
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
        if(!FoundPlayer()&&lostTimeCounter>0)
        {
            lostTimeCounter-=Time.deltaTime;

        }
        else if(FoundPlayer())
        {
            lostTimeCounter = lostTime;
        }
    }

    // 发现敌人
    public virtual bool FoundPlayer()
    {

        return Physics2D.BoxCast(transform.position + (Vector3)centorOffset, checkSize, 0, faceDir, checkDistance, attaclLayer);

    }

    public void switchState(NPCState state)
    {
        // 语法糖
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            NPCState.Skill => skillState,
            _ => null
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }
    public virtual Vector3 GetNewPoint()
    {
        return transform.position;
    }
    #region 触发事件
    public void OnTakeDamage(Transform attackrans) 
    {
        attacker = attackrans;
        if (attackrans.position.x-transform.position.x>=0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attackrans.position.x-transform.position.x<0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        // 受伤被击退
        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackrans.position.x, 0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        // 协程
        StartCoroutine(OnHurt(dir));
    }

    private IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
        isHurt = false;
    }
    public void OnDie()
    {
        gameObject.layer = 2;
        anim.SetBool("dead",true);
        isDead = true;
    }

    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }
    #endregion
    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centorOffset+new Vector3(checkDistance*-transform.localScale.x,0), 0.2f);
    }
}
