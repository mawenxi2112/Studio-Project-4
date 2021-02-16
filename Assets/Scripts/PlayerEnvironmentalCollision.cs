using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerEnvironmentalCollision : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private Tilemap[] map;

    [SerializeField]
    private List<TileData> tileDatas;

    // Key = Variations, Value = TileType
    private Dictionary<TileBase, TILE_TYPE> datafromTiles;

    private TileBase currentTile;

    private void Awake()
    {
        datafromTiles = new Dictionary<TileBase, TILE_TYPE>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                datafromTiles.Add(tile, tileData.tiletype);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        for (int i = 0; i < GameObject.Find("Grid").transform.childCount; i++)
		{
            map[i] = GameObject.Find("Grid").transform.GetChild(i).GetComponent<Tilemap>();
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Vector3 hitPosition = Vector3.zero;
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);

            hitPosition.x = contact.point.x - 0.01f * contact.normal.x;
            hitPosition.y = contact.point.y - 0.01f * contact.normal.y;

            for (int i = 0; i < map.Length; i++)
            {
                if (map[i].GetComponent<TilemapCollider2D>() == null)
                    continue;

                TileBase TileInContact = map[i].GetTile(map[i].WorldToCell(hitPosition));

                if (TileInContact == null)
                    continue;


                switch (datafromTiles[TileInContact])
                {
                    case TILE_TYPE.GROUND:
                        Debug.Log("GROUND");
                        break;

                    case TILE_TYPE.LAVA:
                        Debug.Log("LAVA");
                        break;

                    case TILE_TYPE.WATER:
                        Debug.Log("WATER");
                        break;

                    case TILE_TYPE.WALL:
                        Debug.Log("WALL");
                        break;
                }
            }

        }
    }

    void OnTriggerStay2D(Collider2D other)
	{
        Vector3 hitPosition = Vector3.zero;

        for (int i = 0; i < map.Length; i++)
        {
            // Check if tilemaps is a collidable environment/tiles
            if (map[i].GetComponent<TilemapCollider2D>() == null)
                continue;
            
            // Reference player position for collision detection
            hitPosition.x = gameObject.transform.position.x;
            hitPosition.y = gameObject.transform.position.y - 0.3f;

            // WorldToCell = Get the vec3 int of currentTile using a pos. GetTile = Gets the TileBase using Vec3 int pos.
            TileBase TileInContact = map[i].GetTile(map[i].WorldToCell(hitPosition));

            // If there isn't a tile in that position
            if (TileInContact == null)
                continue;

            switch (datafromTiles[TileInContact])
            {
                case TILE_TYPE.GROUND:
                    Debug.Log("GROUND");
                    break;

                case TILE_TYPE.LAVA:
                    if (!GetComponent<PlayerData>().m_iFrame)
                        Debug.Log("LAVA");
                    break;

                case TILE_TYPE.WATER:
                    Debug.Log("WATER");
                    break;

                case TILE_TYPE.WALL:
                    Debug.Log("WALL");
                    break;
            }
        }
    }
}
