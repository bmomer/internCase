using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
    [Space(10)][Header("Audio")]
    public AudioSource gunShotSound;
    public AudioSource reload;

    [Space(10)][Header("Shooting")]
    [SerializeField] float enemyDetectionRange; 
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] GameObject muzzleFlashParticle;
    public GameObject[] bulletPoints;
    public GameObject closestEnemy = null; 
    private float timeSinceLastShoot = 0f;  
    private Vector2 nearestEnemyDirection = new Vector2(1f,0f); 
    
    [Space(10)]
    public GameManager gm;

    void Awake()
    {
        if(bulletPrefab == null)
            Debug.Log("wtf");
    }
    void Start()
    {
        reload.Play();
    }
    
    void Update()
    {
        DetectClosestEnemy(); 
        timeSinceLastShoot += Time.deltaTime;

        if(timeSinceLastShoot >= gm.shotFrequency)
        {
            Shoot();
            timeSinceLastShoot = 0f;
        } 
    }

    private void Shoot()
    {
        gunShotSound.Play();
        GameObject go = Instantiate(muzzleFlashParticle, bulletPoints[0].transform.position,gameObject.transform.rotation);
        Destroy(go,0.15f);
        
        for(int i = 0 ; i < 4 ; i++)
        {
            if(bulletPoints[i].activeSelf == true) 
            {

                Vector3 direction = gameObject.transform.right;

                Bullet bullet = Instantiate(bulletPrefab, bulletPoints[i].transform.position, gameObject.transform.rotation);
                bullet.Shooted(direction);
            }
        }

        //left diagonal
        for(int i = 4 ; i < 8 ; i++)
        {
            
            if(bulletPoints[i].activeSelf == true)
            {

                Vector3 direction = Quaternion.Euler(0f, 0f, 30f) * gameObject.transform.right;  
                Bullet bullet = Instantiate(bulletPrefab, bulletPoints[i].transform.position, gameObject.transform.rotation * Quaternion.Euler(0f, 0f, 30f));
                

                bullet.Shooted(direction);
            }
        }

        //right diagonal
        for(int i = 8 ; i < 12 ; i++)
        {
            if(bulletPoints[i].activeSelf == true)
            {

                Vector3 direction = Quaternion.Euler(0f, 0f, -30f) * gameObject.transform.right;
                Bullet bullet = Instantiate(bulletPrefab, bulletPoints[i].transform.position, gameObject.transform.rotation * Quaternion.Euler(0f, 0f, -30f));

                bullet.Shooted(direction);
            }
        }

        
        
        
        


    }

    private void DetectClosestEnemy()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position, enemyDetectionRange, (1 << 9)); 

        float closestDistance = Mathf.Infinity;
        
        if(enemyColliders.Length == 0)
        {
            closestEnemy = null;
            closestDistance = Mathf.Infinity;
        }


        foreach(Collider2D enemy in enemyColliders)
        {
            
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.gameObject;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyDetectionRange); 
    }
    

}
