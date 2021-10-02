using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public float speed = 5;
    public float waitTime = .3f;
    public float rotSpeed = 60;
    
    public Transform pathowner;

    void Start()
    {
        Vector3[] stops = new Vector3[pathowner.childCount];
        for (int i = 0; i < stops.Length; i++)
        {
            stops[i] = pathowner.GetChild(i).position;
            stops[i] = new Vector3(stops[i].x, transform.position.y, stops[i].z);
        }
        StartCoroutine (Pathing (stops));

    }

    IEnumerator Pathing(Vector3[] stops)
    {
        transform.position = stops[0];

        int targetStopIndex = 1;
        Vector3 targetStops = stops[targetStopIndex];
        transform.LookAt(targetStops);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetStops, speed * Time.deltaTime);
            
            if (transform.position == targetStops)
            {
                targetStopIndex = (targetStopIndex + 1) % stops.Length;
                targetStops = stops [targetStopIndex];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurntoFace(targetStops));
            }
            yield return null;
        }
    }

    IEnumerator TurntoFace(Vector3 lookTarget)
    {
        Vector3 faceToLookTarget = (lookTarget - transform.position).normalized;
        float targetAgle = 90 - Mathf.Atan2(faceToLookTarget.z, faceToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAgle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAgle, rotSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 startway = pathowner.GetChild(0).position;
        Vector3 previousway = startway;
        
        foreach (Transform way in pathowner) 
        {
            Gizmos.DrawSphere(way.position, .3f);
            Gizmos.DrawLine(previousway, way.position);
            previousway = way.position;
        }
        Gizmos.DrawLine(previousway, startway);

    }
}
