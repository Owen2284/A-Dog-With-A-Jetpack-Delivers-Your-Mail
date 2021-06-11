using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyBehaviour : MonoBehaviour
{
    protected Rigidbody2D body;

    // Start is called before the first frame update
    protected void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
