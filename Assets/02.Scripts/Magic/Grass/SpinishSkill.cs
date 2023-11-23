using Photon.Pun;
using System.Collections;
using UnityEngine;

public class SpinishSkill : Skill
{
    public Transform Projectile; //발사체
    Transform myTransform;

    Vector3 target = Vector3.zero;
    [SerializeField] Vector3 offset;        //offset

    [SerializeField] float moveDistance = 1f;

    [SerializeField] float retationTime;    //유지 시간

    [SerializeField] GameObject signPrefab;     //표식 프리팹 : 1차 맞았을 때 retationTime 만큼 표식이 생김
    [SerializeField] GameObject signObj;        // 표식 오브젝트

    private Transform weaponTr = GameSystem.Instance.plyerWeaponTr;

    protected override void Start()
    {
        myTransform = transform;
        transform.rotation = weaponTr.transform.rotation;
        target = new Vector3(0, 0.0f, moveDistance);

        base.Start();
    }

    protected override void UseSkill()
    {
        StartCoroutine(StraightMovement());
    }

    IEnumerator StraightMovement()
    {
        Projectile.position = myTransform.position + offset;

        float timer = 0f;

        while (timer <= _duration)
        {
            timer += Time.deltaTime;
            Projectile.Translate(target * _speed * Time.deltaTime);

            yield return new WaitForEndOfFrame();

        }
        DestroyObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameSystem.Instance.enemy &&
            other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager player) && PV.IsMine == true)
        {
            if (player.isSolingAttack)
            {

                player.Hit(_ATK * GameSystem.Instance.playerManager.ATK);
                player.isSolingAttack = false;
                if (signObj != null)
                {
                    Destroy(signObj);
                }
            }
            else
            {
                player.Hit(_ATK * GameSystem.Instance.playerManager.ATK);
                StartCoroutine(SetSolingAttack(player));
            }
        }
    }

    IEnumerator SetSolingAttack(PlayerManager player)
    {
        player.isSolingAttack = true;
        GameObject signObj = PhotonNetwork.Instantiate("Skill\\" + signPrefab.name, player.transform.position, Quaternion.identity);
        signObj.transform.parent = player.transform;
        yield return new WaitForSeconds(retationTime);
        player.isSolingAttack = false;
        PhotonNetwork.Destroy(signObj);
    }
}
