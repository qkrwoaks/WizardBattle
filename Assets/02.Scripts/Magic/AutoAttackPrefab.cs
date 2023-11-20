using System.Collections;
using UnityEngine;

public class AutoAttackPrefab : Skill
{
    public Transform Projectile; //πﬂªÁ√º
    Transform myTransform;

    Vector3 target = Vector3.zero;
    [SerializeField] Vector3 offset;        //offset

    [SerializeField] float moveDistance = 1f;

    protected override void Start()
    {
        myTransform = transform;
        transform.rotation = Camera.main.transform.rotation;
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
        float moveTime = _duration;

        while (timer <= moveTime)
        {
            timer += Time.deltaTime;
            Projectile.Translate(target * _speed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        DestroyObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PV.IsMine == false)
        {
            return;
        }

        if (GameSystem.Instance.enemy == other.gameObject &&
            other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager enemy))
        {
            enemy.Hit(ATK * GameSystem.Instance.playerManager.ATK);
            DestroyObject();
        }
    }
}