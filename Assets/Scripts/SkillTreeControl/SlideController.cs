using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Required for Lists
using System.Collections;

public class SlideController : MonoBehaviour
{
    [Header("Settings")]
    public RectTransform targetImage;
    public float animationDuration = 0.5f;

    [Header("Button Setup")]
    [Tooltip("Drag all your buttons here. The script will auto-connect them.")]
    public List<Button> triggerButtons; 

    private Vector2 onScreenPosition;
    private Vector2 offScreenPosition;
    private Coroutine currentCoroutine;

    void Start()
    {
        // 1. Setup Positions (Same as before)
        onScreenPosition = targetImage.anchoredPosition;
        offScreenPosition = new Vector2(Screen.width, onScreenPosition.y);
        targetImage.anchoredPosition = offScreenPosition;

        // 2. PROGRAMMATICALLY CONNECT BUTTONS
        // We loop through the list and tell every button to listen to "SlideIn"
        foreach (Button btn in triggerButtons)
        {
            if (btn != null)
            {
                // This replaces the manual "On Click" drag-and-drop
                btn.onClick.RemoveListener(SlideIn); 
                btn.onClick.AddListener(SlideIn); 
            }
        }
    }

    // This function remains the same
    void SlideIn()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(MoveToPosition(onScreenPosition));
    }

    // This function remains the same
    private IEnumerator MoveToPosition(Vector2 target)
    {
        Vector2 startPos = targetImage.anchoredPosition;
        float elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            t = t * t * (3f - 2f * t); 
            targetImage.anchoredPosition = Vector2.Lerp(startPos, target, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetImage.anchoredPosition = target;
    }
    
    // Good practice: Clean up listeners if this object is destroyed
    void OnDestroy()
    {
        foreach (Button btn in triggerButtons)
        {
            if(btn != null) btn.onClick.RemoveListener(SlideIn);
        }
    }
}