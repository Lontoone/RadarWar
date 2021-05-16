using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
public class CoordinateCamera : MonoBehaviour
{
    [SerializeField]
    CoordinateSide side;
    Camera camera;
    float origin_size;

    public GameObject player;
    public IEnumerator Start()
    {
        yield return null;
        player = GameSceneManager.instance.localPlayer;
        camera = GetComponent<Camera>();
        origin_size = camera.orthographicSize;
        camera.orthographicSize = origin_size * player.GetComponent<PlayerControl>().tankData_clone.sight;
    }

    public void Update()
    {
        if (side == CoordinateSide.x)
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        }
        else if (side == CoordinateSide.y)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
    }

    enum CoordinateSide
    {
        x, y
    }
}
