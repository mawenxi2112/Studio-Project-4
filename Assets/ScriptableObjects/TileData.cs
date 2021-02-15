using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{
	public TileBase[] tiles;

	public bool TakeDamageWhenOn;
	public bool IsMoveable;

	public TILE_TYPE tiletype;
}
public enum TILE_TYPE
{
	NONE = 0,
	GROUND,
	LAVA,
	WATER,
	WALL
};
