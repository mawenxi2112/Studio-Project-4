using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    public OBJECT_TYPE object_type;

    public int coinValue;
    public int healthPackValue;
    public int blockHealth;

    public Vector3 originalPosition;

	public void Start()
	{
        originalPosition = gameObject.GetComponent<Transform>().position;
	}
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
    GATE,
    ENDPLATE,

    // Powerups
    PU_DAMAGE,
    PU_MAXHEALTH,
    PU_SPEED
};
