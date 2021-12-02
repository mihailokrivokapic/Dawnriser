using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{


    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SendMessage("Damage", 10f);
    }
}