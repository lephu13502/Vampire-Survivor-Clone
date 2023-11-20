using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollecter : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollecter;
    [SerializeField] private float pullSpeed;

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollecter = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        playerCollecter.radius = player.currentMagnet;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out ICollectible collectible))
        {
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = (transform.position - col.transform.position).normalized;
            rb.AddForce(forceDirection * pullSpeed);

            collectible.Collect();
        }
    }
}
