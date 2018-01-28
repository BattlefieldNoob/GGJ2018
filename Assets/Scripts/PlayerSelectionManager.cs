using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerSelectionManager : MonoBehaviour
{
    private Dictionary<int, PlayerReadyStatus> PlayerStatusForIndex = new Dictionary<int, PlayerReadyStatus>();

    private PlayerReadyStatus[] playersStatus;

    public Text CanStart;

    private bool gameCanStart = false;

    public int[] LastCycleJoystickIndexes;


    private void Awake()
    {
        playersStatus = GetComponentsInChildren<PlayerReadyStatus>();
    }

    private void OnEnable()
    {
        foreach (var playerStatus in playersStatus)
        {
            playerStatus.Disable();
        }

        StartCoroutine(CheckControllerAvailability());
    }

    private void OnDisable()
    {
        PlayerStatusForIndex.Clear();
        LastCycleJoystickIndexes = new int[0];
        StopAllCoroutines();
    }


    void Update()
    {
        foreach (var key in PlayerStatusForIndex.Keys)
        {
            if (!PlayerStatusForIndex[key].isReady && Input.GetButtonDown("Button" + (key + 1)))
            {
                PlayerStatusForIndex[key].SetReady();
                CheckNumberOfPlayers();
            }
        }
    }


    public void CheckNumberOfPlayers()
    {
        if (PlayerStatusForIndex.Count >= 2 && PlayerStatusForIndex.All(playerReady => playerReady.Value.isReady))
        {
            Debug.Log("Game can Start!");
            gameCanStart = true;
            StartCoroutine(WaitAndStartGame());
        }
    }


    IEnumerator CheckControllerAvailability()
    {
        List<int> joystickOriginalindexes = new List<int>();

        while (true)
        {
            //prendo la lista dei joystick connessi
            var joysticks = Input.GetJoystickNames();

            joystickOriginalindexes.Clear();
            //rimuovo dalla lista i controller senza nome e tengo i loro indici
            for (int i = 0; i < joysticks.Length; i++)
            {
                if (!string.IsNullOrEmpty(joysticks[i]))
                    joystickOriginalindexes.Add(i);
            }

            //controllo il numero di joystick rispetto al ciclo precedente
            if (joystickOriginalindexes.Count != LastCycleJoystickIndexes.Length)
            {
                //è stato aggiunto o rimosso un joystick
                if (joystickOriginalindexes.Count > LastCycleJoystickIndexes.Length)
                {
                    //è stato aggiunto un controller
                    //faccio un'operazione di intersezione tra gli indici vecchi e quelli nuovi
                    var intersection = joystickOriginalindexes.Except(LastCycleJoystickIndexes);
                    //ciclo sull'enumerable chiamando la callback n volte
                    foreach (var i in intersection)
                    {
                        Debug.Log("New Device:" + i);
                        OnDeviceConnected(i);
                    }
                }
                else
                {
                    //è stato rimosso un controller
                    //faccio un'operazione di intersezione tra gli indici vecchi e quelli nuovi
                    var intersection = LastCycleJoystickIndexes.Except(joystickOriginalindexes);
                    //ciclo sull'enumerable chiamando la callback n volte
                    foreach (var i in intersection)
                    {
                        Debug.Log("Removed Device:" + i);
                        OnDeviceDisconnected(i);
                    }
                }
            }

            LastCycleJoystickIndexes = joystickOriginalindexes.ToArray();

            yield return new WaitForSeconds(2f);
        }
    }

    public void OnDeviceConnected(int controllerIndex)
    {
        //Rendo disponibile un nuovo player sulla UI

        //se lo contiene già vuol dire che ho riconnesso un joystick
        if (PlayerStatusForIndex.ContainsKey(controllerIndex))
        {
            PlayerStatusForIndex[controllerIndex].SetNotReady();
        }
        else
        {
            //devo aggiungere un nuovo elemento
            if (PlayerStatusForIndex.Count > 3)
            {
                var key = -1;
                //ho troppi joystick, ne cerco uno che non è connesso
                foreach (var pair in PlayerStatusForIndex)
                {
                    if (!pair.Value.isEnabled)
                    {
                        //ho trovato un joystick da rimuovere
                        key = PlayerStatusForIndex.FirstOrDefault(x => x.Value == pair.Value).Key;
                    }
                }

                if (key != -1)
                {
                    PlayerStatusForIndex.Remove(key);
                }
                else
                {
                    return;
                }
            }

            playersStatus[PlayerStatusForIndex.Count].SetNotReady();
            PlayerStatusForIndex.Add(controllerIndex, playersStatus[PlayerStatusForIndex.Count]);
        }
    }

    public void OnDeviceDisconnected(int controllerIndex)
    {
        //Rimuovo un dispositivo dalla UI
        if (PlayerStatusForIndex.ContainsKey(controllerIndex))
        {
            PlayerStatusForIndex[controllerIndex].Disable();
        }
    }


    private IEnumerator WaitAndStartGame()
    {
        CanStart.text = "Game Can Start!  3";
        yield return new WaitForSeconds(1f);
        CanStart.text = "Game Can Start!  2";
        yield return new WaitForSeconds(1f);
        CanStart.text = "Game Can Start!  1";
        yield return new WaitForSeconds(1f);

        FindObjectOfType<HighLevelGameManager>().EnterGameState();

        GameManager.Instance.WaitGameplaySceneAndStartGame(LastCycleJoystickIndexes.ToArray());
    }
}