using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    private SimpleOre targetOre;
    private Vector2 currentDirection = Vector2.down;
    private bool isDigging = false;

    [Header("检测设置")]
    public float rayDistance = 1.5f;
    public float rayStartOffset = 0.3f;

    void Start()
    {
        if (GetComponent<Rigidbody2D>() == null)
        {
            var rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 3f;
            rb.freezeRotation = true;
        }

        gameObject.tag = "Player";
        Debug.Log("玩家初始化完成");
    }

    void Update()
    {
        // 移动
        float h = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody2D>().velocity = new Vector2(h * 5f, GetComponent<Rigidbody2D>().velocity.y);

        // 转向
        HandleDirectionInput();

        // 找矿石
        FindOre();

        // 挖矿控制
        HandleDigging();

        // 可视化射线
        Vector2 rayStart = (Vector2)transform.position + currentDirection * rayStartOffset;
        Debug.DrawRay(rayStart, currentDirection * (rayDistance - rayStartOffset), GetDirectionColor());
    }

    void HandleDirectionInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentDirection = Vector2.left;
            Debug.Log("转向左边");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentDirection = Vector2.right;
            Debug.Log("转向右边");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentDirection = Vector2.down;
            Debug.Log("转向下边");
        }
    }

    void FindOre()
    {
        Vector2 rayStart = (Vector2)transform.position + currentDirection * rayStartOffset;
        RaycastHit2D hit = Physics2D.Raycast(rayStart, currentDirection, rayDistance - rayStartOffset);

        SimpleOre newTarget = null;

        if (hit.collider != null && hit.collider.CompareTag("Ore"))
        {
            newTarget = hit.collider.GetComponent<SimpleOre>();
            if (newTarget != null)
            {
                Debug.Log($"找到矿石: {newTarget.oreType}");
            }
        }

        // 如果目标变化
        if (newTarget != targetOre)
        {
            // 停止挖掘旧目标
            if (targetOre != null)
            {
                targetOre.StopDigging();
                isDigging = false;
            }

            // 设置新目标
            targetOre = newTarget;
        }
    }

    void HandleDigging()
    {
        if (targetOre == null)
        {
            return;
        }

        // 按下J键开始挖掘
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log($"按下J键，开始挖掘 {targetOre.oreType}");
            targetOre.StartDigging();
            isDigging = true;
        }

        // 持续按住J键时，挖掘进度在矿石脚本中更新

        // 松开J键停止挖掘
        if (Input.GetKeyUp(KeyCode.J) && isDigging)
        {
            Debug.Log("松开J键，停止挖掘");
            targetOre.StopDigging();
            isDigging = false;
        }
    }

    Color GetDirectionColor()
    {
        if (currentDirection == Vector2.down) return Color.red;
        if (currentDirection == Vector2.left) return Color.blue;
        if (currentDirection == Vector2.right) return Color.green;
        return Color.white;
    }
}