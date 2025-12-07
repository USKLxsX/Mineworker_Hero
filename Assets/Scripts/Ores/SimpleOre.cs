using UnityEngine;
using UnityEngine.UI;
using static TreeEditor.TreeEditorHelper;

public class SimpleOre : MonoBehaviour
{
    public enum OreType
    {
        Normal = 0,     // 普通矿石 - 无效果
        Ruby = 1,       // 红宝石矿石 - 加生命
        Blue = 2,       // 蓝宝石矿石 - 加攻击
        Purple = 3,     // 紫水晶矿石 - 加生命和攻击
        Hard = -1,      // 坚硬矿石 - 无法挖掘
        Lava = 4        // 熔岩块 - 扣生命
    }

    [Header("矿石类型标签")]
    public OreType oreType = OreType.Normal;

    [Header("UI图片显示")]
    public SpriteRenderer oreUIImage;

    [Header("矿石图片资源 - 按顺序对应枚举")]
    public Sprite[] oreSprites;

    [Header("基础挖掘时间（秒）")]
    public float baseDigTime = 1.0f;

    // 私有变量
    private float digProgress = 0f;          // 当前挖掘进度（0-1）
    private bool isBeingDug = false;         // 是否正在被挖掘
    private float currentDigTime = 1.0f;     // 当前实际挖掘时间
    private float lastUpdateTime = 0f;       // 上次更新时间（用于计算中断后的进度）
    private bool isFirstDig = true;          // 是否是第一次挖掘

    void Start()
    {
        // 设置Unity标签（用于碰撞检测）
        gameObject.tag = "Ore";

        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        // 根据矿石类型设置基础挖掘时间
        SetBaseDigTime();

        // 初始化挖掘时间
        UpdateDigTimeFromPlayerSpeed();

        // 自动设置UI图片
        AutoSetupOreImage();

        // 初始化时间戳
        lastUpdateTime = Time.time;
    }

    void SetBaseDigTime()
    {
        baseDigTime = oreType switch
        {
            OreType.Normal => 1.0f,     // 普通矿石：1秒
            OreType.Ruby => 1.0f,       // 红宝石：1秒
            OreType.Blue => 1.0f,       // 蓝宝石：1秒
            OreType.Purple => 1.5f,     // 紫水晶：1.5秒（策划案要求）
            OreType.Hard => float.MaxValue, // 坚硬矿石：无法挖掘
            OreType.Lava => 1.0f,       // 熔岩块：1秒
            _ => 1.0f
        };

        // Debug.Log($"初始化{oreType}矿石，基础挖掘时间: {baseDigTime}秒");
    }

    // 根据玩家当前的挖矿速度更新挖掘时间
    public void UpdateDigTimeFromPlayerSpeed()
    {
        if (oreType == OreType.Hard)
        {
            currentDigTime = float.MaxValue;
            return;
        }

        if (GameDateController.Instance != null)
        {
            float playerMineSpeed = GameDateController.Instance.minespeed;
            currentDigTime = baseDigTime / playerMineSpeed;
        }
        else
        {
            currentDigTime = baseDigTime;
        }
    }

    // 核心方法：根据oreType枚举自动设置图片
    public void AutoSetupOreImage()
    {
        if (oreUIImage == null || oreSprites == null || oreSprites.Length == 0)
        {
            Debug.LogError("矿石UI组件未设置完整！");
            return;
        }

        int spriteIndex = (int)oreType + 1;

        if (spriteIndex >= 0 && spriteIndex < oreSprites.Length)
        {
            oreUIImage.sprite = oreSprites[spriteIndex];
        }
    }

