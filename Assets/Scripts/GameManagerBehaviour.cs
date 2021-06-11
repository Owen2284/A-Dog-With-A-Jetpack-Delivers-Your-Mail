using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManagerBehaviour : MonoBehaviour
{
    public int connectionDistance = 1;

    private PlayerBehaviour player;
    private List<DeliverableBehaviour> deliverables;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        // TODO: Replace eventually
        deliverables = GameObject.FindGameObjectsWithTag("Deliverable")
            .Select(x => x.GetComponent<DeliverableBehaviour>()).ToList();
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
            Debug.Log("Chain length: " + chain.Count);
            foreach (var deliverable in deliverables)
            {
                // Skip if already in chain
                if (chain.Contains(deliverable))
                {
                    Debug.Log("Already in chain");
                    continue;
                }

                // Determine distance to deliverable
                var distance = Vector2.Distance(
                    endOfChain.transform.position,
                    deliverable.transform.position
                );

                // If close enough, connect
                Debug.Log(distance);
                if (distance < connectionDistance)
                {
                    endOfChain.SetNextConnection(deliverable);
                    break;
                }
            }
        }
    }
}
