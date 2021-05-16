using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
//雷達單位控制
public class RadarUnitControl : MonoBehaviour
{
    public Transform radarUI_parent;
    public Text textPrefab;

    Vector2 center = new Vector2(0, 0);
    Vector2 mapBounds = new Vector2(14, 14);
    public SpriteRenderer mapSR;


    [ContextMenu("GenerateRadarUnitText")]
    public void GenerateRadarUnitText()
    {


        Vector2 _startPoint = center - mapBounds; //mapSR.bounds.min;
        Vector2 _EndPoint = center + mapBounds;

        GameObject emyptyObj = new GameObject();

        GameObject xParent = Instantiate(emyptyObj, center, Quaternion.identity, radarUI_parent);
        GameObject yParent = Instantiate(emyptyObj, center, Quaternion.identity, radarUI_parent);
        //Destroy(emyptyObj);

        xParent.layer =LayerMask.NameToLayer("xLayer");
        yParent.layer = LayerMask.NameToLayer("yLayer");


        //X軸
        for (float x = _startPoint.x; x <= _EndPoint.x; x++)
        {
            Text text = Instantiate(textPrefab, new Vector2(x, 0), Quaternion.identity, xParent.transform);
            text.text = Mathf.CeilToInt(x).ToString();
            text.gameObject.name = "x " + x;
        }

        //Y軸
        for (float y = _startPoint.y; y <= _EndPoint.y; y++)
        {
            Text text = Instantiate(textPrefab, new Vector2(0, y), Quaternion.identity, yParent.transform);
            text.text = Mathf.CeilToInt(y).ToString();
            text.gameObject.name = "y " + y;
        }
    }
}
