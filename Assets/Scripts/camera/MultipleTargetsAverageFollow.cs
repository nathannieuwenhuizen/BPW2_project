using UnityEngine;
using System.Collections;

public class MultipleTargetsAverageFollow : MonoBehaviour {
    //the z distance the camera should be away from its target(s).
    [SerializeField]
    private float zDistance = -10;
    [SerializeField]
    private float lerpSpeed = 5f;
    //the targets themselves.
    [SerializeField]
    private Transform[] targets;

    //tempPostiion is the vector3 variable that will be calculated.
    private Vector3 tempPosition;

    //is used to calculate the distance of multpile targets positions.
    private Vector3 distance;

	void Update () {

        //sets the position to the first target.
        tempPosition = targets[0].position;

        //if there are more than one target to follow. 
        //It calculates the avarage distances between the targets by calculating the distances 
        //one by one and aply the half of the distance to the tempPosition 
        //and so it goes on until it has every objects transform.
        if (targets.Length > 1)
        {
            for (int i = 1; i < targets.Length; i++)
            {
                distance = tempPosition - targets[i].position;
                tempPosition -= distance/2;
            }
        }

        //declares the z distance from the target.
        tempPosition.z = zDistance;
        //aplies the tempPosition to the position of the object that must have the average position.

        //transform.position = tempPosition;
        transform.position = Vector3.Lerp(transform.position, tempPosition, lerpSpeed * Time.deltaTime);
	}
}
