// UltraSimpleOre.cs - 超级简单的矿石
using UnityEngine;
using static Ore;

public class Ore : MonoBehaviour
{
    public enum OreType
    {
        Normal,     // 普通矿石
        Ruby,       // 红宝石
        Blue,       // 蓝宝石  
        Purple,     // 紫水晶
        Hard,       // 坚硬矿石
        Lava        // 熔岩块
    }
    
    [Header("矿石类型")]
    public OreType oreType = OreType.Normal;
  

    void Update()
    {
        if (Input.GetKey(KeyCode.J) && IsPlayerNearby() && oreType != OreType.Hard)
        {
            Destroy(gameObject, 1f);
        }
    }

    void OnDestroy()
    {
        ApplyOreEffect();
    }

    void ApplyOreEffect()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController == null) return;

        // 使用switch判断矿石类型
        switch (oreType)
        {
            case OreType.Normal:
                Debug.Log("普通矿石被挖掉");
                break;

            case OreType.Ruby:
                playerController.health += 2;
                playerController.maxHealth += 2;
                Debug.Log("红宝石！生命+2");
                break;

            case OreType.Blue:
                playerController.attack += 1;
                Debug.Log("蓝宝石！攻击+1");
                break;

            case OreType.Purple:
                playerController.health += 3;
                playerController.attack += 2;
                playerController.maxHealth += 3;
                Debug.Log("紫水晶！生命+3，攻击+2");
                break;

            case OreType.Lava:
                if (playerController.health > 1)
                {
                    playerController.health -= 1;
                    Debug.Log("熔岩！生命-1");
                }
                break;

            case OreType.Hard:
                Debug.Log("坚硬矿石无法挖掘");
                break;
        }

        ;
    }

    bool IsPlayerNearby()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            return distance < 1.2f;
        }
        return false;
    }
}