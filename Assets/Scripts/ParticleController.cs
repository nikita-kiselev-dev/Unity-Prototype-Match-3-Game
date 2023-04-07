using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParticleController : MonoBehaviour
{
    public static ParticleController Instance;
    
    [SerializeField] private List<GameObject> particleList = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void PlayRandomMatchParticle(GameObject tile)
    {
        int randomParticle = Random.Range(0, particleList.Count);
        Instantiate(particleList[randomParticle], tile.transform.position, tile.transform.rotation);
    }
}
