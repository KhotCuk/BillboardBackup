using UnityEngine;

/// <summary>
/// Script untuk merotasi objek dengan nilai konstan (kecepatan rotasi tetap).
/// </summary>
public class ConstantRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Kecepatan rotasi pada sumbu X (derajat per detik)")]
    [SerializeField] private float rotationSpeedX = 0f;
    
    [Tooltip("Kecepatan rotasi pada sumbu Y (derajat per detik)")]
    [SerializeField] private float rotationSpeedY = 0f;
    
    [Tooltip("Kecepatan rotasi pada sumbu Z (derajat per detik)")]
    [SerializeField] private float rotationSpeedZ = 0f;
    
    [Header("Options")]
    [Tooltip("Jika true, rotasi akan menggunakan Space.World. Jika false, menggunakan Space.Self (local)")]
    [SerializeField] private bool useWorldSpace = false;
    
    [Tooltip("Jika true, rotasi akan di-pause")]
    [SerializeField] private bool pauseRotation = false;
    
    private void Update()
    {
        if (pauseRotation)
            return;
        
        // Hitung rotasi berdasarkan delta time
        float deltaTime = Time.deltaTime;
        Vector3 rotation = new Vector3(
            rotationSpeedX * deltaTime,
            rotationSpeedY * deltaTime,
            rotationSpeedZ * deltaTime
        );
        
        // Terapkan rotasi
        if (useWorldSpace)
        {
            transform.Rotate(rotation, Space.World);
        }
        else
        {
            transform.Rotate(rotation, Space.Self);
        }
    }
    
    /// <summary>
    /// Set kecepatan rotasi untuk semua sumbu sekaligus
    /// </summary>
    public void SetRotationSpeed(float x, float y, float z)
    {
        rotationSpeedX = x;
        rotationSpeedY = y;
        rotationSpeedZ = z;
    }
    
    /// <summary>
    /// Set kecepatan rotasi pada sumbu tertentu
    /// </summary>
    public void SetRotationSpeedX(float speed) => rotationSpeedX = speed;
    public void SetRotationSpeedY(float speed) => rotationSpeedY = speed;
    public void SetRotationSpeedZ(float speed) => rotationSpeedZ = speed;
    
    /// <summary>
    /// Pause atau resume rotasi
    /// </summary>
    public void SetPause(bool pause) => pauseRotation = pause;
    
    /// <summary>
    /// Toggle pause rotasi
    /// </summary>
    public void TogglePause() => pauseRotation = !pauseRotation;
}

