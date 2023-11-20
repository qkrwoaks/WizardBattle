using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObject/SkillData", order = 2)]
public class SkillData : ScriptableObject
{
    [SerializeField] private string skillName;    // 이름
    public string SkillName { get { return skillName; } }

    [SerializeField] private float skillATK;      // 공격력
    public float SkillATK { get { return skillATK; } }

    [SerializeField] private float skillSpeed;    // 투사체 속도
    public float SkillSpeed { get { return skillSpeed; } }

    [SerializeField] private float skillDuration;    // 투사체 속도
    public float SkillDuration { get { return skillDuration; } }

    [SerializeField] private float skillCoolTime; // 쿨타임
    public float SkillCoolTime { get { return skillCoolTime; } }

    [SerializeField] private AudioClip[] skillSFX;  // 스킬 효과음
    public AudioClip[] SkillSFX { get { return skillSFX; } }

    [TextArea] [SerializeField] private string description;  // 스킬 설명
    public string Description { get { return description; } }

    [SerializeField] private Sprite skillSprite;  // 스킬 이미지 아이콘
    public Sprite SkillSprite { get { return skillSprite; } }
}
