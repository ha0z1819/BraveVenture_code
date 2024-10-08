using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeChaseState : BaseState
{
    private Attack attack;
    private Vector3 target;
    private Vector3 moveDir;
    private bool isAttack;//攻击频率计数
    private float attackRateCounter;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.anim.SetBool("chase", true);
        target = enemy.GetNewPoint();
        attack=enemy.GetComponent<Attack>();

    }
    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
            currentEnemy.switchState(NPCState.Patrol);

        //计时器
        attackRateCounter -= Time.deltaTime;

        target = new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y + 1.6f, 0);

        //判断攻击距离
        if (Mathf.Abs(target.x - currentEnemy.transform.position.x) <= attack.attackRange && Mathf.Abs(target.y - currentEnemy.transform.position.y) <= attack.attackRange)
        {
            //攻击
            isAttack = true;
            if (!currentEnemy.isHurt)
                currentEnemy.rb.velocity = Vector2.zero;

            if (attackRateCounter <= 0)
            {
                currentEnemy.anim.SetTrigger("attack");
                attackRateCounter = attack.attackRate;
            }
        }
        else    //超出攻击范围
        {
            isAttack = false;
        }

        moveDir=(target-currentEnemy.transform.position).normalized;

        if (moveDir.x>0)
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        if (moveDir.x<0)
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);

    }

    public override void PhysicsUpdate()
    {
        if (! currentEnemy.isHurt && ! currentEnemy.isDead && !isAttack)
        {
           
            currentEnemy.rb.velocity=moveDir*currentEnemy.currentSpeed*Time.deltaTime;
        }
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("chase", false);
    }

}
