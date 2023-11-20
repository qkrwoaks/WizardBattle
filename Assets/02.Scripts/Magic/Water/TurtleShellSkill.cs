using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class TurtleShellSkill : Skill
{
    [SerializeField]
    private Transform playerTr;     // 플레이어 Transform

    protected override void Start()
    {
        base.Start();
    }

    protected override void UseSkill()
    {
        StartCoroutine(StartSkill());
    }

    private IEnumerator StartSkill()
    {
        PV.RPC("ParentPlayer", RpcTarget.All);

        PV.RPC("DpRPC", RpcTarget.All, 50f);
        GameSystem.Instance.playerManager.speed *= 0.5f;

        yield return new WaitForSeconds(_duration);

        PV.RPC("DpRPC", RpcTarget.All, -50f);
        GameSystem.Instance.playerManager.speed *= 2f;

        DestroyObject();
    }

    [PunRPC]
    public void DpRPC(float dp)
    {
        PlayerManager playerManager = PV.IsMine == true ? GameSystem.Instance.playerManager : GameSystem.Instance.enemyManager;
        playerManager.DP += dp;
    }


    [PunRPC]
    public void ParentPlayer()
    {
        transform.position = PlayerTrInit().position;
        transform.SetParent(PlayerTrInit());                // 스킬이 플레이어를 따라다니게 플레이어의 자식으로 설정
    }
}