using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundMissile : MonoBehaviour
{
    public const string GROUND_MISSILE_GC = "GROUND_MISSILE_GC";
    public Vector2 dir = new Vector2();
    public float damage;
    public float speed = 5;
    public float leftTime = 1;
    private void Start()
    {
        GCManager.RegisterObject(GROUND_MISSILE_GC, gameObject);
    }
    private void OnEnable()
    {
        Invoke("Destory_Self", 1);
    }
    void Destory_Self()
    {
        GCManager.Destory(GROUND_MISSILE_GC, gameObject);
    }
    private void FixedUpdate()
    {
        transform.position = (Vector2)transform.position + dir * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();
        if (player != null)
        {
            Debug.Log("ground missile hits " + collision.transform.position);
            player.Hurt(damage);
        }*/

        Debug.Log("Hit " + collision.gameObject.name);
        HitableObj.Hit_event_c(collision.gameObject, damage);
        //TODO:效果
        GCManager.Destory(GROUND_MISSILE_GC, gameObject);
    }


}
