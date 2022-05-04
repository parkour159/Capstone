using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SetOarPos : MonoBehaviour
{
    public XRController rightHand;
    public Transform oarPos;
    private bool isPressed = false;
    private bool wasPressed = false;
    private bool isKinematice = true;
    private Rigidbody rigid;

    private void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rightHand.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isPressed);

        if(isPressed && !wasPressed)
        {
            transform.position = oarPos.position;
            transform.rotation = oarPos.rotation;
            rigid.isKinematic = true;
            isKinematice = true;
            Debug.Log(rigid.isKinematic);
        }

        if(isKinematice == false)
        {
            rigid.isKinematic = false;
        }

        wasPressed = isPressed;
    }

    public void UnSetKinematic()
    {
        if(isKinematice == true)
        {
            isKinematice = false;
        }
    }
}
