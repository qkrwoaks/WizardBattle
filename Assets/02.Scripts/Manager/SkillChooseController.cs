using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedSkills
{
    public SkillData passiveSkill;
    public List<SkillData> selectedSkills = new List<SkillData>();
    public SkillData utilmateSkill;
}

public enum WizardType
{
    Fire = 0,
    Grass = 1,
    Water = 2,
}

public class SkillChooseController : MonoBehaviour
{
    private PhotonView PV;

    [Header("Wizzard")]
    [SerializeField] GameObject[] wizzardObjects;       //UI�� ������ ������ ������Ʈ �迭
    Button wizzardChooseBtnRight;         //�����縦 ������ �� �ִ� ��ư�� (����, ������)
    [SerializeField] private Button pcWizzardChooseBtnRight;
    [SerializeField] private Button vrWizzardChooseBtnRight;
    Button wizzardChooseBtnLeft;         //�����縦 ������ �� �ִ� ��ư�� (����, ������)
    [SerializeField] Button pcWizzardChooseBtnLeft;
    [SerializeField] Button vrWizzardChooseBtnLeft;
    [SerializeField] private int currentWizzardIndex = 0;                //���� �����縦 ��Ÿ���� �ε���    
    public WizardType MyType;
    [SerializeField] List<SkillData> passiveSkillDatas = new List<SkillData>();
    GameObject[] skillPanel;     //�Ӽ���  + ������ �ε����� �ñر�.
    [SerializeField] GameObject[] pcSkillPanel;
    [SerializeField] GameObject[] vrSkillPanel;

    public SelectedSkills currentSelectedSkills;

    [Header("Selected UI")]
    Image passiveImg;
    [SerializeField] Image pcPassiveImg;
    [SerializeField] Image vrPassiveImg;
    Image[] selectedSkillImgs;
    [SerializeField] Image[] pcSelectedSkillImgs;
    [SerializeField] Image[] vrSelectedSkillImgs;
    Image uitmateImg;
    [SerializeField] Image pcUitmateImg;
    [SerializeField] Image vrUitmateImg;
    [SerializeField] Sprite notSelectedImg;
    private int maxSkillCount = 2;

    TMP_Text warningText;         //��ų �� ���ý� ��� ����
    [SerializeField] TMP_Text pcWarningText;         //��ų �� ���ý� ��� ����
    [SerializeField] TMP_Text vrWarningText;         //��ų �� ���ý� ��� ����

