using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriterender;
    public Color color;
    float alphaedit;

    [SerializeField]
    private GameObject speedPowerUp;

    [SerializeField]
    private GameObject maxHPPowerUp;

    [SerializeField]
    private GameObject damagePowerUp;

    [SerializeField]
    private GameObject coinPowerUp;

    private bool itemDropped;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("Health", GetComponent<ObjectData>().blockHealth);
        spriterender = GetComponent<SpriteRenderer>();
        color = spriterender.color;
        alphaedit = 255;

        itemDropped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetFloat("Health") > 0)
            animator.SetFloat("Health", GetComponent<ObjectData>().blockHealth);

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chest_Finish"))
		{
            if (!itemDropped)
            {
                GameObject powerUp;

                int rand = Random.Range(1, 4);

                if (rand == 1)
                {
                    powerUp = Instantiate(speedPowerUp);
                }
                else if (rand == 2)
                {
                    powerUp = Instantiate(maxHPPowerUp);
                }
                else if (rand == 3)
                {
                    powerUp = Instantiate(damagePowerUp);
                }
                else
                {
                    powerUp = Instantiate(coinPowerUp);
                }

                powerUp.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z);
                powerUp.AddComponent<EntityBounce>();

                EntityBounce powerupBounce = powerUp.GetComponent<EntityBounce>();
                powerupBounce.m_bounceVelocity = 5;
                powerupBounce.m_bounceLoss = 0.75f;
                powerupBounce.m_gravity = -70;
                powerupBounce.StartBounce(6, true);

                itemDropped = true;
            }

			if (color.a > 0)
			{
				alphaedit -= Time.deltaTime * 60;
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
