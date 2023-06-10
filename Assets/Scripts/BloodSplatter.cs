using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    [SerializeField] Sprite[] bloods;

    public void Start()
    {
        int rand = Random.Range(1,4);

        gameObject.GetComponent<SpriteRenderer>().sprite = bloods[rand];
        Destroy(gameObject,10f);
    }
}
