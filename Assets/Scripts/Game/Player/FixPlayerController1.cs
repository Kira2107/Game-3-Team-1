using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FixPlayerController : MonoBehaviour
{
    // 玩家刚体
    private Rigidbody playerRb;
    // Animator 组件（挂载在父物体上）
    private Animator animator;

    // 玩家移动速度
    [SerializeField] private float moveSpeed = 5f;
    
    // 指向摄像机（例如 FreeCamera 或 Cinemachine FreeLook）
    public Transform cameraTransform;
    
    // 交互范围半径
    public float interactRadius = 1f;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        if (playerRb == null)
        {
            UnityEngine.Debug.LogError("Rigidbody 组件未找到，请检查该对象是否挂载了 Rigidbody！");
        }
        // 获取父物体上的 Animator 组件
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            UnityEngine.Debug.LogError("Animator 组件未找到，请检查该对象是否挂载了 Animator！");
        }
        if (cameraTransform == null)
        {
            UnityEngine.Debug.LogError("Camera Transform 未赋值，请在 Inspector 中设置！");
        }
        // 隐藏鼠标并锁定光标
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        animator.SetBool("isMoving", true);
    }

    // 移动输入处理
    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        // 获取摄像机正方向和右方向，但忽略 Y 轴分量
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();
        
        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // 根据摄像机方向计算玩家移动方向
        Vector3 moveDirection = camForward * input.y + camRight * input.x;
        playerRb.velocity = moveDirection * moveSpeed;

        // 当移动输入存在时，播放移动动画
        if (animator != null)
        {
            // 这里设定一个阈值，避免误判轻微抖动为移动
            bool isMoving = moveDirection.sqrMagnitude > 0.001f;
           //animator.SetBool("isMoving", isMoving);
        }
    }

    // 在 LateUpdate 中更新玩家朝向，使其始终面向摄像机的水平方向
    private void LateUpdate()
    {
        if (cameraTransform != null)
        {
            //float cameraYaw = cameraTransform.eulerAngles.x;
            //transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
    }

    // 交互输入处理
    private void OnInteract()
    {
        TryInteract();
    }

    // 在 Scene 视图中绘制交互范围的球体（便于调试）
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
