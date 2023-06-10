using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject gruntPrefab;
    [SerializeField] GameObject brutePrefab;

    public void SpawnEnemies()
    {
        int rand = Random.Range(1,9);
        
        if(rand == 5)
        {
            Instantiate(brutePrefab, transform.position, Quaternion.identity);
        }

        else
        {
            Instantiate(gruntPrefab, transform.position, Quaternion.identity);
        } 
    }
}
