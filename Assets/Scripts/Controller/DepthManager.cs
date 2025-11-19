using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DepthManager : MonoBehaviour
{
    [Header("UI引用")]
    public Text depthText;
    public Text layerText;
    public Slider depthSlider;
    public Text depthValueText; // 显示具体数值的文本

    [Header("深度设置")]
    public float currentDepth = 0f;
    public float maxDepth = 25f; // 第一层最大深度
    public int currentLayer = 1;

    [Header("玩家引用")]
    public Transform playerTransform;

    private float startYPosition; // 玩家起始Y坐标

    void Start()
    {
        // 初始化
        if (playerTransform != null)
        {
            startYPosition = playerTransform.position.y;
        }

        // 根据层数设置最大深度
        UpdateMaxDepthByLayer();

        // 更新UI
        UpdateDepthUI();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            CalculateDepth();
            UpdateDepthUI();
        }
    }

    // 计算当前深度
    void CalculateDepth()
    {
        // 深度 = 起始Y坐标 - 当前Y坐标（因为越往下Y值越小）
        currentDepth = Mathf.Max(0, startYPosition - playerTransform.position.y);

        // 限制深度不超过最大值
        currentDepth = Mathf.Min(currentDepth, maxDepth);
    }

    // 更新UI显示
    void UpdateDepthUI()
    {
        // 更新深度文本
        if (depthText != null)
        {
            depthText.text = $"深度: {currentDepth:F1}m";
        }

        // 更新深度数值文本
        if (depthValueText != null)
        {
            depthValueText.text = $"{currentDepth:F1}/{maxDepth}m";
        }

        // 更新层数文本
        if (layerText != null)
        {
            layerText.text = $"第 {currentLayer} 层";
        }

        // 更新进度条
        if (depthSlider != null)
        {
            depthSlider.maxValue = maxDepth;
            depthSlider.value = currentDepth;
        }
    }

    // 根据层数更新最大深度
    void UpdateMaxDepthByLayer()
    {
        maxDepth = currentLayer switch
        {
            1 => 25f,  // 第一层
            2 => 25f,  // 第二层  
            3 => 30f,  // 第三层
            _ => 25f   // 默认
        };
    }

    // 切换到下一层
    public void GoToNextLayer()
    {
        currentLayer++;
        currentDepth = 0f; // 重置深度

        // 更新玩家起始位置
        if (playerTransform != null)
        {
            startYPosition = playerTransform.position.y;
        }

        UpdateMaxDepthByLayer();
        UpdateDepthUI();

        Debug.Log($"进入第 {currentLayer} 层，最大深度: {maxDepth}m");
    }

    // 检查是否到达底部
    public bool HasReachedBottom()
    {
        return currentDepth >= maxDepth;
    }

    // 获取深度百分比（0-1）
    public float GetDepthPercentage()
    {
        return currentDepth / maxDepth;
    }

    // 设置玩家引用（如果开始时没设置）
    public void SetPlayer(Transform player)
    {
        playerTransform = player;
        startYPosition = playerTransform.position.y;
    }
}