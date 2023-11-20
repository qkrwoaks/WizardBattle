using System.Collections;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private float damage = 2f;

    private void OnTriggerEnter(Collider other)
    {
        StopAllCoroutines();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerManager player = other.gameObject.GetComponent<PlayerManager>();

            //�� ������ ���� �÷��̾����� ���� �������� �� ���� ó�� �Ѵ�
            StartCoroutine(Damage(player));
        }
    }

    private IEnumerator Damage(PlayerManager player)
    {
        float timer = 0.1f;
        float currentTime = 0;

        while (true)
        {
            currentTime += Time.deltaTime;
            if (currentTime > timer)
            {
                player.Hit(damage);
                currentTime = 0;
            }
            yield return null;
        }
    }
}
