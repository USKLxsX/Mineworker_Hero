using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [Header("跟随设置")]
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("拉近设置")]
    public float zoomDuration = 2f;
    public float zoomSize = 3f;
    public Vector3 mineAreaOffset = new Vector3(0, -2f, -10);

    private Camera cam;
    private float originalSize;
    private bool isZooming = false;
    private float zoomTimer = 0f;

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
    }

    void LateUpdate()
    {
        if (isZooming)
        {
            ZoomUpdate();
        }
        else if (player != null)
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        Vector3 desiredPosition = player.position + offset;
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

        // 移动到矿区位置
        Vector3 targetPosition = player.position + mineAreaOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, progress);

        if (progress >= 1f)
        {
            // 拉近完成，可以在这里触发战斗等逻辑
            Debug.Log("摄像头拉近完成，准备进入战斗");
        }
    }

    // 重置摄像头（战斗结束后调用）
    public void ResetCamera()
    {
        isZooming = false;
        cam.orthographicSize = originalSize;
    }
}