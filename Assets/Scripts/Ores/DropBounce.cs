using System.Collections;
using UnityEngine;

public class DropBounce : MonoBehaviour
{
    public float bounceScale = 1.3f;  
    public float bounceDuration = 0.15f;

    Vector3 baseScale;

    void Awake() => baseScale = transform.localScale;

    public void PlayBounce()
    {
        StopAllCoroutines();
        StartCoroutine(BounceRoutine());
    }

    IEnumerator BounceRoutine()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / bounceDuration;
            transform.localScale = baseScale * Mathf.Lerp(1f, bounceScale, t);
            yield return null;
        }
        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / bounceDuration;
            transform.localScale = baseScale * Mathf.Lerp(bounceScale, 1f, t);
            yield return null;
        }
        transform.localScale = baseScale;
    }
}