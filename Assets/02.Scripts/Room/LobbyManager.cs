using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Lobby UI")]
    private TMP_InputField roomNameInput;
    [SerializeField] private TMP_InputField pcRoomNameInput;
    [SerializeField] private TMP_InputField vrRoomNameInput;
    private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField pcPasswordInput;
    [SerializeField] private TMP_InputField vrPasswordInput;
    private TMP_InputField passwordCheckInput;
    [SerializeField] private TMP_InputField pcPasswordCheckInput;
    [SerializeField] private TMP_InputField vrPasswordCheckInput;
    private Toggle privateRoomToggle;
    [SerializeField] private Toggle pcPrivateRoomToggle;
    [SerializeField] private Toggle vrPrivateRoomToggle;
    private Button[] cellBtn;
    [SerializeField] private Button[] pcCellBtn;
    [SerializeField] private Button[] vrCellBtn;
    private Button PreviousBtn;
    [SerializeField] private Button pcPreviousBtn;
    [SerializeField] private Button vrPreviousBtn;
    private Button NextBtn;
    [SerializeField] private Button pcNextBtn;
    [SerializeField] private Button vrNextBtn;
    private GameObject passwordInputPanel;
    [SerializeField] private GameObject pcPasswordInputPanel;
    [SerializeField] private GameObject vrPasswordInputPanel;

    private List<RoomInfo> myList = new List<RoomInfo>();
    private int currentPage = 1, maxPage, multiple;
    private string[] currentRoomName;

    private void Awake()
    {
        InitVR(PCVRSetting.isVR);
    }

    private void InitVR(bool isVR)
    {
        if (isVR)
        {
            roomNameInput = vrRoomNameInput;
            passwordInput = vrPasswordInput;
            passwordCheckInput = vrPasswordCheckInput;
            privateRoomToggle = vrPrivateRoomToggle;
            cellBtn = vrCellBtn;
            PreviousBtn = vrPreviousBtn;
            NextBtn = vrNextBtn;
            passwordInputPanel = vrPasswordInputPanel;
        }
        else
        {
            roomNameInput = pcRoomNameInput;
            passwordInput = pcPasswordInput;
            passwordCheckInput = pcPasswordCheckInput;
            privateRoomToggle = pcPrivateRoomToggle;
            cellBtn = pcCellBtn;
            PreviousBtn = pcPreviousBtn;
            NextBtn = pcNextBtn;
            passwordInputPanel = pcPasswordInputPanel;
        }
    }

    /// <summary>
    /// 방 생성
    /// </summary>
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        if (privateRoomToggle.isOn == true) // 만약 비공개 방이라면
        {
            if (passwordInput.text == string.Empty)
            {
                // 비밀번호가 입력되지 않았으면 방생성 X
                Debug.Log("비밀번호가 입력되지 않았습니다.");
                return;
            }
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "private", true } };
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "private" }; // 여기에 키 값을 등록해야, 필터링이 가능하다.
            PhotonNetwork.CreateRoom(roomNameInput.text == string.Empty ? $"Room{Random.Range(0, 100)}_" : $"{roomNameInput.text}_{passwordInput.text}", roomOptions);
            privateRoomToggle.isOn = false;
        }
        else
        {
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "private", false } };
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "private" }; // 여기에 키 값을 등록해야, 필터링이 가능하다.
            PhotonNetwork.CreateRoom(roomNameInput.text == string.Empty ? $"Room{Random.Range(0, 100)}_" + Random.Range(0, 100) : $"{roomNameInput.text}_", roomOptions);
        }
        roomNameInput.text = "";
    }

    public void JoinRandomOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2; // 인원 지정.
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "private", false } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "private" }; // 여기에 키 값을 등록해야, 필터링이 가능하다.      

        // 방 참가를 시도하고, 실패하면 생성해서 참가함.
        PhotonNetwork.JoinRandomOrCreateRoom(
            expectedCustomRoomProperties: new ExitGames.Client.Photon.Hashtable() { { "private", false } },
            expectedMaxPlayers: 2, roomName: $"Room{Random.Range(0, 100)}_", roomOptions: roomOptions // 생성할 때의 기준.
        );
    }

    /// <summary>
    /// 방 리스트에서 방을 클릭했을 때
    /// </summary>
    public void MyListClick(int num)
    {
        // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자

        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else
        {
            currentRoomName = myList[multiple + num].Name.Split("_");

            if (currentRoomName[1] != string.Empty) // 비공개 방일 때
            {
                // 비밀번호 입력 창 키기
                passwordInputPanel.SetActive(true);
            }
            else   // 공개 방일 때
            {
                PhotonNetwork.JoinRoom(myList[multiple + num].Name);
            }
        }
        MyListRenewal();
    }

    public void InputRoomPassword()
    {
        if (passwordCheckInput.text.Equals(currentRoomName[1]))
        {
            PhotonNetwork.JoinRoom($"{currentRoomName[0]}_{currentRoomName[1]}");
            passwordCheckInput.text = string.Empty;
            passwordInputPanel.SetActive(false);
        }
        else
        {
            passwordCheckInput.text = string.Empty;
            Debug.Log("비밀번호가 틀렸습니다.");
        }
    }

    #region 방리스트 갱신
    void MyListRenewal()
    {
        // 최대페이지
        maxPage = (myList.Count % cellBtn.Length == 0) ? myList.Count / cellBtn.Length : myList.Count / cellBtn.Length + 1;

        // 이전, 다음버튼
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * cellBtn.Length;
        for (int i = 0; i < cellBtn.Length; i++)
        {
            string[] roomName = (multiple + i < myList.Count) ? myList[multiple + i].Name.Split("_") : new string[2];

            cellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            cellBtn[i].transform.GetChild(0).GetComponent<TMP_Text>().text = roomName[0];
            cellBtn[i].transform.GetChild(1).GetComponent<TMP_Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";

            // 방이 비공개라면 자물쇠 이미지 활성화
            if (multiple + i < myList.Count)
            {
                if (roomName[1] != string.Empty)
                {
                    cellBtn[i].transform.GetChild(2).gameObject.SetActive(true);
                }
            }
            else
            {
                cellBtn[i].transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("RoomScene");
        }
    }
    #endregion
}