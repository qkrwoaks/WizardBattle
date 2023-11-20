using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class FPSCharacterManipulation : MonoBehaviourPunCallbacks
{
    private PhotonView PV;
    [SerializeField] private KeyCode jumpKeyCode;  // 점프 키 코드

    [SerializeField] private Transform playerBody; // 플레이어 모델을 관리할 변수
    [SerializeField] private Transform cameraArm;  // 카메라 암 회전을 관리할 변수

    [SerializeField] private PlayerManager playerManager;

    [SerializeField] private float jumpForce;      // 점프력

    [SerializeField] private float sensitivity;    // 마우스 감도

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
            // 내가 아니라면 카메라 끄기
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
    /// 카메라의 회전을 담당하는 함수
    /// </summary>
    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // 프로그래밍에서 이전값과 현재값의 차를 주로 "Delta"라는 용어로 표현함
        Vector3 camAngle = cameraArm.rotation.eulerAngles; // 카메라의 암의 Rotation 값을 오일러 각으로 변환
        float x = camAngle.x - mouseDelta.y;

        if (x < 180f) // 플레이어의 상하 움직임 제한
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x * sensitivity, camAngle.z); // 카메라의 Rotation 값 변환
        playerBody.transform.eulerAngles = new Vector3(0, camAngle.y, 0);                              // 보는 방향으로 변경 
    }

    /// <summary>
    /// 움직임을 담당하는 함수
    /// </summary>
    private void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // 이동 입력 값을 가져옴
        bool isMove = moveInput.magnitude != 0;
        if (isMove) // 움직이고 있을 때 실행되는 함수
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized; // 카메라의 정면을 평면화 시켜 저장
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized; // 카메라의 오른쪽을 평면화 시켜 저장
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x; // 카메라를 통해 이동 방향 저장

            walkEvent?.Invoke();
            //playerBody.forward = moveDir; // 캐릭터가 이동할 때 이동 방향을 바라보게 하기 
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
    /// 점프 확인 담당 함수
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