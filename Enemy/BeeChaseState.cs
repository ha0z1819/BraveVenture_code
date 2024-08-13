using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeChaseState : BaseState
{
    private Vector3 target;
    private Vector3 moveDir;
    public override void OnEnter(Enemy enemy)
    {
        currrentEnemy = enemy;
        currrentEnemy.currentSpeed = currrentEnemy.chaseSpeed;
        target = enemy.GetNewPoint();
    }
    public override void LogicUpdate()
    {
        if (currrentEnemy.lostTimeCounter<=0)
            currrentEnemy.switchState(NPCState.Patrol);

        target = new Vector3(currrentEnemy.attacker.position.x, currrentEnemy.attacker.position.y + 1.5f, 0);
        
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }

}
