using UnityEngine;
using UnityEngine.UI;

public class TimeProgressBar : MonoBehaviour
{
    [Header("时间进度条")]
    public RectTransform timeBar;
    public float totalTime = 60f;
    public float barMaxWidth = 400f;

    [Header("角色设置")]
    public Transform player; // 需要在Inspector中拖拽玩家对象

    private float currentTime;
    private bool isTimerRunning = true;
    private float originalBarWidth;
    private bool hasTriggered = false;

    void Start()
    {
        currentTime = totalTime;
        if (timeBar != null)
        {
            originalBarWidth = timeBar.sizeDelta.x;
        }
        UpdateProgressBar();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Max(0, currentTime);
            UpdateProgressBar();

            if (currentTime <= 0 && !hasTriggered)
            {
                OnTimeUp();
            }
        }
    }

    void UpdateProgressBar()
    {
        if (timeBar != null)
        {
            float progress = currentTime / totalTime;
            Vector2 size = new Vector2(progress * barMaxWidth, timeBar.sizeDelta.y);
            timeBar.sizeDelta = size;
        }
    }

    void OnTimeUp()
    {
        isTimerRunning = false;
        hasTriggered = true;
        Debug.Log("时间到！");

        // 1. 触发相机移动
        if (CameraController.Instance != null)
        {
            CameraController.Instance.StartMoveToTarget();

            // 2. 角色立即出现在摄像机的目标位置
            if (player != null)
            {
                // 使用公开方法获取摄像机的目标Y位置
                float cameraTargetY = CameraController.Instance.GetTargetYPosition();

                // 角色立即移动到摄像机目标位置的Y坐标
                Vector3 playerPos = player.position;
                playerPos.y = cameraTargetY; // 使用摄像机的目标Y位置
                player.position = playerPos;

                Debug.Log($"角色立即出现在Y={cameraTargetY}（摄像机目标位置）");
            }
        }
    }
    public void ResetTimer()
    {
        currentTime = totalTime;
        isTimerRunning = true;
        hasTriggered = false;
        UpdateProgressBar();
    }
}