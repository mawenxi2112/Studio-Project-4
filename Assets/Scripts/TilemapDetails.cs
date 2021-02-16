using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapDetails : MonoBehaviour
{
    public TILEMAP_TYPE tilemaptype;

}
public enum TILEMAP_TYPE
{
    NONE = 0,
    GROUND,
    LAVA,
    WATER,
    WALL
};
