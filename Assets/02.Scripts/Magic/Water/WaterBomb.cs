using System.Collections;
using UnityEngine;

public class WaterBomb : Skill
{
    public Vector3 target = Vector3.zero; //목표지점
    public float firingAngle = 45.0f;     //각도
    public float gravity = 9.8f;          //중력

    Vector3 offset;

    private Transform weaponTr = GameSystem.Instance.plyerWeaponTr;

    public Transform Projectile;          //투사체
    private Transform myTransform;        //투사체 위치

    public Vector3 maxScale;

    protected override void Start()
    {
        transform.rotation = weaponTr.transform.rotation;
        target = weaponTr.transform.forward * 40;
        myTransform = transform;

        base.Start();
    }

    protected override void UseSkill()
    {
        StartCoroutine(ParabolicProjection());
    }

    /// <summary>
    /// 포물선 코드
    /// </summary>
    /// <returns></returns>
    IEnumerator ParabolicProjection()
    {
        // 투사체를 물체를 던지는 위치로 이동하고 필요한 경우 offset을 추가합니다.
        Projectile.position = myTransform.position + offset;

        // 대상까지의 거리 계산
        float target_Distance = Vector3.Distance(Projectile.position, target);

        // 물체를 지정된 각도로 목표물에 던지는 데 필요한 속도를 계산합니다.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // 속도의 X,Y 추출
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // 비행 시간을 계산한다
        float flight_Duration = target_Distance / Vx;

        // 발사체를 회전 시켜 목표물을 향하게 한다
        Projectile.rotation = Quaternion.LookRotation(target - Projectile.position);

        //경과 시간
        float elapse_time = 0;

        Vector3 defaultScale = Projectile.localScale;

        //투사체가 목표 지점 까지 포물선을 그리면서 정해진 시간과 속도 만큼 간다는 주석 
        while (elapse_time < flight_Duration)
        {
            Projectile.localScale = Vector3.Lerp(defaultScale, maxScale, elapse_time / flight_Duration);
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }

        DestroyObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameSystem.Instance.enemy == other.gameObject &&
                    other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager player) && PV.IsMine == true)
        {
            player.Hit(_ATK * GameSystem.Instance.playerManager.ATK);
        }
    }
}