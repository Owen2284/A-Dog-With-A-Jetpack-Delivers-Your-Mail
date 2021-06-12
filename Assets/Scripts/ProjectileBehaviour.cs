using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : BaseEntityBehaviour
{
    public float timeToLive = 3;

    private float currentTimeToLive;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        currentTimeToLive = timeToLive;
    }

    // Update is called once per frame
    void Update()
    {
        currentTimeToLive -= Time.deltaTime;
        if (currentTimeToLive <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D t)
    {
        if (t.gameObject.tag == "Enemy")
        {
            t.gameObject.GetComponent<BaseEnemyBehaviour>().TakeDamage();

            Destroy(this.gameObject);
        }
    }
}
