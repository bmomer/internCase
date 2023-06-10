using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [SerializeField] bool movementBoost;
    [SerializeField] float movementBoostAmount;
    [SerializeField] float movementBoostDuration;
    [SerializeField] bool barrier;
    [SerializeField] float barrierDuration;

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == 8) // player
        {
            
            if(movementBoost)
            {
                CharacterMovement charMove = other.gameObject.GetComponent<CharacterMovement>();
                charMove.MovementBoost(movementBoostDuration, movementBoostAmount);
                charMove.speedUpSound.Play();
            }
                
            

            else if(barrier)
                other.gameObject.GetComponent<CharacterMovement>().Barrier(barrierDuration);

            Destroy(gameObject);
        } 
    }
}
