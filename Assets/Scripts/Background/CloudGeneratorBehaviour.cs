using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGeneratorBehaviour : MonoBehaviour
{
    public List<GameObject> cloudTypes = new List<GameObject>();
    public int cloudCount = 30;

    public float xMin = -30f;
    public float xMax = 30f;
    public float yMin = -30f;
    public float yMax = 30f;

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < cloudCount; i++)
        {
            // Pick a cloud and instantiate
            var cloudType = cloudTypes[Random.Range(0, cloudTypes.Count)];
            var cloudX = Random.Range(xMin, xMax);
            var cloudY = Random.Range(yMin, yMax);
            var cloudZ = transform.position.z;
            var cloud = Instantiate(cloudType, new Vector3(cloudX, cloudY, cloudZ), Quaternion.identity, transform).GetComponent<CloudBehaviour>();

            // Tweak cloud speed, and set X limits
            cloud.speed += Random.Range(-0.1f, 0.3f);
            cloud.startX = xMax;
            cloud.endX = xMin;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
