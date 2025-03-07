using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FixPlayerController : MonoBehaviour
{
    // 玩家刚体
    private Rigidbody playerRb;

    // 玩家移动速度
    [SerializeField] private float moveSpeed = 5f;
    
    // 这里的 cameraTransform 指向你的 FreeCamera（或者 Cinemachine FreeLook 的相机）
    public Transform cameraTransform;
    
    // 交互范围半径
    public float interactRadius = 1f;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        if (playerRb == null)
        {
            Debug.LogError("Rigidbody 组件未找到，请检查该对象是否挂载了 Rigidbody！");
        }
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform 未赋值，请在 Inspector 中设置！");
        }
        // 隐藏鼠标并锁定光标
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // 移动输入处理
    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        // 获取相机正方向和右方向，但忽略 Y 轴分量
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();
        
        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // 根据相机方向计算玩家移动方向
        Vector3 moveDirection = camForward * input.y + camRight * input.x;

        // 应用速度
        playerRb.velocity = moveDirection * moveSpeed;
    }

    // 在 LateUpdate 中更新玩家朝向，让玩家始终面向相机的水平方向
    private void LateUpdate()
    {
        if (cameraTransform != null)
        {
            // 只获取相机的 Y 轴旋转（忽略 X、Z 轴）
            float cameraYaw = cameraTransform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, cameraYaw, 0);
        }
    }

    // 交互输入处理
    private void OnInteract()
    {
        TryInteract();
    }

    // Scene 视图中绘制交互范围的球体（便于调试）
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }

    // 检测并执行交互
    private void TryInteract()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interactable(gameObject);
            }
        }
    }
}
