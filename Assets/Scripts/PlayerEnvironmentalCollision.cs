using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerEnvironmentalCollision : MonoBehaviour
{
	private Rigidbody2D rb;
	public float knockbackMultiplier;

	[SerializeField]
	private Tilemap[] map = null;

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
		if (!GetComponent<PhotonView>().IsMine)
			return;

		rb = GetComponent<Rigidbody2D>();

		if (GameObject.Find("Grid") == null)
			return;

		for (int i = 0; i < GameObject.Find("Grid").transform.childCount; i++)
		{
			map[i] = GameObject.Find("Grid").transform.GetChild(i).GetComponent<Tilemap>();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!GetComponent<PhotonView>().IsMine || GameObject.Find("Grid") == null)
			return;

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
                    break;

                case TILE_TYPE.LAVA:
                    break;

                case TILE_TYPE.WATER:
				    GetComponent<PlayerData>().m_currentMoveSpeed = GetComponent<PlayerData>().m_maxMoveSpeed * 0.2f;
					break;

                case TILE_TYPE.WALL:
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
		if (!GetComponent<PhotonView>().IsMine || GameObject.Find("Grid") == null)
			return;

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
							break;

						case TILE_TYPE.LAVA:
							if (!GetComponent<PlayerData>().m_iFrame)
							{
								GetComponent<PlayerData>().SetCurrentHealth(GetComponent<PlayerData>().m_currentHealth - 1);
								GetComponent<PlayerData>().m_iFrame = true;

								Vector2 dir = -gameObject.GetComponent<PlayerMovement>().movement * knockbackMultiplier;

								rb.AddForce(dir * knockbackMultiplier);
							}
							break;

						case TILE_TYPE.WATER:
							break;

						case TILE_TYPE.WALL:
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
						break;

					case OBJECT_TYPE.HEALTHPACK:
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

					case OBJECT_TYPE.BREAKABLEBLOCK:
						break;

					case OBJECT_TYPE.SURPRISETRAPBLOCK:
						break;

					case OBJECT_TYPE.PRESSUREPLATE:
						break;

					case OBJECT_TYPE.RESETBUTTON:
						break;

					case OBJECT_TYPE.DOOR:
						break;

					case OBJECT_TYPE.GATE:
						break;
				}
			}
		}
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		if (!GetComponent<PhotonView>().IsMine)
			return;

		// Collision with in-game objects with IsTrigger on
		if (collision.gameObject.tag == "Objects")
		{
			switch (collision.gameObject.GetComponent<ObjectData>().object_type)
			{
				case OBJECT_TYPE.COIN:
					GetComponent<PlayerData>().SetCurrency(GetComponent<PlayerData>().m_currency + collision.gameObject.GetComponent<ObjectData>().coinValue);
					//collision.gameObject.SetActive(false);
					
					PlayerData.TransferOwnership(collision.gameObject, this.gameObject);
					PhotonNetwork.Destroy(collision.gameObject);
					break;

				case OBJECT_TYPE.HEALTHPACK:
					if (GetComponent<PlayerData>().m_currentHealth < GetComponent<PlayerData>().m_maxHealth)
					{
						GetComponent<PlayerData>().SetCurrentHealth(GetComponent<PlayerData>().m_currentHealth + collision.gameObject.GetComponent<ObjectData>().healthPackValue);
						//collision.gameObject.SetActive(false);

						PlayerData.TransferOwnership(collision.gameObject, this.gameObject);
						PhotonNetwork.Destroy(collision.gameObject);
					}
					break;

				case OBJECT_TYPE.KEY:
					if (GetComponent<PlayerData>().m_actionKey)
					{
						//GetComponent<PlayerInteraction>().PickUp(collision.gameObject, EQUIPMENT.KEY);
						GetComponent<PhotonView>().RPC("PickUp", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID, EQUIPMENT.KEY);
						GetComponent<PlayerData>().m_actionKey = false;
					}
					break;

				case OBJECT_TYPE.TORCH:
					if (GetComponent<PlayerData>().m_actionKey)
					{
						//GetComponent<PlayerInteraction>().PickUp(collision.gameObject, EQUIPMENT.TORCH);
						GetComponent<PhotonView>().RPC("PickUp", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID, EQUIPMENT.TORCH);
						GetComponent<PlayerData>().m_actionKey = false;
					}
					break;

				case OBJECT_TYPE.BOMB:
					if (GetComponent<PlayerData>().m_actionKey)
					{
						//GetComponent<PlayerInteraction>().PickUp(collision.gameObject, EQUIPMENT.TORCH);
						GetComponent<PhotonView>().RPC("PickUp", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID, EQUIPMENT.BOMB);
						GetComponent<PlayerData>().m_actionKey = false;
					}
					break;

				case OBJECT_TYPE.CHEST:
					break;

				case OBJECT_TYPE.SPIKE:
					if (!GetComponent<PlayerData>().m_iFrame)
					{
						GetComponent<PlayerData>().SetCurrentHealth(GetComponent<PlayerData>().m_currentHealth - 1);
						GetComponent<PlayerData>().m_iFrame = true;
					}
					break;

				case OBJECT_TYPE.MOVEABLEBLOCK:
					break;

				case OBJECT_TYPE.CAMPFIRE:
					if (GetComponent<PlayerData>().m_actionKey && GetComponent<PlayerData>().m_currentEquipment == EQUIPMENT.TORCH)
					{
						collision.gameObject.GetComponent<CampfireScript>().IsLit = !collision.gameObject.GetComponent<CampfireScript>().IsLit;
						GetComponent<PlayerData>().m_actionKey = false;
					}
					break;

				case OBJECT_TYPE.BREAKABLEBLOCK:
					break;

				case OBJECT_TYPE.SURPRISETRAPBLOCK:
					break;

				case OBJECT_TYPE.PRESSUREPLATE:
					break;

				case OBJECT_TYPE.RESETBUTTON:
					break;

				case OBJECT_TYPE.DOOR:
					break;

				case OBJECT_TYPE.GATE:
					break;

                case OBJECT_TYPE.PU_DAMAGE:
                    GetComponent<PlayerData>().m_maxHealth += 10;

                    if (GetComponent<PhotonView>().Controller != collision.gameObject.GetComponent<PhotonView>().Controller)
                        collision.gameObject.GetComponent<PhotonView>().TransferOwnership(GetComponent<PhotonView>().Controller);

                    // Destroy the Damage Powerup
                    PhotonNetwork.Destroy(collision.gameObject);

                    break;

                case OBJECT_TYPE.PU_MAXHEALTH:

                    // Need to change
                    GetComponent<PlayerData>().m_maxHealth += 10;

                    if (GetComponent<PhotonView>().Controller != collision.gameObject.GetComponent<PhotonView>().Controller)
                        collision.gameObject.GetComponent<PhotonView>().TransferOwnership(GetComponent<PhotonView>().Controller);

                    // Destroy the Damage Powerup
                    PhotonNetwork.Destroy(collision.gameObject);

                    break;

                case OBJECT_TYPE.PU_SPEED:

                    // Need to change
                    GetComponent<PlayerData>().m_maxMoveSpeed += 700;

                    if (GetComponent<PhotonView>().Controller != collision.gameObject.GetComponent<PhotonView>().Controller)
                        collision.gameObject.GetComponent<PhotonView>().TransferOwnership(GetComponent<PhotonView>().Controller);

                    // Destroy the Damage Powerup
                    PhotonNetwork.Destroy(collision.gameObject);
                    break;
			}
		}
	}
}
