using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    PhotonView pv;
    //public float speed = 5;
    [HideInInspector]
    public PlayerManager playerManager;

    public TankDataSO tankData_clone;

    public TextMeshProUGUI name_text;
    public Transform shoot_point;

    //ActionController actionController;
    Camera playerCamera;
    public void Start()
    {
        pv = gameObject.GetComponent<PhotonView>();

        //Manager產生player時，順便告訴player產生它的manager是誰
        //用尋找該view ID的PlayerManager
        playerManager = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>();

        //產生tank資料
        string _tankName = (string)pv.InstantiationData[1];
        tankData_clone = MyExtension.CloneData<TankDataSO>(
          Resources.Load<TankDataSO>(GameResourcesControl.TANK_SO_PATH + _tankName));


        if (!pv.IsMine)
        {
            //Destroy(GetComponentInChildren<Camera>());
        }
        else
        {
            //設定視野
            //Camera _camera = GetComponentInChildren<Camera>();
            //_camera.orthographicSize = tankData_clone.sight;

            //playerCamera = playerManager.playerCameraObject.GetComponent<Camera>();
            //playerCamera.orthographicSize = tankData_clone.sight;
            playerManager._playerCamera.orthographicSize = tankData_clone.sight;

            Debug.Log("set view " + playerManager._playerCamera.orthographicSize);
            name_text.text = PhotonNetwork.NickName;


        }

    }



    public void Update()
    {
        if (!pv.IsMine)
            return;
        //temp WASD移動
        //transform.position = transform.position + new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * tankData_clone.moveSpeed * Time.deltaTime;
    }

    public void Hurt(float damage)
    {
        tankData_clone.HP -= damage;
        if (tankData_clone.HP <= 0)
        {
            //TODO: Die
            Debug.Log("Die");
        }
    }

    Coroutine cMove, cRotate, cShoot;

    public void Shoot(Vector2 v2)
    {
        if (cShoot == null)
        {
            cShoot = StartCoroutine(Shoot_coro(v2));
        }
        else
        {
            Debug.Log("shoot 還在冷卻中");
        }

    }

    IEnumerator Shoot_coro(Vector2 v2)
    {
        //TODO:使用interface?
        //產生子彈
        if (tankData_clone.tank_type == PlayerTankType.guerrilla) //直線飛彈
        {
            GroundMissile _gm = GCManager.Instantiate(GroundMissile.GROUND_MISSILE_GC).GetComponent<GroundMissile>();
            _gm.transform.position = shoot_point.position;
            _gm.dir = (shoot_point.position - transform.position).normalized;

        }
        else if (tankData_clone.tank_type == PlayerTankType.artillery)
        {
            SkyMissile _sm = GCManager.Instantiate(SkyMissile.SKY_MISSILE_GC).GetComponent<SkyMissile>();
            _sm.transform.position = shoot_point.position;
            _sm.goalPoint = v2;

        }

        //冷卻時間
        yield return new WaitForSeconds(tankData_clone.shootGapTime);
        cShoot = null;
    }

    public void Move(float _speed)
    {
        if (cMove != null)
        {
            StopCoroutine(cMove);
        }
        cMove = StartCoroutine(Move_coro(_speed));
    }
    public void Rotate(float deg)
    {
        if (cRotate != null)
        {
            StopCoroutine(cRotate);
        }
        cRotate = StartCoroutine(Rotate_coro(deg));
    }

    // *********delta動作**************
    IEnumerator Rotate_coro(float _deg)
    {
        //float min = transform.eulerAngles.z < _deg ? transform.eulerAngles.z : _deg;
        //float max = transform.eulerAngles.z > _deg ? transform.eulerAngles.z : _deg;
        while (transform.eulerAngles.z != _deg)
        {
            transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, _deg, tankData_clone.rotateSpeed * Time.deltaTime));
            yield return null;
        }
    }

    IEnumerator Move_coro(float _speed)
    {
        //while ! goal =>移動
        /*
        while (transform.position != _goal_vector)
        {
            transform.position = Vector2.MoveTowards(transform.position, _goal_vector, tankData_clone.moveSpeed * Time.deltaTime);
            yield return null;
        }*/

        while (_speed != 0)
        {
            transform.position = transform.position + transform.up * _speed * tankData_clone.moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
    //*********************************


    /*
    //由外部呼叫 =>觸發掛載動作
    public void AddMove(Vector2 _offset)
    {
        //set goal
        _goal_vector = (Vector2)transform.position + _offset;
        //actionController.AddAction(tankData_clone.walk_action);
    }
    public void AddRotate(float _offset)
    {
        //set goal 
        _goal_vector = new Vector3(0, 0, _offset + transform.eulerAngles.z);
        //actionController.AddAction(tankData_clone.rotate_action);
    }
    public void AddShoot(Vector2 _dir)
    {
        //TODO:check if in sight rnage range
        _goal_vector = _dir;
        //actionController.AddAction(tankData_clone.shoot_action);
    }*/

    //中斷目前動作
    public void Stop()
    {
        Debug.Log("中斷");
        /*
        if (cCurrentAction != null)
        {
            StopCoroutine(cCurrentAction);
            cCurrentAction = null;

        }
        */

        if (cRotate != null)
            StopCoroutine(cRotate);
        cRotate = null;

        if (cMove != null)
            StopCoroutine(cMove);
        cMove = null;
    }

}
