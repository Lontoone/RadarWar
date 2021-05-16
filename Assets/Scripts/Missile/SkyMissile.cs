using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//高空飛彈
public class SkyMissile : MonoBehaviour
{
    public const string SKY_MISSILE_GC = "SKY_MISSILE_GC";
    public Vector2 goalPoint = new Vector2();
    public float damage;
    public float speed=5;
    public float exploseRadious = 0.25f;

    Collider2D collider;
    private void Start()
    {
        GCManager.RegisterObject(SKY_MISSILE_GC, gameObject);
        collider = gameObject.GetComponent<Collider2D>();
        collider.enabled = false;
    }

    private void FixedUpdate()
    {
        //移動
        transform.position = Vector2.MoveTowards(transform.position, goalPoint, speed * Time.deltaTime);

        //抵達
        if (Vector2.Distance(transform.position , goalPoint)<0.01f) {
            OnReachedGoal();
        }
    }

    void OnReachedGoal() {
        Debug.Log("SKY missile reach goal ");

        //爆炸傷害
        ExplosionDamage(transform.position, damage);

        //TODO:產生爆炸效果
        GCManager.Destory(SKY_MISSILE_GC, gameObject);
    }

    void ExplosionDamage(Vector2 center, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.SendMessage("AddDamage");
            hitCollider.GetComponent<PlayerControl>()?.Hurt(damage);

            //TODO:晃動特效
        }
    }

}
