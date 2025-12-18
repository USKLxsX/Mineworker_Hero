// MouseCursorChanger.cs
using UnityEngine;

public class MouseCursorChanger : MonoBehaviour
{
    private static MouseCursorChanger instance;
    public static MouseCursorChanger Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<MouseCursorChanger>();
                if (instance == null)
                {
                    Debug.Log("No MouseCursorChanger in scene!");
                }
            }
            return instance;
        }
    }
    [Header("鼠标指针纹理")]
    public Texture2D cursorTexture;

    [Header("热点偏移（像素）")]
    public Vector2 hotspot = Vector2.zero;

    [Header("是否自动恢复默认")]
    public bool restoreOnExit = true;

    void Start()
    {

    }

    /// 外部调用：动态换图
    public void ChangeCursor(Texture2D tex, Vector2? hot = null)
    {
        if (tex == null) return;
        Cursor.SetCursor(tex, hot ?? Vector2.zero, CursorMode.Auto);
    }

    /// 外部调用：恢复系统默认
    public void RestoreDefault()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    void OnDestroy()
    {
        if (restoreOnExit)
            RestoreDefault();
    }

    //// 编辑器下调试：点按钮立即看效果
    //[ContextMenu("Test Change")]
    //void TestChange() => ChangeCursor(cursorTexture, hotspot);

    //[ContextMenu("Test Restore")]
    //void TestRestore() => RestoreDefault();
}