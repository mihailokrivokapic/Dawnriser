using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTutorial : MonoBehaviour
{
    public GameObject hoverText;

    [SerializeField]
    private Material glow;

    void Start()
    {
        StartCoroutine("ButtonGlowDelay");        
    }

    IEnumerator ButtonGlowDelay()
    {
        foreach(Transform child in transform)
        { 
            // Get the next child (Button)
            SpriteRenderer current = child.GetComponent<SpriteRenderer>();

            // Cache the default material
            Material defaultMaterial = current.material;

            current.material = glow;
            yield return new WaitForSeconds(1f);
            current.material = defaultMaterial;
        }

        Destroy(gameObject);
        Destroy(hoverText);
    }
}
