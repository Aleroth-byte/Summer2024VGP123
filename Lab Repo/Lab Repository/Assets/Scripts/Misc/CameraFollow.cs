using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;


    public float minXClamp = -0.56f;
    public float maxXClamp = 236.9f;

    public float minYClamp = -0.56f;
    public float maxYClamp = 236.9f;


    private void LateUpdate()
    {
        Vector3 cameraPos = transform.position;

        cameraPos.x = Mathf.Clamp(player.transform.position.x, minXClamp, maxXClamp);
        cameraPos.y = Mathf.Clamp(player.transform.position.y, minXClamp, maxXClamp);

        transform.position = cameraPos;
    }
}
