using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public const int COLUMNS = 8;
    public const int ROWS = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);

    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    private void InitializeList()
    {
        gridPositions.Clear();

        for(int x = 1; x < COLUMNS - 1; x++)
        {
            for(int y = 1; y < ROWS - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    private void BoardSetup()
    {
        boardHolder = new GameObject().transform;

        for(int x = -1; x < COLUMNS + 1; x++)
        {
            for(int y = -1; y < ROWS + 1; y++)
            {
                GameObject newFloor = floorTiles[Random.Range(0, floorTiles.Length)];

                if(x == -1 || x == COLUMNS || y == -1 || y == ROWS)
                {
                    newFloor = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(newFloor, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    private Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];

        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    private void LayoutObjectAtRandom(GameObject[] tiles, int min, int max)
    {
        int count = Random.Range(min, max + 1);

        for(int i = 0; i < count; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tile = tiles[Random.Range(0, tiles.Length)];
            Instantiate(tile, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(COLUMNS - 1, ROWS - 1), Quaternion.identity);
    }

	// Use this for initialization
	private void Start () {
	
	}
	
	// Update is called once per frame
	private void Update () {
	
	}
}
