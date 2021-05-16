using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{


    PhotonView pv;
    GameObject controller;

    //public PlayerData playerData; //TODO: 指定Data

    //public TankDataSO tankData_clone;
    [HideInInspector]
    public Player player;
    public GameObject playerCameraObject;

    [HideInInspector]
    public Camera _playerCamera;

    public void Awake()
    {
        pv = GetComponent<PhotonView>();


        if (pv.IsMine)
        {
            player = PhotonNetwork.LocalPlayer;
            CreateController();
            GameSceneManager.instance.localPlayer = controller;
        }
    }
    void Start()
    {
    }

    void CreateController()
    {
        Debug.Log("Instantiated Player Controller");
        //Transform spawnPoint = SpawnManager.instance.GetSpawnpoint();
        //controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnPoint.position, spawnPoint.rotation, 0, new object[] { pv.ViewID });
        //讀取玩家tank資料，複製一份

        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"),
            Vector2.zero,
            Quaternion.identity,
            0,
            new object[] { pv.ViewID, player.CustomProperties[HashableData.PLAYER_TANK_TYPE] });


        //玩家攝影機
        if (_playerCamera == null)
            _playerCamera = Instantiate(playerCameraObject).GetComponent<Camera>();
    
    }

    private void FixedUpdate()
    {
        if(pv.IsMine)
        _playerCamera.transform.position = new Vector3(controller.transform.position.x, controller.transform.position.y, -3);
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }
}

//報廢
public class PlayerData
{
    public HashableData.PlayerTeamCode teamCode;
    //public HashableData.PlayerTankType tankType;
}