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
    public OreType oreType;
    private float digProgress = 0f;
    private bool isBeingDug = false;

    void Start()
    {
        gameObject.tag = "Ore";

        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    void Update()
    {
        if (isBeingDug)
        {
            digProgress += Time.deltaTime;

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
        if (GameDateController.Instance == null)
        {
            Debug.LogError("GameDateController未找到！");
            return;
        }

        // 根据矿石类型应用效果
        switch (oreType)
        {
            case OreType.Normal:
                Debug.Log("普通矿石被挖掉");
                break;

            case OreType.Ruby:
                GameDateController.Instance.blood += 2;
                Debug.Log($"红宝石！生命值+2，当前生命: {GameDateController.Instance.blood}");
                break;

            case OreType.Blue:
                GameDateController.Instance.attack += 1;
                Debug.Log($"蓝宝石！攻击力+1，当前攻击: {GameDateController.Instance.attack}");
                break;

            case OreType.Purple:
                GameDateController.Instance.blood += 3;
                GameDateController.Instance.attack += 2;
                Debug.Log($"紫水晶！生命值+3，攻击力+2，当前生命: {GameDateController.Instance.blood}，攻击: {GameDateController.Instance.attack}");
                break;

            case OreType.Lava:
                if (GameDateController.Instance.blood > 1)
                {
                    GameDateController.Instance.blood -= 1;
                    Debug.Log($"熔岩！生命值-1，当前生命: {GameDateController.Instance.blood}");
                }
                else
                {
                    Debug.Log("生命值过低，挖熔岩不会扣血");
                }
                break;

            case OreType.Hard:
                // 不会执行到这里
                break;
        }

        // 更新UI显示
        UpdateUI();
    }

    void UpdateUI()
    {
        // 更新生命值和攻击力UI
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
        else
        {
            Debug.LogWarning("UIManager未找到！");
        }
    }
}