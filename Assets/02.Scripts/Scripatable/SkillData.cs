using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObject/SkillData", order = 2)]
public class SkillData : ScriptableObject
{
    [SerializeField] private string skillName;    // �̸�
    public string SkillName { get { return skillName; } }

    [SerializeField] private float skillATK;      // ���ݷ�
    public float SkillATK { get { return skillATK; } }

    [SerializeField] private float skillSpeed;    // ����ü �ӵ�
    public float SkillSpeed { get { return skillSpeed; } }

    [SerializeField] private float skillDuration;    // ����ü �ӵ�
    public float SkillDuration { get { return skillDuration; } }

    [SerializeField] private float skillCoolTime; // ��Ÿ��
    public float SkillCoolTime { get { return skillCoolTime; } }

    [SerializeField] private AudioClip[] skillSFX;  // ��ų ȿ����
    public AudioClip[] SkillSFX { get { return skillSFX; } }

    [TextArea] [SerializeField] private string description;  // ��ų ����
    public string Description { get { return description; } }

    [SerializeField] private Sprite skillSprite;  // ��ų �̹��� ������
    public Sprite SkillSprite { get { return skillSprite; } }
}
