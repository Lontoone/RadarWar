using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//坦克資料
[CreateAssetMenu(menuName ="TankData")]
public class TankDataSO : ScriptableObject
{
    public string tankName;
    public PlayerTankType tank_type;

    public float HP=100;
    public float damage=20;
    public float sight=1;
    public float moveSpeed = 5;
    public float rotateSpeed=1;
    public float shootGapTime = 1;
    public float shootSightAngleRange = 60; //in deg

    public ActionController.mAction walk_action=new ActionController.mAction();
    public ActionController.mAction rotate_action = new ActionController.mAction();
    public ActionController.mAction shoot_action = new ActionController.mAction();

    public Sprite icon;
    
}
public enum PlayerTankType
    {
        signaller, //通訊兵
        guerrilla, //游擊兵
        artillery, //長砲兵

    }
