using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//沒用到
public class RadarUnitItem : MonoBehaviour
{
    Text unit_text;
    public Camera RTcamera;
    public int origin_viewPort_offset = 0; //原始的位移
    public RectTransform radar_rt_image;

    Vector2 cam_start_pos; //用來記錄camera位移
    Vector2 RTCamera_size;
    float sight;


    public void Start()
    {
        unit_text = GetComponent<Text>();
        cam_start_pos = RTcamera.transform.position;

        sight = RTcamera.orthographicSize;
        RTCamera_size = new Vector2(RTcamera.targetTexture.width, RTcamera.targetTexture.height);
    }
    public void Update()
    {
        //world camera offset
        Vector2 cameraOffset = (Vector2)RTcamera.transform.position - cam_start_pos;

        if (cameraOffset == Vector2.zero) { return; }

        //和玩家走反方向
        cameraOffset *= -1;

        //to viewport offset
        Vector2 viewport_cameraOffset = new Vector2(cameraOffset.x / sight, cameraOffset.y / sight);

        //apply origin offset
        //viewport_cameraOffset.x += origin_viewPort_offset;


        //viewport offset to screen offset
        Vector2 screen_offset = RTCamera_size * viewport_cameraOffset;//(Vector2)RTcamera.ViewportToScreenPoint(viewport_cameraOffset);
        //RangeIn_RTcamera_size(ref screen_offset);

        //use screen offset to move the text 
        transform.localPosition = new Vector2(screen_offset.x + transform.localPosition.x, transform.localPosition.y);

        Debug.Log("camera offset " + cameraOffset +
                   "\n viewPort_offset " + viewport_cameraOffset +
                   "\n screenOffset " + screen_offset);

        cam_start_pos = RTcamera.transform.position;


    }

    void RangeIn_RTcamera_size(ref Vector2 input)
    {
        input.x = Mathf.Clamp(input.x, 0, RTCamera_size.x);
        input.y = Mathf.Clamp(input.y, 0, RTCamera_size.y);
    }

}
