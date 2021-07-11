using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyBehaviour : BaseEntityBehaviour
{
    public int health = 1;
    public float moveSpeed = 1f;
    public AudioSource deathSound;
    public float pushForceMultiplier = 1f;

    protected PlayerBehaviour player;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    protected void Update()
    {
        NormaliseVelocityToMax();
    }

    protected void OnCollisionEnter2D(Collision2D t)
    {
        if (t.gameObject.tag == "Player" || t.gameObject.tag == "Delivery")
        {
            // Damage player
            t.gameObject.GetComponent<ConnectableBehaviour>().TakeDamage(this, t);
        }
    }

    public void TakeDamage()
    {
        // Reduce health
        health -= 1;

        // Destroy entity if at zero health
        if (health <= 0)
        {
            if (deathSound != null)
            {
                deathSound.Play();
            }
            Destroy(this.gameObject);
        }
    }
}
