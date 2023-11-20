using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCam;              //추후 VR 작업시 카메라를 따로 찾아줘야 함 + 동기화 작업

    private void Start()
    {
        mainCam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCam.rotation * Vector3.forward,
            mainCam.rotation * Vector3.up);
    }
}