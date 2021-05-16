using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
//using TMPro;
//對話框操作
public class CommandParser : MonoBehaviour
{
    [HideInInspector]
    public PlayerControl currentControlPlayer; //目前在操控的玩家
    CommandChatControl chatControl;

    private void Start()
    {
        chatControl = GetComponent<CommandChatControl>();
        currentControlPlayer = GameSceneManager.instance.localPlayer.GetComponent<PlayerControl>();

        //ParseCommand(@"/move 15 6;");
    }

    public void ParseCommand(string _line, out string result)
    {
        result = "";

        MatchCollection matches = RPGCore.ReadLines(_line);
        if (matches.Count < 1)
        {
            //result = "Syntext Error";

            //預設隊伍聊天群
            string _currentPlayerName = currentControlPlayer.playerManager.player.NickName;
            ChatContent content = new ChatContent(_line, _currentPlayerName);
            chatControl.SendTeamMessage(MyExtension.ObjectToByteArray(content), currentControlPlayer.playerManager.player);

            Debug.Log("隊伍頻: " + _line);

            result = _line;
            return;
        }
        foreach (Match match in matches)
        {
            string tag;
            result = match.Value;

            List<string> args = new List<string>();
            tag = match.Groups["tag"].Captures[0].Value;

            //拆解tag和arg
            for (int i = 0; i < match.Groups["arg"].Captures.Count; i++)
            {
                Debug.Log("tag " + tag + " arg " + match.Groups["arg"].Captures[i]);
                string _arg = match.Groups["arg"].Captures[i].Value;
                ReadArgs(ref _arg);
                args.Add(_arg);
            }

            //判定
            if (CommandLib.CommandDict.ContainsKey(tag))
                CommandLib.CommandDict[tag].Invoke(new object[2] { currentControlPlayer, args.ToArray() });
            else
                CommandChatControl.instance.PrintError("Syntax Error" + args.PrintAll());
        }



    }


    //參數設定
    public void ReadArgs(ref string args)
    {
        if (args == "") { return; }

        //跳脫字元
        args = RPGCore.Put_Back_EscChar(args);
        //自定義變數
        args = RPGCore.ReadCustomVariables(args);

        //arg group
        foreach (Match match in GetMatch(args, RPGCore.REGEX_ARGS_TITLE))
        {
            //title.text = match.Groups["title"].Value;
        }


        #region 

        //animation
        foreach (Match match in GetMatch(args, RPGCore.REGEX_ARGS_ANIMATION))
        {
            GameObject target = GameObject.Find((match.Groups["objectName"].Value));
            if (target != null)
            {
                Animator animator = target.GetComponent<Animator>();
                if (animator != null)
                    animator.Play((match.Groups["clipName"].Value));
                else
                    Debug.LogError("<animation arg error> animator not found");
            }
        }

        //audio
        foreach (Match match in GetMatch(args, RPGCore.REGEX_ARGS_AUDIO))
        {
            string operation = (match.Groups["operation"].Value);
            GameObject target = GameObject.Find((match.Groups["objectName"].Value));
            if (target != null)
            {
                AudioSource audioSource = target.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    if (operation == "play")
                        audioSource.Play();
                    else if (operation == "pause")
                        audioSource.Pause();
                    else if (operation == "stop")
                        audioSource.Stop();
                }
                else
                    Debug.LogError("<audio arg error> audioSource not found");
            }
        }

        //set
        foreach (Match match in GetMatch(args, RPGCore.REGEX_ARGS_SET))
        {
            Debug.Log("SET");
            string _key = Regex.Replace(match.Groups["key"].Value, @"['""]", ""); ;
            string _value = match.Groups["value"].Value;
            string _operator = match.Groups["operator"].Value; //optional
            string _value2 = match.Groups["value2"].Value; //optional

            //判斷value是字典key或值
            bool v1_is_key = Regex.IsMatch(_value, @"['""].*?['""]");
            bool v2_is_key = Regex.IsMatch(_value2, @"['""].*?['""]");

            string v1 = _value, v2 = _value2;
            if (v1_is_key)
                v1 = RPGCore.GetDictValue(Regex.Replace(_value, @"['""]", ""));
            if (v2_is_key)
                v2 = RPGCore.GetDictValue(Regex.Replace(_value2, @"['""]", ""));
            //先做運算
            if (_operator != "")
            {
                int v1_num = int.Parse(v1);
                int v2_num = int.Parse(v2);
                if (_operator == "+")
                    RPGCore.SetDictionaryValue(_key, (v1_num + v2_num).ToString());
                if (_operator == "-")
                    RPGCore.SetDictionaryValue(_key, (v1_num - v2_num).ToString());
                if (_operator == "*")
                    RPGCore.SetDictionaryValue(_key, (v1_num * v2_num).ToString());
                if (_operator == "/")
                    RPGCore.SetDictionaryValue(_key, (v1_num / v2_num).ToString());
            }
            else
            {
                //不做運算直接給值
                RPGCore.SetDictionaryValue(_key, v1);
            }

        }


        //其他 用括號的參數改用統一regex拆解 other
        foreach (Match match in GetMatch(args, RPGCore.REGEX_FUNC_PARA))
        {
            //方法名稱
            string functionName = match.Groups["func"].Value;
            Debug.Log(functionName);
            string[] paras = match.Groups["para"].Value.Split(',');

            //其他方法....


        }
        #endregion
    }

    MatchCollection GetMatch(string arg, string pat)
    {
        Regex regex = new Regex(pat,
                                RegexOptions.IgnoreCase
                                | RegexOptions.Compiled
                                | RegexOptions.Singleline
                                | RegexOptions.IgnorePatternWhitespace);
        MatchCollection match = regex.Matches(arg);
        return match;
    }


    Player GetPlayerByName(string _nickName)
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.NickName.Trim() == _nickName.Trim())
            {
                return p;
            }
        }
        return null;
    }

}
