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
    [SerializeField]
    public SkillCoolTimeUI skillCoolTimeUI;
    [SerializeField]
    public OutcomeUIController outcomeUIController;
    [SerializeField]
    private GameObject wizzardChoosePlace;
    [SerializeField]
    private GameObject skillCanvas;
    [SerializeField]
    public SkillChooseController skillChooseController;
    [SerializeField]
    private Image bloodScreen;
    [SerializeField]
    private GameObject waitingPanel;

    [Space(10f)]
    [SerializeField]
    private Transform[] spawnPoint;
    [SerializeField]
    private GameObject skillSelectCamera;

    [SerializeField]
    public GameObject[] playerModels;

    private void Start()
    {
        wizzardChoosePlace.SetActive(true);
        skillCanvas.SetActive(true);
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
                // VR
                //PhotonNetwork.Instantiate("VRPlayer", spawnPoint[i].position, Quaternion.identity);

                // PC
                string playerModelName = playerModels[(int)skillChooseController.MyType].name;
                Debug.Log(playerModelName);
                GameObject _player = PhotonNetwork.Instantiate(playerModelName, spawnPoint[i].position, Quaternion.identity); ;
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
