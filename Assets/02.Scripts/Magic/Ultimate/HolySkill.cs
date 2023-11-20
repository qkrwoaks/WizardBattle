using Photon.Pun;
using System.Collections;
using UnityEngine;

public class HolySkill : Skill
{
    #region Variable(변수)

    [SerializeField] private GameObject magicEffect;  // 마법 이펙트를 가지고 있는 게임 오브젝트

    #endregion

    #region Function (함수)

    protected override void Start()
    {
        base.Start();
    }

    protected override void UseSkill()
    {
        StartCoroutine(StartGodModeCor()); // 무적 모드 실행
    }

    /// <summary>
    /// 이펙트를 관리 하는 함수
    /// </summary>
    [PunRPC]
    public void ParentPlayer()
    {
        transform.position = PlayerTrInit().position;
        transform.SetParent(PlayerTrInit());                // 스킬이 플레이어를 따라다니게 플레이어의 자식으로 설정
    }

    /// <summary>
    /// 무적 모드 실행 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartGodModeCor()
    {
        // 플레이어 매니저 스크립트 초기화
        PV.RPC("GodModeRPC", RpcTarget.All, true);                            // 플레이어의 갓모드 설정
        PV.RPC("ParentPlayer", RpcTarget.All);                                         // 이펙트 관리 함수 실행
        yield return new WaitForSeconds(_duration);                                    // 쿨타임 만큼 대기
        PV.RPC("GodModeRPC", RpcTarget.All, false);                         // 플레이어의 갓모드 해제
        yield return null;                                                             // 코루틴 종료
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
