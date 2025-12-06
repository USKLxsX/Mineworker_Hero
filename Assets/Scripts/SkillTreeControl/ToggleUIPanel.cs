using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUIPanel : MonoBehaviour
{
    public RectTransform panelRectTransform; // UI����RectTransform���
    public float openPosition = 0f; // ��ʱ��λ��
    public float closedPosition = -200f; // ����ʱ��λ��
    public float transitionDuration = 0.5f; // ��������ʱ��
    public Sprite opensprite;
    public Sprite closedsprite;
    public Image buttonimage;
    public Button button;


    public bool isPanelOpen = true; // �������Ƿ��

    private void Start()
    {

    }

    // ���ô˷������л����Ĵ�/����״̬
    public void TogglePanel()
    {
        isPanelOpen = !isPanelOpen; // �л�״̬
        button.interactable=false;
        if (isPanelOpen)
        {
            buttonimage.sprite = opensprite;
        }
        else
        {
            buttonimage.sprite = closedsprite;
        }
        StartCoroutine(MovePanel()); // ��ʼ����Э��
    }

    // ����Э�̣�����ƽ���ƶ����
    private System.Collections.IEnumerator MovePanel()
    {
        float elapsedTime = 0f;
        float start = panelRectTransform.anchoredPosition.x; // ��ʼλ��
        float end = isPanelOpen ? openPosition : closedPosition; // Ŀ��λ��

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            panelRectTransform.anchoredPosition = new Vector2(Mathf.Lerp(start, end, t), panelRectTransform.anchoredPosition.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȷ������λ��׼ȷ
        panelRectTransform.anchoredPosition = new Vector2(end, panelRectTransform.anchoredPosition.y);
    }
}