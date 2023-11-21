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
    /// �̱�� ���� ���� �ٸ� UI Panel ���̱�
    /// </summary>
    /// <param name="win">�̰���� ������ bool</param>
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
