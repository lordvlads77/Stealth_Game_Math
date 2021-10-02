using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard1 : MonoBehaviour
{
    public float speed1 = 5;
    public float waitTime1 = .3f;
    public float rotSpeed1 = 60;

    public Transform pathowner1; 

   void Start()
    {
        Vector3[] stops1 = new Vector3[pathowner1.childCount];
        for (int i = 0; i < stops1.Length; i++)
        {
            stops1[i] = pathowner1.GetChild(i).position;
            stops1[i] = new Vector3(stops1[i].x, transform.position.y, stops1[i].z);
        }
        StartCoroutine(Pathing1(stops1));

    }

    IEnumerator Pathing1(Vector3[] stops1)
    {
        transform.position = stops1[0];

        int targetStopIndex1 = 1;
        Vector3 targetStops1 = stops1[targetStopIndex1];
        transform.LookAt(targetStops1);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetStops1, speed1 * Time.deltaTime);

            if (transform.position == targetStops1)
            {
                targetStopIndex1 = (targetStopIndex1 + 1) % stops1.Length;
                targetStops1 = stops1[targetStopIndex1];
                yield return new WaitForSeconds(waitTime1);
                yield return StartCoroutine(TurntoFace1(targetStops1));
            }
            yield return null;
        }
    }

    IEnumerator TurntoFace1(Vector3 lookTarget1)
    {
        Vector3 faceToLookTarget1 = (lookTarget1 - transform.position).normalized;
        float targetAgle1 = 90 - Mathf.Atan2(faceToLookTarget1.z, faceToLookTarget1.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAgle1)) > 0.05f)
        {
            float angle1 = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAgle1, rotSpeed1 * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle1;
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 startway1 = pathowner1.GetChild(0).position;
        Vector3 previousway1 = startway1;

        foreach (Transform way1 in pathowner1)
        {
            Gizmos.DrawSphere(way1.position, .3f);
            Gizmos.DrawLine(previousway1, way1.position);
            previousway1 = way1.position;
        }
        Gizmos.DrawLine(previousway1, startway1);

    }
}
