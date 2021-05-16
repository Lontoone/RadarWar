using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
//選擇tank按鈕
public class SelectTankbtn : MonoBehaviourPunCallbacks
{
    public TankDataSO tankData;
    private Image icon_img;
    private void Start()
    {
        //讀取按鈕圖案
        icon_img = gameObject.GetComponent<Image>();
        if (tankData.icon != null)
            icon_img.sprite = tankData.icon;


    }

    public void OnClicked()
    {
        //設定tank custome Property
        Player player = PhotonNetwork.LocalPlayer;

        if (player.CustomProperties.ContainsKey(HashableData.PLAYER_TANK_TYPE))
            player.CustomProperties[HashableData.PLAYER_TANK_TYPE] = tankData.tankName;
        else
        {
            player.CustomProperties.Add(HashableData.PLAYER_TANK_TYPE.ToString(), tankData.tankName);
        }

        player.SetCustomProperties(MyExtension.WrapToHash(new object[]
            { HashableData.PLAYER_TANK_TYPE, tankData.tankName }
        ));

        Debug.Log(player.NickName + " change tank to " + tankData.name);

    }
}
