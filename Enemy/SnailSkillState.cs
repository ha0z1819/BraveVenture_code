

using UnityEngine;

public class SnailSkillState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        Debug.Log("ssss");
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("walk", false);
        currentEnemy.anim.SetBool("hide", true);
        currentEnemy.anim.SetTrigger("skill");

        currentEnemy.lostTimeCounter = currentEnemy.lostTime;

        currentEnemy.GetComponent<Character>().invulnerable = true;//设置无敌
        
        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCounter;
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter<=0)
        {
            currentEnemy.switchState(NPCState.Patrol);
        }
        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCounter;
    }

    public override void PhysicsUpdate()
    {
       
    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("hide", false);
        currentEnemy.GetComponent<Character>().invulnerable = false;
    }
}
