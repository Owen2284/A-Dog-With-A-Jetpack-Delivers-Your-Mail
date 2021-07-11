using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferfishBehaviour : BaseEnemyBehaviour
{
    private Vector3 homePoint;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        homePoint = transform.position;
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();

        var pointToTravelTo = homePoint;

        var directionOfTravel = pointToTravelTo - transform.position;
        body.AddForce(directionOfTravel * moveSpeed * body.mass);
    }

    protected new void OnCollisionEnter2D(Collision2D t)
    {
        if (t.gameObject.tag == "Player" || t.gameObject.tag == "Delivery")
        {
            // Damage player
            t.gameObject.GetComponent<ConnectableBehaviour>().TakeDamage(this, t);
            TakeDamage();
        }
    }
}
