using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviour : BaseEnemyBehaviour
{
    public int attackDistance = 5;
    public int homeStrayDistance = 10;

    private Vector3 homePoint;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        homePoint = transform.position;
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();

        var distanceToPlayer = Vector2.Distance(
            this.transform.position,
            player.transform.position
        );

        var distanceFromHome = Vector2.Distance(
            this.transform.position,
            homePoint
        );

        var pointToTravelTo = homePoint;
        if (distanceToPlayer < attackDistance && distanceFromHome < homeStrayDistance)
        {
            pointToTravelTo = player.transform.position;
        }

        var directionOfTravel = pointToTravelTo - transform.position;
        body.AddForce(directionOfTravel * moveSpeed * body.mass);
    }
}
