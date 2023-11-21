using Photon.Pun;
using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class VRCharacterManipulation : ActionBasedContinuousMoveProvider
{
    [Header("Player")]
    [SerializeField] private Transform model;

    [Header("Camera")]
    [SerializeField] private Camera playerCamera;

    [Header("Photon")]
    [SerializeField] private PhotonView PV;

    Animator animator;

    private void Start()
    {
        animator = model.GetComponent<Animator>();

        StartCoroutine(UpdateCoroutine());

        if (!PV.IsMine)
        {
            playerCamera.enabled = false;
        }
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            switch (locomotionPhase)
            {
                case LocomotionPhase.Idle:
                    animator.SetFloat("MoveSpeed", 0f);
                    break;
                case LocomotionPhase.Moving:
                    animator.SetFloat("MoveSpeed", 0.5f);
                    break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}