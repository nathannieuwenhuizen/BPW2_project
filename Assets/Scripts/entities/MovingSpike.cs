using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpike : MonoBehaviour
{

    [SerializeField]
    private List<Transform> points;

    [SerializeField]
    private float moveSpeed = 1f;

    [Range(0, 4)]
    [SerializeField]
    private float waitduration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[0].position;
        StartCoroutine(WaitBeforeNextPOint(0));

    }

    IEnumerator MovingToPoint(int index)
    {
        while (Vector3.Distance(transform.position, points[index].position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, points[index].position, Time.deltaTime * moveSpeed);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.position = points[index].position;
        StartCoroutine(WaitBeforeNextPOint(index));
    }

    IEnumerator WaitBeforeNextPOint(int index)
    {
        yield return new WaitForSeconds(waitduration);
        index = (points.Count + index + 1) % points.Count;
        StartCoroutine(MovingToPoint(index));

    }

    private void Update()
    {
        //transform.Rotate(new Vector3(0, 0, 5));
    }

    private void OnDrawGizmos()
    {
        //UnityEditor.Handles.color = Color.red;
        for(int i = 0; i < points.Count; i++)
        {
            //UnityEditor.Handles.DrawSolidDisc(points[i].position, Vector3.back, 0.5f);
            if ( i < points.Count - 1)
            {
                Debug.DrawLine(points[i].position, points[i + 1].position);
            } else
            {
                Debug.DrawLine(points[i].position, points[0].position);
            }
        }
    }

}
