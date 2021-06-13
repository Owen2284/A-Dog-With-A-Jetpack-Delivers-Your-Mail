using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectableBehaviour : BaseEntityBehaviour
{
    public int connectionMaxHealth = 3;
    public float invincibilityTime = 1.5f;

    private AudioSource chainBreakSound;

    protected int connectionHealth;
    protected float invincibilityTimeRemaining;

    protected ConnectableBehaviour previousConnectedItem;
    protected ConnectableBehaviour nextConnectedItem;
    private LineRenderer nextItemLine;
    private Joint2D joint;
    private new Renderer renderer;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        connectionHealth = connectionMaxHealth;

        nextItemLine = GetComponent<LineRenderer>();
        joint = GetComponent<Joint2D>();
        renderer = GetComponent<Renderer>();

        chainBreakSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (nextConnectedItem != null)
        {
            // Set up the line coordinates
            nextItemLine.SetPositions(new List<Vector3> {
                this.transform.position,
                nextConnectedItem.transform.position
            }.ToArray());
        }

        if (invincibilityTimeRemaining > 0)
        {
            invincibilityTimeRemaining -= Time.deltaTime;
            float invincibilityFactor = Mathf.Ceil((invincibilityTimeRemaining / invincibilityTime) * (invincibilityTime * 10));
            bool visibleThisFrame = invincibilityFactor % 2 == 1;
            renderer.enabled = visibleThisFrame;
        }
        else
        {
            renderer.enabled = true;
        }
    }

    public void SetNextConnection(ConnectableBehaviour that)
    {
        // Link as data
        this.nextConnectedItem = that;
        that.previousConnectedItem = this;

        // Link with line
        nextItemLine.enabled = true;
        nextItemLine.SetPositions(new List<Vector3> {
            this.transform.position,
            nextConnectedItem.transform.position
        }.ToArray());

        // Link with hinge
        joint.enabled = true;
        joint.connectedBody = that.gameObject.GetComponent<Rigidbody2D>();

        // Reset target connection health
        that.connectionHealth = that.connectionMaxHealth;
    }

    public int GetHealth()
    {
        return connectionHealth;
    }

    public List<ConnectableBehaviour> GetChain()
    {
        var chain = new List<ConnectableBehaviour> {
            this
        };

        if (nextConnectedItem != null)
        {
            chain.AddRange(nextConnectedItem.GetChain());
        }

        return chain;
    }

    public void BreakChain(bool violent = false, bool first = true)
    {
        // Safely disconnect from the previous item
        if (previousConnectedItem != null)
        {
            previousConnectedItem.nextConnectedItem = null;
            previousConnectedItem.joint.enabled = false;
            previousConnectedItem.joint.connectedBody = null;
            previousConnectedItem.nextItemLine.enabled = false;
        }

        // Carry on down the line
        if (nextConnectedItem != null)
        {
            nextConnectedItem.BreakChain(violent, false);
        }

        // Apply random force if it's a violent break, and send alert
        if (violent)
        {
            body.AddForce(new Vector2(Random.Range(-10, 11), Random.Range(-10, 11)));

            if (first)
            {
                gameManager.AddAlert("Chain broken!");
                chainBreakSound.Play();
            }
        }
    }

    public void TakeDamage(BaseEnemyBehaviour enemy, Collision2D collision)
    {
        // Get point of collision
        var pointOfCollision = collision.GetContact(0).point;

        // Push entities away
        PushAwayFrom(pointOfCollision, 1200);
        enemy.PushAwayFrom(pointOfCollision, 1200);

        // Ignore request for damage if invincible
        if (invincibilityTimeRemaining > 0)
        {
            return;
        }

        // Reduce connection health
        connectionHealth -= 1;

        // Break connections if health zero
        if (connectionHealth <= 0)
        {
            this.BreakChain(true, true);
            connectionHealth = 3;
        }

        // Trigger invincibility
        invincibilityTimeRemaining = invincibilityTime;
    }
}
