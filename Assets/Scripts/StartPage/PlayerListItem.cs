using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] Text text;
    [HideInInspector]
    public Player player;

    public Image tank_icon; //目前使用的Tank的icon

    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;

        if (player == PhotonNetwork.LocalPlayer)
            TeamRoomUIControl.instance.player_self = this;
    }
    public void SetUp(Player _player,
                    string _teamCode,
                    PlayerTankType _tankCode = PlayerTankType.guerrilla)
    {
        SetUp(_player);
        SetTeamCode(_teamCode);
        //set 職業
        SetTankCode();

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (targetPlayer != null &&
            targetPlayer == player)
        {
            //改變隊伍
            if (changedProps.ContainsKey(HashableData.PLAYER_TEAM_CODE))
            {
                Debug.Log(player.NickName + " OnPlayerPropertiesUpdate Team to " + (string)changedProps[HashableData.PLAYER_TEAM_CODE]);
                //UI上換panel
                TeamRoomUIControl.instance.ChangeTeam((string)changedProps[HashableData.PLAYER_TEAM_CODE], transform);
            }

            //改變Tank
            if (changedProps.ContainsKey(HashableData.PLAYER_TANK_TYPE))
            {
                string new_tank_name = (string)changedProps[HashableData.PLAYER_TANK_TYPE];
                Debug.Log(player.NickName + " OnPlayerPropertiesUpdate Tank " + new_tank_name);
                //更換小圖示
                tank_icon.sprite = Resources.Load<TankDataSO>(GameResourcesControl.TANK_SO_PATH + new_tank_name).icon;
            }
        }
    }

    public string GetPlayerTeamCode()
    {
        if (player.CustomProperties.ContainsKey(HashableData.PLAYER_TEAM_CODE))
            return (string)player.CustomProperties[HashableData.PLAYER_TEAM_CODE];
        else
            return "";
    }
    public void SetTeamCode(string _newTeamCode)
    {
        if (player.CustomProperties.ContainsKey(HashableData.PLAYER_TEAM_CODE))
            player.CustomProperties[HashableData.PLAYER_TEAM_CODE] = _newTeamCode;
        else
        {
            player.CustomProperties.Add(HashableData.PLAYER_TEAM_CODE.ToString(), _newTeamCode);
        }

        player.SetCustomProperties(MyExtension.WrapToHash(new object[]
            { HashableData.PLAYER_TEAM_CODE, _newTeamCode }
        ));

        Debug.Log(player.NickName + " change team to " + _newTeamCode);
    }

    public void SetTankCode()
    {
        if (player.CustomProperties.ContainsKey(HashableData.PLAYER_TANK_TYPE))
        {
            tank_icon.sprite = Resources.Load<TankDataSO>(GameResourcesControl.TANK_SO_PATH + player.CustomProperties[HashableData.PLAYER_TANK_TYPE]).icon;
        }
        else
        {
            //預設tank
            player.CustomProperties.Add(HashableData.PLAYER_TANK_TYPE.ToString(), "Guerrilla");
        }
    }
}
