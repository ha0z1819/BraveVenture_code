using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public PhysicsCheck physicsCheck;
    public Vector2 inputDirection;
    public SpriteRenderer spriteRenderer;
    [Header("基本参数")]

    public float speed;
    public float jumpForce;
    public bool isCrouch;
    public bool isHurt;
    public float hurtForce;
    private float walkSpeed=>speed/2.5f;
    private float runSpeed;
    
    private Rigidbody2D rb;
    private CapsuleCollider2D Coll;

    private Vector2 originalOffset;
    private Vector2 originalSize;
    private void Awake()
    {
        // 
        inputControl=new PlayerInputControl();
        rb=GetComponent<Rigidbody2D>();
        physicsCheck=GetComponent<PhysicsCheck>();
        inputControl.GamePlay.Jump.started+=Jump;
        Coll=GetComponent<CapsuleCollider2D>();


        originalOffset = Coll.offset;
        originalSize = Coll.size;

        #region 强制走路
        runSpeed = speed;
        inputControl.GamePlay.WalkButton.performed += ctx =>
        {
            if (physicsCheck.isGround)
            {
                speed = walkSpeed;
            }
        };
        inputControl.GamePlay.WalkButton.canceled += ctx =>
        {
            if (physicsCheck.isGround)
            {
                speed = runSpeed;
            }
        };
        #endregion
    }

    

    private void OnEnable(){
        inputControl.Enable();
    }

    private void OnDisable(){
        inputControl.Disable();
    }
    private void  Update(){
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();
    }
    private void FixedUpdate(){
        Move();
    }



    public void Move(){
        if(!isCrouch)
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime,rb.velocity.y);

        // 人物反转  方法一
        // int faceDir=(int)transform.localScale.x;
        // if(inputDirection.x>0){
        //     faceDir = 1;
        // }else{
        //     faceDir = -1;
        // }
        // transform.localScale = new Vector3(inputDirection.x > 0 ? 1 : -1, 1, 1);
        // transform.localScale = new Vector3(faceDir, 1, 1);

        // 方法二
    
        if (inputDirection.x<0)
        {
            spriteRenderer.flipX = true;
        }else if(inputDirection.x>0)
        {
            spriteRenderer.flipX = false;
        }

        // 下蹲
        isCrouch=inputDirection.y<-0.5f&&physicsCheck.isGround;
        if (isCrouch){
            // 修改碰撞体
            Coll.offset = new Vector2(-0.05f, 0.85f);
            Coll.size = new Vector2(0.7f, 1.7f);

        }else{
            // 还原
            Coll.size = originalSize;
            Coll.offset = originalOffset;
        }
    }
    private void Jump(InputAction.CallbackContext context)
    {
        // 给刚体施加一个力（力的方向和力度，力的形式）
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up*jumpForce,ForceMode2D.Impulse);
        }
    }
    public void GetHurt(Transform attacker){
        
    }
}
