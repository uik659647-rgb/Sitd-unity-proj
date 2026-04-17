using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;

public enum HandTypeLeft
{
    Left
}

public class XRHandL : MonoBehaviour
{
    public HandTypeLeft handType;

    private Animator animator;
    private InputDevice inputDevice;

    private float pose4Value;
    private float pose5Value;
    private float pose6Value;
    public PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        GetInputDevice();
        InvokeRepeating("GetInputDevice", 0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            AnimateHand();
        }
    }

    void GetInputDevice()
    {
        InputDeviceCharacteristics controllerCharacteristic = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller;

        if (handType == HandTypeLeft.Left)
        {
            controllerCharacteristic = controllerCharacteristic | InputDeviceCharacteristics.Left;
        }
        else
        {
            controllerCharacteristic = controllerCharacteristic | InputDeviceCharacteristics.Right;
        }

        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristic, inputDevices);

        inputDevice = inputDevices[0];
    }

    void AnimateHand()
    {
        float _pose4Value;
        float _pose5Value;
        inputDevice.TryGetFeatureValue(CommonUsages.trigger, out _pose4Value);
        inputDevice.TryGetFeatureValue(CommonUsages.grip, out _pose5Value);

        pose4Value = Mathf.Lerp(pose4Value, _pose4Value, 0.3f);
        pose5Value = Mathf.Lerp(pose5Value, _pose5Value, 0.3f);

        inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);

        if (primaryButtonValue || secondaryButtonValue){
            pose6Value = Mathf.Lerp(pose6Value, 1, 0.3f);
        }
        else{
            pose6Value = Mathf.Lerp(pose6Value, 0, 0.3f);
        }

        animator.SetFloat("pose4", pose4Value);
        animator.SetFloat("pose5", pose5Value);
        animator.SetFloat("pose6", pose6Value);
    }
}