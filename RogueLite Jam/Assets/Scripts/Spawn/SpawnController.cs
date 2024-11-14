using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour
{
    private GameObject playerObj;
    private Vector2 playerPos;

    [SerializeField] private GameObject[] spawnables;
    private bool spawning = true;
    private float spawnTime = 1.5f;

    private float xBound = 3f;
    private float yBound = 3f;
    private float noSpawnRadius = 5;
    

    void Start()
    {
        playerObj = GameObject.Find("Player");
        StartCoroutine(SpawnTimer());
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GetPlayerPos();
    }

    public Vector2 GetPlayerPos()
    {
        return playerObj.transform.position;
    }

    public Vector2 GenRandomSpawnPos()
    {
        float xPos = Random.Range(-xBound, xBound);
        float yPos = Random.Range(-yBound, yBound);
        if (xPos < 0 )
        {
            xPos -= noSpawnRadius;
            if (xPos - playerPos.x < noSpawnRadius)
            {
                xPos -= noSpawnRadius;
            }
        }
        if (xPos > 0)
        {
            xPos += noSpawnRadius;
            if (xPos - playerPos.x < noSpawnRadius) {
                xPos += noSpawnRadius;
            }
        }
        if (yPos < 0)
        {
            yPos -= noSpawnRadius;
            if (yPos - playerPos.x < noSpawnRadius)
            {
                yPos -= noSpawnRadius;
            }
        }
        if (yPos > 0)
        {
            yPos += noSpawnRadius;
            if (yPos - playerPos.x < noSpawnRadius)
            {
                yPos += noSpawnRadius;
            }
        }
        
        //Debug.Log($"X: {xPos} Y: {yPos}");  
        return new Vector2 (xPos, yPos);
    }

    public GameObject GenRandomSpawnable()
    {
        int idx = Random.Range(0, spawnables.Length);
        return spawnables[idx];
    }

    public GameObject SpawnEnemyAtRandomLoc()
    {
        GameObject toSpawn = GenRandomSpawnable();
        Vector2 spawnPos = GenRandomSpawnPos();
        Quaternion rotation = Quaternion.identity;
        GameObject spawned = Instantiate(toSpawn, spawnPos, rotation);
        return spawned;
    } 

    public IEnumerator SpawnTimer()
    {
        while (spawning)
        {
            yield return new WaitForSeconds(spawnTime);
            SpawnEnemyAtRandomLoc();
        }
    }
}
