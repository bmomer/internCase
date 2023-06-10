using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    [Space(10)]
    [Header("Audio")]

    public AudioSource musicSound;
    [SerializeField] AudioSource upgradeSound;

    [Space(10)]
    [Header("Spawn Adjustment")]

    [SerializeField] GameObject[] enemySpawners;
    [SerializeField] float enemyHealthGainMultiplier;
    [SerializeField] int enemySpawnAmount = 10;
    [SerializeField] float enemySpawnPerThis = 2f;
    [SerializeField] int addToEnemySpawnAmount;
    [SerializeField] float waveInterval;

    [Space(10)]
    [Header("UI")]

    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject lostMenu;
    [SerializeField] GameObject frontBulletButton;
    [SerializeField] GameObject diagonalBulletButton;
    [SerializeField] GameObject fireSpeedButton;
    [SerializeField] GameObject damageButton;
    [SerializeField] GameObject floatingTextPrefab;
    [SerializeField] Text frontBulletUpgradeValueText;
    [SerializeField] Text diagonalBulletUpgradeValueText;
    [SerializeField] Text increaseDamageUpgradeValueText;
    [SerializeField] Text frequencyUpgradeValueText;
    [SerializeField] Text rageSkillUseValueText;
    [SerializeField] Text areaDamageSkillUseValueText;


    
    [Space(10)]
    [Header("Game")]

    public GameObject character;
    public bool lost = false;
    private bool won = false;
    public int goldAmount = 0;
    [SerializeField] float gameTimer = 300f;
    [SerializeField] Text goldAmountText;
    [SerializeField] Text gameTimerText;
    private int goldAmountDisplay = 0;
    private float minutes;
    private float seconds;

    
    [Space(10)]
    [Header("Skills and Buffs")]



    [Space(5)][Header("Rage Skill")]
    [SerializeField] int rageSkillUseValue;
    [SerializeField] float rageCD;
    [SerializeField] CoolDown rageCooldown;
    private bool raging = false;

    [Space(5)][Header("Explosive Skill")]
    [SerializeField] int areaDamageSkillUseValue;
    [SerializeField] float areaDamage = 150f; 
    [SerializeField] float areaDamageCD;
    [SerializeField] float areaDamageRadius = 2f;
    [SerializeField] CoolDown areaDamageCooldown;
    [SerializeField] GameObject explosionPrefab;
    private bool canExplode = true;

    [Space(5)][Header("Passive Skills Adjusment and Costs")]
    [SerializeField] int frontBulletUpgradeValue;
    [SerializeField] int diagonalBulletUpgradeValue;
    [SerializeField] int increaseDamageUpgradeValue;
    [SerializeField] int frequencyUpgradeValue;
    public float shotFrequency = 2f;
    public float bulletDamageMultiplier = 1f;
    public int bulletAmount = 1;
    public bool isPurchased = false;
    private int shotFrequencyUpgradeCount;
    private int increaseDamageUpgradeCount; 
    private bool canUpgradeFrequence = true;

    [Space(10)][Header("Enemy Start Healths")]

    public float gruntHealth;
    public float bruteHealth;



    
    void Start()
    {
        StartCoroutine(LoopWave());
        UpdateTexts();
    }

    void Update()
    {
        GameTimerCountDown();
        UpdateGoldSmoothly();
    }
    
    public void Win()
    {
        won = true;
        winMenu.SetActive(true);
        
        Time.timeScale = 0f;

    }

    public void Lost()
    {
        lostMenu.SetActive(true);
        Time.timeScale = 0f;   
    }

    private void UpgradeSound()
    {
        upgradeSound.Play();
    }

    private void GameTimerCountDown()
    {
        gameTimer -= Time.deltaTime;

        minutes = Mathf.FloorToInt(gameTimer / 60);
        seconds = Mathf.FloorToInt(gameTimer % 60);

        

        if(seconds >= 10 && seconds >= 0)
            gameTimerText.text = "0"+ minutes + " : " + seconds;

        else if(minutes >= 0)
            gameTimerText.text = "0"+ minutes + " : 0" + seconds;     
            
        if(gameTimer <= 0 && !won)
        {
            Win();
            gameTimerText.color = Color.green;
        
        }
        

    }

    private IEnumerator LoopWave()
    {
        for(int i = 0 ; i < enemySpawnAmount ; i++)
        {
            int index = Random.Range(0,enemySpawners.Length);
 
            enemySpawners[index].GetComponent<EnemySpawner>().SpawnEnemies();

            yield return new WaitForSeconds(enemySpawnPerThis);
        }

        StartCoroutine(WaveInterval());
    }

    
    private IEnumerator WaveInterval()
    {
        Debug.Log("interval");
        UpgradeEnemyStats();

        yield return new WaitForSeconds(waveInterval);

        StartCoroutine(LoopWave());
    }

    public void UpdateTexts()
    {   
        if(bulletAmount == 4)
            frontBulletUpgradeValueText.text = "Max";
        else    
            frontBulletUpgradeValueText.text = frontBulletUpgradeValue.ToString();


        if(isPurchased)
            diagonalBulletUpgradeValueText.text = "Max";

        else        
            diagonalBulletUpgradeValueText.text = diagonalBulletUpgradeValue.ToString();


        if(increaseDamageUpgradeCount == 42)
            increaseDamageUpgradeValueText.text = "Max";
        else
            increaseDamageUpgradeValueText.text = increaseDamageUpgradeValue.ToString();

        if(shotFrequencyUpgradeCount == 11)
            frequencyUpgradeValueText.text = "Max";

        else                    
            frequencyUpgradeValueText.text = frequencyUpgradeValue.ToString();

        rageSkillUseValueText.text = rageSkillUseValue.ToString();
        areaDamageSkillUseValueText.text = areaDamageSkillUseValue.ToString();
    }

    private void UpdateGoldSmoothly()
    {
        if(goldAmountDisplay <= goldAmount)
        {
            if(goldAmountDisplay + 50 <= goldAmount)
                goldAmountDisplay +=  50;    
            
            else if (goldAmountDisplay + 25 <= goldAmount)
                goldAmountDisplay += 25;

            else if (goldAmountDisplay + 2 <= goldAmount)
                goldAmountDisplay += 2;    
            
            else if (goldAmountDisplay + 1 <= goldAmount)
                goldAmountDisplay += 1;

        }

        else
        {
            if(goldAmountDisplay - 50 >= goldAmount)
                goldAmountDisplay -=  50;    
            
            else if (goldAmountDisplay - 25 >= goldAmount)
                goldAmountDisplay -= 25;

            else if (goldAmountDisplay - 2 >= goldAmount)
                goldAmountDisplay -= 2;   
            
            else if (goldAmountDisplay - 1 >= goldAmount)
                goldAmountDisplay -= 1;

        }

            
        goldAmountText.text = goldAmountDisplay.ToString();
    }

    private void UpgradeEnemyStats()
    {
        enemySpawnPerThis -= 0.2f;
        if(enemySpawnPerThis <= 0.5f)
            enemySpawnPerThis = 0.5f;

        enemySpawnAmount += addToEnemySpawnAmount;
        gruntHealth *= enemyHealthGainMultiplier;
        bruteHealth *= enemyHealthGainMultiplier;
    }

    private bool IsPurchasable(int value ,int upgradeCost)
    {
        if(value >= upgradeCost)
        {
            goldAmount = value - upgradeCost;
            return true;
        }

        else
            return false;
    }

    private void UpgradeInformer(string str, GameObject buttonGO)
    {
        GameObject go = Instantiate(floatingTextPrefab, buttonGO.transform.position, Quaternion.identity);
        go.transform.SetParent(transform.GetChild(0));
        go.GetComponent<FloatingText>().floatingTXT.text = str;
        go.GetComponent<Animator>().speed = 0.5f;

        Destroy(go,3f);
    }

    public void AddBulletToFront()
    {
        
        if(bulletAmount < 4)
        {
            if(IsPurchasable(goldAmount, frontBulletUpgradeValue))
            {

            UpgradeSound();
            UpgradeInformer("+Front Bullet" ,frontBulletButton);

            character.GetComponent<CharacterShooting>().gunShotSound.pitch -= 0.2f;
                
                frontBulletUpgradeValue += frontBulletUpgradeValue;
                

                for(int i = 0 ; i <= bulletAmount ; i++)
                {
                    if(character.GetComponent<CharacterShooting>().bulletPoints[i].activeSelf == false)
                        character.GetComponent<CharacterShooting>().bulletPoints[i].SetActive(true);  
                }

                if(isPurchased == true)
                {
                    for(int i = 0 ; i <= bulletAmount ; i++)
                    {
                        if(character.GetComponent<CharacterShooting>().bulletPoints[i+4].activeSelf == false)
                                character.GetComponent<CharacterShooting>().bulletPoints[i+4].SetActive(true);

                        if(character.GetComponent<CharacterShooting>().bulletPoints[i + 8].activeSelf == false)
                                character.GetComponent<CharacterShooting>().bulletPoints[i + 8].SetActive(true);        
                    }
                }

                bulletAmount++;
                UpdateTexts();
            } 
        }  
    }

    public void AddBulletDiagonal()
    {
        if(isPurchased == false)
        {
            if(IsPurchasable(goldAmount, diagonalBulletUpgradeValue))
            {
                UpgradeSound();
                UpgradeInformer("+Diagonal Bullet" ,diagonalBulletButton);

                character.GetComponent<CharacterShooting>().gunShotSound.pitch -= 0.2f;
                for(int i = 0 ; i < bulletAmount ; i++)
                {
                    if(character.GetComponent<CharacterShooting>().bulletPoints[i+4].activeSelf == false)
                            character.GetComponent<CharacterShooting>().bulletPoints[i+4].SetActive(true);

                    if(character.GetComponent<CharacterShooting>().bulletPoints[i + 8].activeSelf == false)
                            character.GetComponent<CharacterShooting>().bulletPoints[i + 8].SetActive(true);        
                }

            isPurchased = true; 
            }

            UpdateTexts(); 
        }
            
    }

    public void ShotFrequency()
    {
        if(shotFrequencyUpgradeCount < 11 && canUpgradeFrequence)
        {
            if(IsPurchasable(goldAmount, frequencyUpgradeValue))
            {
                UpgradeSound();
                UpgradeInformer("+Attack Speed" ,fireSpeedButton);

                frequencyUpgradeValue = frequencyUpgradeValue * 10 / 9;
                

                shotFrequency -= 0.1f;
                shotFrequencyUpgradeCount++;
                UpdateTexts();
            }
        }
    }

    public void IncreaseDamage()
    {
        if(increaseDamageUpgradeCount < 42)
        {
            if(IsPurchasable(goldAmount, increaseDamageUpgradeValue))
            {
                UpgradeSound();
                UpgradeInformer("+Damage" ,damageButton);

                increaseDamageUpgradeValue = increaseDamageUpgradeValue * 10 / 9; 
                
                bulletDamageMultiplier += 0.1f;
                increaseDamageUpgradeCount++;
                UpdateTexts();
            }
        }
    }
    
    public void Rage()
    {
        if(raging == false)
        {
            canUpgradeFrequence = false;
            if(IsPurchasable(goldAmount, rageSkillUseValue))
            {
                character.GetComponent<CharacterStats>().rageSound.Play();
                character.GetComponent<CharacterMovement>().rageAuraPrefab.GetComponent<ParticleSystem>().Play();
                raging = true;
                
                rageCooldown.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;
                rageCooldown.gameObject.SetActive(true);
                rageCooldown.Cooldown(rageCD);

                UpdateTexts();
                StartCoroutine(RageDuration());
                StartCoroutine(RageCoolDown());
            }
        }
    }

    private IEnumerator RageDuration()
    {
        float shotFrequencyTemp = shotFrequency;
        shotFrequency = 0.1f;

        yield return new WaitForSeconds(5f);

        canUpgradeFrequence = true;
        character.GetComponent<CharacterMovement>().rageAuraPrefab.GetComponent<ParticleSystem>().Stop();
        shotFrequency = shotFrequencyTemp;
    }

    private IEnumerator RageCoolDown()
    {
        yield return new WaitForSeconds(rageCD);
        raging = false;
        rageCooldown.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = true;
    }


    public void AreaDamage()
    {
        if(canExplode)
        {
            if(IsPurchasable(goldAmount, areaDamageSkillUseValue))
            {
                GameObject go = Instantiate(explosionPrefab, character.transform.position , Quaternion.identity);
                Destroy(go, 8f);

                canExplode = false;

                areaDamageCooldown.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;
                areaDamageCooldown.gameObject.SetActive(true);
                areaDamageCooldown.Cooldown(areaDamageCD);

                UpdateTexts();
                Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(character.transform.position, areaDamageRadius, (1 << 9));//enemies

                foreach(Collider2D collider in enemyColliders)
                {
                    collider.GetComponent<Enemy>().SlowDownTheEnemy();
                    collider.GetComponent<EnemyStats>().TakeDamage(areaDamage);
                }
            }

            StartCoroutine(AreaDamageCoolDown());
        }
    }

    private IEnumerator AreaDamageCoolDown()
    {
        yield return new WaitForSeconds(areaDamageCD);
        canExplode = true;
        areaDamageCooldown.gameObject.transform.parent.gameObject.GetComponent<Button>().interactable = true;
    }

}
