using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Space(10)][Header("Audio")]
    [SerializeField] AudioSource bulletHitWallSound;
    [SerializeField] AudioSource bulletHitFleshSound;

    [SerializeField] AudioClip[] bulletHitWallClips;
    [SerializeField] AudioClip[] bulletHitFleshClips;

    [Space(10)][Header("Bullet Values")]
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float maxVelocity = 5f;
    [SerializeField] int bulletDamage = 10;
    public bool isFlying = false;
    private Vector2 moveDirection;
    
    [Space(10)]
    [SerializeField] GameObject bloodSplatPrefab;

    Rigidbody2D rb;
    GameManager gm;
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();    
    }

    void Update()
    {
        
        FlyOnDirection();
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.layer == 9) //enemy
        {
            HitFleshSound();
            GameObject go = Instantiate(bloodSplatPrefab, gameObject.transform.position, gameObject.transform.rotation);
            collision.gameObject.GetComponent<EnemyStats>().TakeDamage(bulletDamage * gm.bulletDamageMultiplier);
        }

        else if(collision.gameObject.layer == 11 || collision.gameObject.layer == 16)//wall
            HitWallSound();
            
              
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject,1f);
    }

    private void HitFleshSound()
    {
        int rand = Random.Range(0,6);

        bulletHitFleshSound.clip = bulletHitFleshClips[rand];
        bulletHitFleshSound.Play();
    }

    private void HitWallSound()
    {
        int rand = Random.Range(0,7);

        bulletHitWallSound.clip = bulletHitWallClips[rand];
        bulletHitWallSound.Play();
    }

    public void Shooted(Vector3 direction)
    {
        moveDirection = direction;
    }

    private void FlyOnDirection()
    {
        Vector2 force = moveDirection * bulletSpeed;
        
        rb.AddForce(force);

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }
}
