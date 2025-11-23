using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unlock : MonoBehaviour
{
    [SerializeField] string fileName;
    
    public List<GameObject> parentSkills = new List<GameObject>();
    public GameObject skill;
    
    public List<GameObject> childSkills = new List<GameObject>();

    public void UnlockSkill()
    {
        if (CheckStatus(skill))
        {
            Debug.Log("Skill is already active");
        }
        else
        {
            if (IsAnyParentUnlocked())
            {
                List<SkillActiveInputEntry> entries = FileHandler.LoadFromJSON<SkillActiveInputEntry>(fileName);

                var existingEntry = entries.FirstOrDefault(s => s.skillName == skill.name);
                if (existingEntry == null)
                {
                    entries.Add(new SkillActiveInputEntry(skill.name, true));
                }
                else
                {
                    existingEntry.isActive = true;
                }

                FileHandler.SaveToJSON<SkillActiveInputEntry>(entries, fileName);
                
                Debug.Log("Unlocked Skill: " + skill.name);
            }
            else
            {
                Debug.Log("No valid parent skill is unlocked. Cannot unlock.");
            }
        }
    }

    public void LockSkill()
    {
        List<SkillActiveInputEntry> entries = FileHandler.LoadFromJSON<SkillActiveInputEntry>(fileName);
        
        var skillEntry = entries.FirstOrDefault(s => s.skillName == skill.name);
        
        if (skillEntry != null)
        {
            entries.Remove(skillEntry);
            FileHandler.SaveToJSON<SkillActiveInputEntry>(entries, fileName);
            Debug.Log($"Removed skill: {skill.name}");

            // Give the money back
            
        }
        
        foreach (GameObject child in childSkills)
        {
            if (child != null)
            {
                Unlock childUnlockScript = child.GetComponent<Unlock>();
                if (childUnlockScript != null)
                {
                    childUnlockScript.LockSkill();
                }
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

    private bool IsAnyParentUnlocked()
    {
        if (parentSkills.Count == 0) return false; 

        foreach (GameObject parent in parentSkills)
        {
            if (parent != null)
            {
                if (CheckStatus(parent))
                {
                    return true;
                }
            }
        }
        
        return false;
    }
}
