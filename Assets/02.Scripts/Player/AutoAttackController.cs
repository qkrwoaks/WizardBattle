using Photon.Pun;
using UnityEngine;

public class AutoAttackController : MonoBehaviour
{
    [SerializeField] GameObject autoAttackPrefab;         //속성별로 저장

    [SerializeField] float timer;

    void Update()
    {
        if (GameSystem.Instance.playerManager.PV.IsMine == true)
        {
            FireCheck();
        }
    }

    private void FireCheck()
    {
        timer += Time.deltaTime;

        if (GameSystem.Instance.playerManager.PV.IsMine == true && Input.GetMouseButtonDown(0) && timer >= GameSystem.Instance.playerManager.attackSpeed)
        {
            Fire();
            timer = 0f;
        }
    }

    private void Fire()
    {
        PhotonNetwork.Instantiate("AutoAttack\\" + autoAttackPrefab.name, Camera.main.transform.position, Quaternion.identity);
    }
}