using UnityEngine;
using static TreeEditor.TreeEditorHelper;

public class SimpleOre : MonoBehaviour
{
    public enum OreType
    {
        Normal,     // 普通矿石 - 无效果
        Ruby,       // 红宝石矿石 - 加生命
        Blue,       // 蓝宝石矿石 - 加攻击
        Purple,     // 紫水晶矿石 - 加生命和攻击
        Hard,       // 坚硬矿石 - 无法挖掘
        Lava        // 熔岩块 - 扣生命
    }
    [Header("矿石类型")]
    public OreType oreType = OreType.Normal;

    private float digProgress = 0f;
    private bool isBeingDug = false;

    void Start()
    {
        gameObject.tag = "Ore";

        // 确保有碰撞器
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    void Update()
    {
        if (isBeingDug)
        {
            digProgress += Time.deltaTime * GameDateController.Instance.minespeed;

            if (digProgress >= 1f)
            {
                CompleteDigging();
            }
        }
    }

    public void StartDigging()
    {
        if (oreType == OreType.Hard)
        {
            Debug.Log("坚硬矿石无法挖掘！");
            return;
        }

        if (!isBeingDug)
        {
            isBeingDug = true;
        }
    }

    public void StopDigging()
    {
        if (isBeingDug)
        {
            isBeingDug = false;
        }
    }

    void CompleteDigging()
    {
        ApplyOreEffect();
        Destroy(gameObject);
    }

    void ApplyOreEffect()
    {
        if (GameDateController.Instance == null) return;

        // 使用枚举switch语句
        switch (oreType)
        {
            case OreType.Ruby:
                GameDateController.Instance.blood += 2;
                break;

            case OreType.Blue:
                GameDateController.Instance.attack += 1;
                break;

            case OreType.Purple:
                GameDateController.Instance.blood += 3;
                GameDateController.Instance.attack += 2;
                break;

            case OreType.Lava:
                if (GameDateController.Instance.blood > 1)
                {
                    GameDateController.Instance.blood -= 1;
                }
                break;

            case OreType.Normal:
                // 普通矿石无效果
                break;
        }

        // 更新UI
        UpdateUI();
    }

    void UpdateUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateBloodUI();
            UIManager.Instance.UpdateAttackUI();

            // 如果是宝石，增加计数
            if (oreType == OreType.Ruby || oreType == OreType.Blue || oreType == OreType.Purple)
            {
                UIManager.Instance.AddGem(oreType);
            }
        }
    }
}