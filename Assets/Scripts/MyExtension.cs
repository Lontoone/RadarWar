
using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

public static class MyExtension
{
    // Convert an object to a byte array
    public static byte[] ObjectToByteArray(Object obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    // Convert a byte array to an Object
    public static Object ByteArrayToObject(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }
    }

    public static T CloneData<T>(T origin_data)
    {

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<T, T>();
        });
        var mapper = config.CreateMapper();
        return mapper.Map<T>(origin_data);


    }
    public static string PrintAll(this List<string> arr)
    {
        return arr.ToArray().PrintAll();
    }

    public static string PrintAll(this string[] arr)
    {
        string res = "";
        for (int i = 0; i < arr.Length; i++)
        {
            res += arr[i];
        }
        return res;
    }
    public static string PrintAll(this string[] arr, int start_index)
    {
        string res = "";
        for (int i = start_index; i < arr.Length; i++)
        {
            res += arr[i];
        }
        return res;
    }

    public static ExitGames.Client.Photon.Hashtable WrapToHash(Object[] key_value_array)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        for (int i = 0; i < key_value_array.Length; i += 2)
        {
            hash.Add(key_value_array[i], key_value_array[i + 1]);
        }
        return hash;

    }

    public static Photon.Realtime.Player[] GetPlayersInSameTeam(this Photon.Realtime.Player _p)
    {
        List<Photon.Realtime.Player> players = new List<Photon.Realtime.Player>();
        foreach (Photon.Realtime.Player p in Photon.Pun.PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties[HashableData.PLAYER_TEAM_CODE] == _p.CustomProperties[HashableData.PLAYER_TEAM_CODE])
            {
                players.Add(p);
            }
        }

        return players.ToArray();
    }

    public static Photon.Realtime.Player GetPlayerByName(this string _nickName)
    {
        foreach (Photon.Realtime.Player p in Photon.Pun.PhotonNetwork.PlayerList)
        {
            if (p.NickName.Trim() == _nickName.Trim())
            {
                return p;
            }
        }
        return null;
    }

    //ref :https://stackoverflow.com/questions/2656116/replace-named-group-in-regex-with-value
    public static string Replace(this string input, System.Text.RegularExpressions.Regex regex, string groupName, string replacement)
    {
        return regex.Replace(input, m =>
        {
            return ReplaceNamedGroup(input, groupName, replacement, m);
        });
    }

    private static string ReplaceNamedGroup(string input, string groupName, string replacement, Match m)
    {
        string capture = m.Value;
        capture = capture.Remove(m.Groups[groupName].Index - m.Index, m.Groups[groupName].Length);
        capture = capture.Insert(m.Groups[groupName].Index - m.Index, replacement);
        return capture;
    }

}
