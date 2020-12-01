using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    [SerializeField] private bool _showController = false;
    [SerializeField] private InputDeviceCharacteristics _controllerCharacteristics;
    [SerializeField] private List<GameObject> _controllerPrefabs;
    [SerializeField] private GameObject _handModelPrefab;

    private InputDevice _targetDevice;
    private GameObject _spawnedController;
    private GameObject _spawnedHandModel;
    private Animator _handAnimator;

    void Start()
    {
        TryInitialize();
    }

    void Update()
    {
        if (!_targetDevice.isValid)
        {
            TryInitialize();
            return;
        }

        if(!_showController)
            UpdateHandAnimations();
    }

    private void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(_controllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            _targetDevice = devices[0];
            GameObject controllerPrefab = _controllerPrefabs.Find(controller => controller.name == _targetDevice.name);
            if (controllerPrefab)
            {
                _spawnedController = Instantiate(controllerPrefab, transform);
            }
            else
            {
                Debug.LogError($"Could not find a correct controller model for controller device named:{_targetDevice.name}");
            }

            _spawnedHandModel = Instantiate(_handModelPrefab, transform);
            _handAnimator = _spawnedHandModel.GetComponent<Animator>();

            _spawnedController.SetActive(_showController);
            _spawnedHandModel.SetActive(!_showController);

        }
    }

    private void UpdateHandAnimations()
    {
        if (_targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            _handAnimator.SetFloat("trigger", triggerValue);
        }
        else
        {
            _handAnimator.SetFloat("trigger", 0f);
        }

        if (_targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            _handAnimator.SetFloat("grip", gripValue);
        }
        else
        {
            _handAnimator.SetFloat("grip", 0f);
        }
    }
}
