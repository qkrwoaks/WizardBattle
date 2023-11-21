using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTimeUI : MonoBehaviour
{
    [SerializeField] private Image[] image_fill;
    [SerializeField] private Image[] skillIconImg;
    [SerializeField] private Image playerImg;
    [SerializeField] private Image playerImgT;      // 테두리 이미지

    [SerializeField] private PhotonView PV;

    private float[] time_start = { 0, 0 };
    public bool[] isEnded = { true, true };

    private SkillController skillController;

    private void Awake()
    {
        if (PCVRSetting.isVR && PV.IsMine)
        {
            GameSystem.Instance.skillCoolTimeUI = this;
        }
    }

    void Update()
    {
        if (isEnded[0] && isEnded[1])
            return;
        Check_CoolTime();
    }

    public void Init_UI()
    {
        skillController = GameSystem.Instance.player.GetComponent<SkillController>();

        for (int i = 0; i < 2; i++)
        {
            image_fill[i].type = Image.Type.Filled;
            image_fill[i].fillMethod = Image.FillMethod.Radial360;
            image_fill[i].fillOrigin = (int)Image.Origin360.Top;
            image_fill[i].fillClockwise = false;
            skillIconImg[i].sprite = skillController.skill[i].SkillSprite;
        }

        PlayerManager playerManager = GameSystem.Instance.playerManager;
        playerImg.sprite = skillController.passiveSkill.SkillSprite;
        switch (playerManager.playerData.WizardType)
        {
            case WizardType.Fire:
                playerImgT.color = Color.red;
                break;
            case WizardType.Water:
                playerImgT.color = Color.blue;
                break;
            case WizardType.Grass:
                playerImgT.color = Color.green;
                break;
            default:
                break;
        }
    }

    private void Check_CoolTime()
    {
        skillController.skillCurrentTime[0] = Time.time - time_start[0];

        if (skillController.skillCurrentTime[0] < skillController.skillCoolTime[0])
        {
            Set_FillAmount(skillController.skillCoolTime[0] - skillController.skillCurrentTime[0], 0);
        }
        else if (!isEnded[0])
        {
            End_CoolTime(0);
        }

        skillController.skillCurrentTime[1] = Time.time - time_start[1];

        if (skillController.skillCurrentTime[1] < skillController.skillCoolTime[1])
        {
            Set_FillAmount(skillController.skillCoolTime[1] - skillController.skillCurrentTime[1], 1);
        }
        else if (!isEnded[1])
        {
            End_CoolTime(1);
        }
    }

    private void End_CoolTime(int index)
    {
        Set_FillAmount(0, index);
        isEnded[index] = true;
    }

    public void Trigger_Skill(int index)
    {
        if (!isEnded[index])
        {
            return;
        }
        Reset_CoolTime(index);
    }

    private void Reset_CoolTime(int index)
    {
        skillController.skillCurrentTime[index] = skillController.skillCoolTime[index];
        time_start[index] = Time.time;
        Set_FillAmount(skillController.skillCoolTime[index], index);
        isEnded[index] = false;
    }
    private void Set_FillAmount(float _value, int index)
    {
        image_fill[index].fillAmount = _value / skillController.skillCoolTime[index];
        string txt = _value.ToString("0.0");
    }
}