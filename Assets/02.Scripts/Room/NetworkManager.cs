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
        Screen.SetResolution(1920, 1080, true); // 해상도, 풀스크린

        PhotonNetwork.SendRate = 60;            // 데이터를 전송하는 초당 횟수
        PhotonNetwork.SerializationRate = 30;   // 전송이 빨라짐
        
        // 플레이어 수에 따라 변경되는 경기장 세팅에 사용. true면 Master클라이언트에서 LoadLevel()을 호출할 수 있다.
        // 이때 방의 모든 클라이언트가 마스터 클라이언트와 동일한 레벨을 자동으로 로드함. 즉 true시 레벨 동기화.
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
        // 상태 텍스트
        statusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    // 접속
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    // 연결 성공 시 로비 접속
    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();    

    // 연결 끊기
    public void Disconnect() => PhotonNetwork.Disconnect();

    // 로비 접속 될 때 실행
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

    // 연결이 끊어 질 때
    public override void OnDisconnected(DisconnectCause cause)
    {
        lobbyPanel.SetActive(false);
    }
}