using UnityEngine;
using System.Collections;
/*
Camerashake for cameras.
Must be assigned to the camera itself and other movements 
of the camera must be done by the PARENT! of the camera.
*/
public class CameraShake : MonoBehaviour {

    private bool isShacking = false;
    private Vector3 randomPos;

    public float intensity;
    void Start()
    {
        //Shake(10f, 10f, 0.01f);
    }

    //public function that starts the camera shaking.
    public void Shake(float duration = 0.1f, float _intensity = 10f, float shakerate = 0.01f)
    {
        intensity = _intensity;
        StartCoroutine(CameraShaking(duration, shakerate));
    }

    //this numerator handles how long the shake is active.
    IEnumerator CameraShaking(float duration, float shakerate)
    {
        isShacking = true;
        StartCoroutine(RandomPositions(shakerate));
        yield return new WaitForSeconds(duration);
        isShacking = false;
    }
    public void StopShake()
    {
        if (isShacking)
        {
            isShacking = false;
            StopAllCoroutines();
        }
    }
    //this numerator is active as long isAvtive is true. It calculates the randompositions and waits for the framerate, then it puts it back.
    IEnumerator RandomPositions(float shakerate)
    {
        while(isShacking)
        {
            randomPos = new Vector3(Random.Range(-intensity, intensity)/100f, Random.Range(-intensity, intensity)/100f,0f);
            transform.position += randomPos;
            yield return new WaitForSeconds(shakerate);
            transform.position -= randomPos;
            yield return new WaitForFixedUpdate();
        }
    }

}
