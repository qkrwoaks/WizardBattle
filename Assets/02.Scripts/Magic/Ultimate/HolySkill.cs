using Photon.Pun;
using System.Collections;
using UnityEngine;

public class HolySkill : Skill
{
    #region Variable(����)

    [SerializeField] private GameObject magicEffect;  // ���� ����Ʈ�� ������ �ִ� ���� ������Ʈ

    #endregion

    #region Function (�Լ�)

    protected override void Start()
    {
        base.Start();
    }

    protected override void UseSkill()
    {
        StartCoroutine(StartGodModeCor()); // ���� ��� ����
    }

    /// <summary>
    /// ����Ʈ�� ���� �ϴ� �Լ�
    /// </summary>
    [PunRPC]
    public void ParentPlayer()
    {
        transform.position = PlayerTrInit().position;
        transform.SetParent(PlayerTrInit());                // ��ų�� �÷��̾ ����ٴϰ� �÷��̾��� �ڽ����� ����
    }

    /// <summary>
    /// ���� ��� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartGodModeCor()
    {
        // �÷��̾� �Ŵ��� ��ũ��Ʈ �ʱ�ȭ
        PV.RPC("GodModeRPC", RpcTarget.All, true);                            // �÷��̾��� ����� ����
        PV.RPC("ParentPlayer", RpcTarget.All);                                         // ����Ʈ ���� �Լ� ����
        yield return new WaitForSeconds(_duration);                                    // ��Ÿ�� ��ŭ ���
        PV.RPC("GodModeRPC", RpcTarget.All, false);                         // �÷��̾��� ����� ����
        yield return null;                                                             // �ڷ�ƾ ����
        DestroyObject();
    }

    [PunRPC]
    public void GodModeRPC(bool isgodmode)
    {
        PlayerManager player = PV.IsMine == true ? GameSystem.Instance.playerManager : GameSystem.Instance.enemyManager;
        player.isGodMode = isgodmode;
    }

    #endregion
}
