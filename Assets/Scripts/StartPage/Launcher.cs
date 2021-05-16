using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{

    public static Launcher instance;
    [SerializeField] UnityEngine.UI.InputField roomNameInputField;
    [SerializeField] UnityEngine.UI.Text errorText;
    [SerializeField] UnityEngine.UI.Text roomNameText;
    [SerializeField] Transform roomListItemPrefab;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject StartGameButton;

    public void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //connect to server
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("connect to lobby");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.instance.OpenMenu("title");
        Debug.Log("Join lobby");

        //TODO 設定玩家名稱
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000).ToString("0000");
    }


    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text)) { return; }
        RoomOptions options = new RoomOptions();

        options.BroadcastPropsChangeToAll = true; //有人property change 通知所有人
        //TODO:房間options setting

        PhotonNetwork.CreateRoom(roomNameInputField.text, options);

        //避免玩家在等待server時亂點其他按鈕s
        MenuManager.instance.OpenMenu("loading");
    }


    public override void OnJoinedRoom() //called when create or join a room
    {
        MenuManager.instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        //清除組隊列表
        foreach (Transform child in TeamRoomUIControl.instance.teamA_panel)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in TeamRoomUIControl.instance.teamB_panel)
        {
            Destroy(child.gameObject);
        }

        //列出目前人數
        Player[] players = PhotonNetwork.PlayerList;
        Debug.Log("房間人數 " + players.Length.ToString());
        for (int i = 0; i < players.Length; i++)
        {
            //TODO: [BUG]分配隊伍 -
            string _tc = (string)players[i].CustomProperties[HashableData.PLAYER_TEAM_CODE];

            if (string.IsNullOrEmpty(_tc))
                _tc = HashableData.PlayerTeamCode.A.ToString();

            Debug.Log(players[i].NickName + " " + _tc);
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i], _tc);
        }

        //只有host可以開始遊戲按鈕
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //switch host時讓下個host可以開始遊戲
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room create Failed " + message;
        MenuManager.instance.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("loading");
    }


    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform child in roomListContent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //分配隊伍
        string _teamCode = TeamRoomUIControl.instance.teamA_Count < TeamRoomUIControl.instance.teamB_Count ?
            HashableData.PlayerTeamCode.A.ToString() : HashableData.PlayerTeamCode.B.ToString();

        Debug.Log("分配team " + _teamCode);

        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer, _teamCode);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
