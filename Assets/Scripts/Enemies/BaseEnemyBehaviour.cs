using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyBehaviour : BaseEntityBehaviour
{
    public int health = 1;
    public float moveSpeed = 1f;

    protected PlayerBehaviour player;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void OnCollisionEnter2D(Collision2D t)
    {
        if (t.gameObject.tag == "Player" || t.gameObject.tag == "Delivery")
        {
            // Damage player
            t.gameObject.GetComponent<ConnectableBehaviour>().TakeDamage(this, t);
        }
    }
}
