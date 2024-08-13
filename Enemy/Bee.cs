
using UnityEngine;
using UnityEngine.Rendering;

public class Bee : Enemy
{
    [Header("移动范围")]
    public float patroRadius;

    protected override void Awake() {
        base.Awake();
        patrolState=new BeePatrolState();
        chaseState=new BeeChaseState();
    }
    public override bool FoundPlayer()
    {
       var obj= Physics2D.OverlapCircle(transform.position, checkDistance, attaclLayer);
        if(obj)
        {
            attacker = obj.transform;
        }
        return obj;
    }
    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spwanPoint, patroRadius);
    }
    public override Vector3 GetNewPoint()
    {
        var targetX=Random.Range(-patroRadius, patroRadius);
        var targetY = Random.Range(-patroRadius, patroRadius);

        return spwanPoint + new Vector3(targetX, targetY);
    }
    public override void Move()
    {
       
    }
}
