using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : Bar
{
    public override float maxValue { get; set; } = 100;
    public override float currentValue { get; set; } = 0;

    private Color yellow = new Color(203, 218, 3);
    private Material current;

    // The methods below should be seriously considered to change.
    // This definetely isn't the most eficcent way of adding the shader to an object.
    // Swapping material seems like a difficult process. I should look into how to apply shaders to objects over scripts.
    // For now, this will work, as it's not causing any performance issues.
    [SerializeField]
    private Image image;
    [SerializeField]
    private Material flash;

    // Chacter stats values
    [SerializeField]
    private CharacterStats player;

    [SerializeField]
    private Slider fill;

    private void Start()
    {
        EventManager.instance._onDamageTaken += UpdateBar;
        fill.maxValue = maxValue;
        FillBar(maxValue);
        current = image.material;
    }

    public override void FillBar(float amount)
    {
        currentValue = amount;
        fill.value = currentValue;
    }
    
    public void UpdateBar(float amount)
    {
        fill.value = player.currentHealth;
        StartCoroutine("OnDamageTakenFlash");
    }
    
    IEnumerator OnDamageTakenFlash()
    {
        image.material = flash;
        yield return new WaitForSeconds(0.1f);
        image.material = current;
        
    }

    private void OnDisable()
    {
        EventManager.instance._onDamageTaken -= UpdateBar;
    }

}
