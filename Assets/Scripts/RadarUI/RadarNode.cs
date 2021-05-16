using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarNode : MonoBehaviour
{
    public const string NODE_GC_KEY = "NODE_GC_KEY";
    public float life_time = 1;

    [HideInInspector]
    public GameObject representObj;

    private void OnEnable()
    {
        //Destroy(gameObject, life_time); //TODO:回收
        Invoke("_Test", 1);

    }
    void _Test()
    {
        RadarUpdateControl.instance.spottedPlayers.Remove(representObj);
        Debug.Log(RadarUpdateControl.instance.spottedPlayers.Count);
        GCManager.Destory(NODE_GC_KEY, gameObject);
    }
    //TODO:fade off 特效



}
