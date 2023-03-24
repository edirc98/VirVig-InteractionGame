using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public Transform CubesParent;
    public List<GameObject> CubePrefabs;
    public List<Transform> SpawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnCube()
    {
        int prefabIdx = Random.Range(0, CubePrefabs.Count);
        int spawnPointIdx = Random.Range(0, SpawnPoints.Count);
        SpawnCube(prefabIdx, spawnPointIdx);
    }
    private void SpawnCube(int PrefabIndex, int SpawnPointIndex)
    {
        Instantiate(CubePrefabs[PrefabIndex], SpawnPoints[SpawnPointIndex].position, Quaternion.identity, CubesParent);
    }
}
