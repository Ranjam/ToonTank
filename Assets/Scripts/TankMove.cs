using UnityEngine;
using System.Collections;

public class TankMove : MonoBehaviour {
    // 坦克控制器
    private TankController tank_;
    // 坦克速度
    public float speed = 10.0f;
    // 坦克角速度
    public float angularSpeed = 3.0f;
    // Joystick
    public ETCJoystick directionJoystick;

    void Start() {
        tank_ = this.GetComponent<TankController>();
    }

    void Update() { }

    // 每帧固定调用
    void FixedUpdate() {

        if (SystemInfo.deviceType == DeviceType.Desktop) {
            GetDesktopInput();
        }
        else if (SystemInfo.deviceType == DeviceType.Handheld) {
            GetMobileInput();
        }
    }

    void GetDesktopInput() {
        // 获取用户纵向输入
        float verticalInput = Input.GetAxis("Vertical");
        tank_.Move(verticalInput * speed);
        // 获取用户横向输入
        float horizontalInput = Input.GetAxis("Horizontal");
        tank_.Look(horizontalInput * angularSpeed);
        // 鼠标输入方向
        //float horizontalInputMouse = Input.GetAxis("Mouse X");
        //tank_.Look(horizontalInputMouse*angularSpeed);
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0)) {
            tank_.StorePower(1.0f * Time.deltaTime);
        }
        // 开火
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)) {
            tank_.Fire();
        }
    }

    void GetMobileInput() {

        // 移动
        float dx = directionJoystick.axisX.axisValue;
        float dy = directionJoystick.axisY.axisValue;
        Vector2 joyValue = new Vector2(dx, dy);
        tank_.LookAt(joyValue);
        tank_.Move(joyValue.magnitude * speed);

        // 攻击
        if (Input.touchCount > 0) {
            foreach (var touch in Input.touches) {
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
                    Vector2 touchPosition = touch.position;
                    if (touchPosition.x > Screen.width / 2) {
                        tank_.StorePower(1.0f * Time.deltaTime);
                    }
                } else if (touch.phase == TouchPhase.Ended) {
                    Vector2 touchPosition = touch.position;
                    if (touchPosition.x > Screen.width / 2) {
                        tank_.Fire();
                    }
                }
            }
        }
    }
}