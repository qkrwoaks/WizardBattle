using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCam;              //���� VR �۾��� ī�޶� ���� ã����� �� + ����ȭ �۾�

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