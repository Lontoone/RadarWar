using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    void Awake()
    {
        //若其他manager存在，那就不需要自己
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        //若沒有其他Manager存在，則保留自己
        DontDestroyOnLoad(gameObject);
        instance = this;

    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadScene)
    {

        if (scene.buildIndex == 1) //in Game scene
        {

            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
            Debug.Log("[ TODO ]: 產生玩家");

            //GameSceneManager.instance.CreatePlayer();
        }
    }
}
