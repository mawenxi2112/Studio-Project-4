using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    public OBJECT_TYPE object_type;

    public int coinValue;
    public int healthPackValue;
    public bool campfireLitOrNot;
    public int blockHealth;
}
public enum OBJECT_TYPE
{
    NONE = 0,
    COIN,
    HEALTHPACK,
    KEY,
    TORCH,
    BOMB,
    CHEST,
    SPIKE,
    MOVEABLEBLOCK,
    CAMPFIRE,
    BREAKABLEBLOCK,
    SURPRISETRAPBLOCK,
    PRESSUREPLATE,
    RESETBUTTON,
    DOOR,
    GATE
};
