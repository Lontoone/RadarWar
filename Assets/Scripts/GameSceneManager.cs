using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//主要遊戲控制
public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;
    //public List<PhotonView> players = new List<PhotonView>();

    PhotonView pv;
    public GameObject localPlayer;

    public void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        pv = GetComponent<PhotonView>();
    }

    
    public void Start()
    {
      
    }
    /*
    //新增玩家
    public GameObject CreatePlayer()
    {
        GameObject _newPlayer = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
        players.Add(_newPlayer.GetComponent<PhotonView>());
        return _newPlayer;
    }*/
}
