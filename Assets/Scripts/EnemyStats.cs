using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class EnemyStats : MonoBehaviour
{
    [Space(10)][Header("Audio")]
    [SerializeField] AudioSource growlSound;
    [SerializeField] AudioSource deathSound;
    [SerializeField] int growlPerThis = 5;
    [SerializeField] AudioClip[] bruteGrowls;
    [SerializeField] AudioClip[] gruntGrowls;
    [SerializeField] AudioClip[] bruteDeathSounds;
    [SerializeField] AudioClip[] gruntDeathSounds;

    [Space(10)][Header("Stats")]
    [SerializeField] bool isGrunt = false;
    [SerializeField] float currentEnemyHealth = 100f;
    [SerializeField] float enemyDamage = 10f;
    [SerializeField] HealthBar healthBar;
    private bool isDead = false;
    private float enemyMaxHealth;
    [SerializeField] float knockBackForce;

    [Space(10)][Header("Drops")]
    [SerializeField] GameObject movementBuffPrefab;
    [SerializeField] GameObject barrierPrefab;
    [SerializeField] GameObject[] ores;


    [Space(10)]
    [SerializeField] GameObject floatingTextPrefab;
    public List<GameObject> floatingTexts;
    [SerializeField] GameObject bloodSplatterPrefab;
    GameManager gm;
    
    AIPath aIPath;
    Rigidbody2D rb;
    CharacterMovement chr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        aIPath = GetComponent<AIPath>(); 
        chr = FindObjectOfType<CharacterMovement>();   
    }

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        StartCoroutine(Growl(growlPerThis));

        if(isGrunt)
        {
            currentEnemyHealth = gm.gruntHealth;
            enemyMaxHealth = currentEnemyHealth;
        }
        else
        {
            currentEnemyHealth = gm.bruteHealth;        
            enemyMaxHealth = currentEnemyHealth;
        }
    }
    
    private IEnumerator Growl(int delay)
    {
        yield return new WaitForSeconds(delay);
        int rand = Random.Range(0,3);

        if(isGrunt)
        {
            growlSound.clip = gruntGrowls[rand];
        }

        else
        {
            growlSound.clip = bruteGrowls[rand];
        }

        int rand2 = Random.Range(growlPerThis, growlPerThis*2);

        growlSound.Play();
        StartCoroutine(Growl(rand2));
    }

    private float timeSinceLastFloating = 0f;
    void Update()
    {
        timeSinceLastFloating += Time.deltaTime;

        if(timeSinceLastFloating >= 0.15f && floatingTexts.Count > 0)
        {
            if(floatingTexts.Count > 0)
            {
                floatingTexts[0].SetActive(true);
                floatingTexts[0].transform.SetParent(gameObject.transform);
                floatingTexts[0].GetComponent<FloatingText>().floatingTXT.text = ((int)damageTemp).ToString();
                floatingTexts.RemoveAt(0);
            }

            timeSinceLastFloating = 0f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
         if(collision.gameObject.layer == 8)//player
        {
            collision.gameObject.GetComponent<CharacterStats>().TakeDamage(enemyDamage);
            collision.gameObject.GetComponent<CharacterMovement>().KnockBackPlayer(collision.gameObject.transform.position - transform.position, isGrunt);
        } 
    }

    private float damageTemp;
    public void TakeDamage(float damage)
    {
        KnockBack();

        currentEnemyHealth -= damage;
        damageTemp = damage;

        GameObject floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        floatingText.SetActive(false);
        floatingTexts.Add(floatingText);

        

        healthBar.UpdateHealthBar(currentEnemyHealth, enemyMaxHealth);

        if(currentEnemyHealth <= 0 && !isDead)
        {
            aIPath.enabled = true;
            
            isDead = true;
            gameObject.GetComponent<EnemyIndicator>().DisableOnDeath();
            Died();
        }
            
    }

    private void KnockBack()
    {
        aIPath.enabled = false;
        Vector3 direction = transform.position - chr.gameObject.transform.position;
        direction.Normalize();

        rb.AddForce(direction * knockBackForce, ForceMode2D.Impulse);
        StartCoroutine(KnockBackDelay());
    }

    private IEnumerator KnockBackDelay()
    {
        yield return new WaitForSeconds(0.3f);
        aIPath.enabled = true;
    }

    private void Died()
    {
        growlSound.enabled = false;
        int rand = Random.Range(0,3);
        if(isGrunt)
            deathSound.clip = gruntDeathSounds[rand];

        else
            deathSound.clip = bruteDeathSounds[rand];

        deathSound.Play();

        Instantiate(bloodSplatterPrefab, transform.position, Quaternion.identity);
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        healthBar.gameObject.SetActive(false);
        
        int rand2 = Random.Range(0,10);

        if(rand2 < 3)
            DropBuff();

        else    
            DropOre();

        Destroy(gameObject,2f);
    }

    private void DropOre()
    {
        int rand = Random.Range(0,100);
        GameObject ore = null;

        if(rand > 10)
        {
            if(rand < 50)
                ore = ores[0];

            else if(rand < 85)
                ore = ores[1];

            else if(rand < 90)
                ore = ores[2];

            else if(rand < 95)
                ore = ores[3];

            else if(rand == 99)
                ore = ores[4];                
        }

        if(ore != null)
            Instantiate(ore, transform.position, Quaternion.identity);
    }

    private void DropBuff()
    {
        int rand = Random.Range(0,100);
        GameObject buff = null;

        if(rand >= 80)
        {
            if(rand < 90)
                buff = movementBuffPrefab;

            else if(rand <= 99)
                buff = barrierPrefab;
             
        }

        if(buff != null)
            Instantiate(buff, transform.position, Quaternion.identity);
    }


}
