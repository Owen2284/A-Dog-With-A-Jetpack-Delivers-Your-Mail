using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryBehaviour : ConnectableBehaviour
{
    private GameObject target;

    private float timeSinceSpawn = 0;
    private Renderer rendy;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        rendy = transform.GetComponent<Renderer>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        timeSinceSpawn += Time.deltaTime;

        // Remove if fallen off the map
        if (transform.position.y < -400)
        {
            gameManager.HandleLostDelivery(this);
        }
    }

    public Color GetColor()
    {
        if (rendy == null)
        {
            rendy = transform.GetComponent<Renderer>();
        }

        return rendy.material.color;
    }

    public void SetTarget(MailboxBehaviour mailbox)
    {
        target = mailbox.gameObject;

        if (rendy == null)
        {
            rendy = transform.GetComponent<Renderer>();
        }

        rendy.material.color = mailbox.color;
    }

    private void OnCollisionEnter2D(Collision2D t)
    {
        if (t.gameObject.tag == "Mailbox" && target == t.gameObject)
        {
            gameManager.Deliver(this);
        }
    }

    public int GetScoreMultiplier()
    {
        if (timeSinceSpawn < 30)
        {
            return 4;
        }
        else if (timeSinceSpawn < 60)
        {
            return 2;
        }
        return 1;
    }
}
