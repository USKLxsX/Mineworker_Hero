using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockButton : MonoBehaviour
{
    public Button button;

    public void ChangeListener(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(gameObject.GetComponent<Unlock>().LockSkill);
        button.onClick.AddListener(gameObject.GetComponent<ShowInfo>().ShowSkillInfo);
    }
}
