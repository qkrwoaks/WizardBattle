using Photon.Pun;
using System.Collections;
using UnityEngine;

public class CottonguardSkill : Skill
{
    [SerializeField]
    private Transform playerTr;                         // �÷��̾� Transform

    private Vector3 offset = new Vector3(0, 1.5f, 0);

    protected override void Start()
    {
        playerTr = PlayerTrInit();
        base.Start();
    }

    protected override void UseSkill()
    {
        StartCoroutine(StartSkill());
    }

    private IEnumerator StartSkill()
    {
        offset += Camera.main.transform.forward;             // �÷��̾ �ٶ󺸴� ���� ���ϱ�

        transform.position = playerTr.position + offset;
        transform.rotation = Camera.main.transform.rotation; // �÷��̾� �������� ������

        PV.RPC("GodModeRPC", RpcTarget.All, true);

        yield return new WaitForSeconds(_duration);

        PV.RPC("GodModeRPC", RpcTarget.All, false);

        yield return new WaitForSeconds(1f);
        DestroyObject();            // ���ӽð��� ������ ����
    }

    [PunRPC]
    public void GodModeRPC(bool isgodmode)
    {
        PlayerManager player = PV.IsMine == true ? GameSystem.Instance.playerManager : GameSystem.Instance.enemyManager;
        player.isGodMode = isgodmode;
    }
}
