using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManagerBehaviour : MonoBehaviour
{
    public int initialDeliveryCount = 3;

    public float connectionDistance = 1;
    public int maxChainLength = 5;

    public Vector2 gameplayArea = new Vector2(240, 150);

    public List<GameObject> flockTypes = new List<GameObject>();
    public int initialFlocks = 6;
    public int flocksPerDelivery = 2;

    public float initialTime = 60;
    public float bonusTimePerDelivery = 10;

    public float timeBetweenNewDeliveries = 30;

    public float alertRetentionTime = 5;

    public GameObject homeIsland;

    public List<Vector2> islandPositions;

    public List<GameObject> islandPrefabs;
    public List<GameObject> deliveryPrefabs;

    public AudioSource newDeliveriesSound;
    public AudioSource chainConnectSound;
    public AudioSource deliveryMadeSound;

    private int score;
    private int deliveryTotal;
    private float remainingTime;
    private float timeUntilNewDeliveries;
    private PlayerBehaviour player;
    private List<DeliveryBehaviour> deliveries;
    private List<MailboxBehaviour> mailboxes;

    private List<AlertItem> alerts = new List<AlertItem>();

    private LineRenderer lineRenderer;

    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        // Find components
        lineRenderer = GetComponent<LineRenderer>();

        // Find player
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        // Generate islands
        foreach (var islandPosition in islandPositions)
        {
            var selectedIslandPrefab = islandPrefabs[Random.Range(0, islandPrefabs.Count)];

            var island = Instantiate(selectedIslandPrefab,
                new Vector3(islandPosition.x + Random.Range(-15f, 15f), islandPosition.y + Random.Range(-15f, 15f), 5),
                Quaternion.identity);

            islandPrefabs.Remove(selectedIslandPrefab);
        }

        // Find mailboxes
        mailboxes = GameObject.FindGameObjectsWithTag("Mailbox")
            .Select(x => x.GetComponent<MailboxBehaviour>()).ToList();

        // Generate initial deliveries
        deliveries = new List<DeliveryBehaviour>();
        GenerateDeliveries(initialDeliveryCount, false);

        // Generate initial enemies
        GenerateFlocks(initialFlocks);

        // Start timers and initialise score
        timeUntilNewDeliveries = timeBetweenNewDeliveries;
        remainingTime = initialTime;
        score = 0;
        deliveryTotal = 0;

        // Show game start alert
        AddAlert("Go!");
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
            gameOver = true;
            player.DisableControl();
        }

        // If game isn't over, run systems as usual
        if (!gameOver)
        {
            // Handle pausing
            if (Input.GetKeyDown("escape"))
            {
                Time.timeScale = 1 - Time.timeScale;
            }

            // Link processing
            var potentialNextLink = GetPotentialNextLink();

            // Show notifier on dog if potential link present
            player.ShowExclaim(potentialNextLink != null);

            if (potentialNextLink != null)
            {
                // Get chain and current end of chain
                var chain = player.GetChain();
                var endOfChain = chain.Last();

                // Render preview line
                lineRenderer.enabled = true;
                lineRenderer.SetPositions(new List<Vector3> {
                    new Vector3(
                        endOfChain.transform.position.x,
                        endOfChain.transform.position.y,
                        -1
                    ),
                    new Vector3(
                        potentialNextLink.transform.position.x,
                        potentialNextLink.transform.position.y,
                        -1
                    )
                }.ToArray());

                // Check to see if the connect key has been hit
                if (Input.GetKeyDown("space") && !IsPaused())
                {
                    // If it has, then make connection
                    endOfChain.SetNextConnection(potentialNextLink);

                    // Play sound
                    chainConnectSound.Play();
                }
            }
            else
            {
                // Hide line renderer when no preview to display
                lineRenderer.enabled = false;
            }

            // Create new deliveries when timer expires / when no deliveries remain
            if (timeUntilNewDeliveries <= 0 || deliveries.Count == 0)
            {
                // Create new deliveries
                GenerateDeliveries(initialDeliveryCount);

                // Reset timer
                timeUntilNewDeliveries = timeBetweenNewDeliveries;
            }
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

    private void GenerateDeliveries(int count, bool alert = true)
    {
        for (var i = 0; i < count; i++)
        {
            var deliveryPrefab = deliveryPrefabs[Random.Range(0, deliveryPrefabs.Count)];
            var mailbox = mailboxes[Random.Range(0, mailboxes.Count)];
            var delivery = Instantiate(deliveryPrefab, new Vector3(2 + Random.Range(-2f, 2f), 10f + Random.Range(-2f, 2f), 0), Quaternion.identity).GetComponent<DeliveryBehaviour>();
            delivery.SetTarget(mailbox);
            deliveries.Add(delivery);
        }

        if (alert)
        {
            AddAlert("New deliveries available on Mail Island!");
            newDeliveriesSound.Play();
        }
    }

    private void GenerateFlocks(int count)
    {
        var terrain = GameObject.FindGameObjectsWithTag("Terrain");

        for (var i = 0; i < count; i++)
        {
            // Determine spawn location (prevent spawning too close to an island)
            Vector2 flockPosition = new Vector2(0, 0);
            bool safeToSpawn = false;
            do
            {
                flockPosition = new Vector2(
                    Random.Range(-1 * gameplayArea.x, gameplayArea.x),
                    Random.Range(-1 * gameplayArea.y, gameplayArea.y)
                );

                bool notNearTerrain = true;
                foreach (var ter in terrain)
                {
                    var distanceToTerrain = Vector2.Distance(
                        ter.transform.position,
                        flockPosition
                    );

                    if (distanceToTerrain < 20)
                    {
                        notNearTerrain = false;
                    }
                }

                safeToSpawn = notNearTerrain;
            } while (!safeToSpawn);

            // Instantiate flock
            var flockType = flockTypes[Random.Range(0, flockTypes.Count)];
            var flock = Instantiate(flockType, flockPosition, Quaternion.identity).GetComponent<DeliveryBehaviour>();
        }
    }

    public void Deliver(DeliveryBehaviour delivery)
    {
        // Remove the delivery from the chain
        delivery.BreakChain();

        // Only handle these when the game is still going
        if (!gameOver)
        {
            // Adjust score/time
            remainingTime += bonusTimePerDelivery;
            var multiplier = delivery.GetScoreMultiplier();
            var scoreEarned = 1000 * multiplier;
            score += scoreEarned;

            // Increment delivery total
            deliveryTotal += 1;

            // Display alert
            AddAlert($"Delivery made! +{scoreEarned}");
            deliveryMadeSound.Play();
        }

        // Remove delivery from list
        deliveries.Remove(delivery);

        // Destroy delivery
        Destroy(delivery.gameObject);

        // Add new enemies to the map
        GenerateFlocks(flocksPerDelivery);
    }

    public void HandleLostDelivery(DeliveryBehaviour delivery)
    {
        // Remove the delivery from the chain
        delivery.BreakChain();

        // Remove delivery from list
        deliveries.Remove(delivery);

        // Destroy delivery
        Destroy(delivery.gameObject);

        // Send alert if not already present
        var alertText = "Delivery lost...";
        if (!alerts.Exists(x => x.Message == alertText))
        {
            AddAlert(alertText);
        }
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

    public int GetDeliveryTotal()
    {
        return deliveryTotal;
    }

    public void AddAlert(string message)
    {
        // Store alert
        alerts.Add(new AlertItem(message, alertRetentionTime));
    }

    public List<string> GetAlerts()
    {
        return alerts.Select(x => x.Message).ToList();
    }

    public MinimapData GetMinimapData()
    {
        var data = new MinimapData();

        data.PlayerLocation = player.transform.position / gameplayArea;

        data.HomeLocation = homeIsland.transform.position / gameplayArea;

        foreach (var mailbox in mailboxes)
        {
            data.MailboxLocations.Add((mailbox.transform.position / gameplayArea, mailbox.color));
        }

        var chain = player.GetChain();
        chain.Remove(player);

        data.MaiboxesToHighlight = chain.Select(x => ((DeliveryBehaviour)x).GetColor()).ToList();

        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = Camera.main.aspect * halfHeight;

        data.CameraOffsetMax = new Vector2(halfWidth, halfHeight);
        data.CameraOffsetMin = new Vector2(-halfWidth, -halfHeight);

        return data;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public bool IsPaused()
    {
        return Time.timeScale == 0;
    }

    public void TimePenalty(float penalty)
    {
        remainingTime = Mathf.Max(remainingTime - penalty, 0);
    }

    public DeliveryBehaviour GetPotentialNextLink()
    {
        // Get the item currently at the end of the chain of items
        var chain = player.GetChain();
        var endOfChain = chain.Last();

        // Return null if chain full
        if (chain.Count > maxChainLength)
        {
            return null;
        }

        // Find closest delivery to end of chain
        DeliveryBehaviour closestSoFar = null;
        float closestDistance = connectionDistance + 1f;
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

            // If close enough, and closest one so far, then save in running variables
            if (distance < connectionDistance && distance < closestDistance)
            {
                closestSoFar = delivery;
                closestDistance = distance;
            }
        }

        // Return the closest delivery that's within range (if none are, null is returned)
        return closestSoFar;
    }

    public void AwardScore(int scoreAwarded)
    {
        score += scoreAwarded;
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
    public List<Color> MaiboxesToHighlight { get; set; }
    public Vector2 CameraOffsetMax { get; set; }
    public Vector2 CameraOffsetMin { get; set; }
}