using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectableBehaviour : RigidBodyBehaviour
{
    protected ConnectableBehaviour previousConnectedItem;
    protected ConnectableBehaviour nextConnectedItem;
    private LineRenderer nextItemLine;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        nextItemLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (nextConnectedItem != null)
        {
            nextItemLine.SetPositions(new List<Vector3> {
                this.transform.position,
                nextConnectedItem.transform.position
            }.ToArray());
            Debug.Log("Trying to draw line");
        }
    }

    public void SetNextConnection(ConnectableBehaviour that)
    {
        // Link as data
        this.nextConnectedItem = that;
        that.previousConnectedItem = this;
    }

    public List<ConnectableBehaviour> GetChain()
    {
        var chain = new List<ConnectableBehaviour> {
            this
        };

        if (nextConnectedItem != null)
        {
            chain.AddRange(nextConnectedItem.GetChain());
        }

        return chain;
    }
}
