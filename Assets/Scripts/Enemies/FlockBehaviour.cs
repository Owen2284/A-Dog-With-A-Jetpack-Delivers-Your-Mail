using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockBehaviour : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public int spawnCountMin = 1;
    public int spawnCountMax = 1;

    private List<BaseEnemyBehaviour> enemies;

    // Start is called before the first frame update
    void Start()
    {
        var numberToSpawn = Random.Range(spawnCountMin, spawnCountMax + 1);

        enemies = new List<BaseEnemyBehaviour>();

        for (var i = 0; i < numberToSpawn; i++)
        {
            var enemy = (Instantiate(enemyToSpawn, transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0), Quaternion.identity)).GetComponent<BaseEnemyBehaviour>();
            enemies.Add(enemy);
        }

        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
