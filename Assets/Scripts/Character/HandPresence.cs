using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice targetDevice;
    public GameObject handModelPrefabs;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    private void Start()
    {
        TryInitialize();
    }
    void TryInitialize()
    {
        // �Է���ġ�� �� controllerCharacteristics�� �ش�Ǵ� �Է���ġ�� ã�� devices ����Ʈ�� �߰���.
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log("item name : " + item.name + ", item characteristics : " + item.characteristics);
        }
        if (devices.Count > 0)
        {
            targetDevice = devices[0];

            spawnedHandModel = Instantiate(handModelPrefabs, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    void UpdateHandAnimation()
    {
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }

    }

    void Update()
    {
        if (!targetDevice.isValid)
            TryInitialize();
        else
            UpdateHandAnimation();
    }
}
