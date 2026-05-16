using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Manager : MonoBehaviour
{
    public static Room_Manager Instance { get; private set; }

    [SerializeField] private List<Room> rooms = new List<Room>();   

    public enum RoomState
    {
        Locked,
        Unlocked
    }
    [System.Serializable]
    public class Room
    {
        public string roomName;
        public Transform[] spawnPoints;
        public bool isUnlocked;
    }
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

    public List<Transform> GetActiveSpawnPoints()
    {
        List<Transform> spawnPoints = new();

        //List<Transform> spawnPoints = new List<Transform>();

        foreach(var room in rooms)
        {
            if (!room.isUnlocked) continue;
            spawnPoints.AddRange(room.spawnPoints);
        }
        return spawnPoints;
    }

    public void Unlockroom(string roomName)
    {
        foreach(var room in rooms)
        {
            if(room.roomName == roomName)
            {
                room.isUnlocked = true;
                break;
            }
        }
    }

}
