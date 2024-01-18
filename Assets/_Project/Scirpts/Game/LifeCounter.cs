using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lifeCounterText;

    [SerializeField] private int currentLifeAmount = 3;

    private void Awake()
    {
        UpdateLifeCounterText();
    }

    public void DecrementLife()
    {
        currentLifeAmount--;
        
        UpdateLifeCounterText();
    }

    private void UpdateLifeCounterText()
    {
        lifeCounterText.text = currentLifeAmount.ToString();
    }

    public bool IsLifeEnded()
    {
        return currentLifeAmount == 0;
    }
}
