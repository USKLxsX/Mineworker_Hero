using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unlock : MonoBehaviour
{
    [SerializeField] string fileName;
    
    public GameObject parentSkill;
    public GameObject skill;

    public void UnlockSkill()
    {
        if (CheckStatus(skill))
        {
            Debug.Log("Skill is already active");
        }
        else
        {
            if (CheckStatus(parentSkill))
            {
                List<SkillActiveInputEntry> entries = FileHandler.LoadFromJSON<SkillActiveInputEntry>(fileName);
                
                entries.Add(new SkillActiveInputEntry(skill.name, true));
                
                FileHandler.SaveToJSON<SkillActiveInputEntry>(entries, fileName);
            
                Debug.Log("Unlocked Skill");
            }
            else
            {
                Debug.Log("Parent Skill not unlocked");
            }
        }
    }

    public void TestRead()
    {
        List<SkillActiveInputEntry> skillList = FileHandler.LoadFromJSON<SkillActiveInputEntry>(fileName);
        
        var skillLoaded = skillList.FirstOrDefault(s => s.skillName == skill.name);

        if (skillLoaded != null)
        {
            bool isActive = skillLoaded.isActive;
            Debug.Log($"{skill.name} is active: {isActive}");
        }
        else
        {
            Debug.Log($"Skill '{skill.name}' not found");
        }
        
    }

    public Boolean CheckStatus(GameObject skill)
    {
        if (skill.name == "ParentNull")
        {
            return true;
        }
        
        List<SkillActiveInputEntry> skillList = FileHandler.LoadFromJSON<SkillActiveInputEntry>(fileName);
        
        var skillLoaded = skillList.FirstOrDefault(s => s.skillName == skill.name);

        if (skillLoaded != null)
        {
            bool isActive = skillLoaded.isActive;
            return isActive;
        }
        else
        {
            return false;
        }
    }

    private void Start()
    {
        
    }
}
