using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("DisconnectPanel")]
    [SerializeField]
    private TMP_InputField nicknameInput;
    [SerializeField]
    private GameObject disconnectPanel;

    [Header("Lobby")]
    [SerializeField]
    private GameObject lobbyPanel;

    [Header("ETC")]
    [SerializeField]
    private TMP_Text statusText;
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true); // �ػ�, Ǯ��ũ��

        PhotonNetwork.SendRate = 60;            // �����͸� �����ϴ� �ʴ� Ƚ��
        PhotonNetwork.SerializationRate = 30;   // ������ ������
        
        // �÷��̾� ���� ���� ����Ǵ� ����� ���ÿ� ���. true�� MasterŬ���̾�Ʈ���� LoadLevel()�� ȣ���� �� �ִ�.
        // �̶� ���� ��� Ŭ���̾�Ʈ�� ������ Ŭ���̾�Ʈ�� ������ ������ �ڵ����� �ε���. �� true�� ���� ����ȭ.
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        if(PhotonNetwork.IsConnected == true)
        {
            disconnectPanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }
    }

    private void Update()
    {
        // ���� �ؽ�Ʈ
        statusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    // ����
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    // ���� ���� �� �κ� ����
    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();    

    // ���� ����
    public void Disconnect() => PhotonNetwork.Disconnect();

    // �κ� ���� �� �� ����
    public override void OnJoinedLobby()
    {
        lobbyPanel.SetActive(true);
        disconnectPanel.SetActive(false);
        if (nicknameInput.text == string.Empty)
        {
            PhotonNetwork.LocalPlayer.NickName = $"Player{Random.Range(0, 100)}";
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = nicknameInput.text;
        }
    }

    // ������ ���� �� ��
    public override void OnDisconnected(DisconnectCause cause)
    {
        lobbyPanel.SetActive(false);
    }
}