    void Update()
    {
        if (isBeingDug)
        {
            // 实时更新挖矿速度（确保挖矿速度变化时进度正确）
            UpdateDigTimeFromPlayerSpeed();

            // 计算时间增量
            float deltaTime = Time.time - lastUpdateTime;
            lastUpdateTime = Time.time;

            // 计算进度增量
            float progressIncrement = 0f;
            if (currentDigTime > 0 && currentDigTime < float.MaxValue)
            {
                progressIncrement = deltaTime / currentDigTime;
            }

            // 更新进度
            digProgress += progressIncrement;

            // 限制进度在0-1之间
            digProgress = Mathf.Clamp01(digProgress);

            
            // 检查是否完成挖掘
            if (digProgress >= 1f)
            {
                CompleteDigging();
            }
        }
        else
        {
            // 不在挖掘时也更新时间戳，防止deltaTime过大
            lastUpdateTime = Time.time;
        }
    }

    // 开始挖掘 - 保存进度版本
    public void StartDigging()
    {
        if (oreType == OreType.Hard)
        {
            Debug.Log("坚硬矿石无法挖掘！");
            return;
        }

        if (!isBeingDug)
        {
            // 重置时间戳
            lastUpdateTime = Time.time;

            // 如果是第一次挖掘，重置进度
            if (isFirstDig)
            {
                digProgress = 0f;
                isFirstDig = false;
            }
            else
            {
                // 不是第一次挖掘，保留之前的进度
                Debug.Log($"继续挖掘{oreType}，当前进度: {digProgress:P0}");
            }

            // 更新挖矿速度
            UpdateDigTimeFromPlayerSpeed();

            // 开始挖掘
            isBeingDug = true;

            // 计算预计剩余时间
            float remainingTime = GetRemainingDigTime();
            Debug.Log($"开始/继续挖掘{oreType}，进度: {digProgress:P0}，预计剩余时间: {remainingTime:F2}秒");
        }
    }

    // 停止挖掘 - 保存当前进度
    public void StopDigging()
    {
        if (isBeingDug)
        {
            isBeingDug = false;
            Debug.Log($"暂停挖掘{oreType}，保存进度: {digProgress:P0}");
        }
    }

    // 完全重置挖掘进度（当玩家离开或选择重新开始时调用）
    public void ResetDigProgress()
    {
        digProgress = 0f;
        isBeingDug = false;
        isFirstDig = true;
        Debug.Log($"重置{oreType}矿石的挖掘进度");
    }

    // 获取剩余挖掘时间
    public float GetRemainingDigTime()
    {
        if (oreType == OreType.Hard || currentDigTime >= float.MaxValue)
            return float.MaxValue;

        float remainingProgress = 1f - digProgress;
        return remainingProgress * currentDigTime;
    }

    // 获取当前挖掘进度（0-1）
    public float GetDigProgress()
    {
        return digProgress;
    }

    // 是否正在被挖掘
    public bool IsBeingDug()
    {
        return isBeingDug;
    }

    // 更新进度条视觉（如果需要的话）
    void UpdateProgressVisual()
    {
        // 这里可以添加进度条UI
        // 例如：在矿石上方显示进度条
    }

    void CompleteDigging()
    {
        Debug.Log($"完成挖掘{oreType}矿石");
        ApplyOreEffect();
        Destroy(gameObject);
    }

    void ApplyOreEffect()
    {
        if (GameDateController.Instance == null) return;

        switch (oreType)
        {
            case OreType.Normal:
                Debug.Log("普通矿石被挖掉");
                break;

            case OreType.Ruby:
                GameDateController.Instance.blood += 2;
                Debug.Log($"红宝石！生命值+2");
                break;

            case OreType.Blue:
                GameDateController.Instance.attack += 1;
                Debug.Log($"蓝宝石！攻击力+1");
                break;

            case OreType.Purple:
                GameDateController.Instance.blood += 3;
                GameDateController.Instance.attack += 2;
                Debug.Log($"紫水晶！生命值+3，攻击力+2");
                break;

            case OreType.Lava:
                if (GameDateController.Instance.blood > 1)
                {
                    GameDateController.Instance.blood -= 1;
                    Debug.Log($"熔岩！生命值-1");
                }
                break;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateBloodUI();
            UIManager.Instance.UpdateAttackUI();

            if (oreType == OreType.Ruby || oreType == OreType.Blue || oreType == OreType.Purple)
            {
                UIManager.Instance.AddGem(oreType);
            }
        }
    }

}