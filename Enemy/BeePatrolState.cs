using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePatrolState : BaseState
{
    private Vector3 target;
    private Vector3 moveDir;
    public override void OnEnter(Enemy enemy)
    {
        currrentEnemy = enemy;
        currrentEnemy.currentSpeed = currrentEnemy.normalSpeed;
        target = enemy.GetNewPoint();
    }
    public override void LogicUpdate()
    {
        if (currrentEnemy.FoundPlayer())
            currrentEnemy.switchState(NPCState.Chase);
        
        if (Mathf.Abs(target.x-currrentEnemy.transform.position.x) < 0.1f&&Mathf.Abs(target.y - currrentEnemy.transform.position.y) < 0.1f)
        {
            currrentEnemy.wait = true;
            target = currrentEnemy.GetNewPoint();
        }

        moveDir=(target-currrentEnemy.transform.position).normalized;

        if (moveDir.x>0)
            currrentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        if (moveDir.x<0)
            currrentEnemy.transform.localScale = new Vector3(1, 1, 1);

        

    }


    public override void PhysicsUpdate()
    {
        if ( ! currrentEnemy.wait && ! currrentEnemy.isHurt && ! currrentEnemy.isDead)
        {
           
            currrentEnemy.rb.velocity=moveDir*currrentEnemy.currentSpeed*Time.deltaTime;
        }else
        {
            currrentEnemy.rb.velocity= Vector3.zero;
        }
    }
    public override void OnExit()
    {
    }

}
