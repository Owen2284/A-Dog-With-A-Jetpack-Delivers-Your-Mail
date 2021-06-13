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

    private float timeToNextShot;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        timeToNextShot = 0;
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
                    Instantiate(smokeTrail, new Vector3(transform.position.x + (0.1f * h), transform.position.y + (-0.5f * v), 2), new Quaternion(0, 0, Random.Range(0, 360), 1));

                    // Also play sound effect
                    // TODO
                }

                if (Input.GetMouseButton(0) && timeToNextShot == 0)
                {
                    Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);

                    var distance = Vector2.Distance(worldPosition, playerPosition);

                    var liveProjectile = Instantiate(projectile, playerPosition, Quaternion.identity);
                    liveProjectile.GetComponent<Rigidbody2D>().velocity = ((worldPosition - playerPosition) / distance) * projectileSpeed;

                    timeToNextShot = timeBetweenShots;
                }
            }

            timeToNextShot = Mathf.Max(timeToNextShot - Time.deltaTime, 0);
        }
    }

    public void DisableControl()
    {
        // Game is over, so prevent control from here on out
        preventControl = true;
    }
}
