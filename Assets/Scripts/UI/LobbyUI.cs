using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    public List<GameObject> panel;

    private void Start()
    {
        ClickButton(0);
    }

    public void ClickButton(int id)
    {
        for(int i = 0; i<panel.Count; i++)
        {
            if(i == id)
            {
                panel[i].SetActive(true);
            }
            else
            {
                panel[i].SetActive(false);
            }
        }
    }
}