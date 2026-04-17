using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 2.0f;
    float mouse_X;
    float mouse_Y;
    public GameObject cameraAnchorObject;
    public GameObject cameraObject;
    private float offset_Y;

    public GameObject targetObject;

    public static int cameraMode = 0;

    void Start()
    {
        offset_Y = cameraAnchorObject.transform.position.y - targetObject.transform.position.y;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (cameraMode == 0)
            {
                cameraMode = 1;
            }
            else
            {
                cameraMode = 0;
            }
        }

        mouse_X = Input.GetAxis("Mouse X") * rotationSpeed;
        mouse_Y = Input.GetAxis("Mouse Y") * rotationSpeed;
        if (cameraMode == 0)
        {
            cameraObject.transform.RotateAround(cameraAnchorObject.transform.position, Vector3.up, mouse_X);
            cameraObject.transform.RotateAround(cameraAnchorObject.transform.position, cameraObject.transform.right, -mouse_Y);
        }
        else
        {
            if (Input.GetMouseButton(1))
            {
                cameraObject.transform.RotateAround(cameraAnchorObject.transform.position, Vector3.up, mouse_X);
                cameraObject.transform.RotateAround(cameraAnchorObject.transform.position, cameraObject.transform.right, -mouse_Y);
            }
        }

        // カメラアンカーをターゲットオブジェクトの位置に移動
        cameraAnchorObject.transform.position = new Vector3(
            targetObject.transform.position.x,
            targetObject.transform.position.y + offset_Y,
            targetObject.transform.position.z
        );

    }
}
