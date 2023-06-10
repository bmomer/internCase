using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{   

    [Space(10)][Header("Audio")]
    public AudioSource speedUpSound;
    public AudioSource barrierSound;

    [Space(10)][Header("Control")]
    public Transform characterPosition;
    public float movementSpeed = 5f;
    public Joystick joystick;
    [SerializeField] float rotationSpeed;
    private Rigidbody2D rb;
    private float movementSpeedTemp;
    private Vector2 movement;
    private float moveHorizontal;
    private float moveVertical;
    private bool knockbackState = false;

    [Space(10)][Header("Skill Effects")][Space(5)]


    [SerializeField] CoolDown barrierCoolDown;
    [SerializeField] GameObject barrierPrefab; 
    public GameObject barrier;
    private float barrierDuration;
    private float barrierDurationTemp;
    private bool hasBarrier = false;

    [Space(10)]
    public GameObject rageAuraPrefab;
    float auraPrefabz;

    [Space(10)]
    [SerializeField] CoolDown movBuffCoolDown;
    public GameObject speedTrail;
    private bool movementBoostActive = false;
    private float movementBoostDurationTemp;
    private float movementBoostDuration;



    void Awake()
    {
        auraPrefabz = rageAuraPrefab.transform.position.z;
        speedTrail.GetComponent<ParticleSystem>().Stop();
        rageAuraPrefab.GetComponent<ParticleSystem>().Stop();

        barrierDurationTemp = barrierDuration;
        movementSpeedTemp = movementSpeed;
        characterPosition = this.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    float durationB = 0f;
    float durationM = 0f;

    void Update()
    {
        rageAuraPrefab.transform.position = new Vector3(transform.position.x, transform.position.y , auraPrefabz);
        characterPosition = this.transform;

        
        moveHorizontal = joystick.Horizontal;
        moveVertical = joystick.Vertical;
        

        movement = new Vector2(moveHorizontal,moveVertical) * movementSpeed;

        if(barrier)
        {
            durationB += Time.deltaTime;

            if(durationB >= barrierDuration)
            {
                durationB = 0f;
                barrierDuration = barrierDurationTemp;
                hasBarrier = false;
                Destroy(barrier);
            }
        }

        if(movementBoostActive)
        {
            durationM += Time.deltaTime;

            if(durationM >= movementBoostDuration)
            {
                durationM = 0f;
                movementBoostDuration = movementBoostDurationTemp;
                movementBoostActive = false;
                movementSpeed = movementSpeedTemp;
            }
        } 

        
    }

    void FixedUpdate()
    {   
        if(gameObject.GetComponent<CharacterShooting>().closestEnemy != null)
        {
            FaceTowardsEnemy();
        }

        else
        {
            FaceWhereYouMove();
        }  

        if(!knockbackState)
            rb.velocity = movement;    
    }

    public void MovementBoost(float duration, float speedBuff)
    {
        
        speedTrail.GetComponent<ParticleSystem>().Play();
           
        movBuffCoolDown.gameObject.transform.parent.gameObject.SetActive(true);
        movBuffCoolDown.BuffTimer(duration);

        if(!movementBoostActive)
        {
            movementBoostActive = true;
            movementSpeed += speedBuff;
            movementBoostDuration += duration; 
        }

        else
        {
            movementBoostDuration += duration;
        }

        Debug.Log(movementBoostDuration);
    }

    
    public void Barrier(float duration)
    {
        barrierSound.Play();
        barrierCoolDown.gameObject.transform.parent.gameObject.SetActive(true);
        barrierCoolDown.BuffTimer(duration);

        if(!hasBarrier)
        {
            barrierDuration += duration;
            barrier = Instantiate(barrierPrefab , transform.position, Quaternion.identity);
            barrier.GetComponent<Barrier>().SetTarget(transform);
            hasBarrier = true;
        }

        else
        {
            barrierDuration += duration;
        }
    }   

    
    public void KnockBackPlayer(Vector2 direction,bool isGrunt)
    {
        direction.Normalize();
        float knockBackStrength;

        if(isGrunt)
            knockBackStrength = 10f;

        else
            knockBackStrength = 15f;

        knockbackState = true;
        StartCoroutine(NormalState());                
        rb.AddForce(direction * knockBackStrength ,ForceMode2D.Impulse);
    }

    private IEnumerator NormalState()
    {
        yield return new WaitForSeconds(0.1f);
        knockbackState = false;
    }
    
    Quaternion desiredRotation;
    private void FaceTowardsEnemy()
    {
        Vector3 direction = gameObject.GetComponent<CharacterShooting>().closestEnemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        desiredRotation =  Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation ,rotationSpeed * Time.deltaTime);
    }

    private void FaceWhereYouMove()
    {
        if(moveHorizontal != 0 || moveVertical != 0) 
        {
            float moveAngle = Mathf.Atan2(moveVertical, moveHorizontal) * Mathf.Rad2Deg;
            desiredRotation = Quaternion.AngleAxis(moveAngle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation ,8f * Time.deltaTime);
            
        }
    }

    
}
