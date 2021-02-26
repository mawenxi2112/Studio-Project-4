using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseScreenScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject WinScreen;
    public GameObject LoseScreen;
    void Start()
    {
        WinScreen = this.transform.GetChild(0).gameObject;
        LoseScreen = this.transform.GetChild(1).gameObject;
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (WinScreen.activeSelf || LoseScreen.activeSelf)
            return;

        //if() check if player wins for now is default turned off



        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
        int deadCounter = 0;
        for (int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i].GetComponent<PlayerData>().m_currentHealth <= 0)
            {
                deadCounter++;
            }
        }
        if (deadCounter == playerList.Length)
            LoseScreen.SetActive(true);
    }
}
