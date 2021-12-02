using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private EnemyController enemy;

    private void Start()
    {
        enemy = GetComponent<EnemyController>();  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        enemy.CheckDetectionHitBox();
    }
}
