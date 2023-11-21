using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : Singleton<GameSystem>
{
    [Header("Player")]
    public GameObject player;
    public Transform plyerWeaponTr;
    public PlayerManager playerManager;

    [Space(10f)]
    [Header("Enemy")]
    public GameObject enemy;
    public Transform enemyWeaponTr;
    public PlayerManager enemyManager;

    [Space(10f)]

    [Header("UI")]
    [SerializeField]
    private GameObject battleUICanvas;
    public SkillCoolTimeUI skillCoolTimeUI;
    [SerializeField]
    public OutcomeUIController outcomeUIController;
    [SerializeField]
    private GameObject wizzardChoosePlace;
    private GameObject skillCanvas;
    [SerializeField] private GameObject pcSkillCanvas;
    [SerializeField] private GameObject vrSkillCanvas;
    public SkillChooseController skillChooseController;
    [SerializeField]
    private Image bloodScreen;
    private GameObject waitingPanel;
    [SerializeField] private GameObject pcWaitingPanel;
    [SerializeField] private GameObject vrWaitingPanel;

    [Space(10f)]
    [SerializeField]
    private Transform[] spawnPoint;
    private GameObject skillSelectCamera;
    [SerializeField] private GameObject pcSkillSelectCamera;
    [SerializeField] private GameObject vrSkillSelectCamera;

    public GameObject[] playerModels;
    [SerializeField] private GameObject[] pcPlayerModels;
    [SerializeField] private GameObject[] vrPlayerModels;

    public override void Awake()
    {
        base.Awake();
        InitVR(PCVRSetting.isVR);
    }

    private void Start()
    {
        wizzardChoosePlace.SetActive(true);
        skillCanvas.SetActive(true);
    }

    private void InitVR(bool isVR)
    {
        if (isVR)
        {
            skillCanvas = vrSkillCanvas;
            waitingPanel = vrWaitingPanel;
            skillSelectCamera = vrSkillSelectCamera;
            playerModels = vrPlayerModels;
        }
        else
        {
            skillCanvas = pcSkillCanvas;
            waitingPanel = pcWaitingPanel;
            skillSelectCamera = pcSkillSelectCamera;
            playerModels = pcPlayerModels;
        }
    }

    public void Ready()
    {
        wizzardChoosePlace.SetActive(false);
        skillCanvas.SetActive(false);
        waitingPanel.SetActive(true);
    }

    public void GameStart()
    {
        skillSelectCamera.SetActive(false);
        wizzardChoosePlace.SetActive(false);
        skillCanvas.SetActive(false);
        if (waitingPanel.activeSelf == true)
        {
            waitingPanel.SetActive(false);
        }
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.NickName == PhotonNetwork.PlayerList[i].NickName)
            {
                string playerModelName = playerModels[(int)skillChooseController.MyType].name;

                GameObject _player = PhotonNetwork.Instantiate(playerModelName, spawnPoint[i].position, Quaternion.identity);
                PlayerManager playerManager = _player.GetComponent<PlayerManager>();

                for (int j = 0; j < playerManager.skillController.skill.Length; j++)
                {
                    playerManager.skillController.skill[j] = skillChooseController.currentSelectedSkills.selectedSkills[j];
                }
                playerManager.skillController.ultimateSkill = skillChooseController.currentSelectedSkills.utilmateSkill;
            }
        }
        battleUICanvas.SetActive(true);
    }

    public IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(1, 0, 0, Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);
        bloodScreen.color = Color.clear;
    }
}
