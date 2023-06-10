using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
    public GameObject indicator;
    public Transform target;
    private bool enemyAlive = true;

    Renderer rd;
    void Start()
    {
        rd = GetComponent<Renderer>();
        target = FindObjectOfType<CharacterMovement>().transform;
    }

    public void DisableOnDeath()
    {
        enemyAlive = false;
    }

    void Update()
    {
        //Debug.Log(rd.isVisible);
        FindCharacterDirection();    
    }
    
    private void FindCharacterDirection()
    {
        if(enemyAlive)
        {
            if(rd.isVisible == false)
            {
                if(indicator.activeSelf == false)
                    indicator.SetActive(true);

                Vector2 direction = target.position - transform.position;
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 50f, (1 << 15));

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation =  Quaternion.AngleAxis(angle - 45f, Vector3.forward);
                indicator.transform.rotation = rotation;

                

                if(hits.Length > 0 && hits[0].collider != null)
                {
                    if(hits[0].collider.gameObject.layer == 15)
                        indicator.transform.position = hits[0].point;
                }    
            }

            else
            {
                if(indicator.activeSelf == true)
                    indicator.SetActive(false);
            } 
        }  
    }
}
