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

    private Dictionary<TileBase, TileData> datafromTiles;
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
        rb = GetComponent<Rigidbody2D>();

        for (int i = 0; i < GameObject.Find("Grid").transform.childCount; i++)
		{
            map[i] = GameObject.Find("Grid").transform.GetChild(i).GetComponent<Tilemap>();
            Debug.Log("Loop: " + i);
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

                if (datafromTiles[TileInContact].TakeDamageWhenOn)
                    Debug.Log("At position " + map[i].WorldToCell(hitPosition) + ", standing on " + TileInContact + ", taking damage");
                else if (!datafromTiles[TileInContact].TakeDamageWhenOn)
                    Debug.Log("At position " + map[i].WorldToCell(hitPosition) + ", standing on " + TileInContact + ", not taking damage");

                switch (datafromTiles[TileInContact].tiletype)
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
            if (map[i].GetComponent<TilemapCollider2D>() == null)
                continue;

            hitPosition.x = gameObject.transform.position.x;
            hitPosition.y = gameObject.transform.position.y - 0.3f;

            TileBase TileInContact = map[i].GetTile(map[i].WorldToCell(hitPosition));

            if (TileInContact == null)
                continue;

            if (datafromTiles[TileInContact].TakeDamageWhenOn)
                Debug.Log("At position " + map[i].WorldToCell(hitPosition) + ", standing on " + TileInContact + ", taking damage");
            else if (!datafromTiles[TileInContact].TakeDamageWhenOn)
                Debug.Log("At position " + map[i].WorldToCell(hitPosition) + ", standing on " + TileInContact + ", not taking damage");

            switch (datafromTiles[TileInContact].tiletype)
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
