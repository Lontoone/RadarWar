using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Menu : MonoBehaviour
{
    public string menuName;

    

    [HideInInspector]
    public bool open;

    public void Start()
    {
        open = gameObject.activeSelf;
    }

    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }
    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }


}
