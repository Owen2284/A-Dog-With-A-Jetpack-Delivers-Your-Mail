using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBehaviour : MonoBehaviour
{
    public GameObject target;

    private new ParticleSystem particleSystem;
    private float timeToLive;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        var psMain = particleSystem.main;
        timeToLive = psMain.startLifetime.constant;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            this.transform.position = target.transform.position;
        }

        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
