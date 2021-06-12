using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManagerBehaviour : MonoBehaviour
{
    public int initialDeliveryCount = 3;

    public int connectionDistance = 1;
    public int maxChainLength = 5;

    // TODO: Move to camera script?
    public float cameraMinZoom = 2;
    public float cameraMaxZoom = 16;

    public float initialTime = 60;
    public float bonusTimePerDelivery = 10;

    public float timeBetweenNewDeliveries = 30;

    public List<GameObject> deliveryPrefabs;

    private int score;
    private float remainingTime;
    private float timeUntilNewDeliveries;
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
        GenerateNewDeliveries(initialDeliveryCount);

        // Start timers and initialise score
        timeUntilNewDeliveries = timeBetweenNewDeliveries;
        remainingTime = initialTime;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Tick timers
        remainingTime -= Time.deltaTime;
        timeUntilNewDeliveries -= Time.deltaTime;

        // Get the item currently at the end of the chain of items
        var chain = player.GetChain();
        var endOfChain = chain.Last();

        // Check to see if the connect key has been hit
        if (Input.GetKeyDown("space"))
        {
            if (chain.Count <= maxChainLength)
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
        }

        // Handle camera zoom
        Camera.main.orthographicSize = Mathf.Min(Mathf.Max(Camera.main.orthographicSize - Input.mouseScrollDelta.y, cameraMinZoom), cameraMaxZoom);

        // Create new deliveries when timer expires / when no deliveries remain
        if (timeUntilNewDeliveries <= 0 || deliveries.Count == 0)
        {
            // Create new deliveries
            GenerateNewDeliveries(initialDeliveryCount);

            // Reset timer
            timeUntilNewDeliveries = timeBetweenNewDeliveries;
        }
    }

    private void GenerateNewDeliveries(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var deliveryPrefab = deliveryPrefabs[Random.Range(0, deliveryPrefabs.Count)];
            var mailbox = mailboxes[Random.Range(0, mailboxes.Count)];
            var delivery = Instantiate(deliveryPrefab, new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), Quaternion.identity).GetComponent<DeliveryBehaviour>();
            delivery.SetTarget(mailbox);
            deliveries.Add(delivery);
        }
    }

    public void Deliver(DeliveryBehaviour delivery)
    {
        // Remove the delivery from the chain
        delivery.BreakChain();

        // Adjust score/time
        remainingTime += bonusTimePerDelivery;
        score += 1000 * delivery.GetScoreMultiplier();

        // Remove delivery from list
        deliveries.Remove(delivery);

        // Destroy delivery
        Destroy(delivery.gameObject);
    }

    public PlayerBehaviour GetPlayer()
    {
        return player;
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    public int GetScore()
    {
        return score;
    }
}
