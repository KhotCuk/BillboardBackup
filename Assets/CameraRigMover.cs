using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// Simple movement controller for a GameObject (no camera / no mouse-look).
/// </summary>
public class CameraRigMover : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float acceleration = 12f;

    private Vector3 _velocity;

    private void Update()
    {
        UpdateMove();
    }

    private void UpdateMove()
    {
#if ENABLE_INPUT_SYSTEM
        float inputX = 0f;
        float inputZ = 0f;
        if (Keyboard.current != null)
        {
            inputX += Keyboard.current.dKey.isPressed ? 1f : 0f;
            inputX -= Keyboard.current.aKey.isPressed ? 1f : 0f;
            inputZ += Keyboard.current.wKey.isPressed ? 1f : 0f;
            inputZ -= Keyboard.current.sKey.isPressed ? 1f : 0f;

            inputX += Keyboard.current.rightArrowKey.isPressed ? 1f : 0f;
            inputX -= Keyboard.current.leftArrowKey.isPressed ? 1f : 0f;
            inputZ += Keyboard.current.upArrowKey.isPressed ? 1f : 0f;
            inputZ -= Keyboard.current.downArrowKey.isPressed ? 1f : 0f;
        }
#else
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");
#endif
        Vector3 inputDir = new Vector3(inputX, 0f, inputZ).normalized;

        float targetSpeed = moveSpeed;
#if ENABLE_INPUT_SYSTEM
        bool sprinting = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
#else
        bool sprinting = Input.GetKey(KeyCode.LeftShift);
#endif
        if (sprinting)
            targetSpeed *= sprintMultiplier;

        Vector3 targetVelocity = transform.TransformDirection(inputDir) * targetSpeed;
        _velocity = Vector3.Lerp(_velocity, targetVelocity, 1f - Mathf.Exp(-acceleration * Time.deltaTime));

        transform.position += _velocity * Time.deltaTime;
    }
}

