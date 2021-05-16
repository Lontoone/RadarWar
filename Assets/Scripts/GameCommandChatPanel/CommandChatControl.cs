using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//遊戲指令欄位控制
public class CommandChatControl : MonoBehaviourPunCallbacks
{
    public static CommandChatControl instance;
    public InputFieldCaretPosition InputField;
    public GameObject chatPanel;
    public ChatItem chatItemPrefab;
    CommandParser parser;
    PhotonView pv;

    public GameObject intelliSense;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public void Start()
    {
        pv = GetComponent<PhotonView>();
        parser = GetComponent<CommandParser>();

    }
  

    public void Update()
    {
        //送出訊息
        if (Input.GetKeyDown(KeyCode.Return) &&
            !string.IsNullOrEmpty(InputField.text) && InputField.text != "")
        {
            ChatContent _content = new ChatContent(InputField.text, PhotonNetwork.NickName);
            var _content_data = MyExtension.ObjectToByteArray(_content);

            //解析指令
            string result;
            parser.ParseCommand(_content.message, out result);
            Print(result, PhotonNetwork.NickName);


            //pv.RPC("ReceiveChatMessage", RpcTarget.All, _content_data); //暫時: 全頻
            InputField.text = "";
            InputField.Select();
            InputField.ActivateInputField();
        }
    }

    public void Print(string _context, string _fromName)
    {
        ChatContent content = new ChatContent(_context, _fromName);
        ReceiveChatMessage(MyExtension.ObjectToByteArray(content));
    }
    public void PrintError(string _context)
    {
        //印出error訊息
        _context = @"<color=""red"">" + _context + "</color>";
        Print(_context, "sys");
    }

    //全頻對話
    public void SendPublicMessage(byte[] data)
    {
        //密語
        ChatContent _content = new ChatContent("to all :" + InputField.text, PhotonNetwork.NickName);
        var _content_data = MyExtension.ObjectToByteArray(_content);

        pv.RPC("ReceiveChatMessage", RpcTarget.All, _content_data);
    }

    //隊伍頻
    public void SendTeamMessage(byte[] data, Player from_player)
    {
        Player[] same_team_players = from_player.GetPlayersInSameTeam();

        for (int i = 0; i < same_team_players.Length; i++)
        {
            pv.RPC("ReceiveChatMessage", same_team_players[i], data);
        }
    }

    public void SendPrivateMessage(byte[] data, Player tragetPlayer)
    {
        //密語
        //ChatContent _content = MyExtension.ByteArrayToObject(data) as ChatContent;
        //var _content_data = MyExtension.ObjectToByteArray(_content);

        Debug.Log(" 密語 " + tragetPlayer.NickName);

        pv.RPC("ReceiveChatMessage", tragetPlayer, data);
    }



    [PunRPC]
    void ReceiveChatMessage(byte[] data)
    {
        //收到訊息
        ChatContent _content = (ChatContent)MyExtension.ByteArrayToObject(data);

        Debug.Log(" ReceiveChatMessage " + _content.message);

        ChatItem _text = Instantiate(chatItemPrefab, chatPanel.transform);
        _text.message_text.text = _content.message;
        _text.name_text.text = _content.from;


    }

}

[System.Serializable]
public class ChatContent
{
    public ChatContent() { }
    public ChatContent(string _message, string _from)
    {
        message = _message;
        from = _from;
    }

    public string message = "";
    public string from = "";
    public string to = "";
}
