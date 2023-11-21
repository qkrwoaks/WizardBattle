using UnityEngine;
using UnityEngine.XR;

public class PlatformSetting : MonoBehaviour
{
    [SerializeField] private GameObject PC;
    [SerializeField] private GameObject VR;

    private void Awake()
    {
        bool isVR = PCVRSetting.isVR;

        PC.SetActive(!isVR);
        VR.SetActive(isVR);
    }
}

public class PCVRSetting
{
    public static bool isVR = XRSettings.isDeviceActive;
}