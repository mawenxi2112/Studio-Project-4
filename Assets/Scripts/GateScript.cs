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
    public List<GameObject> ListOfObjectMessGate;

    void Awake()
	{
        ListOfObjectRequiredToOpenGate = new List<GameObject>();
        ListOfObjectMessGate = new List<GameObject>();
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
        GameObject ObjectsToMessGate = GateParent.transform.Find("ObjectsToMessGate").gameObject;
        GameObject ObjectsToUnlockGate = GateParent.transform.Find("ObjectsToUnlockGate").gameObject;
        for (int i = 0; i < ObjectsToUnlockGate.transform.childCount; i++)
		{
            ListOfObjectRequiredToOpenGate.Add(ObjectsToUnlockGate.transform.GetChild(i).gameObject);
		}
        if (ObjectsToMessGate != null)
        {
            for (int i = 0; i < ObjectsToMessGate.transform.childCount; i++)
            {
                ListOfObjectMessGate.Add(ObjectsToMessGate.transform.GetChild(i).gameObject);
            }
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
                    if (!ListOfObjectRequiredToOpenGate[i].GetComponent<Animator>().GetBool("IsLit"))
					{
                        WillGateOpen = false;
					}
                    break;

                case OBJECT_TYPE.PRESSUREPLATE:
                    if (!ListOfObjectRequiredToOpenGate[i].GetComponent<Animator>().GetBool("isPressed"))
                    {
                        WillGateOpen = false;
                    }
                    break;
            }

            if (!WillGateOpen)
                break;
		}

        for (int i = 0; i < ListOfObjectMessGate.Count; i++)
        {
            switch (ListOfObjectMessGate[i].GetComponent<ObjectData>().object_type)
            {
                case OBJECT_TYPE.CAMPFIRE:
                    if (ListOfObjectMessGate[i].GetComponent<Animator>().GetBool("IsLit"))
                    {
                        WillGateOpen = false;
                    }
                    break;

                case OBJECT_TYPE.PRESSUREPLATE:
                    if (ListOfObjectMessGate[i].GetComponent<Animator>().GetBool("isPressed"))
                    {
                        WillGateOpen = false;
                    }
                    break;
            }
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

        boxCollider.enabled = true;
        LeftCollider.GetComponent<BoxCollider2D>().enabled = false;
        RightCollider.GetComponent<BoxCollider2D>().enabled = false;
    }
}
