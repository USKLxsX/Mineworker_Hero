using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowInfo : MonoBehaviour
{
    [SerializeField] string fileName;
    
    public Image logo;
    public Sprite icon;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text statusText;

    public GameObject skill;
    
    public string title;
    public string description;

    private Boolean _status;

    public void ShowSkillInfo()
    {
        titleText.text = title;
        descriptionText.text = description;
        logo.sprite = icon;

        List<SkillActiveInputEntry> entries = FileHandler.LoadFromJSON<SkillActiveInputEntry>(fileName);

        var existingEntry = entries.FirstOrDefault(s => s.skillName == skill.name);

        if (existingEntry == null)
        {
            statusText.text = "Disabled";
            return;
        }

        _status = existingEntry.isActive;

        if (_status)
        {
            statusText.text = "Enabled";
        }
        else
        {
            statusText.text = "Disabled";
        }
    }
}
