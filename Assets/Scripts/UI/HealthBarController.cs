using System.Collections;
using System.Collections.Generic;
using Behaviours;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public HealthSystem _healthSystem;
    private Slider _healthbarindicator;
    private float lastHealth;
    void Start()
    {
        _healthbarindicator = this.GetComponent<Slider>();
        lastHealth = _healthSystem.Health;
        _healthbarindicator.maxValue = _healthSystem.MAX_HEALTH;
        _healthbarindicator.value = _healthbarindicator.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastHealth != _healthSystem.Health)
        {
            _healthbarindicator.value = _healthSystem.Health;
        }
    }
}
