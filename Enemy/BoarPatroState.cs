using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BaseState
{
    
    public override void OnEnter(Enemy enemy)
    {
       currrentEnemy=enemy;
       currrentEnemy.currentSpeed=currrentEnemy.normalSpeed;
    }

    public override void LogicUpdate()
    {
        // TODO:发现Player切换到chase
        if(currrentEnemy.FoundPlayer())
        {
            currrentEnemy.switchState(NPCState.Chase);  
        }

        if (!currrentEnemy.physicsCheck.isGround||(currrentEnemy.physicsCheck.touchLeftWall&&currrentEnemy.faceDir.x<0)||(currrentEnemy.physicsCheck.touchRightWall&&currrentEnemy.faceDir.x>0))
        {
            currrentEnemy.wait=true;
            currrentEnemy.anim.SetBool("walk", false); 
        }else
        {
            currrentEnemy.anim.SetBool("walk", true);
        }
    }
 

    public override void PhysicsUpdate()
    {
        
    } 
    public override void OnExit()
    {
        // currrentEnemy.lostTimeCounter = currrentEnemy.lostTime;
        currrentEnemy.anim.SetBool("walk", false);
        Debug.Log("exit");
    }
}
