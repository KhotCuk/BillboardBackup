using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.InputSystem;

public class SimpleSplineDolly : MonoBehaviour
{
    [Header("Movement")]
    public Transform target;
    public float duration = 3f;
    public bool alignToSpline = true;

    [Header("Spline")]
    [SerializeField] private SplineContainer spline;

    [Header("Camera Switching")]
    [SerializeField] private CameraToggle cameraToggle;
    public bool autoCameraSwitch = true;
    [SerializeField] private float sideCamThreshold = 0.05f;   // 5%
    [SerializeField] private float backCamThreshold = 0.95f;   // 95%

    private float progress      = 0f;
    private bool  isPlaying     = false;
    private float totalLength   = 0f;
    private int   currentCamIdx = -1;   // 0=front, 1=side, 2=back

    void Start()
    {
        if (spline == null)
            spline = GetComponent<SplineContainer>();

        totalLength = spline.Spline.GetLength();

        if (cameraToggle != null && autoCameraSwitch)
            cameraToggle.SetCamera(cameraToggle.VCamFront);
    }

    void Update()
    {
        if (Keyboard.current?.pKey.wasPressedThisFrame == true && !isPlaying)
        {
            isPlaying     = true;
            progress      = 0f;
            currentCamIdx = -1;
        }

        if (isPlaying)
        {
            progress += Time.deltaTime / duration;
            progress  = Mathf.Clamp01(progress);

            Spline    spl            = spline.Spline;
            Transform splineTransform = spline.transform;

            Vector3 localPos = spl.EvaluatePosition(progress);
            target.position  = splineTransform.TransformPoint(localPos);

            if (alignToSpline)
            {
                Vector3 localTangent = spl.EvaluateTangent(progress);
                if (localTangent.sqrMagnitude > 0.001f)
                {
                    Vector3 worldTangent = splineTransform.TransformDirection(localTangent);
                    target.rotation = Quaternion.LookRotation(worldTangent);
                }
            }

            HandleCameraSwitch();

            if (progress >= 1f)
                isPlaying = false;
        }
    }

    void HandleCameraSwitch()
    {
        if (cameraToggle == null || !autoCameraSwitch) return;

        int targetIdx;
        if      (progress >= backCamThreshold) targetIdx = 2;
        else if (progress >= sideCamThreshold) targetIdx = 1;
        else                                   targetIdx = 0;

        if (targetIdx == currentCamIdx) return;

        currentCamIdx = targetIdx;
        switch (targetIdx)
        {
            case 0: cameraToggle.SetCamera(cameraToggle.VCamFront); break;
            case 1: cameraToggle.SetCamera(cameraToggle.VCamSide);  break;
            case 2: cameraToggle.SetCamera(cameraToggle.VCamBack);  break;
        }
    }

    // Info jarak — bisa dibaca oleh UI script jika diperlukan
    public float TotalLength    => totalLength;
    public float DistanceTraveled => progress * totalLength;
}
