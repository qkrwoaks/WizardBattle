using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class FPSCharacterManipulation : MonoBehaviourPunCallbacks
{
    private PhotonView PV;
    [SerializeField] private KeyCode jumpKeyCode;  // ���� Ű �ڵ�

    [SerializeField] private Transform playerBody; // �÷��̾� ���� ������ ����
    [SerializeField] private Transform cameraArm;  // ī�޶� �� ȸ���� ������ ����

    [SerializeField] private PlayerManager playerManager;

    [SerializeField] private float jumpForce;      // ������

    [SerializeField] private float sensitivity;    // ���콺 ����

    public UnityEvent walkEvent;
    public UnityEvent jumpEvent;

    bool isJump;

    int moveAnimParam = Animator.StringToHash("MoveSpeed");
    int jumpAnimParam = Animator.StringToHash("isJump");

    Animator animator;
    Rigidbody rigid;

    private void Awake()
    {
        animator = playerBody.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    protected void Start()
    {
        if (PV.IsMine == false)
        {
            // ���� �ƴ϶�� ī�޶� ����
            cameraArm.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            LookAround();
            Move();
            Jump();
        }
    }

    /// <summary>
    /// ī�޶��� ȸ���� ����ϴ� �Լ�
    /// </summary>
    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // ���α׷��ֿ��� �������� ���簪�� ���� �ַ� "Delta"��� ���� ǥ����
        Vector3 camAngle = cameraArm.rotation.eulerAngles; // ī�޶��� ���� Rotation ���� ���Ϸ� ������ ��ȯ
        float x = camAngle.x - mouseDelta.y;

        if (x < 180f) // �÷��̾��� ���� ������ ����
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x * sensitivity, camAngle.z); // ī�޶��� Rotation �� ��ȯ
        playerBody.transform.eulerAngles = new Vector3(0, camAngle.y, 0);                              // ���� �������� ���� 
    }

    /// <summary>
    /// �������� ����ϴ� �Լ�
    /// </summary>
    private void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // �̵� �Է� ���� ������
        bool isMove = moveInput.magnitude != 0;
        if (isMove) // �����̰� ���� �� ����Ǵ� �Լ�
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized; // ī�޶��� ������ ���ȭ ���� ����
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized; // ī�޶��� �������� ���ȭ ���� ����
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x; // ī�޶� ���� �̵� ���� ����

            walkEvent?.Invoke();
            //playerBody.forward = moveDir; // ĳ���Ͱ� �̵��� �� �̵� ������ �ٶ󺸰� �ϱ� 
            animator.SetFloat(moveAnimParam, 0.5f);
            if (playerManager == null)
            {
                transform.position += moveDir * Time.deltaTime * 8f;
            }
            else
            {
                transform.position += moveDir * Time.deltaTime * playerManager.speed;
            }
        }
        else
        {
            animator.SetFloat(moveAnimParam, 0);
        }
    }

    /// <summary>
    /// ���� Ȯ�� ��� �Լ�
    /// </summary>
    private void Jump()
    {
        if (Input.GetKeyDown(jumpKeyCode) && !isJump)
        {
            jumpEvent?.Invoke();

            isJump = true;

            animator.SetBool(jumpAnimParam, isJump);

            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            isJump = false;
            animator.SetBool(jumpAnimParam, isJump);
        }
    }
}