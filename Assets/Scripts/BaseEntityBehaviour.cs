using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntityBehaviour : MonoBehaviour
{
    protected Rigidbody2D body;
    protected GameManagerBehaviour gameManager;

    // Start is called before the first frame update
    protected void Start()
    {
        body = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
