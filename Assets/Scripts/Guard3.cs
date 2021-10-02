using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard3 : MonoBehaviour
{
    public float speed2 = 5;
    public float waitTime2 = .3f;
    public float rotSpeed2 = 60;

    public Transform pathowner2;

    void Start()
    {
        Vector3[] stops2 = new Vector3[pathowner2.childCount];
        for (int i = 0; i < stops2.Length; i++)
        {
            stops2[i] = pathowner2.GetChild(i).position;
            stops2[i] = new Vector3(stops2[i].x, transform.position.y, stops2[i].z);
        }
        StartCoroutine(Pathing2(stops2));

    }

    //This snippets is for creating and following the path of empty objects
    IEnumerator Pathing2(Vector3[] stops2)
    {
        // The first stops2 and the 2nd stops2 are different variables they just have the same name
        transform.position = stops2[0];

        int targetStopIndex2 = 1;
        Vector3 targetStops2 = stops2[targetStopIndex2];
        transform.LookAt(targetStops2);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetStops2, speed2 * Time.deltaTime);

            if (transform.position == targetStops2)
            {
                targetStopIndex2 = (targetStopIndex2 + 1) % stops2.Length;
                targetStops2 = stops2[targetStopIndex2];
                yield return new WaitForSeconds(waitTime2);
                yield return StartCoroutine(TurntoFace2(targetStops2));
            }
            yield return null;
        }
    }

    IEnumerator TurntoFace2(Vector3 lookTarget2)
    {
        Vector3 faceToLookTarget2 = (lookTarget2 - transform.position).normalized;
        float targetAgle2 = 90 - Mathf.Atan2(faceToLookTarget2.z, faceToLookTarget2.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAgle2)) > 0.05f)
        {
            float angle2 = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAgle2, rotSpeed2 * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle2;
            yield return null;
        }
    }

    //This snippet down here allows you to see the gizmos for the empty objects
    void OnDrawGizmos()
    {
        Vector3 startway2 = pathowner2.GetChild(0).position;
        Vector3 previousway2 = startway2;

        foreach (Transform way2 in pathowner2)
        {
            Gizmos.DrawSphere(way2.position, .3f);
            Gizmos.DrawLine(previousway2, way2.position);
            previousway2 = way2.position;
        }
        Gizmos.DrawLine(previousway2, startway2);

    }
}
