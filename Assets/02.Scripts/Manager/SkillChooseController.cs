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
    [SerializeField] GameObject[] wizzardObjects;       //UI에 보여질 마법사 오브젝트 배열
    Button wizzardChooseBtnRight;         //마법사를 선택할 수 있는 버튼들 (왼쪽, 오른쪽)
    [SerializeField] private Button pcWizzardChooseBtnRight;
    [SerializeField] private Button vrWizzardChooseBtnRight;
    Button wizzardChooseBtnLeft;         //마법사를 선택할 수 있는 버튼들 (왼쪽, 오른쪽)
    [SerializeField] Button pcWizzardChooseBtnLeft;
    [SerializeField] Button vrWizzardChooseBtnLeft;
    [SerializeField] private int currentWizzardIndex = 0;                //현재 마법사를 나타내는 인덱스    
    public WizardType MyType;
    [SerializeField] List<SkillData> passiveSkillDatas = new List<SkillData>();
    GameObject[] skillPanel;     //속성별  + 마지막 인덱스는 궁극기.
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

    TMP_Text warningText;         //스킬 미 선택시 경고 글자
    [SerializeField] TMP_Text pcWarningText;         //스킬 미 선택시 경고 글자
    [SerializeField] TMP_Text vrWarningText;         //스킬 미 선택시 경고 글자

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
    /// 캐릭터를 고르는 함수
    /// </summary>
    /// <param name="index"></param>
    private void ChangeIndexBtnClick(int index)
    {
        currentWizzardIndex += index;

        if (wizzardObjects.Length <= currentWizzardIndex) { currentWizzardIndex = 0; }
        else if (currentWizzardIndex < 0) currentWizzardIndex = wizzardObjects.Length - 1;

        if (currentSelectedSkills.selectedSkills != null)
        {
            //스킬 초기화
            for (int i = 0; i < currentSelectedSkills.selectedSkills.Count; i++)
            {
                selectedSkillImgs[i].sprite = notSelectedImg;
            }
            currentSelectedSkills.selectedSkills = null;
        }

        SetWizzardObject();
    }

    /// <summary>
    /// 캐릭터 오브젝트 함수 (UI Image에 나타나는 오브젝트)
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
    /// 일반 스킬 추가 버튼 클릭
    /// </summary>
    /// <param name="skillData"></param>
    public void SelectSkillBtnClick(SkillData skillData)
    {
        if (currentSelectedSkills.selectedSkills == null)       //예외처리 초기 세팅
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

            if (currentSelectedSkills.selectedSkills.Count >= maxSkillCount)        //2개 이상 스킬 선택 시
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
    /// 궁극기 스킬 추가 버튼 클릭
    /// </summary>
    /// <param name="skillData"></param>
    public void SelectUtilSkillBtnClick(SkillData skillData)
    {
        currentSelectedSkills.utilmateSkill = skillData;

        SetSkillUI(currentSelectedSkills);
    }


    /// <summary>
    /// 선택된 스킬의 UI Set
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
