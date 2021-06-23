using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailboxBehaviour : BaseEntityBehaviour
{
    public Color color = Color.white;
    public ParticleSystem confetti;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        transform.GetComponent<Renderer>().material.color = color;

        var psMain = confetti.main;
        psMain.startColor = new ParticleSystem.MinMaxGradient(
            new Color(color.r * 0.8f, color.g * 0.8f, color.b * 0.8f),
            new Color(color.r * 1.5f, color.g * 1.5f, color.b * 1.5f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D t)
    {
        if (t.gameObject.tag == "Delivery")
        {
            var delivery = t.gameObject.GetComponent<DeliveryBehaviour>();

            if (delivery.HasTarget(this))
            {
                gameManager.Deliver(delivery);
                confetti.Play();
            }
        }
    }
}
