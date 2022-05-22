using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InstantiateGun : MonoBehaviour
{
    public GameObject ak74;
    public GameObject m1911;
    public XRController rightController;
    public XRController leftController;
    public Transform createGunPos;

    private bool isRightControllerPressed = false;
    private bool wasRightControllerPressed = false;
    private bool isLeftControllerPressed = false;
    private bool wasLeftControllerPressed = false;

    Coroutine ak74Coroutine;
    Coroutine m1911Coroutine;

    private void Start()
    {
        ak74.transform.Find("Baked_Mesh").GetComponent<MeshRenderer>().enabled = false;
        m1911.SetActive(false);
    }

    void Update()
    {
        rightController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isRightControllerPressed);
        leftController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isLeftControllerPressed);

        // 오른손 A버튼, 왼손 X버튼 동시에 눌렀을 때는 아무런 동작 X
        if(isRightControllerPressed && isLeftControllerPressed)
        {
            return;
        }

        /// 오른손 A버튼을 누르는 순간 기존에 생성되어 있던 총을 파괴하고 AK74를 생성한다.
        /// 또한, 기존에 진행중이던 코루틴을 모두 Stop한다.
        /// AK74의 Rigidbody의 Kinematic을 true로 바꿔준다.
        /// 코루틴 함수를 이용해 3초 뒤 해당 총의 Kinematic을 나타내는 bool 변수를 false로 변경해 중력을 적용받도록 한다.
        else if(isRightControllerPressed && !wasRightControllerPressed)
        {
            
            if (ak74.transform.Find("Baked_Mesh").GetComponent<MeshRenderer>().enabled == true)
            {
                ak74.transform.Find("Baked_Mesh").GetComponent<MeshRenderer>().enabled = false;
                ak74.GetComponent<TwoHandGrabInteractable>().enabled = false;
                if(ak74Coroutine != null)
                {
                    StopCoroutine(ak74Coroutine);
                }
            }
            else if(m1911.activeSelf == true)
            {
                m1911.SetActive(false);
                if (m1911Coroutine != null)
                {
                    StopCoroutine(m1911Coroutine);
                }
            }

            else
            {
                ak74.transform.position = createGunPos.position;
                ak74.transform.rotation = createGunPos.rotation;
                ak74.GetComponent<Rigidbody>().isKinematic = true;
                ak74.transform.Find("Baked_Mesh").GetComponent<MeshRenderer>().enabled = true;
                ak74.GetComponent<TwoHandGrabInteractable>().enabled = true;
                ak74Coroutine = StartCoroutine(SetGunKinematic(ak74));
            }
            
        }

        /// 왼손 X버튼을 누르는 순간 기존에 생성되어 있던 총을 파괴하고 M1911을 생성한다.
        /// M1911의 Rigidbody의 Kinematic을 true로 바꿔준다.
        /// 코루틴 함수를 이용해 3초 뒤 해당 총의 Kinematic을 나타내는 bool 변수를 false로 변경해 중력을 적용받도록 한다.
        else if (isLeftControllerPressed && !wasLeftControllerPressed)
        {
            if (ak74.transform.Find("Baked_Mesh").GetComponent<MeshRenderer>().enabled == true)
            {
                ak74.transform.Find("Baked_Mesh").GetComponent<MeshRenderer>().enabled = false;
                ak74.GetComponent<TwoHandGrabInteractable>().enabled = false;
                if (ak74Coroutine != null)
                {
                    StopCoroutine(ak74Coroutine);
                }
            }
            else if (m1911.activeSelf == true)
            {
                m1911.SetActive(false);
                if (m1911Coroutine != null)
                {
                    StopCoroutine(m1911Coroutine);
                }
            }

            else
            {
                m1911.transform.position = createGunPos.position;
                m1911.transform.rotation = createGunPos.rotation;
                m1911.GetComponent<Rigidbody>().isKinematic = true;
                m1911.SetActive(true);
                m1911Coroutine = StartCoroutine(SetGunKinematic(m1911));
            }
            
        }

        wasRightControllerPressed = isRightControllerPressed;
        wasLeftControllerPressed = isLeftControllerPressed;
    }

    IEnumerator SetGunKinematic(GameObject gun)
    {
        yield return new WaitForSeconds(2f);
        if(gun.GetComponent<Rigidbody>().isKinematic == true)
        {
            gun.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
