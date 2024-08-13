

using UnityEngine;

public class SnailSkillState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        Debug.Log("ssss");
        currrentEnemy = enemy;
        currrentEnemy.currentSpeed = currrentEnemy.chaseSpeed;
        currrentEnemy.anim.SetBool("walk", false);
        currrentEnemy.anim.SetBool("hide", true);
        currrentEnemy.anim.SetTrigger("skill");

        currrentEnemy.lostTimeCounter = currrentEnemy.lostTime;

        currrentEnemy.GetComponent<Character>().invulnerable = true;//设置无敌
        
        currrentEnemy.GetComponent<Character>().invulnerableCounter = currrentEnemy.lostTimeCounter;
    }
    public override void LogicUpdate()
    {
        if (currrentEnemy.lostTimeCounter<=0)
        {
            currrentEnemy.switchState(NPCState.Patrol);
        }
        currrentEnemy.GetComponent<Character>().invulnerableCounter = currrentEnemy.lostTimeCounter;
    }

    public override void PhysicsUpdate()
    {
       
    }
    public override void OnExit()
    {
        currrentEnemy.anim.SetBool("hide", false);
        currrentEnemy.GetComponent<Character>().invulnerable = false;
    }
}
