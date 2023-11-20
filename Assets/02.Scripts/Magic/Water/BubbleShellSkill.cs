using Photon.Pun;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;

public class BubbleShellSkill : Skill
{
    private float accumulateDamage;
    private float increaseDP;

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
        transform.GetComponent<SphereCollider>().enabled = true;
        PV.RPC("DpRPC", RpcTarget.All, 50f);

        yield return new WaitForSeconds(_duration / 2);

        PV.RPC("DpRPC", RpcTarget.All, -50f);
        transform.GetComponent<SphereCollider>().enabled = false;

        //increaseDP = accumulateDamage / 2;
        //GameSystem.Instance.playerManager.shield += increaseDP;

        yield return new WaitForSeconds(_duration / 2);

        //GameSystem.Instance.playerManager.shield = 0;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Skill>(out Skill skill) && PV.IsMine == true)
        {
            accumulateDamage += skill.ATK * GameSystem.Instance.enemyManager.ATK;
        }
        else if (other.gameObject.TryGetComponent<SolingProjectingSkill>(out SolingProjectingSkill soling) && PV.IsMine == true)
        {
            accumulateDamage += soling._damage * GameSystem.Instance.enemyManager.ATK;
        }
    }
}