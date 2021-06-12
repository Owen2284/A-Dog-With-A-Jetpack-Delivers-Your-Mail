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

    public Vector2 gameplayArea = new Vector2(240, 150);

    public float initialTime = 60;
    public float bonusTimePerDelivery = 10;

    public float timeBetweenNewDeliveries = 30;

    public float alertRetentionTime = 5;

    public List<GameObject> deliveryPrefabs;

    private int score;
    private float remainingTime;
    private float timeUntilNewDeliveries;
    private PlayerBehaviour player;
    private List<DeliveryBehaviour> deliveries;
    private List<MailboxBehaviour> mailboxes;

    private List<AlertItem> alerts;

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

        // Show game start alert
        alerts = new List<AlertItem> {
            new AlertItem("Go!", alertRetentionTime)
        };
    }

    // Update is called once per frame
    void Update()
    {
        // Tick timers
        remainingTime = Mathf.Max(remainingTime - Time.deltaTime, 0);
        timeUntilNewDeliveries -= Time.deltaTime;

        // End game if timer has hit zero
        if (remainingTime == 0)
        {
            // TODO
        }

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
                        // Make connection
                        endOfChain.SetNextConnection(delivery);

                        // Display effect and play sound
                        // TODO
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

        // Time out old alerts
        for (var alertNum = alerts.Count - 1; alertNum >= 0; alertNum--)
        {
            var alert = alerts[alertNum];

            alert.TimeToLive -= Time.deltaTime;

            if (alert.TimeToLive <= 0)
            {
                alerts.Remove(alert);
            }
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
        var multiplier = delivery.GetScoreMultiplier();
        var scoreEarned = 1000 * multiplier;
        score += scoreEarned;

        // Remove delivery from list
        deliveries.Remove(delivery);

        // Display alert
        AddAlert($"Package delivered: +{scoreEarned}");

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

    public void AddAlert(string message, bool positive = true)
    {
        // Store alert
        alerts.Add(new AlertItem(message, alertRetentionTime));

        // Play sound
        // TODO
    }

    public List<string> GetAlerts()
    {
        return alerts.Select(x => x.Message).ToList();
    }

    public MinimapData GetMinimapData()
    {
        var data = new MinimapData();

        data.PlayerLocation = player.transform.position / gameplayArea;

        data.HomeLocation = new Vector2(0, 0);

        foreach (var mailbox in mailboxes)
        {
            data.MailboxLocations.Add((mailbox.transform.position / gameplayArea, mailbox.color));
        }

        return data;
    }
}

public class AlertItem
{
    public string Message { get; set; }

    public float TimeToLive { get; set; }

    public AlertItem(string message, float timeToLive)
    {
        Message = message;
        TimeToLive = timeToLive;
    }
}

public class MinimapData
{
    public Vector2 PlayerLocation { get; set; }
    public Vector2 HomeLocation { get; set; }
    public List<(Vector2, Color)> MailboxLocations { get; set; } = new List<(Vector2, Color)>();
}