    private bool isReady = false;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        InitVR(PCVRSetting.isVR);
    }

    void Start()
    {
        currentSelectedSkills = new SelectedSkills();

        SetUI();
        SetWizzardObject();
    }

    private void InitVR(bool isVR)
    {
        if (isVR)
        {
            wizzardChooseBtnRight = vrWizzardChooseBtnRight;
            wizzardChooseBtnLeft = vrWizzardChooseBtnLeft;
            skillPanel = vrSkillPanel;
            passiveImg = vrPassiveImg;
            selectedSkillImgs = vrSelectedSkillImgs;
            uitmateImg = vrUitmateImg;
            warningText = vrWarningText;
        }
        else
        {
            wizzardChooseBtnRight = pcWizzardChooseBtnRight;
            wizzardChooseBtnLeft = pcWizzardChooseBtnLeft;
            skillPanel = pcSkillPanel;
            passiveImg = pcPassiveImg;
            selectedSkillImgs = pcSelectedSkillImgs;
            uitmateImg = pcUitmateImg;
            warningText = pcWarningText;
        }
    }

    /// <summary>
    /// Button Event Set
    /// </summary>
    private void SetUI()
    {
        wizzardChooseBtnRight.onClick.AddListener(() => ChangeIndexBtnClick(1));
        wizzardChooseBtnLeft.onClick.AddListener(() => ChangeIndexBtnClick(-1));

        warningText.gameObject.SetActive(false);

        for (int i = 0; i < skillPanel.Length; i++)
        {
            skillPanel[i].SetActive(false);

            if (i == currentWizzardIndex)
                skillPanel[i].SetActive(true);
        }
    }

    /// <summary>
    /// ĳ���͸� ���� �Լ�
    /// </summary>
    /// <param name="index"></param>
    private void ChangeIndexBtnClick(int index)
    {
        currentWizzardIndex += index;

        if (wizzardObjects.Length <= currentWizzardIndex) { currentWizzardIndex = 0; }
        else if (currentWizzardIndex < 0) currentWizzardIndex = wizzardObjects.Length - 1;

        if (currentSelectedSkills.selectedSkills != null)
        {
            //��ų �ʱ�ȭ
            for (int i = 0; i < currentSelectedSkills.selectedSkills.Count; i++)
            {
                selectedSkillImgs[i].sprite = notSelectedImg;
            }
            currentSelectedSkills.selectedSkills = null;
        }

        SetWizzardObject();
    }

    /// <summary>
    /// ĳ���� ������Ʈ �Լ� (UI Image�� ��Ÿ���� ������Ʈ)
    /// </summary>
    private void SetWizzardObject()
    {
        MyType = (WizardType)currentWizzardIndex;

        for (int i = 0; i < wizzardObjects.Length; i++)
        {
            wizzardObjects[i].SetActive(false);
            skillPanel[i].SetActive(false);

            if (i == currentWizzardIndex)
            {
                wizzardObjects[i].SetActive(true);
                skillPanel[i].SetActive(true);
            }
        }

        currentSelectedSkills.passiveSkill = passiveSkillDatas[currentWizzardIndex];

        SetSkillUI(currentSelectedSkills);
    }

    /// <summary>
    /// �Ϲ� ��ų �߰� ��ư Ŭ��
    /// </summary>
    /// <param name="skillData"></param>
    public void SelectSkillBtnClick(SkillData skillData)
    {
        if (currentSelectedSkills.selectedSkills == null)       //����ó�� �ʱ� ����
        {
            currentSelectedSkills.selectedSkills = new List<SkillData>();
            currentSelectedSkills.selectedSkills.Add(skillData);
        }
        else
        {
            for (int i = 0; i < currentSelectedSkills.selectedSkills.Count; i++)
            {
                if (currentSelectedSkills.selectedSkills[i].SkillName == skillData.SkillName)
                {
                    return;
                }
            }

            if (currentSelectedSkills.selectedSkills.Count >= maxSkillCount)        //2�� �̻� ��ų ���� ��
            {
                currentSelectedSkills.selectedSkills[0] = currentSelectedSkills.selectedSkills[1];
                currentSelectedSkills.selectedSkills[1] = skillData;
            }
            else
            {
                currentSelectedSkills.selectedSkills.Add(skillData);
            }
        }
        SetSkillUI(currentSelectedSkills);
    }

    /// <summary>
    /// �ñر� ��ų �߰� ��ư Ŭ��
    /// </summary>
    /// <param name="skillData"></param>
    public void SelectUtilSkillBtnClick(SkillData skillData)
    {
        currentSelectedSkills.utilmateSkill = skillData;

        SetSkillUI(currentSelectedSkills);
    }


    /// <summary>
    /// ���õ� ��ų�� UI Set
    /// </summary>
    /// <param name="curruntSkills"></param>
    private void SetSkillUI(SelectedSkills curruntSkills)
    {
        passiveImg.sprite = curruntSkills.passiveSkill.SkillSprite;

        if (curruntSkills.selectedSkills != null)
        {
            for (int i = 0; i < currentSelectedSkills.selectedSkills.Count; i++)
            {
                selectedSkillImgs[i].sprite = curruntSkills.selectedSkills[i].SkillSprite;
            }
        }

        if (curruntSkills.utilmateSkill != null)
        {
            uitmateImg.sprite = curruntSkills.utilmateSkill.SkillSprite;
        }
    }

    public void StartBtnClick()
    {
        if (currentSelectedSkills.selectedSkills.Count < 2)
        {
            StartCoroutine(ShowWarrning());
        }
        else if (currentSelectedSkills.utilmateSkill == null)
        {
            wizzardChooseBtnLeft.interactable = false;
            wizzardChooseBtnRight.interactable = false;

            skillPanel[currentWizzardIndex].SetActive(false);
            skillPanel[skillPanel.Length - 1].SetActive(true);
        }
        else if (currentSelectedSkills.utilmateSkill != null)
        {
            if (isReady == false)
            {
                PV.RPC("ReadyRPC", RpcTarget.All);
                GameSystem.Instance.Ready();
            }
            else
            {
                PV.RPC("GameStartRPC", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void ReadyRPC()
    {
        isReady = true;
    }

    [PunRPC]
    private void GameStartRPC()
    {
        GameSystem.Instance.GameStart();
    }

    IEnumerator ShowWarrning()
    {
        warningText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        warningText.gameObject.SetActive(false);
    }
}
