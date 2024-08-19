using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public PhysicsCheck physicsCheck;
    public Vector2 inputDirection;
    public SpriteRenderer spriteRenderer;
    private PlayerAnimation playerAnimation;
    private Character character;


    [Header("基本参数")]
    public float speed;
    public float jumpForce;
    public float hurtForce;
    public float SlideDistance;
    public float SlideSpeed;
    private float walkSpeed=>speed/2.5f;
    private float runSpeed;
    public float wallJumpForce;
    public float slidePowerCost;

    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    [Header("状态")]
    public bool isDead;
    public bool isHurt;
    public bool isCrouch;
    public bool isAttack;
    public bool wallJump;
    public bool isSlide;
    
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
        playerAnimation=GetComponent<PlayerAnimation>();
        character=GetComponent<Character>();

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

        // 攻击
        inputControl.GamePlay.Attack.started += PlayerAttack;
        // 滑铲
        inputControl.GamePlay.Slide.started += Slide;

    }


    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }
    private void  Update()
    {
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();

        // 检测人物是否在空中，改变其摩擦力
        CheckState();
    }

    private void CheckState()
    {
        Coll.sharedMaterial = physicsCheck.isGround ? normal : wall;

        if (physicsCheck.onWall)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
        else
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        if(wallJump&&rb.velocity.y<0f)
        {
            wallJump = false;
        }

        if (isDead || isSlide)
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        else
            gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void FixedUpdate()
    {
        // 受伤就无法移动
        if ( !isHurt && ! isAttack)
            Move();
    }



    public void Move(){ 
        if( !isCrouch && !wallJump)
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
            // spriteRenderer.flipX = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }else if(inputDirection.x>0)
        {
            // spriteRenderer.flipX = false;
            transform.localScale = new Vector3(1, 1, 1);
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
    private void Slide(InputAction.CallbackContext context)
    {
        if (!isSlide && physicsCheck.isGround && character.currentPower >= slidePowerCost)
        {
            isSlide = true;
            var targetPos = new Vector3(transform.position.x + SlideDistance * transform.localScale.x, transform.position.y);
            
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            StartCoroutine(TriggerSlide(targetPos));

            character.OnSlide(slidePowerCost);
        }
    
    }

    private IEnumerator TriggerSlide(Vector3 target)
    {
        do
        {
            yield return null;
            if (!physicsCheck.isGround)
            {
                break;
            }
            if ((physicsCheck.touchLeftWall&&transform.localScale.x<0f)||(physicsCheck.touchRightWall&&transform.lossyScale.x>0f))
            {
                isSlide=false;
                break;
            }
            Debug.Log(MathF.Abs(target.x - transform.position.x));
            rb.MovePosition(new Vector2(transform.position.x + transform.localScale.x * SlideSpeed, transform.position.y));
        } while (MathF.Abs(target.x - transform.position.x) > 0.15f);
    
        isSlide=false;
         gameObject.layer = LayerMask.NameToLayer("Player");
    }
    private void Jump(InputAction.CallbackContext context)
    {
        // 给刚体施加一个力（力的方向和力度，力的形式）
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up*jumpForce,ForceMode2D.Impulse);

            // 打断滑铲携程
            isSlide = false;
            StopAllCoroutines();
        }
        else if(physicsCheck.onWall)
        {
            rb.AddForce(new Vector2(-inputDirection.x,2f)*wallJumpForce,ForceMode2D.Impulse);
            wallJump = true;
        }
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayAttack();
        isAttack=true;
        
    }

    #region UnityEvent
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        // 人物的速度停下来
        rb.velocity=Vector2.zero;
        // 计算受伤的方向  并朝反方向
        Vector2 dir=new Vector2(transform.position.x-attacker.position.x, 0).normalized;
        // 施加一个力
        rb.AddForce(dir*hurtForce,ForceMode2D.Impulse);
    }
    public void PlayerDead()
    {
        isDead = true;
        inputControl.GamePlay.Disable();
    }
    #endregion
}
