using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class RoomEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _entryNumber;
    [SerializeField] private TextMeshProUGUI _roomName;
    [SerializeField] private TextMeshProUGUI _joinedPlayers;

    public void Initialize(int entryNumber, string roomName, int joinedPlayers)
    {
        _entryNumber.text = "#" + entryNumber;
        _roomName.text = roomName;
        _joinedPlayers.text = "Joined " + joinedPlayers + "/4";
    }
}