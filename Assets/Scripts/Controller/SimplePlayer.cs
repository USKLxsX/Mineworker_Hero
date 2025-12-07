using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    private SimpleOre targetOre;
    private Vector2 currentDirection = Vector2.down;

    [Header("检测设置")]
    public float rayDistance = 1.5f;
    public float rayStartOffset = 0.3f;

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
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentDirection = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentDirection = Vector2.down;
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
                Debug.Log($"找到矿石: 类型={newTarget.oreType}");
            }
        }

        if (newTarget != targetOre)
        {
            if (targetOre != null)
            {
                targetOre.StopDigging();
            }

            targetOre = newTarget;
        }
    }

    void HandleDigging()
    {
        if (targetOre == null) return;

        if (Input.GetKey(KeyCode.J))
        {
            targetOre.StartDigging();
        }
        else if (Input.GetKeyUp(KeyCode.J))
        {
            targetOre.StopDigging();
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