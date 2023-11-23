using System.Collections;
using UnityEngine;

public class WaterBomb : Skill
{
    public Vector3 target = Vector3.zero; //��ǥ����
    public float firingAngle = 45.0f;     //����
    public float gravity = 9.8f;          //�߷�

    Vector3 offset;

    private Transform weaponTr = GameSystem.Instance.plyerWeaponTr;

    public Transform Projectile;          //����ü
    private Transform myTransform;        //����ü ��ġ

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
    /// ������ �ڵ�
    /// </summary>
    /// <returns></returns>
    IEnumerator ParabolicProjection()
    {
        // ����ü�� ��ü�� ������ ��ġ�� �̵��ϰ� �ʿ��� ��� offset�� �߰��մϴ�.
        Projectile.position = myTransform.position + offset;

        // �������� �Ÿ� ���
        float target_Distance = Vector3.Distance(Projectile.position, target);

        // ��ü�� ������ ������ ��ǥ���� ������ �� �ʿ��� �ӵ��� ����մϴ�.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // �ӵ��� X,Y ����
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // ���� �ð��� ����Ѵ�
        float flight_Duration = target_Distance / Vx;

        // �߻�ü�� ȸ�� ���� ��ǥ���� ���ϰ� �Ѵ�
        Projectile.rotation = Quaternion.LookRotation(target - Projectile.position);

        //��� �ð�
        float elapse_time = 0;

        Vector3 defaultScale = Projectile.localScale;

        //����ü�� ��ǥ ���� ���� �������� �׸��鼭 ������ �ð��� �ӵ� ��ŭ ���ٴ� �ּ� 
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