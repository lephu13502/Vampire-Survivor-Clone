using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }
    public List<Drops> drops;

    private void OnDestroy()
    {
        float randomNumber = UnityEngine.Random.Range(0f, 100f);
        List<Drops> possibleDrops = new List<Drops>();
        foreach (Drops rate in drops)
        {
            if (randomNumber <= rate.dropRate)
            {
                possibleDrops.Add(rate);
            }
        }
        if (possibleDrops.Count > 0)
        {
            Drops newDrops = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            Instantiate(newDrops.itemPrefab, transform.position, Quaternion.identity);
        }
    }
}
