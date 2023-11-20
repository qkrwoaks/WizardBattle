using Photon.Pun;
using System.Collections;
using UnityEngine;

public class HitFireSkill : Skill
{
    [SerializeField]
    private Transform playerTr;     // 플레이어 Transform

    private float accumulateDamage;
    private float increaseATK;

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
        PV.RPC("DpRPC", RpcTarget.All, 50f);

        PV.RPC("ParentPlayer", RpcTarget.All);

        yield return new WaitForSeconds(_duration / 2);

        PV.RPC("DpRPC", RpcTarget.All, -50f);
        transform.GetComponent<SphereCollider>().enabled = false;
        increaseATK = accumulateDamage / 100;
        PV.RPC("DpRPC", RpcTarget.All, increaseATK);

        yield return new WaitForSeconds(_duration / 2);

        PV.RPC("DpRPC", RpcTarget.All, -increaseATK);

        DestroyObject();        // 지속시간이 끝나면 삭제
    }



    [PunRPC]
    public void DpRPC(float dp)
    {
        PlayerManager playerManager = PV.IsMine == true ? GameSystem.Instance.playerManager : GameSystem.Instance.enemyManager;
        playerManager.DP += dp;
    }

    [PunRPC]
    public void ATKRPC(float atk)
    {
        PlayerManager playerManager = PV.IsMine == true ? GameSystem.Instance.playerManager : GameSystem.Instance.enemyManager;
        playerManager.ATK += atk;
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
