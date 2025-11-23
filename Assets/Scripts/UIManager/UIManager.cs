using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SimpleOre;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI组件 - 拖拽到这里")]
    public TextMeshProUGUI bloodText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI rubyCountText;
    public TextMeshProUGUI blueCountText;
    public TextMeshProUGUI purpleCountText;

    private int rubyCount = 0;
    private int blueCount = 0;
    private int purpleCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 检查组件引用
        CheckUIReferences();
        UpdateAllUI();
    }

    void CheckUIReferences()
    {
        if (bloodText == null) Debug.LogError("bloodText 未赋值！");
        if (attackText == null) Debug.LogError("attackText 未赋值！");
        if (rubyCountText == null) Debug.LogError("rubyCountText 未赋值！");
        if (blueCountText == null) Debug.LogError("blueCountText 未赋值！");
        if (purpleCountText == null) Debug.LogError("purpleCountText 未赋值！");
    }

    public void UpdateAllUI()
    {
        UpdateBloodUI();
        UpdateAttackUI();
        UpdateGemCountUI();
    }

    public void UpdateBloodUI()
    {
        if (bloodText != null && GameDateController.Instance != null)
        {
            bloodText.text = "生命值：" + GameDateController.Instance.blood.ToString();
            Debug.Log("更新生命值UI: " + GameDateController.Instance.blood);
        }
    }

    public void UpdateAttackUI()
    {
        if (attackText != null && GameDateController.Instance != null)
        {
            attackText.text = "攻击力：" + GameDateController.Instance.attack.ToString();
            Debug.Log("更新攻击力UI: " + GameDateController.Instance.attack);
        }
    }

    public void UpdateGemCountUI()
    {
        if (rubyCountText != null)
        {
            rubyCountText.text =  rubyCount.ToString();
            Debug.Log("更新红宝石数量: " + rubyCount);
        }
        if (blueCountText != null)
        {
            blueCountText.text =  blueCount.ToString();
            Debug.Log("更新蓝宝石数量: " + blueCount);
        }
        if (purpleCountText != null)
        {
            purpleCountText.text = purpleCount.ToString();
            Debug.Log("更新紫水晶数量: " + purpleCount);
        }
    }

    public void AddGem(OreType oreType)
    {
        switch (oreType)
        {
            case OreType.Ruby:
                rubyCount++;
                break;
            case OreType.Blue:
                blueCount++;
                break;
            case OreType.Purple:
                purpleCount++;
                break;
        }
        UpdateGemCountUI();
    }
}