using System.Collections;
using System.Collections.Generic;
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
        Lava = 4,        // 熔岩块 - 扣生命
    }

    [Header("矿石类型标签")]
    public OreType oreType = OreType.Normal;

    [Header("UI图片显示")]
    public SpriteRenderer oreUIImage;

    [Header("矿石图片资源 - 按顺序对应枚举")]
    public Sprite[] oreSprites;

    [Header("基础挖掘时间（秒）")]
    public float baseDigTime = 1.0f;

    [Header("鼠标点击功能")]
    public bool canClickToMine = true; // 是否可以通过鼠标点击挖矿

    private float digProgress = 0f;
    private bool isBeingDug = false;
    private float currentDigTime;
    public bool isMinedByMouse = false;

    void Start()
    {
        gameObject.tag = "Ore";

        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        SetBaseDigTime();
        UpdateDigTimeFromPlayerSpeed();
        AutoSetupOreImage();
    }

    void SetBaseDigTime()
    {
        baseDigTime = oreType switch
        {
            OreType.Normal => 1.0f,
            OreType.Ruby => 1.0f,
            OreType.Blue => 1.0f,
            OreType.Purple => 1.5f,
            OreType.Hard => float.MaxValue,
            OreType.Lava => 1.0f,
            _ => 1.0f
        };
    }

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

    public void AutoSetupOreImage()
    {
        if (oreUIImage == null || oreSprites == null || oreSprites.Length == 0)
            return;

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
            isMinedByMouse = false;
        }
    }

    public void StopDigging()
    {
        if (isBeingDug)
        {
            isBeingDug = false;
        }
    }

    void OnMouseDown()
    {
        if (!canClickToMine) return;

        // 检查F键挖矿管理器
        if (FMineModeManager.Instance == null)
        {
            Debug.LogWarning("F键挖矿管理器未找到！无法使用鼠标挖矿");
            return;
        }

        // 尝试使用F键挖矿
        if (FMineModeManager.Instance.TryUseFMouseMine())
        {
            // 成功使用F键挖矿
            Debug.Log($"F键鼠标挖矿：挖掉{oreType}矿石");
            isMinedByMouse = true;
            OreDropSpawner.Instance.DropOreIcon(this,true);
            ParticlesController.Instance.PlayParticle(this, 1.0f);
            Destroy(gameObject);
        }
        else
        {
            // F键模式未激活或已使用
            if (!FMineModeManager.Instance.isFMouseMineActive)
            {
                Debug.Log("请先按F键激活鼠标挖矿模式！");
            }
            else if (FMineModeManager.Instance.hasUsedFMouseMine)
            {
                Debug.Log("F键模式已使用，请再次按F键激活");
            }
        }
    }

    void CompleteDigging()
    {
        isMinedByMouse = false;
        OreDropSpawner.Instance.DropOreIcon(this,false);
        ParticlesController.Instance.PlayParticle(this, 1.0f);
        Destroy(gameObject);
    }
}