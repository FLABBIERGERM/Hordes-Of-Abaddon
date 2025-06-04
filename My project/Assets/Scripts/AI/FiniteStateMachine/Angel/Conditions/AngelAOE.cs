using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelAOE : MonoBehaviour
{
    public int damage = 10;
    public float duration = 1.0f;
    public string playerTag = "Player";

    private float startTime;
    // Start is called before the first frame update

    void Start()
    {
        startTime = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Angel AOE hit a player");
            GameManager.Instance.TookDamage(-damage);
        }
    }


    private void Update()
    {
        if (Time.time - startTime >= duration)
        {
            Destroy(gameObject);
        }
    }
}
