using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriterender;
    public Color color;
    float alphaedit; 

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("Health", GetComponent<ObjectData>().blockHealth);
        spriterender = GetComponent<SpriteRenderer>();
        color = spriterender.color;
        alphaedit = 255;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetFloat("Health") > 0)
            animator.SetFloat("Health", GetComponent<ObjectData>().blockHealth);

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chest_Finish"))
		{
			if (color.a > 0)
			{
				alphaedit -= Time.deltaTime * 50;
				color = new Color(1, 1, 1, alphaedit / 255);
				spriterender.color = color;
            }
            if (color.a <= 0)
			{
                Destroy(gameObject);
			}
        }
	}
}
