using Photon.Pun;
using UnityEngine;

public class AutoAttackController : MonoBehaviour
{
    [SerializeField] GameObject autoAttackPrefab;         //�Ӽ����� ����

    [SerializeField] float timer;

    [SerializeField] private Camera playerCamera;

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
    public void VRFireCheck()
    {
        if (GameSystem.Instance.playerManager.PV.IsMine == true && timer >= GameSystem.Instance.playerManager.attackSpeed)
        {
            Fire();
            timer = 0f;
        }
    }

    private void Fire()
    {
        PhotonNetwork.Instantiate("AutoAttack\\" + autoAttackPrefab.name, GameSystem.Instance.playerManager.weaponTr.position, Quaternion.identity);
    }
}