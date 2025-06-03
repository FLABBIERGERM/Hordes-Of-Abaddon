using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelBeamCannon : MonoBehaviour
{
    public LineRenderer beamRenderer;
    public float beamLength = 75f;
    public float beamDuration = 0.3f;
    public float fireDelay = 0.4f;
    public string playerTag = "Player";
    public int damage = 2;

    public LayerMask hitMask;
    public float maxOffset = 2f; // test this

    public void FireAt(Transform target)
    {
        Vector3 lockedTargetPosition = target.position;

        lockedTargetPosition += new Vector3(
            Random.Range(-maxOffset, maxOffset),
            0,
           Random.Range(-maxOffset, maxOffset));
        StartCoroutine(FireWithDelay(lockedTargetPosition));

        //Vector3 targetPosition = target.position;
        //Vector3 dir = (targetPosition - transform.position).normalized;
        //Ray ray = new Ray(transform.position, dir);

        //Vector3 endPoint = transform.position + dir * beamLength;

        //if (Physics.Raycast(ray, out RaycastHit hit, beamLength, hitMask))
        //{
        //    endPoint = hit.point;

        //    if (hit.collider.CompareTag("Player"))
        //    {
        //        Debug.Log("Hit Player With Beam!");
        //        // add in the damage later
        //    }
        //}

        //StartCoroutine(ShowBeam(endPoint));
    }

    private IEnumerator FireWithDelay(Vector3 lockedTargettPosition)
    {
        yield return new WaitForSeconds(fireDelay);

        Vector3 dir = (lockedTargettPosition - transform.position).normalized;
        Ray ray = new Ray(transform.position, dir);

        Vector3 endPoint = transform.position + dir * beamLength;

        if (Physics.Raycast(ray, out RaycastHit hit, beamLength, hitMask))
        {
            endPoint = hit.point;
            StartCoroutine(ShowBeam(endPoint));
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Hit Player With Beam!");
                // add in the damage later
                GameManager.Instance.TookDamage(-damage);
            }
        }
        else
        {
            StartCoroutine(ShowBeam(endPoint));
        }
    }
    private IEnumerator ShowBeam(Vector3 end)
    {
        beamRenderer.SetPosition(0, transform.position);
        beamRenderer.SetPosition(1, end);
        beamRenderer.enabled = true;

        yield return new WaitForSeconds(beamDuration);
        beamRenderer.enabled = false;
    }
}
