using UnityEngine;
using UnityEngine.XR;

public class PlatformSetting : MonoBehaviour
{
    [SerializeField] private GameObject PC;
    [SerializeField] private GameObject VR;

    public static bool isVR = XRSettings.isDeviceActive;

    private void Awake()
    {
        PC.SetActive(!isVR);
        VR.SetActive(isVR);
    }
}