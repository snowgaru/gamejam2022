using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "SO/Skill/SKillSO")]
public class SkillSO : ScriptableObject
{
    public float damage;
    public Sprite icon;
    public GameObject effect = null;

}
