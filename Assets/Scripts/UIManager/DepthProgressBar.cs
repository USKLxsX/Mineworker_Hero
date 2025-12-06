using UnityEngine;
using UnityEngine.UI;

public class DepthProgressBar : MonoBehaviour
{
    [Header("深度进度条")]
    public RectTransform depthBar;
    public Transform player;
    public float maxDepth = 30f;
    public float barMaxHeight = 400f; // 进度条最大高度

    private float startY;
    private float originalBarHeight;

    void Start()
    {
        if (player != null)
        {
            startY = player.position.y;
        }
        if (depthBar != null)
        {
            originalBarHeight = depthBar.sizeDelta.y;
        }
    }

    void Update()
    {
        if (player != null)
        {
            float currentDepth = Mathf.Max(0, startY - player.position.y);
            float progress = currentDepth / maxDepth;

            if (depthBar != null)
            {
                // 竖直方向变短：越往下深度越大，进度条高度越小
                Vector2 size = new Vector2(depthBar.sizeDelta.x, (1 - progress) * barMaxHeight);
                depthBar.sizeDelta = size;
            }
        }
    }

    // 重置深度进度（进入新层时调用）
    public void ResetDepth()
    {
        if (player != null)
        {
            startY = player.position.y;
        }
    }
}