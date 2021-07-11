using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehaviour : BaseEnemyBehaviour
{
    public GameObject projectilePrefab;
    public float playerDetectionRange = 5f;
    public float timeBetweenShots = 2f;
    public float projectileSpeed = 1f;

    private AudioSource meow;
    private float timeToNextShot;

    private BaseEnemyBehaviour balloon;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        meow = GetComponent<AudioSource>();
        balloon = transform.Find("Balloon").GetComponent<BaseEnemyBehaviour>();

        timeToNextShot += timeBetweenShots * Random.Range(0.8f, 1.2f);
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();

        // Move death sound down to balloon
        if (balloon != null && balloon.deathSound == null)
        {
            balloon.deathSound = deathSound;
            deathSound = null;
        }

        Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 myPosition = new Vector2(transform.position.x, transform.position.y);

        var distanceToPlayer = Vector2.Distance(
            playerPosition,
            myPosition
        );

        // Handle shooting and timer if player in range
        if (distanceToPlayer < playerDetectionRange)
        {
            if (timeToNextShot <= 0)
            {
                // Fire shot
                var liveProjectile = Instantiate(projectilePrefab, myPosition, Quaternion.identity);
                liveProjectile.GetComponent<Rigidbody2D>().velocity = ((playerPosition - myPosition)) * projectileSpeed;

                // Play meow sound effect
                meow.pitch = Mathf.Max(Random.Range(0.0f, 1.8f), 1f);
                meow.Play();

                // Reset timer
                timeToNextShot = timeBetweenShots * Random.Range(0.9f, 1.1f);
            }
            else
            {
                timeToNextShot -= Time.deltaTime;

            }
        }

        // Remove if fallen off the map
        if (transform.position.y < -400)
        {
            TakeDamage();
        }
    }
}
