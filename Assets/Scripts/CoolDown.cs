using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDown : MonoBehaviour
{
    Image coolDown;
    private bool buffTimer = false;
    private bool skillCoolDown = false;
    private float fillTime = 0f;
    private float currentTime = 0f;

    [SerializeField] bool isMovSpeedBuff = false;

    CharacterMovement charMov;

    
    void Awake()
    {
        charMov = FindObjectOfType<CharacterMovement>();
        coolDown = GetComponent<Image>();
    }

    void Update()
    {
        if(buffTimer)
        {
            currentTime += Time.deltaTime;
            coolDown.fillAmount = (currentTime / fillTime);

            if(currentTime >= fillTime)
            {
                TimerReset();
            }
        }

        if(skillCoolDown)
        {
            currentTime -= Time.deltaTime;
            coolDown.fillAmount = currentTime / fillTime;

            if(currentTime <= 0)
            {
                CoolDownReset();
            }
        }    
    }

    private void TimerReset()
    {
        buffTimer = false;
        fillTime = 0f;
        currentTime = 0f;
        gameObject.transform.parent.gameObject.SetActive(false);

        if(isMovSpeedBuff)
            charMov.speedTrail.GetComponent<ParticleSystem>().Stop();
        
    }

    private void CoolDownReset()
    {   
        skillCoolDown = false;
        fillTime = 0f;
        currentTime = 0f;
        gameObject.SetActive(false);
    }

    public void Cooldown(float amount)
    {
        fillTime += amount;
        currentTime = fillTime;
        skillCoolDown = true;
    }

    public void BuffTimer(float amount)
    {
        fillTime += amount;
        buffTimer = true;

        
        
    }
}
