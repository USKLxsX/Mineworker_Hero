using UnityEngine;
using UnityEngine.UI;

public class TimeProgressBar : MonoBehaviour
{
    [Header("时间进度条")]
    public RectTransform timeBar;
    public float totalTime = 60f;
    public float barMaxWidth = 400f; // 进度条最大宽度

    private float currentTime;
    private bool isTimerRunning = true;
    private float originalBarWidth;

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

            if (currentTime <= 0)
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
        Debug.Log("时间到！");
        CameraController.Instance?.ZoomToMineArea();
    }

    // 重置计时器（可选）
    public void ResetTimer()
    {
        currentTime = totalTime;
        isTimerRunning = true;
        UpdateProgressBar();
    }
}