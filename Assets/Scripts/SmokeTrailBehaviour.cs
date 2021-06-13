using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeTrailBehaviour : MonoBehaviour
{
    public float trailDuration = 0.4f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        trailDuration -= Time.deltaTime;

        if (trailDuration <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
