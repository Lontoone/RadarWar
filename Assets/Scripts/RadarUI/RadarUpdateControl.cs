using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarUpdateControl : MonoBehaviour
{
    public static RadarUpdateControl instance;
    public Transform uiBar;
    public float speed = 10;

    [SerializeField]
    GameObject playerObj;
    PlayerControl playerControl;

    public GameObject node_prefab;

    [HideInInspector]
    public List<GameObject> spottedPlayers = new List<GameObject>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        playerObj = GameSceneManager.instance.localPlayer;  //等等使用
        playerControl = playerObj.GetComponent<PlayerControl>();
        GCManager.RegisterObject(RadarNode.NODE_GC_KEY, Instantiate(node_prefab));
    }

    Vector2 _temp_dir;

    private void FixedUpdate()
    {
        float zDir = Time.fixedTime % 360 * speed;
        uiBar.transform.eulerAngles = new Vector3(0, 0, zDir);


        //從玩家開始發射ray
        float _theta = Mathf.Deg2Rad * (zDir + 90); //圖片直立的
        Vector2 _dir = new Vector2(
            Mathf.Cos(_theta),
            Mathf.Sin(_theta)
            );
        RaycastHit2D[] hits = Physics2D.RaycastAll(playerObj.transform.position, _dir, playerControl.tankData_clone.sight);

        _temp_dir = _dir; //debug

        Debug.DrawRay(transform.position, _dir, Color.red, 1, true);

        //若ray打到物件=> 在world座標上產生node
        if (hits.Length > 0)
        {
            Debug.Log("hit length " + hits.Length);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform != playerObj.transform &&
                    !spottedPlayers.Contains(hits[i].transform.gameObject))
                {
           
                    GameObject _node = GCManager.Instantiate(RadarNode.NODE_GC_KEY);

                    _node.transform.position = hits[i].transform.position;

                    _node.GetComponent<RadarNode>().representObj = hits[i].transform.gameObject;
                    spottedPlayers.Add(hits[i].transform.gameObject);
                }
            }
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(playerObj.transform.position, _temp_dir);
    }

}
