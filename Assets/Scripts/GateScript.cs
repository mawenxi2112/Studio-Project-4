using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D boxCollider;
    public bool isGateLock;
    public GameObject LeftCollider;
    public GameObject RightCollider;

    public List<GameObject> ListOfObjectRequiredToOpenGate;

	void Awake()
	{
        ListOfObjectRequiredToOpenGate = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        isGateLock = true;
        animator.SetBool("IsLock", isGateLock);

        // Link ObjectsRequiredToOpenGate from the scene to the gate
        GameObject GateParent = gameObject.transform.parent.gameObject;
        GameObject ObjectsToUnlockGate = GateParent.transform.Find("ObjectsToUnlockGate").gameObject;
        for (int i = 0; i < ObjectsToUnlockGate.transform.childCount; i++)
		{
            ListOfObjectRequiredToOpenGate.Add(ObjectsToUnlockGate.transform.GetChild(i).gameObject);
		}
    }

    // Update is called once per frame
    void Update()
    {
        bool WillGateOpen = true;

        for (int i = 0; i < ListOfObjectRequiredToOpenGate.Count; i++)
		{
            switch(ListOfObjectRequiredToOpenGate[i].GetComponent<ObjectData>().object_type)
			{
                case OBJECT_TYPE.CAMPFIRE:
                    if (!ListOfObjectRequiredToOpenGate[i].GetComponent<CampfireScript>().IsLit)
					{
                        WillGateOpen = false;
					}
                    break;

                case OBJECT_TYPE.PRESSUREPLATE:
                    if (!ListOfObjectRequiredToOpenGate[i].GetComponent<PressurePlateScript>().isSteppedOn)
                    {
                        WillGateOpen = false;
                    }
                    break;
            }

            if (!WillGateOpen)
                break;
		}

        if (WillGateOpen)
            UnlockGate();
        else if (!WillGateOpen)
            CloseGate();

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Gate_Opened"))
        {
            boxCollider.enabled = false;
            LeftCollider.GetComponent<BoxCollider2D>().enabled = true;
            RightCollider.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void UnlockGate()
	{
        isGateLock = false;
        animator.SetBool("IsLock", isGateLock);
	}

    public void CloseGate()
	{
        isGateLock = true;
        animator.SetBool("IsLock", isGateLock);
	}
}
