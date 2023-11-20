using Photon.Pun;
using System.Collections;
using UnityEngine;

public class SolingSkill : Skill
{
    public float _delayTime = 10f;

    protected override void UseSkill()
    {
        if (PV.IsMine == true)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    IEnumerator AttackCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(_delayTime);
            SolingProjectingSkill projecting =  PhotonNetwork.Instantiate("Skill\\" + "SolingProjecting", transform.position, Quaternion.identity).GetComponent<SolingProjectingSkill>();
            projecting._speed = _speed;
            projecting._damage = _ATK * GameSystem.Instance.playerManager.ATK;
            projecting._duration = _duration;
        }
        DestroyObject();
    }
}