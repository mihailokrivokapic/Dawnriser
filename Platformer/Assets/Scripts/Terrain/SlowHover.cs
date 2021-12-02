using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowHover : MonoBehaviour
{
    // Variables
    [Header("Parameters")]
    [Space]
    public float floatDistance = 1f;
    public float floatSpeed = 0.1f;
    public float currentDuration;

    private Vector3 pos;

    private void Start()
    {
        Vector3 pos = transform.position;
    }

    private void Update()
    {
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatDistance + pos.y;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        
    }

}
