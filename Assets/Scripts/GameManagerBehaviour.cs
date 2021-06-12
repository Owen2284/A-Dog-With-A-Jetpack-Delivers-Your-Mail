using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManagerBehaviour : MonoBehaviour
{
    public int connectionDistance = 1;

    public float remainingTime = 60;

    public GameObject deliverablePrefab;

    private int score;
    private PlayerBehaviour player;
    private List<DeliverableBehaviour> deliverables;
    private List<MailboxBehaviour> mailboxes;

    // Start is called before the first frame update
    void Start()
    {
        // Find player
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        // Find mailboxes
        mailboxes = GameObject.FindGameObjectsWithTag("Mailbox")
            .Select(x => x.GetComponent<MailboxBehaviour>()).ToList();

        // Generate initial deliverables
        deliverables = new List<DeliverableBehaviour>();
        for (var i = 0; i < 3; i++)
        {
            var deliverable = Instantiate(deliverablePrefab, new Vector2(i - 2, 0), Quaternion.identity).GetComponent<DeliverableBehaviour>();
            deliverable.SetTarget(mailboxes[Random.Range(0, mailboxes.Count)]);
            deliverables.Add(deliverable);
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
            foreach (var deliverable in deliverables)
            {
                // Skip if already in chain
                if (chain.Contains(deliverable))
                {
                    continue;
                }

                // Determine distance to deliverable
                var distance = Vector2.Distance(
                    endOfChain.transform.position,
                    deliverable.transform.position
                );

                // If close enough, connect
                if (distance < connectionDistance)
                {
                    endOfChain.SetNextConnection(deliverable);
                    break;
                }
            }
        }

        remainingTime -= Time.deltaTime;
    }

    public void Deliver(DeliverableBehaviour delivery)
    {
        // Remove the delivery from the chain
        delivery.BreakChain();

        // Adjust score/time
        remainingTime += 20f;
        score += 1000 * delivery.GetScoreMultiplier();

        // TODO: Replace with GUI display
        Debug.Log("Score: " + score);

        // Remove delivery from list
        deliverables.Remove(delivery);

        // Destroy delivery
        Destroy(delivery.gameObject);
    }
}
