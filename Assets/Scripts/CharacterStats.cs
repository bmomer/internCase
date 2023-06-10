using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [Space(10)][Header("Audio")]
    public AudioSource rageSound;
    public AudioSource playerHurtSound;
    public AudioSource goldPickupSound;
    public AudioClip[] hurtClips;

    [Space(10)][Header("Health")]
    [SerializeField] float characterMaxHealth;
    [SerializeField] HealthBar healthBar;
    private float currentCharacterHealth = 100;
    
    [Space(10)]
    [SerializeField] GameObject pickUpParticle;
    [SerializeField] GameObject floatingTextPrefab;

    GameManager gm;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        currentCharacterHealth = characterMaxHealth;    
    }

    public void TakeDamage(float damage)
    {
        HurtSound();
        currentCharacterHealth -= damage;

        healthBar.UpdateHealthBar(currentCharacterHealth , characterMaxHealth);

        if(currentCharacterHealth <= 0 && gm.lost == false)
        {
            gm.lost = true;
            gm.Lost();
        }
            
    }

    private void HurtSound()
    {
        int rand = Random.Range(0,3);
        playerHurtSound.clip = hurtClips[rand];
        playerHurtSound.Play();
    }

    void  OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == 13) //ore
        {
            goldPickupSound.Play();
            GameObject go = Instantiate(pickUpParticle, gameObject.transform.position , Quaternion.identity);
            
            Destroy(go,5);

            GameObject floatingText =  Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            floatingText.GetComponent<FloatingText>().floatingTXT.color = Color.yellow;
            floatingText.GetComponent<FloatingText>().floatingTXT.text = "+" + other.gameObject.GetComponent<ore>().value.ToString();
            Destroy(floatingText,2f);

            gm.goldAmount += other.gameObject.GetComponent<ore>().value;
            gm.UpdateTexts();
            Destroy(other.gameObject);
        }
    }
}
