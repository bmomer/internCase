using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject character;

    [Space(5)][Header("Camera Follow")]
    [SerializeField] float timeOffset;
    [SerializeField] Vector3 posOffset;
    private Vector3 desiredPositon;

    [Space(5)][Header("Camera Map Restrictions")]
    [SerializeField] float leftLimit;
    [SerializeField] float rightLimit;
    [SerializeField] float bottomLimit;
    [SerializeField] float topLimit;

    [Space(5)][Header("Camera Boundaries")]
    [SerializeField] BoxCollider2D camBox;
    [SerializeField] float offSet = 1f;
    private Camera cam;
    private float sizeX, sizeY, ratio;


    void Start()
    {
        cam = GetComponent<Camera>();
        SetColliderSize();
    }

    private void SetColliderSize()
    {
        sizeY = cam.orthographicSize * 2;
        ratio = (float)Screen.width/(float)Screen.height;
        sizeX = sizeY * ratio;

        camBox.size = new Vector2(sizeX - (offSet * ratio), sizeY - offSet);
    }

    
    void  FixedUpdate()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = character.transform.position;

        Quaternion characterRotation = character.transform.rotation;
        Vector3 offsetPosition = characterRotation * posOffset;
        desiredPositon = character.transform.position + offsetPosition;
        desiredPositon.z = -10f;
        



        transform.position = Vector3.Lerp(transform.position, desiredPositon, timeOffset * Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), Mathf.Clamp(transform.position.y, bottomLimit, topLimit), transform.position.z);
    }
}
