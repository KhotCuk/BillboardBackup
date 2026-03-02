using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LerpAtoB : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Transform yang akan digerakkan")]
    [SerializeField] private Transform target;

    [Header("Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Settings")]
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private Key triggerKey = Key.G;

    private Coroutine _coroutine;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current[triggerKey].wasPressedThisFrame)
            StartLerp();
    }

    public void StartLerp()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(LerpRoutine());
    }

    private IEnumerator LerpRoutine()
    {
        Vector3 from = pointA.position;
        Vector3 to   = pointB.position;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = curve.Evaluate(elapsed / duration);
            target.position = Vector3.LerpUnclamped(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.position = to;
        _coroutine = null;
    }
}
