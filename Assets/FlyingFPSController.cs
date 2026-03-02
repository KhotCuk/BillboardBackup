using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// FPS-style flying movement controller with mouse look.
/// Karakter terbang tanpa gravitasi, rotasi dikontrol mouse seperti FPS normal.
/// </summary>
public class FlyingFPSController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float acceleration = 10f;
    
    [Header("Mouse Look Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float verticalLookLimit = 80f; // Batas rotasi vertikal (derajat)
    
    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;
    private Vector3 velocity;
    private bool isCursorLocked = true;
    
    private void Start()
    {
        // Lock cursor ke tengah layar saat play
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update()
    {
        // Toggle cursor lock dengan Escape
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
#else
        if (Input.GetKeyDown(KeyCode.Escape))
#endif
        {
            isCursorLocked = !isCursorLocked;
            if (isCursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        
        // Update mouse look hanya jika cursor terkunci
        if (isCursorLocked)
        {
            UpdateMouseLook();
        }
        
        UpdateMovement();
    }
    
    private void UpdateMouseLook()
    {
        // Input mouse untuk rotasi
#if ENABLE_INPUT_SYSTEM
        float mouseX = 0f;
        float mouseY = 0f;
        if (Mouse.current != null)
        {
            mouseX = Mouse.current.delta.ReadValue().x * mouseSensitivity * 0.01f;
            mouseY = Mouse.current.delta.ReadValue().y * mouseSensitivity * 0.01f;
        }
#else
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
#endif
        
        // Rotasi horizontal (Y-axis) - putar karakter
        horizontalRotation += mouseX;
        
        // Rotasi vertikal (X-axis) - batasi rotasi kamera
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        
        // Terapkan rotasi ke transform
        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
    }
    
    private void UpdateMovement()
    {
        // Input keyboard untuk movement
        float inputX = 0f;
        float inputZ = 0f;
        float inputY = 0f;
        
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            // Horizontal movement (A/D atau Left/Right Arrow)
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                inputX += 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                inputX -= 1f;
            
            // Vertical movement (W/S atau Up/Down Arrow)
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                inputZ += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                inputZ -= 1f;
            
            // Vertical movement (up/down) - Space untuk naik, Left Ctrl untuk turun
            if (Keyboard.current.spaceKey.isPressed)
                inputY += 1f;
            if (Keyboard.current.leftCtrlKey.isPressed)
                inputY -= 1f;
        }
#else
        inputX = Input.GetAxisRaw("Horizontal");
        inputZ = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKey(KeyCode.Space))
            inputY += 1f;
        if (Input.GetKey(KeyCode.LeftControl))
            inputY -= 1f;
#endif
        
        // Normalize input direction
        Vector3 inputDir = new Vector3(inputX, inputY, inputZ).normalized;
        
        // Sprint multiplier
        float targetSpeed = moveSpeed;
#if ENABLE_INPUT_SYSTEM
        bool sprinting = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
#else
        bool sprinting = Input.GetKey(KeyCode.LeftShift);
#endif
        if (sprinting)
            targetSpeed *= sprintMultiplier;
        
        // Calculate target velocity berdasarkan arah transform (forward, right, up)
        Vector3 targetVelocity = transform.forward * inputZ * targetSpeed +
                                 transform.right * inputX * targetSpeed +
                                 transform.up * inputY * targetSpeed;
        
        // Smooth acceleration
        velocity = Vector3.Lerp(velocity, targetVelocity, 1f - Mathf.Exp(-acceleration * Time.deltaTime));
        
        // Apply movement
        transform.position += velocity * Time.deltaTime;
    }
}

