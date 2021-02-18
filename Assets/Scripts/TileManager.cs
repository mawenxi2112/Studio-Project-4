using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap[] map;

    [SerializeField]
    private List<TileData> tileDatas;

    // Key = Variations, Value = TileType
    private Dictionary<TileBase, TileData> datafromTiles;

    private List<Vector2>[] ListOfTilePosition;

    private void Awake()
    {
        datafromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                datafromTiles.Add(tile, tileData);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ListOfTilePosition = new List<Vector2>[map.Length];

        FindAllTilePosition(-50, 50, -50, 50);

        Debug.Log("Number of TileMap in Listarray" + ListOfTilePosition.Length);

        for (int i = 0; i < ListOfTilePosition.Length; i++)
		{
            Debug.Log("Number of tiles in " + i + " tilearray: " + ListOfTilePosition[i].Count);
		}

        List<Vector2> watertilemap = ReturnListInArray(2);

        for (int i = 0; i < watertilemap.Count; i++)
		{
            Debug.Log(i + " position: " + watertilemap[i]);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindAllTilePosition(int xStartBound, int xEndBound,int yStartBound, int yEndBound)
	{
        for (int i = 0; i < map.Length; i++)
		{
            ListOfTilePosition[i] = new List<Vector2>();

            for (int y = yStartBound; y < yEndBound; y++)
            {
                for (int x = xStartBound; x < xEndBound; x++)
                {
                    TileBase tile = map[i].GetTile(new Vector3Int(x, y, 0));
                    if (tile != null)
                    {
                        ListOfTilePosition[i].Add(new Vector2(x + 0.5f, y + 0.5f));
                    }
                }
            }
        }
	}

    List<Vector2> ReturnListInArray(int id)
	{
        return ListOfTilePosition[id];
	}
}
