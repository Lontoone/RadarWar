using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Missile : MonoBehaviour
{
    public const string MISSILE_GC_KEY = "MISSILE_GC_KEY";
    public float speed = 5;
    private void Start()
    {
        GCManager.RegisterObject(MISSILE_GC_KEY, gameObject);
    }

    private void FixedUpdate()
    {
        //TODO 移動

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO: 打到物件
        Debug.Log(""+collision.gameObject.name);
    }
}
