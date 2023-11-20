using Photon.Pun;
using UnityEngine;

public class SolingProjectingSkill : MonoBehaviour
{
    private PhotonView PV;
    public float _speed;
    public float _damage;
    public float _duration;

    private float dis;
    private float waitTime;
    public Transform targetTr;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        FindTarget();

        dis = Vector3.Distance(transform.position, targetTr.position);

        //투사체 생성 처음에 넓게 멀어지며 회전하기 위해서
        //투사체의 회전을 캐릭터위치에서 투사체의 위치의 방향으로 놓습니다
        transform.rotation = Quaternion.LookRotation(transform.position - targetTr.position);

        Destroy(gameObject, 6f);
    }

    private void FindTarget()
    {
        targetTr = PV.IsMine ? GameSystem.Instance.enemy.transform : GameSystem.Instance.player.transform;

        if (targetTr == null)
        {
            Debug.LogError("Not Found Target!!");
        }
    }

    void Update()
    {
        InductionMove();
    }

    void InductionMove()
    {
        if (targetTr == null) return;

        waitTime += Time.deltaTime;
        //1.5초 동안 천천히 forward 방향으로 전진
        if (waitTime < 1.5f)
        {
            _speed = Time.deltaTime;
            transform.Translate(transform.forward * _speed, Space.World);
        }
        else
        {
            // 1.5초 이후 타겟방향으로 lerp위치이동

            _speed += Time.deltaTime;
            float t = _speed / dis;

            transform.position = Vector3.LerpUnclamped(transform.position, targetTr.position, t);
        }

        // 매프레임마다 타겟방향으로 포탄이 방향을 바꿈
        //투사체 위치 - 포탄위치 = 투사체가 타겟한테서의 방향
        Vector3 directionVec = targetTr.position - transform.position;
        Quaternion qua = Quaternion.LookRotation(directionVec);
        transform.rotation = Quaternion.Slerp(transform.rotation, qua, Time.deltaTime * 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameSystem.Instance.enemy == other.gameObject &&
                    other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager player) && PV.IsMine == true)
        {
            player.Hit(_damage);
            Destroy(gameObject);
        }
    }
}
