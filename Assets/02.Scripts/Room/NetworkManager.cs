using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("DisconnectPanel")]
    [SerializeField] private TMP_InputField pcNicknameInput;
    [SerializeField] private TMP_InputField vrNicknameInput;
    [SerializeField] private GameObject pcDisconnectPanel;
    [SerializeField] private GameObject vrDisconnectPanel;

    [Header("Lobby")]
    [SerializeField] private GameObject pcLobbyPanel;
    [SerializeField] private GameObject vrLobbyPanel;

    [Header("ETC")]
    [SerializeField] private TMP_Text pcStatusText;
    [SerializeField] private TMP_Text vrStatusText;

    private TMP_InputField nicknameInput;
    private GameObject disconnectPanel;
    private GameObject lobbyPanel;
    private TMP_Text statusText;

    private void Awake()
    {
        InitVR(PCVRSetting.isVR);

        Screen.SetResolution(1920, 1080, true); // �ػ�, Ǯ��ũ��

        PhotonNetwork.SendRate = 60;            // �����͸� �����ϴ� �ʴ� Ƚ��
        PhotonNetwork.SerializationRate = 30;   // ������ ������

        // �÷��̾� ���� ���� ����Ǵ� ����� ���ÿ� ���. true�� MasterŬ���̾�Ʈ���� LoadLevel()�� ȣ���� �� �ִ�.
        // �̶� ���� ��� Ŭ���̾�Ʈ�� ������ Ŭ���̾�Ʈ�� ������ ������ �ڵ����� �ε���. �� true�� ���� ����ȭ.
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected == true)
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

    private void InitVR(bool isVR)
    {
        if (isVR)
        {
            nicknameInput = vrNicknameInput;
            disconnectPanel = vrDisconnectPanel;
            lobbyPanel = vrLobbyPanel;
            statusText = vrStatusText;
        }
        else
        {
            nicknameInput = pcNicknameInput;
            disconnectPanel = pcDisconnectPanel;
            lobbyPanel = pcLobbyPanel;
            statusText = pcStatusText;
        }
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