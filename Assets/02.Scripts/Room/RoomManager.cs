using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("Room UI")]
    [SerializeField]
    private GameObject roomPanel;

    [SerializeField]
    private GameObject[] playerList;
    private TMP_Text[] playerListText;  

    [SerializeField]
    private TMP_Text roomNameText;
    [SerializeField]
    private Button startBnt;

    private void Start()
    {
        PhotonNetwork.Instantiate("RoomPlayer", Vector3.zero, Quaternion.identity);

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
    /// �ٸ� �÷��̾ �濡 ������ ��
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
    /// �ٸ� �÷��̾ ���� ������ ��
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