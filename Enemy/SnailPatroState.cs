using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailPatroState : BaseState
{
     public override void OnEnter(Enemy enemy)
    {
        currrentEnemy = enemy;
        currrentEnemy.currentSpeed = currrentEnemy.normalSpeed;
    }

    public override void LogicUpdate()
    {
        if (currrentEnemy.FoundPlayer())
        {
            Debug.Log("faxian player");
            currrentEnemy.switchState(NPCState.Skill);  
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
        
    }

    
}
