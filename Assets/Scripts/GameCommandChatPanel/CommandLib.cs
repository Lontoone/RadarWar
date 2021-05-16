using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//指令區
public class CommandLib : MonoBehaviour
{
    public static Dictionary<string, Action<object[]>> CommandDict = new Dictionary<string, Action<object[]>>() {
        {"move", Move },
        {"rotate", Rotate },
        {"stop", Stop },
        {"shoot", Shoot},
        { "m",PrivateMessage}
    };

    public static Dictionary<string, string> InfoDict = new Dictionary<string, string>() {
        {"move", "set speed from float(-1~1)" },
        {"rotate", "float _direction (deg)" },
        {"stop", "stop all function"  },
        {"shoot", "shoot x y"},
        { "m","_playerName message"}
    };
    /*==============CTL CMD===============*/
    public static void Move(object[] args)
    {
        Debug.Log(args[0]);
        PlayerControl player = (PlayerControl)args[0];
        string[] datas = args[1] as string[];

        player.Move(float.Parse(datas[0]));

        Debug.Log(datas.Length);
    }

    public static void Rotate(object[] args)
    {
        PlayerControl player = (PlayerControl)args[0];
        string[] datas = args[1] as string[];

        float _s;
        if (float.TryParse(datas[0], out _s))
        {
            player.Rotate(_s);
        }
    }

    public static void Shoot(object[] args)
    {
        PlayerControl player = (PlayerControl)args[0];
        string[] datas = args[1] as string[];

        float x, y;
        if (float.TryParse(datas[0], out x) && float.TryParse(datas[1], out y))
        {
            Vector2 _v2 = new Vector2(x, y);
            player.Shoot(_v2);
        }
        else
        {
            //chatControl.PrintError("invalid syntax");
        }
    }

    public static void Stop(object[] args)
    {
        PlayerControl player = (PlayerControl)args[0];
        player.Stop();
    }



    /*==============CHAT CMD===============*/
    public static void PrivateMessage(object[] args)
    {
        //密語
        PlayerControl from_player = (PlayerControl)args[0];
        string[] datas = args[1] as string[];

        Player _target_player = datas[0].GetPlayerByName();

        //組合句子
        string message = datas.PrintAll(1);
        /*
        for (int i = 1; i < datas.Length; i++)
        {
            message += datas[i];
        }
        */

        ChatContent content = new ChatContent(message, from_player.playerManager.player.NickName);

        if (_target_player == null)
        {
            Debug.Log("找不到玩家");
            return;
        }
        else
        {
            Debug.Log(_target_player.NickName);
            CommandChatControl.instance.SendPrivateMessage(MyExtension.ObjectToByteArray(content), _target_player);
            //同時也顯示在自己面板上
            //chatControl.Print(_line, currentControlPlayer.playerManager.player.NickName);
        }
    }
}
