using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileBehaviour : BaseProjectileBehaviour
{
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
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
