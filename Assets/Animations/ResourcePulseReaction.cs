using UnityEngine;
using System.Collections;

public class ResourcePulseReaction : MonoBehaviour
{
    [Header("Pulse Settings")]
    public float duration = 1.5f;
    public float pulseStrength = 0.05f;
    public float pulseSpeed = 8f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void Pulse()
    {
        StopAllCoroutines();
        StartCoroutine(PulseRoutine());
    }

    IEnumerator PulseRoutine()
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float wave = Mathf.Sin(timer * pulseSpeed);
            float scaleMultiplier =
                1f + wave * pulseStrength;
            transform.localScale =
                originalScale * scaleMultiplier;
            yield return null;
        }
        transform.localScale = originalScale;
    }
}