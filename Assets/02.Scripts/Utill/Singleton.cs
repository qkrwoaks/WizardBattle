using UnityEngine;

/// <summary>
/// �̱��� ���̽� 
/// </summary>
/// <typeparam name="T">Singleton Ŭ������ ��ӹ��� TŸ�Ը� �� �� ����</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    // �� ���� �� �ı����� ���� ����
    public bool isDontDestroy;

    // �ν��Ͻ� ��ü�� ���� �ʵ�
    private static T _instance;

    public static T Instance
    {
        get
        {
            // �ν��Ͻ��� ������ Ȯ��
            if (_instance == null)
            {
                // ���ٸ� ������ ã��
                _instance = FindObjectOfType(typeof(T)) as T;

                // ã�Ƶ� ���ٸ�
                if (_instance == null)
                {
                    // �����Ͽ� TŸ�� ������Ʈ�� ����
                    var go = new GameObject(typeof(T).ToString());
                    _instance = go.AddComponent<T>();
                }
            }

            // ��������� ��ȯ �ÿ��� �ν��Ͻ��� ������ ������
            return _instance;
        }
    }

    public virtual void Awake()
    {
        // Awake�� ���Դٴ� ���� �� ����� ���̶�Ű�� ��ü�� �����Ѵٴ� ��
        // �� �� ���� 2���� -> ���� �̸� ���� TŸ�� ��ũ��Ʈ�� ���� ��ü�� ��ġ�߰ų�
        //                      �ٸ� ��ũ��Ʈ�� ���� TŸ�� �ν��Ͻ��� �����Ͽ� ��ü�� ������ ���

        // �� �� �̱��� ��뿡 ��Ģ���� �ο�
        // ex) �� ������ �̱����� ���� ��ü��� ���� �̸� ��ũ��Ʈ�� ���� ��ü�� ��ġ�ϴ� ������ ����Ѵ�

        // ���� ���, Ÿ ��ũ��Ʈ���� awake �������� ���� ������ tŸ�԰�ü�� �������� �ʾҴٸ�
        // �ν��Ͻ��� ������ null��

        // �׷� null�� �ƴ� ���, ���� tŸ�� ��ü�� �ϳ� �� ������ ���
        // �ش� ��ü�� �ı������ν� ��������� �� �ϳ��� �ν��Ͻ����� ������ �� �ִ�.

        if (_instance == null)
        {
            _instance = this as T;

            if (isDontDestroy)
                DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
