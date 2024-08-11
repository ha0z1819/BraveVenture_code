using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
      public override void OnEnter(Enemy enemy)
    {
        Debug.Log("chase");
        currrentEnemy = enemy;
        currrentEnemy.currentSpeed=currrentEnemy.chaseSpeed;
        currrentEnemy.anim.SetBool("run", true);

    }
    public override void LogicUpdate()
    {
        if(currrentEnemy.lostTimeCounter<=0)
        {
            currrentEnemy.switchState(NPCState.Patrol);
        }
        if (!currrentEnemy.physicsCheck.isGround||(currrentEnemy.physicsCheck.touchLeftWall&&currrentEnemy.faceDir.x<0)||(currrentEnemy.physicsCheck.touchRightWall&&currrentEnemy.faceDir.x>0))
        {
            Debug.Log("反转方向");
            currrentEnemy.transform.localScale=new Vector3(currrentEnemy.faceDir.x, 1, 1);
        }
    }

    public override void PhysicsUpdate()
    {
       
    }

    public override void OnExit()
    {
        currrentEnemy.anim.SetBool("run", false);
    }

    
}
