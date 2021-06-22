using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCornerBehaviour : MonoBehaviour
{
    public BoundaryCornerBehaviour nextCorner;

    // Start is called before the first frame update
    void Start()
    {
        var lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.SetPositions(new List<Vector3> {
            new Vector3(
                this.transform.position.x,
                this.transform.position.y,
                10
            ),
            new Vector3(
                nextCorner.transform.position.x,
                nextCorner.transform.position.y,
                10
            )
        }.ToArray());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
