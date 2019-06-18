using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleExplosion : PoolObject
{
    // Start is called before the first frame update
    public void Explode()
    {
        GetComponent<ParticleSystem>().Play();
        StartCoroutine(CheckingIfRunning());
    }
    IEnumerator CheckingIfRunning()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        while (ps.IsAlive())
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy();
    }
}
