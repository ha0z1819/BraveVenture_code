using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStartBar : MonoBehaviour
{
    private Character currentCharacter;
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;
    private bool isRecovering;

    private void Update() {
        if(healthDelayImage.fillAmount>healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime/2;
        }
        if(isRecovering)
        {
            float persentage = currentCharacter.currentPower / currentCharacter.maxPower;
            powerImage.fillAmount = persentage;
            if(persentage>1)
            {
                isRecovering = false;
                return;
            }
        }
    }

    /// <summary>
    /// 接受health的变更百分比
    /// </summary>
    /// <param name="persentage">百分比current/max</param>
    public void OnHealthChange(float persentage)
    {
        healthImage.fillAmount = persentage;
    }

    public void OnPowerChange(Character character)
    {
        isRecovering = true;
        currentCharacter = character;


    }
}
