using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : ConnectableBehaviour
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        body.AddForce(transform.up * v * 10);
        body.AddForce(transform.right * h * 10);
    }
}
