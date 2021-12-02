using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class StatManager : MonoBehaviour
{
    // Used to get all the stats out of the player and update them
    [SerializeField]
    private CharacterStats player;

    // Strings
    [Header("Text parameters")]
    public string coinText = "";
    public string armorText = "";
    public string attackDamageText = "";
    public string abilityPowerText = "";

    // References to the UI
    public TextMeshProUGUI coins;
    public TextMeshProUGUI armorTextGUI;
    public TextMeshProUGUI attackDamageGUI;
    public TextMeshProUGUI abilityPowerGUI;

    void Start()
    {
        coins = GetComponent<TextMeshProUGUI>();
        // NOTE: Add textmesh for the rest of the stats
        // NOTE: FIX THE CODE
    }

    void Update()
    {
        // coins.text = player.armor.baseValue.ToString();
        coins.text = coinText + player.coins.ToString();
        armorTextGUI.text = armorText + player.armorText.ToString();
        attackDamageGUI.text = attackDamageText + player.attackDamageText.ToString();
        abilityPowerGUI.text = abilityPowerText + player.abilityPowerText.ToString();
    }

    // Have a callback method which updates the UI on some change
}
