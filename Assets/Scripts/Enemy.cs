using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    CharacterMovement character;
    public float speed = 5f; // düşman hareket hızı
    private Transform characterPos; // karakterin konumu
    Rigidbody2D rb;
    AIDestinationSetter aIDestinationSetter;
    AIPath aIPath;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        aIPath = GetComponent<AIPath>();
    }


    void Start()
    {
        aIPath.maxSpeed = speed;
        character = FindObjectOfType<CharacterMovement>();
        characterPos = character.characterPosition; //update methodundan gerçek zamanlı olarak karakterin pozisyonunu çekmek için
        aIDestinationSetter.target = characterPos;
    }

    void FixedUpdate()
    {
        //Move();
    }

    private void Move()
    {
        //karakterin pozisyonunun bulunması
        Vector2 direction = characterPos.position - transform.position;
        direction.Normalize(); // karakter hız ayarını sadece speed variable ile yapmak için bu vektörü normalize ettik (max magnitude 1)

        //karaktere doğru hareket
        transform.Translate(direction * speed * Time.deltaTime); 
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.layer == 11)//wall
            gameObject.GetComponent<Collider2D>().isTrigger = false;
            
           
    }

    public void EnemyKnockback(Vector3 force)
    {
        aIPath.enabled = false;
        StartCoroutine(KnockBackEndDelay());
        rb.AddForce(-force,ForceMode2D.Impulse);
    }

    private IEnumerator KnockBackEndDelay()
    {
        yield return new WaitForSeconds(0.5f);
        aIPath.enabled = true;
    }

    public void SlowDownTheEnemy()
    {
        StartCoroutine(EnemySlowDuration());
    }
    
    private IEnumerator EnemySlowDuration()
    {
        float enemySpeedTemp = speed;
        aIPath.maxSpeed = speed *= 0.2f;

        
        yield return new WaitForSeconds(5f);

        aIPath.maxSpeed = speed = enemySpeedTemp;
    }
}








