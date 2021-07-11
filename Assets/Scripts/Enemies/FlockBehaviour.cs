using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlockBehaviour : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public int spawnCountMin = 1;
    public int spawnCountMax = 1;

    private List<BaseEnemyBehaviour> enemies;
    private AudioSource deathNoise;

    private bool allGone = false;
    private float destroyTimer = 5;

    // Start is called before the first frame update
    void Start()
    {
        deathNoise = GetComponent<AudioSource>();

        var numberToSpawn = Random.Range(spawnCountMin, spawnCountMax + 1);

        enemies = new List<BaseEnemyBehaviour>();

        for (var i = 0; i < numberToSpawn; i++)
        {
            var enemy = (Instantiate(enemyToSpawn, transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0), Quaternion.identity)).GetComponent<BaseEnemyBehaviour>();

            if (deathNoise != null)
            {
                enemy.deathSound = deathNoise;
            }

            enemies.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!allGone && (enemies.Count == 0 || enemies.Count(x => x == null) == enemies.Count))
        {
            allGone = true;
        }

        if (allGone)
        {
            destroyTimer -= Time.deltaTime;

            if (destroyTimer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
