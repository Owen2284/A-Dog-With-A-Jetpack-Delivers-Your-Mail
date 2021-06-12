using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : ConnectableBehaviour
{
    private bool preventControl = false;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (invincibilityTimeRemaining <= invincibilityTime / 2 && !preventControl)
        {
            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");

            body.AddForce(transform.up * v * body.mass * 10);
            body.AddForce(transform.right * h * body.mass * 10);
        }
    }

    public void DisableControl()
    {
        // Game is over, so prevent control from here on out
        preventControl = true;
    }
}
