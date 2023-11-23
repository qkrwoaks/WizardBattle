using System.Collections;
using UnityEngine;
using Photon.Pun;

public class FieldSkill : Skill
{
    #region Variable(변수)
    [SerializeField] private GameObject magicEffect;         // 마법 이펙트를 가지고 있는 게임 오브젝트
    [SerializeField] private GameObject expectedRangePrefab; // 예상 스킬 범위를 보여주는 게임 오브젝트 프리팹

    [SerializeField] private float offset;                   // 오프셋

    [SerializeField] private float skillRange = 50;          // 최대 스킬 허용 범위

    private GameObject expectedRange;                        // 예상 범위 오브젝트
    private Transform weaponTr;

    private RaycastHit rayHit;                               // 광선이 닿았을 때 실행되는 구조체
    private Ray ray;                                         // 예상 범위의 광선

    private Transform ERTransform => expectedRange.transform;// 가독성 코드

    private GameObject magicObject;
    #endregion

    #region Function(함수)

    private void Awake()
    {
        if (PV == null)
        {
            PV = GetComponent<PhotonView>();
        }
    }

    protected override void Start()
    {
        if (PV.IsMine == false)
        {
            return;
        }

        weaponTr = GameSystem.Instance.plyerWeaponTr; // 플레이어 카메라 초기화
        base.Start();               // 부모 클래스의 Start 호출
    }

    /// <summary>
    /// 스킬을 사용하는 함수
    /// </summary>
    protected override void UseSkill()
    {
        expectedRange = Instantiate(expectedRangePrefab, transform.position, transform.rotation); // 예상 범위 오브젝트 생성
        ray = new Ray();                                                                          // 새로운 광선 생성
        StartCoroutine(ShowExpectedLocation());                                                   // 예상 스킬 사용 범위를 보여주는 함수 실행
    }

    /// <summary>
    /// 예상 스킬 사용 범위를 보여주는 함수
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowExpectedLocation()
    {
        while (Input.GetMouseButton(0) == false)                                   // 마우스 왼쪽 버튼을 누르지 않았을 때 (차후 변경 가능)
        {
            ray.origin = weaponTr.transform.position;                          // 광선 위치 초기화
            ray.direction = weaponTr.transform.forward;                        // 광선 방향 초기화
            if (Physics.Raycast(ray.origin, ray.direction, out rayHit, skillRange)) // 어떠한 물체가 광선에 닿았을 때
            {
                ERTransform.position = rayHit.point + Vector3.up * 0.5f;                   // 예상 스킬 범위 오브젝트 위치 초기화
            }
            yield return new WaitForEndOfFrame();                                  // 프레임 마다 대기
        }
        StartCoroutine(StartSkill());                                              // 스킬을 시작 함수
        yield return null;                                                         // 코루틴 종료
    }

    /// <summary>
    /// 스킬을 시작하는 함수
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartSkill()
    {
        // 장판 마법 소환
        magicObject = PhotonNetwork.Instantiate(magicEffect.name,
            new Vector3(ERTransform.position.x, ERTransform.position.y + offset, ERTransform.position.z),
            Quaternion.Euler(new Vector3(-90f, 0, 0)));

        magicObject.GetComponent<FieldSkillEffect>().skillATK = _ATK * GameSystem.Instance.playerManager.ATK;

        Destroy(expectedRange);                                // 예상 스킬 범위 오브젝트 삭제

        yield return new WaitForSeconds(_duration - 1f);   // 쿨타임 -1 초 대기
        magicObject.GetComponent<ParticleSystem>().Stop(); // 파티클 종료

        DestroyEffect();
        yield return new WaitForSeconds(1f);               // 1초 대기

        DestroyObject();

        StopCoroutine(StartSkill());
    }

    private void DestroyEffect()
    {
        if (magicObject != null)
        {
            PhotonNetwork.Destroy(magicObject);
        }
    }

#if UNITY_EDITOR // 유니티 에디터일 때

    /// <summary>
    /// 기즈모를 그릴때 호출되는 함수
    /// </summary>
    private void OnDrawGizmos()
    {
        Debug.DrawRay(ray.origin, ray.direction * skillRange, Color.red); // 광선의 색을 유니티 에디터에서 보이게 함
    }
#endif

    #endregion
}