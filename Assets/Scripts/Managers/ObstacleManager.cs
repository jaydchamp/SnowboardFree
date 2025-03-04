using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject[] stonePrefabs;
    public GameObject[] treePrefabs;
    public GameObject skierPrefab;
    public GameObject treeLinePrefab; 

    private float spawnRangeX = 19;
    private float spawnPosY = -4;

    private float startingDelay = 2.0f; //time before first object is seen

    private float stoneAdjustedInterval;
    private float stoneUpdatingInterval;
    private float stoneBaseInterval = 3.5f; //how long between each spawned stone

    private float treeAdjustedInterval;
    private float treeUpdatingInterval;
    private float treeBaseInterval = 1.5f; //how long between each spawned tree

    private float skierAdjustedInterval;
    private float skierUpdatingInterval;
    private float skierBaseInterval = 8.0f; //length between spawned skiers 

    public bool spawningStatus;
    private float speedRatio;

    //public ScrollingBackground scrollingBackground;
    public static ObstacleManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        spawningStatus = true;
        stoneUpdatingInterval = stoneBaseInterval;
        treeUpdatingInterval = treeBaseInterval;
        skierUpdatingInterval = skierBaseInterval;

        InvokeRepeating("SpawnRandomSkier", startingDelay, skierBaseInterval);
        InvokeRepeating("SpawnTreeLine", 0.0f, 0.3f);
    }

    void Update()
    {
        //updating spawning rate
        speedRatio = ScrollingBackground.Instance.getMaxSpeed() / ScrollingBackground.Instance.getScrollSpeed();
        
        //when player stops moving down the mountain, stop spawning obstacles
        if (ScrollingBackground.Instance.speed <= 0)
        {
            spawningStatus = false;
        }
        else
        {
            spawningStatus = true;
        }

        UpdateStone();
        UpdateTree();
        //UpdateSkier();
    }

    public void SetSpawningStatus(bool set)
    {
        spawningStatus = set;
    }

    //spawn leftside + rightside skier
    void SpawnRandomSkier()
    {
        if(spawningStatus)
        {
            Vector3 spawnPosition = new Vector2(Random.Range(-15,15), 38);//Vector2(25, Random.Range(15,22));
            Instantiate(skierPrefab, spawnPosition, skierPrefab.transform.rotation);
        }
    }
    void SpawnRandomStone()
    {
        if (spawningStatus)
        {
            int stoneIndex = Random.Range(0, stonePrefabs.Length);
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), spawnPosY, 0);

            Instantiate(stonePrefabs[stoneIndex], /*new Vector3(0,-4,0) */spawnPosition, stonePrefabs[stoneIndex].transform.rotation);
        }
    }
    void SpawnRandomTree()
    {
        if (spawningStatus)
        {
            int treeIndex = Random.Range(0, treePrefabs.Length);
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), spawnPosY, 0);

            Instantiate(treePrefabs[treeIndex], /*new Vector3(0,-4,0) */spawnPosition, treePrefabs[treeIndex].transform.rotation);
        }
    }

    public void UpdateStone()
    {
        stoneAdjustedInterval = (stoneBaseInterval * speedRatio) / 3;

        //if the counter is still counting down
        if (stoneUpdatingInterval > 0 && spawningStatus)
        {
            //continue counting down
            stoneUpdatingInterval -= Time.deltaTime;

            //if 0
            if (stoneUpdatingInterval <= 0)
            {
                //time between spawning is done, spawn a new
                Invoke("SpawnRandomStone", 0.0f);

                //reset spawning to current spawn rate
                stoneUpdatingInterval = stoneAdjustedInterval;
            }
        }
    }

    public void UpdateTree()
    {
        treeAdjustedInterval = (treeBaseInterval * speedRatio) / 3;

        //if the counter is still counting down
        if (treeUpdatingInterval > 0 && spawningStatus)
        {
            //continue counting down
            treeUpdatingInterval -= Time.deltaTime;

            //if 0
            if (treeUpdatingInterval <= 0)
            {
                //time between spawning is done, spawn a new
                Invoke("SpawnRandomTree", 0.0f);

                //reset spawning to current spawn rate
                treeUpdatingInterval = treeAdjustedInterval;
            }
        }
    }

    public void SpawnTreeLine()
    {
            //left side
            Vector2 spawnPos = new Vector2(-20.0f, 0.5f);
            Instantiate(treeLinePrefab, spawnPos, treeLinePrefab.transform.rotation);

            //right side
            spawnPos = new Vector2(20.0f, 0.5f);
            Instantiate(treeLinePrefab, spawnPos, treeLinePrefab.transform.rotation);
    }

/*    public void UpdateSkier()
    {
        skierAdjustedInterval = (skierBaseInterval * speedRatio) / 3;

        //if the counter is still counting down
        if (skierUpdatingInterval > 0 && spawningStatus)
        {
            //continue counting down
            skierUpdatingInterval -= Time.deltaTime;

            //if 0
            if (skierUpdatingInterval <= 0)
            {
                //time between spawning is done, spawn a new
                Invoke("SpawnRandomSkier", 0.0f);

                //reset spawning to current spawn rate
                skierUpdatingInterval = skierAdjustedInterval;
            }
        }
    }
*/}
