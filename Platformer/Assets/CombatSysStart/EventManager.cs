using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public event Action<float> _onDamageTaken;
    public event Action _onDeath;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(this);
    }

    public void OnDamageTaken(float amount)
    {
        if (_onDamageTaken != null)
        {
            _onDamageTaken(amount);
        }
    }

    public void OnDeath()
    {
        if(_onDeath != null)
        {
            _onDeath();
        }
    }
}
