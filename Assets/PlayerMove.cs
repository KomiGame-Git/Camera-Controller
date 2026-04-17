using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private float jumpForce = 5.0f;

    private float moveHorizontal;

    private float moveVertical;

    [SerializeField]
    private GameObject cameraObject;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (cameraObject == null)
        {
            cameraObject = GameObject.FindWithTag("MainCamera");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // キーボードの入力を取得
        // Horizontal: A,Dキーまたは←,→キー、Vertical: W,Sキーまたは↑,↓キー
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        // cameraModeが0のときはカメラ方向に合わせる
        // 0のときはカメラ方向に合わせる
        // 1のときはキーボード入力方向に合わせる
        if (CameraController.cameraMode == 0)
        {
            FollowCameraDirection();
        }
        else
        {
            FollowKeyboardDirection();
        }

        // プレイヤーを移動させる
        MovePosition();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void MovePosition()
    {
        // 移動方向を決定
        Vector3 inputDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);
        // 正規化:矢印の長さを1にする
        inputDirection.Normalize();

        Vector3 moveVelocity;
        if (CameraController.cameraMode == 0)
        {
            // カメラの向きに合わせて移動方向を変換
            moveVelocity = transform.TransformDirection(inputDirection) * speed;
        }
        else
        {
            // カメラの向きを取得
            Vector3 cameraForward = cameraObject.transform.forward;
            // 水平方向のみに制限
            Vector3 cameraDirection = new Vector3(cameraForward.x, 0, cameraForward.z);

            // カメラの向きに対する入力方向の角度を計算
            float angle = Vector3.SignedAngle(Vector3.forward, cameraDirection, Vector3.up);
            // 入力方向をカメラの向きに合わせて回転
            Vector3 rotatedInputDirection = Quaternion.Euler(0, angle, 0) * inputDirection;

            moveVelocity = rotatedInputDirection * speed;
        }

        // 重力の影響を維持
        Vector3 gravity = new Vector3(0, rb.velocity.y, 0);
        moveVelocity += gravity;

        // Rigidbodyの速度(velocity)を設定
        rb.velocity = moveVelocity;
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void FollowKeyboardDirection()
    {
        // カメラの向きを取得
        Vector3 cameraForward = cameraObject.transform.forward;
        // 水平方向のみに制限
        Vector3 cameraDirection = new Vector3(cameraForward.x, 0, cameraForward.z);
        // 入力方向をカメラの向きに合わせて回転させるためのベクトルを作成
        Vector3 inputDirection = new Vector3(moveHorizontal, 0, moveVertical);

        // moveHorizontalかmoveVerticalのいずれかがある程度の大きさがある場合のみ向きを変える
        if (Mathf.Abs(moveHorizontal) > 0.01f || Mathf.Abs(moveVertical) > 0.01f)
        {
            // カメラの向きに対する入力方向の角度を計算
            float angle = Vector3.SignedAngle(Vector3.forward, cameraDirection, Vector3.up);
            // 入力方向をカメラの向きに合わせて回転
            Vector3 rotatedInputDirection = Quaternion.Euler(0, angle, 0) * inputDirection;
            // プレイヤー目線での向きたい方向を表す角度(rotation: Quaternion)をQuaternion.LookRotationで取得
            Quaternion targetRotation = Quaternion.LookRotation(rotatedInputDirection);
            // その角度にプレイヤーの向きを変える
            transform.rotation = targetRotation;

            // プレイヤーの向きを徐々に変える場合は以下を使う
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f);
        }
    }

    void FollowCameraDirection()
    {
        // カメラの向きを取得
        Vector3 cameraForward = cameraObject.transform.forward;
        // 水平方向のみに制限
        Vector3 direction = new Vector3(cameraForward.x, 0, cameraForward.z);

        // プレイヤー目線での向きたい方向を表す角度(rotation: Quaternion)をQuaternion.LookRotationで取得
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        // その角度にプレイヤーの向きを変える
        transform.rotation = targetRotation;

        // プレイヤーの向きを徐々に変える場合は以下を使う
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f);
    }
}
