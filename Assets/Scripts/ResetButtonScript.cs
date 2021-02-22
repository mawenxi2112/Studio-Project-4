using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButtonScript : MonoBehaviour
{
    public Animator animator;
    public bool isSteppedOn;

    public List<GameObject> ListOfObjectToReset;

	void Awake()
	{
        ListOfObjectToReset = new List<GameObject>();
	}

	// Start is called before the first frame update
	void Start()
    {
        animator = GetComponent<Animator>();
        isSteppedOn = false;

        // Link ObjectsToReset from the scene to the reset button
        GameObject ResetButtonParent = gameObject.transform.parent.gameObject;
        GameObject ObjectsToReset = ResetButtonParent.transform.Find("ObjectsToReset").gameObject;
        for (int i = 0; i < ObjectsToReset.transform.childCount; i++)
        {
            ListOfObjectToReset.Add(ObjectsToReset.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

	void OnTriggerStay2D(Collider2D collision)
	{
        if (collision.CompareTag("Player") || collision.CompareTag("Objects"))
        {
            if (animator.GetFloat("TriggerChange") == 0)
            {
                animator.SetFloat("TriggerChange", 1);
                isSteppedOn = true;

                for (int i = 0; i < ListOfObjectToReset.Count; i++)
				{
                    ListOfObjectToReset[i].GetComponent<Transform>().position = ListOfObjectToReset[i].GetComponent<ObjectData>().originalPosition;
				}
            }
        }
    }

	void OnTriggerExit2D(Collider2D collision)
	{
        if (animator.GetFloat("TriggerChange") == 1)
        {
            animator.SetFloat("TriggerChange", 0);
            isSteppedOn = false;
        }
    }
}
