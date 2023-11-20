using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UltimateSkillController : Singleton<UltimateSkillController>
{
    PlayerManager player => GameSystem.Instance.playerManager;

    private bool usedSkill;

    [SerializeField] Image ultimateGaugeImg;
    [SerializeField] GameObject particleObj;
    [SerializeField] TMP_Text gaugeTxt;

    void Start()
    {
        particleObj.SetActive(false);
        usedSkill = false;
    }

    // Update is called once per frame
    void Update()
    {
        SetUtilGauge();
    }

    private void SetUtilGauge()
    {
        if(usedSkill) return;
        else if (ultimateGaugeImg.fillAmount >= 1)
        {
            ultimateGaugeImg.fillAmount = 1;
            gaugeTxt.text = "100%";
            particleObj.SetActive(true);
            GameSystem.Instance.playerManager.skillController.canUltimateSkill = true;
            return;
        }

        float playerGaugeData = (player.playerData.WizardHp - player.HP)/50;

        gaugeTxt.text = Mathf.Round((playerGaugeData * 100)).ToString() + "%";
        ultimateGaugeImg.fillAmount = playerGaugeData;

    }

    /// <summary>
    /// 궁극기 사용 다음에 호출할 UI 함수 @정명직
    /// </summary>
    public void UltimateSkill()
    {
        usedSkill = true;
        gaugeTxt.gameObject.SetActive(false);
        ultimateGaugeImg.color = Color.grey;
        gaugeTxt.text = "0%";
    }
}