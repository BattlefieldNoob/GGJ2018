using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    #region Singleton

    public static EventManager Instance;


    private void Awake()
    {
        if (Instance != null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion


    public UnityEvent OnInfectWaveFinished;

    void Start()
    {
        OnInfectWaveFinished=new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
    }
}