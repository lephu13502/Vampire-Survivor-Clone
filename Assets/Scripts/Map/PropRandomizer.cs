using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    [SerializeField] private List<GameObject> propSpawnPoints;
    [SerializeField] private List<GameObject> propPrefabs;

    private void Start()
    {
        SpawnProps();
    }

    private void SpawnProps()
    {
        foreach (GameObject spawnPoints in propSpawnPoints)
        {
            int rand = Random.Range(0, propPrefabs.Count);
            GameObject prop = Instantiate(propPrefabs[rand], spawnPoints.transform.position, Quaternion.identity);
            prop.transform.parent = spawnPoints.transform;
        }
    }
}
