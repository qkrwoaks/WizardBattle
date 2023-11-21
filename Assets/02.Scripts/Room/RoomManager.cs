using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("Room UI")]
    private GameObject roomPanel;
    [SerializeField] private GameObject pcRoomPanel;
    [SerializeField] private GameObject vrRoomPanel;

    private GameObject[] playerList;
    [SerializeField] private GameObject[] pcPlayerList;
    [SerializeField] private GameObject[] vrPlayerList;
    private TMP_Text[] playerListText;

    private TMP_Text roomNameText;
    [SerializeField] private TMP_Text pcRoomNameText;
    [SerializeField] private TMP_Text vrRoomNameText;

    private Button startBnt;
    [SerializeField] private Button pcStartBnt;
    [SerializeField] private Button vrStartBnt;

    bool isVR;

    private void Awake()
    {
        isVR = PCVRSetting.isVR;
        InitVR(isVR);
    }

    private void InitVR(bool isVR)
    {
        if (isVR)
        {
            roomPanel = vrRoomPanel;
            playerList = vrPlayerList;
            roomNameText = vrRoomNameText;
            startBnt = vrStartBnt;
        }
        else
        {
            roomPanel = pcRoomPanel;
            playerList = pcPlayerList;
            roomNameText = pcRoomNameText;
            startBnt = pcStartBnt;
        }
    }

    private void Start()
    {
        if (isVR)
        {
            PhotonNetwork.Instantiate("VRRoomPlayer", Vector3.zero, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("RoomPlayer", Vector3.zero, Quaternion.identity);
        }

        playerListText = new TMP_Text[playerList.Length];

        for (int i = 0; i < playerList.Length; i++)
        {
            playerListText[i] = playerList[i].transform.GetChild(0).GetComponent<TMP_Text>();
            playerList[i].SetActive(false);
        }

        roomNameText.text = (string)(PhotonNetwork.CurrentRoom.Name.Split("_", System.StringSplitOptions.None).GetValue(0));

        if (PhotonNetwork.IsMasterClient == true)
        {
            startBnt.gameObject.SetActive(true);
            startBnt.interactable = false;
        }
        else
        {
            startBnt.gameObject.SetActive(false);
        }

        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            startBnt.interactable = true;
        }

        RoomRenewal();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        LoadingSceneManager.LoadScene("CreateRoom");
    }

    private void RoomRenewal()
    {
        for (int i = 0; i < playerList.Length; i++)
        {
            playerList[i].SetActive(false);
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerList[i].SetActive(true);
            playerListText[i].text = PhotonNetwork.PlayerList[i].NickName;
        }
    }

    /// <summary>
    /// 다른 플레이어가 방에 들어왔을 때
    /// </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();

        if (PhotonNetwork.IsMasterClient == true)
        {
            startBnt.interactable = true;
        }
    }

    /// <summary>
    /// 다른 플레이어가 방을 나갔을 때
    /// </summary>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();

        if (PhotonNetwork.IsMasterClient == true)
        {
            startBnt.interactable = false;
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        RoomRenewal();

        if (PhotonNetwork.NickName == newMasterClient.NickName)
        {
            startBnt.gameObject.SetActive(true);
            startBnt.interactable = false;
        }
    }

    public void GameStart()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
}