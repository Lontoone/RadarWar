using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//分隊伍UI控制
public class TeamRoomUIControl : MonoBehaviour
{
    public Transform teamA_panel;
    public Transform teamB_panel;

    //隊伍人數
    public int teamA_Count { get =>teamA_panel.childCount; }
    public int teamB_Count { get => teamB_panel.childCount; }

    [HideInInspector]
    public PlayerListItem player_self;

    public static TeamRoomUIControl instance;
    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ChangeTeam(string teamCode, Transform playerListItem)
    {
        if (teamCode == HashableData.PlayerTeamCode.A.ToString())
        {
            playerListItem.SetParent(teamA_panel);
        }
        else
        {
            playerListItem.SetParent(teamB_panel);
        }
    }

    //例如按下"switch team"按鈕
    public void Switch_Self()
    {
        string _new_teamCode = player_self.GetPlayerTeamCode() == HashableData.PlayerTeamCode.A.ToString() ?
             HashableData.PlayerTeamCode.B.ToString() : HashableData.PlayerTeamCode.A.ToString();

        player_self.SetTeamCode(_new_teamCode);
    }
}
