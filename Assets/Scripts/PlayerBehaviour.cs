using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : ConnectableBehaviour
{
    public float timeBetweenShots = 0.75f;
    public float projectileSpeed = 10f;

    private bool preventControl = false;

    public GameObject projectile;

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

                body.AddForce(transform.up * v * body.mass * 10);
                body.AddForce(transform.right * h * body.mass * 10);

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
