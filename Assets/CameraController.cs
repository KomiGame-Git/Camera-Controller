using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 2.0f;
    [SerializeField, Range(60f, 90f)]
    float maxAngle_Y = 80.0f;
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

        Vector3 targetToCamera = (cameraObject.transform.position - targetObject.transform.position).normalized;
        Vector3 targetBack = new Vector3(targetToCamera.x, 0f, targetToCamera.z).normalized;
        Vector3 targetRight = Vector3.Cross(targetToCamera, targetBack).normalized;
        if (targetToCamera.y < 0)
        {
            targetRight = -targetRight;
        }

        Debug.DrawLine(targetObject.transform.position, targetObject.transform.position + targetBack * 2f, Color.red);
        Debug.DrawLine(targetObject.transform.position, targetObject.transform.position + targetRight * 2f, Color.green);
        Debug.DrawLine(targetObject.transform.position, targetObject.transform.position + targetToCamera * 2f, Color.blue);

        float angle = Vector3.SignedAngle(targetToCamera, targetBack, targetRight);
        Debug.Log($"targetToCamera: {targetToCamera}, angle: {angle}");
        if (cameraMode == 0)
        {
            // カメラをマウスの動きに合わせて回転
            cameraObject.transform.RotateAround(cameraAnchorObject.transform.position, Vector3.up, mouse_X);

            if (-mouse_Y > 0)
            {
                if (angle < maxAngle_Y - rotationSpeed)
                {
                    cameraObject.transform.RotateAround(cameraAnchorObject.transform.position, cameraObject.transform.right, -mouse_Y);
                }
            }
            else if (-mouse_Y < 0)
            {
                if (angle > -maxAngle_Y + rotationSpeed)
                {
                    cameraObject.transform.RotateAround(cameraAnchorObject.transform.position, cameraObject.transform.right, -mouse_Y);
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(1))
            {
                // 右クリックが押されている場合のみカメラを回転
                cameraObject.transform.RotateAround(cameraAnchorObject.transform.position, Vector3.up, mouse_X);

                if (-mouse_Y > 0)
                {
                    if (angle < maxAngle_Y - rotationSpeed)
                    {
                        cameraObject.transform.RotateAround(cameraAnchorObject.transform.position, cameraObject.transform.right, -mouse_Y);
                    }
                }
                else if (-mouse_Y < 0)
                {
                    if (angle > -maxAngle_Y + rotationSpeed)
                    {
                        cameraObject.transform.RotateAround(cameraAnchorObject.transform.position, cameraObject.transform.right, -mouse_Y);
                    }
                }
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
