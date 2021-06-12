using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBehaviour : MonoBehaviour
{
    public float speed = 0.2f;

    public float startX;
    public float endX;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);

        if (transform.position.x < endX)
        {
            Vector3 position = new Vector3(startX, transform.position.y, transform.position.z);
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            Instantiate(this, position, rotation);

            Destroy(this.gameObject);
        }
    }
}
