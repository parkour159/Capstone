using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandGrabInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    public Vector3 lookRotationOffset;
    private Quaternion attachInitialRotation;
    [HideInInspector] public XRBaseInteractor secondInteractor;
    public enum TwoHandRotationType { None, First, Second};
    public TwoHandRotationType twoHandRotationType;
    // 밑에 2개 들어간 코드들은 두번째 손 Pivot 없애도록 해주는 변수 및 코드들임.
    // 테스트 확인 후 지울지 판단하자.
    public bool snapToSecondHand = true;
    private Quaternion initialRotationOffset;

    void Start()
    {
        foreach (var item in secondHandGrabPoints)
        {
            item.onSelectEntered.AddListener(OnSecondHandGrab);
            item.onSelectExited.AddListener(OnSecondHandRelease);
        }
    }

    void Update()
    {
        
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(secondInteractor && selectingInteractor)
        {   
            //Compute the rotation
            if (snapToSecondHand)
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation();
            else
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation() * initialRotationOffset;

            //selectingInteractor.attachTransform.rotation = GetTwoHandRotation();
        }
        base.ProcessInteractable(updatePhase);
    }

    private Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation;
        if(twoHandRotationType == TwoHandRotationType.None)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position) * Quaternion.Euler(lookRotationOffset);
        }
        else if(twoHandRotationType == TwoHandRotationType.First)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.transform.up) * Quaternion.Euler(lookRotationOffset);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, secondInteractor.attachTransform.up) * Quaternion.Euler(lookRotationOffset);
        }

        return targetRotation;
    }

    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        Debug.Log("SECOND HAND GRAB");
        secondInteractor = interactor;
        initialRotationOffset = Quaternion.Inverse(GetTwoHandRotation()) * selectingInteractor.attachTransform.rotation;
    }

    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        Debug.Log("SECOND HAND RELEASE");
        secondInteractor = null;
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        Debug.Log("FIRST HAND GRAB");
        base.OnSelectEntered(interactor);
        attachInitialRotation = interactor.attachTransform.localRotation;
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        Debug.Log("FIRST HAND RELEASE");
        base.OnSelectExited(interactor);
        secondInteractor = null;
        interactor.attachTransform.localRotation = attachInitialRotation;
    }

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool isalreadygrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor) && !isalreadygrabbed;
    }
}
