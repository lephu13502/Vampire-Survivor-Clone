using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController Instance { get; private set; }

    [SerializeField] private List<GameObject> terrainChunks;
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask terrainMask;
    [SerializeField] private float checkerRadius;

    private Vector3 noTerrainPosition;
    private PlayerMovement playerMovement;
    public GameObject currentChunk;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    private GameObject latestChunk;
    [SerializeField] private float maxOpDist; //Must be greater than the length and width of the tilemap
    private float opDist;
    private float optimizerCooldown;
    [SerializeField] private float optimizerCooldownDur;


    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    private void ChunkChecker()
    {
        if (!currentChunk)
        {
            return;
        }

        if (playerMovement.moveDir.x > 0 && playerMovement.moveDir.y == 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right").position;  //Right
                SpawnChunk();
            }
        }
        else if (playerMovement.moveDir.x < 0 && playerMovement.moveDir.y == 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left").position;    //Left
                SpawnChunk();
            }
        }
        else if (playerMovement.moveDir.y > 0 && playerMovement.moveDir.x == 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Up").position; //Up
                SpawnChunk();
            }
        }
        else if (playerMovement.moveDir.y < 0 && playerMovement.moveDir.x == 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Down").position;    //Down
                SpawnChunk();
            }
        }
        else if (playerMovement.moveDir.x > 0 && playerMovement.moveDir.y > 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right Up").position;   //Right up
                SpawnChunk();
            }
        }
        else if (playerMovement.moveDir.x > 0 && playerMovement.moveDir.y < 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right Down").position;  //Right down
                SpawnChunk();
            }
        }
        else if (playerMovement.moveDir.x < 0 && playerMovement.moveDir.y > 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left Up").position;  //Left up
                SpawnChunk();
            }
        }
        else if (playerMovement.moveDir.x < 0 && playerMovement.moveDir.y < 0)
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left Down").position; //Left down
                SpawnChunk();
            }
        }
    }
    private void SpawnChunk()
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }
    private void ChunkOptimizer()
    {
        optimizerCooldown -= Time.deltaTime;

        if (optimizerCooldown <= 0f)
        {
            optimizerCooldown = optimizerCooldownDur;   //Check every 1 second to save cost, change this value to lower to check more times
        }
        else
        {
            return;
        }
        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > maxOpDist)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
