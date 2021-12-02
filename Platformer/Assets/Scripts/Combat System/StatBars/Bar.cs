using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bar : MonoBehaviour
{
    abstract public float maxValue { get; set; }
    abstract public float currentValue { get; set; }
    Color maxColor;
    Color minColor;


    public abstract void FillBar(float amount);
}
