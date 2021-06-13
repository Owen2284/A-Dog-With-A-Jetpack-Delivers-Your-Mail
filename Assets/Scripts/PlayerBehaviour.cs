using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : ConnectableBehaviour
{
    public float moveSpeed = 10;
    public float timeBetweenShots = 0.75f;
    public float projectileSpeed = 10f;

    private bool preventControl = false;

    public GameObject projectile;
    public GameObject smokeTrail;

    public GameObject exclaim;

    public AudioSource smokeTrailSfx;
    public AudioSource bark;

    public SpriteRenderer jetpack;

    private float timeToNextShot;

    private SpriteRenderer spriteRenderer;

    private bool wasPlacingSmokeLastUpdate;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();

        timeToNextShot = 0;

        ShowExclaim(false);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (!preventControl)
        {
            if (invincibilityTimeRemaining <= invincibilityTime / 2)
            {
                float v = Input.GetAxisRaw("Vertical");
                float h = Input.GetAxisRaw("Horizontal");

                body.AddForce(transform.up * v * body.mass * moveSpeed);
                body.AddForce(transform.right * h * body.mass * moveSpeed);

                // Place smoke trail if moving
                if (v != 0 || h != 0)
                {
                    Instantiate(smokeTrail, new Vector3(transform.position.x + (0.1f * h), transform.position.y + (-0.5f * v), 2), new Quaternion(0, 0, Random.Range(0, 0.99f), 1));

                    // Also play sound effect
                    if (!wasPlacingSmokeLastUpdate)
                    {
                        smokeTrailSfx.Play();
                    }
                    wasPlacingSmokeLastUpdate = true;
                }
                else
                {
                    if (wasPlacingSmokeLastUpdate)
                    {
                        smokeTrailSfx.Stop();
                    }
                    wasPlacingSmokeLastUpdate = false;
                }

                // Flip dog
                if (h > 0)
                {
                    spriteRenderer.flipX = false;
                }
                else if (h < 0)
                {
                    spriteRenderer.flipX = true;
                }
                if (v > 0)
                {
                    spriteRenderer.flipY = false;
                    jetpack.flipY = false;
                }
                else if (v < 0)
                {
                    spriteRenderer.flipY = true;
                    jetpack.flipY = true;
                }

                // Projectile firing
                if (Input.GetMouseButton(0) && timeToNextShot == 0)
                {
                    Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);

                    var distance = Vector2.Distance(worldPosition, playerPosition);

                    var liveProjectile = Instantiate(projectile, playerPosition, Quaternion.identity);
                    liveProjectile.GetComponent<Rigidbody2D>().velocity = ((worldPosition - playerPosition) / distance) * projectileSpeed;

                    // Play the bark when firing
                    bark.Play();

                    timeToNextShot = timeBetweenShots;
                }

                // Position resetting
                if (Input.GetKeyDown("r"))
                {
                    BreakChain();
                    transform.position = new Vector3(-8.76f, -1.1f, 0);
                    gameManager.AddAlert("Reset!");
                    gameManager.TimePenalty(10f);
                }

                // Manual disconnecting
                if (Input.GetKeyDown("tab"))
                {
                    BreakChain();
                    gameManager.AddAlert("Dropping cargo!");
                }
            }

            timeToNextShot = Mathf.Max(timeToNextShot - Time.deltaTime, 0);
        }
        else
        {
            smokeTrailSfx.Stop();
        }
    }

    public void DisableControl()
    {
        // Game is over, so prevent control from here on out
        preventControl = true;
    }

    public void ShowExclaim(bool show)
    {
        exclaim.SetActive(show);
    }
}
