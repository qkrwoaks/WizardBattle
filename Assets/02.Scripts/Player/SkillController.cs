using UnityEngine;
using Photon.Pun;

public class SkillController : MonoBehaviour
{
    private PlayerManager playerManager;           // 추후 수정

    public SkillData[] skill;                      // 일반 스킬
    public SkillData ultimateSkill;                // 궁극기
    public SkillData passiveSkill;                 // 패시브

    public float[] skillCoolTime;                  // 스킬 쿨타임
    public float[] skillCurrentTime;               // 스킬 타이머 
    public bool canUltimateSkill;                  // 궁극기가 사용가능한지

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        skillCurrentTime = new float[skill.Length];
        skillCoolTime = new float[skill.Length];
    }

    /// <summary>
    /// 선택한 스킬들 쿨타임 데이터값을 초기화하는 함수
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