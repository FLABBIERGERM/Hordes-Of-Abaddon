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
        Debug.Log("The current round is: " + currentRound + " total enemys are" + currentRound * 3);
        currentRoundState = RoundState.RoundBegin;
        StartCoroutine(RoundWait());
        currentRoundState = RoundState.RoundPlaying;
        SpawnManager.Instance.StartSpawning(currentRound);
    }

    public void AllEnemysDefeated()
    {
            RoundEnd();
    }

    public void RoundEnd()
    {
        currentRoundState = RoundState.RoundEnd;

        Invoke(nameof(RoundStart), 2f);// adds a delay of 5 seconds between rounds or atleast should.

        currentRound++;
    }
    private IEnumerator RoundWait()
    {
        yield return new WaitForSeconds(5f);
    }// new idea to just make a round wait for round start and then while playing have spawning.
}
