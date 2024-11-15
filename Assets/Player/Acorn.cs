using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acorn : MonoBehaviour
{
    [SerializeField] private int ammo;
    [SerializeField] private GameObject indicator;
    protected Rigidbody2D rb;
    private Transform playerTransform;
    [SerializeField] private float attractionDistance = 2;
    [SerializeField] private float attractionSpeed = 2;
    private PlayerStats playerStats;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = FindObjectOfType<PlayerController>().transform;
        playerStats = DataDictionary.PlayerStats;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerStats.AmmoCount < playerStats.MaxAmmo)
        {
            SFXManager.instance.PlayAmmoCollect();
            DataDictionary.PlayerStats.AmmoCount += ammo;
            var obj = Instantiate(indicator);
            obj.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {

        rb.velocity = rb.velocity * 0.8f;

        if (playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            // If within attraction distance, move towards the player
            if (distanceToPlayer < attractionDistance && playerStats.AmmoCount < playerStats.MaxAmmo)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                rb.velocity = direction * attractionSpeed;
            }
        }
    }
}
