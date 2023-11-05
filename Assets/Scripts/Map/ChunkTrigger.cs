using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    [SerializeField] private GameObject targetMap;

    private MapController mapController;

    private void Start()
    {
        mapController = FindObjectOfType<MapController>();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            mapController.currentChunk = targetMap;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (mapController.currentChunk == targetMap)
            {
                mapController.currentChunk = null;
            }
        }
    }
}
