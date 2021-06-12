using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManagerBehaviour : MonoBehaviour
{
    public int connectionDistance = 1;

    public float remainingTime = 60;

    public List<GameObject> deliveryPrefabs;

    private int score;
    private PlayerBehaviour player;
    private List<DeliveryBehaviour> deliveries;
    private List<MailboxBehaviour> mailboxes;

    // Start is called before the first frame update
    void Start()
    {
        // Find player
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        // Find mailboxes
        mailboxes = GameObject.FindGameObjectsWithTag("Mailbox")
            .Select(x => x.GetComponent<MailboxBehaviour>()).ToList();

        // Generate initial deliveries
        deliveries = new List<DeliveryBehaviour>();
        for (var i = 0; i < 3; i++)
        {
            var deliveryPrefab = deliveryPrefabs[Random.Range(0, deliveryPrefabs.Count)];
            var mailbox = mailboxes[Random.Range(0, mailboxes.Count)];
            var delivery = Instantiate(deliveryPrefab, new Vector2(i - 2, 0), Quaternion.identity).GetComponent<DeliveryBehaviour>();
            delivery.SetTarget(mailbox);
            deliveries.Add(delivery);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get the item currently at the end of the chain of items
        var chain = player.GetChain();
        var endOfChain = chain.Last();

        // Check to see if the connect key has been hit
        if (Input.GetKeyDown("space"))
        {
            foreach (var delivery in deliveries)
            {
                // Skip if already in chain
                if (chain.Contains(delivery))
                {
                    continue;
                }

                // Determine distance to delivery
                var distance = Vector2.Distance(
                    endOfChain.transform.position,
                    delivery.transform.position
                );

                // If close enough, connect
                if (distance < connectionDistance)
                {
                    endOfChain.SetNextConnection(delivery);
                    break;
                }
            }
        }

        remainingTime -= Time.deltaTime;
    }

    public void Deliver(DeliveryBehaviour delivery)
    {
        // Remove the delivery from the chain
        delivery.BreakChain();

        // Adjust score/time
        remainingTime += 20f;
        score += 1000 * delivery.GetScoreMultiplier();

        // TODO: Replace with GUI display
        Debug.Log("Score: " + score);

        // Remove delivery from list
        deliveries.Remove(delivery);

        // Destroy delivery
        Destroy(delivery.gameObject);
    }

    public PlayerBehaviour GetPlayer()
    {
        return player;
    }
}
