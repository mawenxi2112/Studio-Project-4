using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{
	// Same type of tiles but different variation/design
	public TileBase[] tiles;

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
