using System.Collections;
using UnityEngine;

public class FinishSkill : Skill
{
    [SerializeField] Vector3 target;
    Vector3 offset = Vector3.zero;          //ray ½ò offset ¼³Á¤

    private float waitTime = 4f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void UseSkill()
    {
        transform.rotation = Camera.main.transform.rotation;

        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        transform.GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(waitTime);
    
        transform.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(_duration);

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
        }
    }
}