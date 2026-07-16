using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    bool isCursorVisible = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = isCursorVisible;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (CameraController.cameraMode == 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isCursorVisible = !isCursorVisible;
                Cursor.visible = isCursorVisible;
                if (isCursorVisible)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
        else
        {
            isCursorVisible = true;
            Cursor.visible = isCursorVisible;

            if (Input.GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.Locked;

            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

    }
}
