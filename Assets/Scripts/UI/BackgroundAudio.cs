using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class BackgroundAudio : MonoBehaviour
{
    public AudioSource audioSound;

    public Transform playerCamera;
    public Transform setUIPos;
    public GameObject rightRay;
    public GameObject leftRay;
    public XRController leftController;

    private bool isPressed;
    private bool wasPressed;

    private void Start()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        leftController.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out isPressed);
        if(isPressed && !wasPressed)
        {
            if(gameObject.GetComponent<Canvas>().enabled == true)
            {
                gameObject.GetComponent<Canvas>().enabled = false;
                rightRay.SetActive(false);
                leftRay.SetActive(false);
            }
            else
            {
                gameObject.transform.position = setUIPos.position;
                gameObject.transform.LookAt(playerCamera);
                gameObject.GetComponent<Canvas>().enabled = true;
                rightRay.SetActive(true);
                leftRay.SetActive(true);
            }
        }

        wasPressed = isPressed;
    }

    public void Audio_Bar(float value)
    {
        audioSound.volume = value;
    }
}
