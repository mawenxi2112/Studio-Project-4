using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerEnvironmentalCollision : MonoBehaviour
{
    private Rigidbody2D rb;
	public float knockbackMultiplier = 30;

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
        Vector3 hitPosition = Vector3.zero;

        for (int i = 0; i < map.Length; i++)
        {
            if (map[i].GetComponent<TilemapDetails>().tilemaptype == TILEMAP_TYPE.WALL)
                continue;

            // Reference player position to check for current tile standing on
            hitPosition.x = gameObject.transform.position.x;
            hitPosition.y = gameObject.transform.position.y - 0.3f;

            // WorldToCell = Get the vec3 int of currentTile using a pos. GetTile = Gets the TileBase using Vec3 int pos.
            if (map[i].GetTile(map[i].WorldToCell(hitPosition)) != null)
                currentTile = map[i].GetTile(map[i].WorldToCell(hitPosition));
        }

        if (currentTile != null)
        {
            switch (datafromTiles[currentTile])
            {
                case TILE_TYPE.GROUND:
                    //Debug.Log("GROUND");
                    break;

                case TILE_TYPE.LAVA:
                    //Debug.Log("LAVA");
                    break;

                case TILE_TYPE.WATER:
                    //Debug.Log("WATER");
				    GetComponent<PlayerData>().m_currentMoveSpeed = 2f;
					break;

                case TILE_TYPE.WALL:
                    //Debug.Log("WALL");
                    break;
            }

            if (datafromTiles[currentTile] != TILE_TYPE.WATER)
            {
                GetComponent<PlayerData>().m_currentMoveSpeed = GetComponent<PlayerData>().m_maxMoveSpeed;
            }
        }
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		Vector3 hitPosition = Vector3.zero;

		foreach (ContactPoint2D contact in collision.contacts)
		{
			Debug.DrawRay(contact.point, contact.normal, Color.white);

			hitPosition.x = contact.point.x - 0.01f * contact.normal.x;
			hitPosition.y = contact.point.y - 0.01f * contact.normal.y;

			// Collision with any tiles with isTrigger off
			if (collision.gameObject.tag == "Tiles")
			{
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
							if (!GetComponent<PlayerData>().m_iFrame)
							{
								GetComponent<PlayerData>().SetCurrentHealth(GetComponent<PlayerData>().m_currentHealth - 1);
								GetComponent<PlayerData>().m_iFrame = true;

								Vector2 dir = -gameObject.GetComponent<PlayerMovement>().movement;

								rb.AddForce(dir * knockbackMultiplier);

								Debug.Log("Player's new health: " + GetComponent<PlayerData>().m_currentHealth);
							}
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
			// Collision with in-game objects with IsTrigger off
			else if (collision.gameObject.tag == "Objects")
			{
				switch(collision.gameObject.GetComponent<ObjectData>().object_type)
				{
					case OBJECT_TYPE.COIN:
						GetComponent<PlayerData>().SetCurrency(GetComponent<PlayerData>().m_currency + collision.gameObject.GetComponent<ObjectData>().coinValue);
						collision.gameObject.SetActive(false);
						break;

					case OBJECT_TYPE.HEALTHPACK:
						if (GetComponent<PlayerData>().m_currentHealth < GetComponent<PlayerData>().m_maxHealth)
						{
							GetComponent<PlayerData>().SetCurrentHealth(GetComponent<PlayerData>().m_currentHealth + collision.gameObject.GetComponent<ObjectData>().healthPackValue);
							collision.gameObject.SetActive(false);
						}
						break;

					case OBJECT_TYPE.KEY:
						break;

					case OBJECT_TYPE.TORCH:
						break;

					case OBJECT_TYPE.BOMB:
						break;

					case OBJECT_TYPE.CHEST:
						break;

					case OBJECT_TYPE.SPIKE:
						break;

					case OBJECT_TYPE.MOVEABLEBLOCK:
						break;

					case OBJECT_TYPE.CAMPFIRE:
						break;
				}
			}
		}
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		Vector3 hitPosition = Vector3.zero;

		// Collision with any tiles with isTrigger on
		if (collision.gameObject.tag == "Tiles")
		{
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
		// Collision with in-game objects with IsTrigger on
		else if (collision.gameObject.tag == "Objects")
		{
			switch (collision.gameObject.GetComponent<ObjectData>().object_type)
			{
				case OBJECT_TYPE.COIN:
					GetComponent<PlayerData>().SetCurrency(GetComponent<PlayerData>().m_currency + collision.gameObject.GetComponent<ObjectData>().coinValue);
					collision.gameObject.SetActive(false);
					break;

				case OBJECT_TYPE.HEALTHPACK:
					if (GetComponent<PlayerData>().m_currentHealth < GetComponent<PlayerData>().m_maxHealth)
					{
						GetComponent<PlayerData>().SetCurrentHealth(GetComponent<PlayerData>().m_currentHealth + collision.gameObject.GetComponent<ObjectData>().healthPackValue);
						collision.gameObject.SetActive(false);
					}
					break;

				case OBJECT_TYPE.KEY:
					if (Input.GetKeyDown(KeyCode.Mouse0))
					{
						GetComponent<PlayerData>().m_holdingKey = true;
						collision.gameObject.SetActive(false);
					}
					break;

				case OBJECT_TYPE.TORCH:
					if (Input.GetKeyDown(KeyCode.Mouse0))
					{
						GetComponent<PlayerData>().m_holdingTorch = true;
						collision.gameObject.SetActive(false);
					}
					break;

				case OBJECT_TYPE.BOMB:
					break;

				case OBJECT_TYPE.CHEST:
					break;

				case OBJECT_TYPE.SPIKE:
					if (!GetComponent<PlayerData>().m_iFrame)
					{
						GetComponent<PlayerData>().SetCurrentHealth(GetComponent<PlayerData>().m_currentHealth - 1);
						GetComponent<PlayerData>().m_iFrame = true;
						Debug.Log("Player's new health: " + GetComponent<PlayerData>().m_currentHealth);
					}
					break;

				case OBJECT_TYPE.MOVEABLEBLOCK:
					break;

				case OBJECT_TYPE.CAMPFIRE:
					if (Input.GetKeyDown(KeyCode.Mouse0) && GetComponent<PlayerData>().m_holdingTorch && !collision.gameObject.GetComponent<ObjectData>().campfireLitOrNot)
					{
						Debug.Log("CAMPFIRE");
						collision.gameObject.GetComponent<ObjectData>().campfireLitOrNot = !collision.gameObject.GetComponent<ObjectData>().campfireLitOrNot;
						collision.gameObject.GetComponent<Animator>().SetBool("IsLit", collision.gameObject.GetComponent<ObjectData>().campfireLitOrNot);
					}
					break;
			}
		}
	}

}
