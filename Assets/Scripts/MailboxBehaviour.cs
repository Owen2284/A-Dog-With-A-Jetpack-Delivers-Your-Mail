using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailboxBehaviour : BaseEntityBehaviour
{
    public Color color = Color.white;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        transform.GetComponent<Renderer>().material.color = color;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
