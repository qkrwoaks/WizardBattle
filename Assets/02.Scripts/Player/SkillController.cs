using UnityEngine;
using Photon.Pun;

public class SkillController : MonoBehaviour
{
    private PlayerManager playerManager;           // ���� ����

    public SkillData[] skill;                      // �Ϲ� ��ų
    public SkillData ultimateSkill;                // �ñر�
    public SkillData passiveSkill;                 // �нú�

    public float[] skillCoolTime;                  // ��ų ��Ÿ��
    public float[] skillCurrentTime;               // ��ų Ÿ�̸� 
    public bool canUltimateSkill;                  // �ñرⰡ ��밡������

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        skillCurrentTime = new float[skill.Length];
        skillCoolTime = new float[skill.Length];
    }

    /// <summary>
    /// ������ ��ų�� ��Ÿ�� �����Ͱ��� �ʱ�ȭ�ϴ� �Լ�
    /// </summary>
    public void InitSkillCoolTime()
    {
        for (int i = 0; i < skill.Length; i++)
        {
            skillCoolTime[i] = skill[i].SkillCoolTime;
            skillCurrentTime[i] = skillCoolTime[i];
        }
    }

    private void Update()
    {
        if (playerManager.PV.IsMine)
        {
            InputSkill();
        }
    }

    private void InputSkill()
    {
        if (Input.GetKeyDown(KeyCode.E) && GameSystem.Instance.skillCoolTimeUI.isEnded[0] == true)
        {
            UseSkill(skill[0].SkillName);
            GameSystem.Instance.skillCoolTimeUI.Trigger_Skill(0);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && GameSystem.Instance.skillCoolTimeUI.isEnded[1] == true)
        {
            UseSkill(skill[1].SkillName);
            GameSystem.Instance.skillCoolTimeUI.Trigger_Skill(1);
        }

        if (Input.GetKeyDown(KeyCode.Q) && canUltimateSkill)
        {
            UseSkill(ultimateSkill.SkillName);
            UltimateSkillController.Instance.UltimateSkill();
            canUltimateSkill = false;
        }
    }

    private void UseSkill(string skillName)
    {
        PhotonNetwork.Instantiate("Skill\\" + skillName, playerManager.weaponTr.position, Quaternion.identity);
    }

    public void FirstSkill()
    {
        if (GameSystem.Instance.skillCoolTimeUI.isEnded[0] == true)
        {
            UseSkill(skill[0].SkillName);
            GameSystem.Instance.skillCoolTimeUI.Trigger_Skill(0);
        }
    }

    public void SecondSkill()
    {
        if (GameSystem.Instance.skillCoolTimeUI.isEnded[1] == true)
        {
            UseSkill(skill[1].SkillName);
            GameSystem.Instance.skillCoolTimeUI.Trigger_Skill(1);
        }
    }

    public void UltimateSkill()
    {
        if (canUltimateSkill)
        {
            UseSkill(ultimateSkill.SkillName);
            UltimateSkillController.Instance.UltimateSkill();
            canUltimateSkill = false;
        }
    }
}