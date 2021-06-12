using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryBehaviour : ConnectableBehaviour
{
    private GameObject target;

    private float timeSinceSpawn = 0;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        timeSinceSpawn += Time.deltaTime;
    }

    public void SetTarget(MailboxBehaviour mailbox)
    {
        target = mailbox.gameObject;

        transform.GetComponent<Renderer>().material.color = mailbox.color;
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
        Debug.Log(timeSinceSpawn);

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
