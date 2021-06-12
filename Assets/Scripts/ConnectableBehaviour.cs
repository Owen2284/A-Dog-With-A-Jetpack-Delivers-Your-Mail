using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectableBehaviour : BaseEntityBehaviour
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
    }

    public void SetNextConnection(ConnectableBehaviour that)
    {
        // Link as data
        this.nextConnectedItem = that;
        that.previousConnectedItem = this;

        // Link with line
        nextItemLine.enabled = true;
        nextItemLine.SetPositions(new List<Vector3> {
            this.transform.position,
            nextConnectedItem.transform.position
        }.ToArray());

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

    public void BreakChain(bool violent = false)
    {
        // Safely disconnect from the previous item
        if (previousConnectedItem != null)
        {
            previousConnectedItem.nextConnectedItem = null;
            previousConnectedItem.joint.enabled = false;
            previousConnectedItem.joint.connectedBody = null;
            previousConnectedItem.nextItemLine.enabled = false;
        }

        // Carry on down the line
        if (nextConnectedItem != null)
        {
            nextConnectedItem.BreakChain();
        }

        // Apply random force if it's a violent break
        if (violent)
        {
            body.AddForce(new Vector2(Random.Range(-10, 11), Random.Range(-10, 11)));
        }
    }
}
