using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;
    public enum RoundState { RoundBegin, RoundPlaying, RoundEnd}

    public RoundState currentRoundState;

    private int currentRound = 1;
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
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        RoundStart();
    }
    public void RoundStart()
    {
        currentRoundState = RoundState.RoundBegin;

        SpawnManager.Instance.StartSpawning(currentRound);
    }

    public void EnemyDefeated()
    {
            RoundEnd();
    }

    public void RoundEnd()
    {
        currentRoundState = RoundState.RoundEnd;

        Invoke(nameof(currentRoundState), 5f);// adds a delay of 5 seconds between rounds or atleast should.

        currentRound++;
    }
}
