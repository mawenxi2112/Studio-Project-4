using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseReward : MonoBehaviour
{
    public bool rewardGiven;

    public GameObject rewardButton1;
    public GameObject rewardButton2;
    public GameObject rewardButton3;

    public PlayerData player;

    void Start()
    {
        SetAllInActive();
    }

    void Update()
    {
        if (rewardGiven == true)
        {
            SetAllInActive();
        }
    }
    
    public void OnButtonClick(int button)
    {
        if (button == 1)
        {
            player.m_maxHealth += 1;

            rewardGiven = true;
        }
        else if (button == 2)
        {
            player.m_maxMoveSpeed += 50;

            rewardGiven = true;
        }
        else
        {
            player.m_currentAttack += 1;

            rewardGiven = true;
        }
    }

    private void SetAllInActive()
    {
        rewardButton1.SetActive(false);
        rewardButton2.SetActive(false);
        rewardButton3.SetActive(false);
    }

    private void SetAllActive()
    {
        rewardButton1.SetActive(true);
        rewardButton2.SetActive(true);
        rewardButton3.SetActive(true);
    }

    public void AllowChooseReward()
    {
        SetAllActive();
    }
}
