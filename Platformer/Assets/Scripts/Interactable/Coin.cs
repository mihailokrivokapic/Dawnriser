using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Interactable
{
    private int value = 2;

    // Segment: 1
    [SerializeField]
    private ParticleSystem pSystem;
    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        pSystem = GetComponentInChildren<ParticleSystem>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public override void Interact()
    {
        pSystem.Play(true);
        sr.enabled = false;
        col.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        Interact();
        col.GetComponent<CharacterStats>().coins += value;
        Destroy(gameObject, 1f);
    }
}
