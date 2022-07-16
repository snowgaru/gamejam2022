using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SkillSO", menuName = "SkillSO", order = 0)]
//[CreateAssetMenu(menuName = "SO/Skill/SKillSO")]
public class SkillSO : ScriptableObject
{
    public float damage;
    public Sprite icon;
    public GameObject effect = null;
    public bool isGuard;
}
