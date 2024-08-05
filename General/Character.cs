using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;

    [Header("受伤无敌")]
    // 无敌持续时间
    public float invulnerableDuration;
    // 无敌时间计时
    private float invulnerableCounter;
    public bool invulnerable;

    public UnityEvent<Transform> OnTakeDamage;







    // 方法函数
    private void Start(){
        currentHealth=maxHealth;
    }
    private void Update(){
        // 判断无敌时间
        if(invulnerable){
            invulnerableCounter -= Time.deltaTime;
            if(invulnerableCounter <= 0){
                invulnerable = false;
            }
        }
    }
    /// <summary>伤害触发</summary>
    public void TakeDamage(Attack attacker){
        if (invulnerable)
            return;
        if (currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            // 受伤触发无敌
            TriggerInvulnerable();
            // 播放受伤动作
            OnTakeDamage?.Invoke(attacker.transform);
        }else
        {
            currentHealth = 100;
            // 触发死亡
        }
    }
     
     /// <summary>触发受伤无敌</summary>
    private void TriggerInvulnerable(){
        if(!invulnerable){
            invulnerable=true;
            invulnerableCounter=invulnerableDuration;
        }
    }
}
