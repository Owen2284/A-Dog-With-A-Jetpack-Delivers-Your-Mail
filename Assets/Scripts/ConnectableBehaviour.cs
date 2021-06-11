using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectableBehaviour : RigidBodyBehaviour
{
    protected ConnectableBehaviour previousConnectedItem;
    protected ConnectableBehaviour nextConnectedItem;
    private LineRenderer nextItemLine;
    private Joint2D joint;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        nextItemLine = GetComponent<LineRenderer>();
        joint = GetComponent<Joint2D>();
        Debug.Log(joint == null);
    }

    // Update is called once per frame
    protected void Update()
    {
        if (nextConnectedItem != null)
        {
            // Set up the line coordinates
            nextItemLine.SetPositions(new List<Vector3> {
                this.transform.position,
                nextConnectedItem.transform.position
            }.ToArray());
        }
        else
        {
            joint.enabled = false;
        }
    }

    public void SetNextConnection(ConnectableBehaviour that)
    {
        // Link as data
        this.nextConnectedItem = that;
        that.previousConnectedItem = this;

        // Link with hinge
        joint.enabled = true;
        joint.connectedBody = that.gameObject.GetComponent<Rigidbody2D>();
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
