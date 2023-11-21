using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OutcomeUIController : MonoBehaviour
{
    [SerializeField] GameObject[] outcomePanel;         // 0: Win / 1: Lose
    [SerializeField] GameObject particle;
    [SerializeField] float waitTimer;

    [SerializeField] private Image bloodScreen;

    [SerializeField] private PhotonView PV;

    private void Awake()
    {
        if (PCVRSetting.isVR && PV.IsMine)
        {
            GameSystem.Instance.outcomeUIController = this;
            GameSystem.Instance.bloodScreen = bloodScreen;
        }
    }

    /// <summary>
    /// 이기고 짐에 따라서 다른 UI Panel 보이기
    /// </summary>
    /// <param name="win">이겼는지 졌는지 bool</param>
    public void ShowOutcomeUI(bool win)
    {
        StopAllCoroutines();
        StartCoroutine(ShowOutcomeUICoroutine(win));
    }

    IEnumerator ShowOutcomeUICoroutine(bool win)
    {
        GameObject currentPanel = win == true ? outcomePanel[0] : outcomePanel[1];
        currentPanel.SetActive(true);
        particle.SetActive(true);
        yield return new WaitForSeconds(waitTimer);
        currentPanel.SetActive(false);
        particle.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("RoomScene");
        }
    }
}
