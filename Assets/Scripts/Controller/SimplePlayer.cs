using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    public SimpleOre targetOre;
    private Vector2 currentDirection = Vector2.down;
    private bool isDigging = false;

    [Header("检测设置")]
    public float rayDistance = 1.5f;
    public float detectionWidth = 0.3f; // 检测宽度

    void Start()
    {
        if (GetComponent<Rigidbody2D>() == null)
        {
            var rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 3f;
            rb.freezeRotation = true;
        }

        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
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

        // 使用BoxCast检测矿石（更宽的范围）
        FindOreWithBoxCast();

        // 挖矿控制
        HandleDigging();

        // 可视化
        VisualizeDetection();
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

    void FindOreWithBoxCast()
    {
        // 使用BoxCast检测，范围更宽
        Vector2 size = new Vector2(detectionWidth, detectionWidth);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, size, 0f, currentDirection, rayDistance);

        SimpleOre newTarget = null;
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                Debug.Log($"检测到物体: {hit.collider.name}, 标签: {hit.collider.tag}");

                if (hit.collider.CompareTag("Ore"))
                {
                    newTarget = hit.collider.GetComponent<SimpleOre>();
                    if (newTarget != null)
                    {
                        Debug.Log($"找到矿石: {newTarget.oreType}");
                    }
                }
            }
        }

        if (newTarget != targetOre)
        {
            if (targetOre != null)
            {
                targetOre.StopDigging();
                isDigging = false;
            }

            targetOre = newTarget;
        }
    }

    void HandleDigging()
    {
        if (targetOre == null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.J) && !isDigging)
        {
            Debug.Log($"开始挖掘 {targetOre.oreType}");
            targetOre.StartDigging();
            isDigging = true;
        }

        if (Input.GetKeyUp(KeyCode.J) && isDigging)
        {
            Debug.Log("停止挖掘");
            targetOre.StopDigging();
            isDigging = false;
        }
    }

    void VisualizeDetection()
    {
        // 可视化BoxCast范围
        Vector2 size = new Vector2(detectionWidth, detectionWidth);
        Vector2 endPos = (Vector2)transform.position + currentDirection * rayDistance;

        // 绘制BoxCast范围
        Debug.DrawLine(transform.position, endPos, GetDirectionColor());

        // 绘制检测框
        Vector2 perpendicular = new Vector2(-currentDirection.y, currentDirection.x);
        Vector2 topLeft = (Vector2)transform.position + perpendicular * detectionWidth * 0.5f;
        Vector2 topRight = (Vector2)transform.position - perpendicular * detectionWidth * 0.5f;
        Vector2 bottomLeft = topLeft + currentDirection * rayDistance;
        Vector2 bottomRight = topRight + currentDirection * rayDistance;

        Debug.DrawLine(topLeft, topRight, Color.yellow);
        Debug.DrawLine(topLeft, bottomLeft, Color.yellow);
        Debug.DrawLine(topRight, bottomRight, Color.yellow);
        Debug.DrawLine(bottomLeft, bottomRight, Color.yellow);
    }

    Color GetDirectionColor()
    {
        if (currentDirection == Vector2.down) return Color.red;
        if (currentDirection == Vector2.left) return Color.blue;
        if (currentDirection == Vector2.right) return Color.green;
        return Color.white;
    }
}