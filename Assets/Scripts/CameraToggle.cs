using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraToggle : MonoBehaviour
{
    [Header("VCam References")]
    [SerializeField] private CinemachineCamera vcamFront;   // drag VCam_Front
    [SerializeField] private CinemachineCamera vcamSide;    // drag VCam_Side
    [SerializeField] private CinemachineCamera vcamBack;    // drag VCam_Back

    public CinemachineCamera VCamFront => vcamFront;
    public CinemachineCamera VCamSide  => vcamSide;
    public CinemachineCamera VCamBack  => vcamBack;

    private const int priorityHigh = 10;
    private const int priorityLow  = 5;

    void Start()
    {
        SetCamera(vcamFront);
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame) SetCamera(vcamFront);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) SetCamera(vcamSide);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) SetCamera(vcamBack);
    }

    public void SetCamera(CinemachineCamera active)
    {
        vcamFront.Priority = priorityLow;
        vcamSide.Priority  = priorityLow;
        vcamBack.Priority  = priorityLow;

        active.Priority = priorityHigh;

        Debug.Log($"Active camera: {active.name}");
    }
}
