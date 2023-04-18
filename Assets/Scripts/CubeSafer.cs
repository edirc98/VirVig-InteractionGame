using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSafer : MonoBehaviour
{
    public CubeSpawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(other.gameObject,spawner.SpawnPoints[0].position, Quaternion.identity, spawner.CubesParent);
        Destroy(other.gameObject);
    }


}
