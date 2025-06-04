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
    [SerializeField] private BaseStats baseStats;
    [Header("Angel Audio")]
    [SerializeField] private AudioSource angelAudioSource;
    [SerializeField] private AudioClip angelRangedAttack;
    [SerializeField] private AudioClip angelMeleeAttack;
    [SerializeField] private AudioClip angelFormChange;
    [SerializeField] private AudioClip angelAOE;
    [SerializeField] private AudioClip angelDeath;
    [SerializeField] private AudioClip angelSpawn;
    public Animator Aanimator => animator;

    [Header("Mutant & Zombie Audio")]
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

    [SerializeField] private List<AngelBeamCannon> angelBeamCannons;

    public bool spawned = false;
    private void Start()
    {
        RegisterEnemy(baseStats);
    }
    private void Update()
    {
        animator.SetFloat("HorizontalSpeed", GetHorizontalAgentVelocity().magnitude);
        animator.SetBool("IsTraversingLink", navMeshAgent.isOnOffMeshLink);

        enemyRandomNoise();

    }
    public void chargeTauntAudio()
    {
        if(mutantAudioSource.isPlaying == true)
        {
            mutantAudioSource.Stop();
        }
        mutantAudioSource.PlayOneShot(chargeTaunt);
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
        if(angelSpawn != null && angelAudioSource.isPlaying != true)
        {
            // this is where the angel voice lines can go if i get to them.
        }
    }

    public void charginAttack()
    {
        if (mutantAudioSource.isPlaying == true)
        {
            mutantAudioSource.Stop();
        }
        mutantAudioSource.PlayOneShot(chargeInAction,2.0f);
    }
    public void chargeBreathing() 
    {
        if (mutantAudioSource.isPlaying == true)
        {
            mutantAudioSource.Stop();
        }
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
            if (mutantAudioSource.isPlaying == true)
            {
                mutantAudioSource.Stop();
            }
            mutantAudioSource.PlayOneShot(mutantDeath);
        }
        else
        {
            if (zombieAudioSource.isPlaying == true)
            {
                zombieAudioSource.Stop();
            }
            zombieAudioSource.PlayOneShot(zombieDeath);
        }
        if(angelSpawn != null)
        {
            if(angelAudioSource.isPlaying == true)
            {
                angelAudioSource.Stop();
            }
            angelAudioSource.PlayOneShot(angelDeath);
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

    // this is the angels giant AOE
    public void heavensDescent()
    {
        animator.SetTrigger("Big_AOE");
        animator.SetBool("AOE_Going", true);
        angelAudioSource.PlayOneShot(angelAOE); 
    }
    public void heavensExplosion()
    {
        Instantiate(aiStateController.aiBlackboard.AOEPrefab, navMeshAgent.transform.position, Quaternion.identity);
        aiStateController.aiBlackboard.aoeOver = true;
        //animator.SetTrigger("Aoe_Over");
        // gotta find and add in audio for the actuall effect im not sure on what i should do yet thinking a choir singing in unison
    }
    public void SpawnStart()
    {
        if (mutantSpawn != null)
        {

            mutantAudioSource.PlayOneShot(mutantSpawn,0.8f);
        }
        else
        {
            zombieAudioSource.PlayOneShot(zombieSpawn, 0.7f);
        }
        if (angelSpawn != null)
        {
            angelAudioSource.PlayOneShot(angelSpawn, 0.8f);
        }
    }
    private IEnumerator RandomNoiseMutant()
    {
        if (mutantAudioSource.isPlaying != true)
        {
            int SongChoice = Random.Range(0, mutantSounds.Count);
            if (mutantAudioSource.isPlaying == true)
            {
                mutantAudioSource.Stop();
            }
            mutantAudioSource.PlayOneShot(mutantSounds[SongChoice],0.60f);
            Debug.Log(("This is the Mutant noise:") + SongChoice);
        }
        yield return new WaitForSeconds(Random.Range(4f,15f));
    }

    private IEnumerator RandomNoiseZombie()
    {
        if (zombieAudioSource.isPlaying != true)
        {
            int SongChoice = Random.Range(0, zombieSounds.Count);
            if (zombieAudioSource.isPlaying == true)
            {
                zombieAudioSource.Stop();
            }
            zombieAudioSource.PlayOneShot(zombieSounds[SongChoice],0.6f);
            Debug.Log(("This is the zombie noise:") + SongChoice);
        }
        yield return new WaitForSeconds(Random.Range(4f, 15f));
    }

    public void FireAngelBeamAtTarget()
    {
        if(angelBeamCannons == null || angelBeamCannons.Count == 0 || aiStateController == null)
        {
            return;
        }
        Transform target = aiStateController.aiBlackboard.chaseTarget;
        if (target != null) { return; }

       int handIndex = animator.GetInteger("HandIndex");
       float handNum = (handIndex);
       animator.SetFloat("HIblend", handNum);
        //animator.SetInteger("HandIndex", handIndex);
        //animator.SetFloat("HIblend", handIndex);

        if(angelAudioSource!=null && angelRangedAttack != null)
        {
            angelAudioSource.PlayOneShot(angelRangedAttack);
        }

        angelBeamCannons[handIndex].FireAt(target);
    }

    public void RegisterEnemy(BaseStats enemyStats)
    {
        enemyStats.angelHalf.AddListener(ReceivedOnAngelHalfHealth);
    }
    private void ReceivedOnAngelHalfHealth()
    {
        Debug.Log("The angel has reached half hp and should go to phase 2");
        aiStateController.aiBlackboard.currentPhase = 2;
        animator.SetBool("Form_Change", true);
        navMeshAgent.GetComponent<Collider>().enabled = false;
    }
    public void FormAnimationOver()
    {
        animator.SetBool("FormFinished", true);
        aiStateController.aiBlackboard.formChanged = true;
        navMeshAgent.GetComponent<Collider>().enabled = true;
    }
}
