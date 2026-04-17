using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;

public enum HandType
{
    Left,
    Right
}

public class XRHandController : MonoBehaviour
{
    public HandType handType;

    private Animator animator;
    private InputDevice inputDevice;

    private float pose1Value;
    private float pose2Value;
    private float pose3Value;

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

        if (handType == HandType.Left)
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
        float _pose1Value;
        float _pose2Value;
        inputDevice.TryGetFeatureValue(CommonUsages.trigger, out _pose1Value);
        inputDevice.TryGetFeatureValue(CommonUsages.grip, out _pose2Value);

        pose1Value = Mathf.Lerp(pose1Value, _pose1Value, 0.3f);
        pose2Value = Mathf.Lerp(pose2Value, _pose2Value, 0.3f);
        

        inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);

        if (primaryButtonValue || secondaryButtonValue){
            pose3Value = Mathf.Lerp(pose3Value, 1, 0.3f);
        }
        else{
            pose3Value = Mathf.Lerp(pose3Value, 0, 0.3f);
        }

        animator.SetFloat("pose1", pose1Value);
        animator.SetFloat("pose2", pose2Value);
        animator.SetFloat("pose3", pose3Value);
    }
}