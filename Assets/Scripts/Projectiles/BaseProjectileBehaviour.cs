using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectileBehaviour : BaseEntityBehaviour
{
    public float timeToLive = 3;

    private float currentTimeToLive;

    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();

        currentTimeToLive = timeToLive;
    }

    // Update is called once per frame
    protected void Update()
    {
        currentTimeToLive -= Time.deltaTime;
        if (currentTimeToLive <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
