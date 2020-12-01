using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
    [SerializeField] private XRController _teleportRay;
    [SerializeField] private InputHelpers.Button _teleportActivationButton;
    [SerializeField] private float _activationThreshold = 0.1f;
    
    void Update()
    {
        if (_teleportRay)
        {
            _teleportRay.gameObject.SetActive(CheckIfActivated(_teleportRay));
        }
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, _teleportActivationButton, out bool isActivated,
            _activationThreshold);
        return isActivated;
    }
}
