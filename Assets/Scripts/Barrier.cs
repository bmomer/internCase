using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] AudioSource barrierBumpSound;
    [SerializeField] AudioClip[] barrierBumpClips;
    [SerializeField] float knockBackForce;
    private bool move = false;
    private Transform target;

    void Update()
    {
        if(move)
        {
            transform.position = target.position;
        }    
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
        move = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 9) //enemy
        {
            BumpSound();
            Vector3 direction = transform.position - collision.gameObject.transform.position;
            direction.Normalize();

            Vector3 force = direction * knockBackForce;
            Debug.Log(force);
            collision.gameObject.GetComponent<Enemy>().EnemyKnockback(force);
        }
    }

    private void BumpSound()
    {
        int rand = Random.Range(0,2);
        barrierBumpSound.clip = barrierBumpClips[rand];
        barrierBumpSound.Play();
    }
}
