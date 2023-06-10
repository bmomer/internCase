using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{   
    private Camera cam;
    [SerializeField] Transform target;
    [SerializeField] Slider slider;
    [SerializeField] Vector3 offset;

    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth / maxHealth;
    }

    void Update()
    {
        transform.rotation = cam.transform.rotation;
        transform.position = target.position + offset;
    }
}
