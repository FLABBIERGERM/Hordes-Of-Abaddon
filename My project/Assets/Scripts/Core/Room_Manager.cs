using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Manager : MonoBehaviour
{
    public static Room_Manager Instance { get; private set; }

    [SerializeField] private Transform[] spawnPoints;

    // where player spawns // 3 spawn points
    [SerializeField] private Transform[] courtYard;

    // left room from spawn // 2 spawn points
    [SerializeField] private Transform[] schoolRoom;

    //upstairs right side room // 1 spawn point
    [SerializeField] private Transform[] bedRoom;

    // past the bedroom or through big main doors around corner // 2 spawn points
    [SerializeField] private Transform[] lunchRoom;

    // located back right of the mess hall.
    [SerializeField] private Transform[] kitchen;

    // this is the halways that open once some doors are open// 
    [SerializeField] private Transform[] hallWays;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
