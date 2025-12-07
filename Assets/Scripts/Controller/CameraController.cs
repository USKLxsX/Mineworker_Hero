using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [Header("跟随设置")]
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("限制设置")]
    public float minX = 0f;  // 最小X位置（固定）
    public float maxX = 0f;  // 最大X位置（固定，与minX相同）

    [Header("拉近设置")]
    public float zoomDuration = 2f;
    public float zoomSize = 3f;
    public Vector3 mineAreaOffset = new Vector3(0, -2f, -10);

    private Camera cam;
    private float originalSize;
    private bool isZooming = false;
    private float zoomTimer = 0f;
    private float fixedXPosition; // 固定的X位置

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

        cam = GetComponent<Camera>();
        originalSize = cam.orthographicSize;

        // 记录初始X位置
        fixedXPosition = transform.position.x;
    }

    void LateUpdate()
    {
        if (isZooming)
        {
            ZoomUpdate();
        }
        else if (player != null)
        {
            FollowPlayerVerticalOnly();
        }
    }

    void FollowPlayerVerticalOnly()
    {
        // 只跟随玩家的Y轴位置，X轴保持固定
        float targetY = player.position.y + offset.y;

        // 创建目标位置：固定X，跟随Y
        Vector3 desiredPosition = new Vector3(fixedXPosition, targetY, offset.z);

        // 平滑移动
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public void ZoomToMineArea()
    {
        if (player != null && !isZooming)
        {
            isZooming = true;
            zoomTimer = 0f;
        }
    }

    void ZoomUpdate()
    {
        zoomTimer += Time.deltaTime;
        float progress = Mathf.Clamp01(zoomTimer / zoomDuration);

        // 平滑拉近
        cam.orthographicSize = Mathf.Lerp(originalSize, zoomSize, progress);

        // 移动到矿区位置（同样只改变Y轴）
        float targetY = player.position.y + mineAreaOffset.y;
        Vector3 targetPosition = new Vector3(fixedXPosition, targetY, mineAreaOffset.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, progress);

        if (progress >= 1f)
        {
            Debug.Log("摄像头拉近完成，准备进入战斗");
        }
    }

    // 重置摄像头（战斗结束后调用）
    public void ResetCamera()
    {
        isZooming = false;
        cam.orthographicSize = originalSize;
    }

    // 设置固定的X位置（可选）
    public void SetFixedXPosition(float xPosition)
    {
        fixedXPosition = xPosition;
        Vector3 newPosition = new Vector3(fixedXPosition, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }
}