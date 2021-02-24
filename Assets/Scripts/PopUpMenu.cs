using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpMenu : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame


    public void PopUp()
    {
        this.gameObject.SetActive(true);
    }
}
