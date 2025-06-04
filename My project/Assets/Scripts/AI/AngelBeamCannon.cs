using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelBeamCannon : MonoBehaviour
{
    public GameObject beamRenderer;
    public float beamLength = 75f;
    public float beamDuration = 1f;
    public float fireDelay = 0.4f;
    public string playerTag = "Player";
    public int damage = 2;

    public LayerMask hitMask;
    public float maxOffset = 2f; // test this
    private LineRenderer activeBeam;
    public void FireAt(Transform target)
    {
        if (beamRenderer == null)
        {
            Debug.LogError("BeamRenderer is not assigned!");
        }
        Vector3 lockedTargetPosition = target.position +new Vector3(0f,1.5f,0f);

        lockedTargetPosition += new Vector3(
            Random.Range(-maxOffset, maxOffset),
            0,
           Random.Range(-maxOffset, maxOffset));
        StartCoroutine(FireWithDelay(lockedTargetPosition));
    }

    private IEnumerator FireWithDelay(Vector3 lockedTargettPosition)
    {
        yield return new WaitForSeconds(fireDelay);

        Vector3 dir = (lockedTargettPosition - transform.position).normalized;
        Ray ray = new Ray(transform.position, dir);

        Vector3 endPoint = transform.position + dir * beamLength;

        if (Physics.Raycast(ray, out RaycastHit hit, beamLength))
        {
            endPoint = hit.point;
            StartCoroutine(ShowBeam(endPoint));
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Hit Player With Beam!");
                // add in the damage later
                GameManager.Instance.TookDamage(-damage);
            }
            Debug.Log("what did we hit if anything?" + hit.collider.name);

        }
        else
        {
            StartCoroutine(ShowBeam(endPoint));
        }
    }
    private IEnumerator ShowBeam(Vector3 end)
    {
        if(activeBeam == null)
        {
            GameObject beamInstance = Instantiate(beamRenderer, transform.position, Quaternion.identity);
            activeBeam = beamInstance.GetComponent<LineRenderer>();
        }
        activeBeam.SetPosition(0, transform.position);
        activeBeam.SetPosition(1, end);
        activeBeam.enabled = true;

        yield return new WaitForSeconds(beamDuration);
        activeBeam.enabled = false;
       // Destroy(activeBeam);
    }
}
