﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    
    
    public class PlayerInfectedEvent:UnityEvent<int>{}
    
    
    #region Singleton

    public static EventManager Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            OnInfectWaveFinished=new UnityEvent();
            OnLastPlayerInfectedPerMatch=new PlayerInfectedEvent();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion


    public UnityEvent OnInfectWaveFinished;

    public PlayerInfectedEvent OnLastPlayerInfectedPerMatch;

    // Update is called once per frame
    void Update()
    {
    }
}