using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBehaviour : BaseProjectileBehaviour
{
    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D t)
    {
        if (t.gameObject.tag == "Player" || t.gameObject.tag == "Delivery")
        {
            Debug.Log("Enemy projectile collision");

            t.gameObject.GetComponent<ConnectableBehaviour>().TakeDamage(t);

            Destroy(this.gameObject);
        }
    }
}
