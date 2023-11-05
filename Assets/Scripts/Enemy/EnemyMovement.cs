using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform player;
    public EnemyScriptableObject enemyData;


    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyData.MoveSpeed * Time.deltaTime);
    }
}
