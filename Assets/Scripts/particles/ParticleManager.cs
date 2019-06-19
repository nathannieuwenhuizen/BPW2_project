using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [Header("Character particles")]
    public ParticleGroup deathParticle;
    public ParticleGroup landImpactParticle;
    public ParticleGroup characterJumpParticle;

    public static ParticleManager instance;
    void Awake()
    {
        instance = this;

        InitiatePool(deathParticle);
        InitiatePool(landImpactParticle);
        InitiatePool(characterJumpParticle);

    }
    public void InitiatePool(ParticleGroup particle)
    {
        PoolManager.instance.CreatePool(particle.particlePrefab, particle.amount);
    }

    public GameObject SpawnParticle(ParticleGroup particle, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject obj = PoolManager.instance.ReuseObject(particle.particlePrefab, spawnPosition, spawnRotation);
        if (obj.GetComponent<ParticleExplosion>())
        {
            obj.GetComponent<ParticleExplosion>().Explode();
        }
        return obj;
    }
}
[System.Serializable]
public class ParticleGroup: System.Object
{
    public GameObject particlePrefab;
    public int amount;
}
