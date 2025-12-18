using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static SimpleOre;

public class FlyingOreIcon : MonoBehaviour
{
    public float arcHeight = 80f; 

    Image img;
    RectTransform rt;

    void Awake()
    {
        img = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
    }


    public void StartFlying(Vector2 startScreenPos, RectTransform endAnchor, float duration,OreType theoreType,bool ismouse)
    {
        rt.anchoredPosition = ScreenPointToAnchored(startScreenPos);
        Vector2 endPos = endAnchor.anchoredPosition;
        Vector2 midPos = (rt.anchoredPosition + endPos) * 0.5f + Vector2.up * arcHeight;

        StartCoroutine(FlyRoutine(rt.anchoredPosition, midPos, endPos, duration,theoreType,ismouse));
    }

    IEnumerator FlyRoutine(Vector2 start, Vector2 mid, Vector2 end, float t, OreType theoreType, bool ismouse)
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<DropBounce>()?.PlayBounce();
        float elapsed = 0;
        while (elapsed < t)
        {
            elapsed += Time.deltaTime;
            float p = elapsed / t;
            Vector2 pos = Vector2.Lerp(
                            Vector2.Lerp(start, mid, p),
                            Vector2.Lerp(mid, end, p),
                            p);
            rt.anchoredPosition = pos;
            rt.localScale = Vector3.one * Mathf.Lerp(1f, 0.3f, p);
            yield return null;
        }


        OreDropSpawner.Instance.ApplyOreEffect(theoreType,ismouse);
        Destroy(gameObject);
    }



    Vector2 ScreenPointToAnchored(Vector2 screenPoint)
    {
        Vector2 localPoint;
        Canvas canvas = GetComponentInParent<Canvas>();
        Camera cam = canvas.worldCamera;   
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            cam = null;                      

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rt.parent as RectTransform,
            screenPoint,
            cam,  
            out localPoint);
        return localPoint;
    }
}