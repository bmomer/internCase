using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ore : MonoBehaviour
{
    
    public int value;
    private GameObject character;
    private bool move = false;
    [SerializeField] float pickupSpeed = 3f;
    
    void Update()
    {
        if(move)
        {
            Vector2 direction = character.transform.position - transform.position;
            direction.Normalize(); 

            transform.Translate(direction * pickupSpeed * Time.deltaTime);
            pickupSpeed += Time.deltaTime * 2;    
        }
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 8)// character
        {
            AutoPickup(collider.gameObject);
        }
    }

    private void AutoPickup(GameObject go)
    {
        character = go;
        move = true;
    }
}
