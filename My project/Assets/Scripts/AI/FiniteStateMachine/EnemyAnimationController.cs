using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource footstepSoundclip;
    [SerializeField] private AudioClip stepin;
    [SerializeField] private AiStateController aiStateController;

    [SerializeField] private AudioSource mutantAudioSource;
    [SerializeField] private AudioSource zombieAudioSource;

    [SerializeField] private AudioClip zombieSpawn;
    [SerializeField] private AudioClip zombieAttack;
    [SerializeField] private AudioClip zombieDeath;
    [SerializeField] private List<AudioClip> zombieSounds;

    [SerializeField] private AudioClip mutantSpawn;
    [SerializeField] private AudioClip mutantAttack;
    [SerializeField] private AudioClip mutantDeath;
    [SerializeField] private List<AudioClip> mutantSounds;

    [SerializeField] public AudioClip chargeTaunt;
    [SerializeField] public AudioClip chargeInAction;
    [SerializeField] public AudioClip chargeSlam;
    [SerializeField] public AudioClip chargeEnd;


    public bool spawned = false;

    private void Update()
    {
        animator.SetFloat("HorizontalSpeed", GetHorizontalAgentVelocity().magnitude);
        animator.SetBool("IsTraversingLink", navMeshAgent.isOnOffMeshLink);
        enemyRandomNoise();

    }
    private void enemyRandomNoise()
    {
        if(mutantSpawn != null  && mutantAudioSource.isPlaying != true)
        {
            StartCoroutine(RandomNoiseMutant());

        }
        if (zombieSpawn != null  && zombieAudioSource.isPlaying != true)
        {
            StartCoroutine(RandomNoiseZombie());
        }
    }

    public void charginAttack()
    {
        mutantAudioSource.PlayOneShot(chargeInAction,2.0f);
    }
    public void chargeBreathing() 
    {
            mutantAudioSource.PlayOneShot(chargeEnd,0.6f);
    }
    private Vector3 GetHorizontalAgentVelocity()
    {
        return new Vector3(navMeshAgent.velocity.x, 0f, navMeshAgent.velocity.z);
    }
    public void EnemyWalk()
    {
        footstepSoundclip.PlayOneShot(stepin);
    }
    public void Dying()
    {

        aiStateController.aiBlackboard.dead = true;
        if (mutantSpawn != null)
        {
            mutantAudioSource.PlayOneShot(mutantDeath);
        }
        else
        {
            zombieAudioSource.PlayOneShot(zombieDeath);
        }
    }

    public void Slammed()
    {
        Instantiate(aiStateController.aiBlackboard.chargeCrash, aiStateController.aiBlackboard.chargeLocation, Quaternion.identity);
        mutantAudioSource.PlayOneShot(chargeSlam);
    }
    public void SpawnEnd()
    {
        animator.SetBool("Spawned", true);
        aiStateController.aiBlackboard.spawned = true;
    }

    public void SpawnStart()
    {
        if (mutantSpawn != null)
        {
            mutantAudioSource.PlayOneShot(mutantSpawn);
        }
        else
        {
            zombieAudioSource.PlayOneShot(zombieSpawn);
        }
    }
    private IEnumerator RandomNoiseMutant()
    {
        if (mutantAudioSource.isPlaying != true)
        {
            int SongChoice = Random.Range(0, mutantSounds.Count);

            mutantAudioSource.PlayOneShot(mutantSounds[SongChoice]);
            Debug.Log(("This is the song that is play:") + SongChoice);
        }
        yield return new WaitForSeconds(Random.Range(4f,15f));
    }

    private IEnumerator RandomNoiseZombie()
    {
        if (zombieAudioSource.isPlaying != true)
        {
            int SongChoice = Random.Range(0, zombieSounds.Count);

            zombieAudioSource.PlayOneShot(zombieSounds[SongChoice]);
            Debug.Log(("This is the song that is play:") + SongChoice);
        }
        yield return new WaitForSeconds(Random.Range(4f, 15f));
    }
}
