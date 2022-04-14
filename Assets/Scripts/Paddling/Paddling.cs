using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddling : MonoBehaviour
{
    [HideInInspector] public bool isTriggered = false;
    [HideInInspector] public Vector3 oarTriggerEnterPos;
    [HideInInspector] public Vector3 oarTriggerExitPos;
    [HideInInspector] public Vector3 _currentBoatPos;
    [HideInInspector] public Vector3 _previousBoatPos;

    public float paddlingForce = 5f;
    public float reversePaddlingForce = 1.5f;
    public float dragForce = 0.7f;
    public float rightRotationForce = 0.7f;
    public float leftRotationForce = -0.7f;
    public float minTermBetweentPaddling = 0.2f;

    public Transform paddle;
    public Transform player;

    Rigidbody _rigidbody;
    Vector3 _boatPosDist;
    Vector3 _oarDir;
    float oarTriggerStayTime = 0f;
    float boatMinVelocity = 0.05f;
    float boatMinAngularVelocity = 0.1f;
    float totalPaddlingForce = 0f;
    float totalRotationForce = 0f;
    float coolDownTimer = 0f;

    float twoHandGrabTime = 0f;

    private TwoHandGrabInteractable twoHandGrab;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        twoHandGrab = GameObject.Find("Oar").GetComponent<TwoHandGrabInteractable>();
    }

    private void Start()
    {
        _previousBoatPos = transform.position;
    }

    private void Update()
    {
        if (isTriggered)
        {
            oarTriggerStayTime += Time.deltaTime;
        }
        if (twoHandGrab.secondInteractor)
        {
            twoHandGrabTime += Time.deltaTime;
        }
        else
        {
            twoHandGrabTime = 0f;
        }
        if (_rigidbody.velocity.sqrMagnitude > boatMinVelocity * boatMinVelocity)
        {
            player.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
        // ��Ʈ�� ������ ��ȭ�� ����� �÷��̾��� rotation�� y��� ��Ʈ�� rotation�� y���� ��ġ��Ű�� �ڵ�
        //if (_rigidbody.angularVelocity.sqrMagnitude > boatMinAngularVelocity * boatMinAngularVelocity)
        //{
        //    player.rotation = Quaternion.Euler(new Vector3(player.rotation.x, transform.localRotation.eulerAngles.y, player.rotation.z));
        //    Debug.Log("Rotation : " + player.rotation.eulerAngles + ", " + transform.rotation.eulerAngles);
        //}

    }

    void FixedUpdate()
    {
        coolDownTimer += Time.fixedDeltaTime;
        if(coolDownTimer > minTermBetweentPaddling && twoHandGrabTime > 0.5f) // 0.2�� �̻� ���� �ΰ� ����� ��. + �ݵ�� �븦 ������� ��� 0.5�� ���Ŀ� ����� ��.
        {
            if (_rigidbody.velocity.sqrMagnitude < boatMinVelocity * boatMinVelocity && oarTriggerStayTime > 0.001f && !isTriggered)
            {
                // ���� ����
                _oarDir = oarTriggerExitPos - oarTriggerEnterPos;


                // �븦 ��Ʈ�� ������ ���⿡��
                if (oarTriggerEnterPos.x > this.transform.InverseTransformDirection(transform.position).x)
                {
                    // �տ��� �ڷ� ���´ٸ� ��Ʈ�� �ݽð����(����)���� ���� �޴´�.
                    if (_oarDir.z < 0)
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * leftRotationForce;
                        Debug.Log("��Ʈ ������ �ݽð����");
                    }

                    // �ڿ��� ������ ���´ٸ� ��Ʈ�� �ð����(������)���� ���� �޴´�.
                    else
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * rightRotationForce;
                        Debug.Log("��Ʈ ������ �ð����");
                    }
                }

                // �븦 ��Ʈ�� ���� ���⿡�� ���´ٸ�
                else
                {
                    // �տ��� �ڷ� ���´ٸ� ��Ʈ�� �ð����(������)���� ���� �޴´�.
                    if (_oarDir.z < 0)
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * rightRotationForce;
                        Debug.Log("��Ʈ ���� �ð����");
                    }

                    // �ڿ��� ������ ���´ٸ� ��Ʈ�� �ݽð����(����)���� ���� �޴´�.
                    else
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * leftRotationForce;
                        Debug.Log("��Ʈ ���� �ݽð����");
                    }
                }

                _oarDir = new Vector3(0, 0, _oarDir.z);

                // �� ���� �� �ð� �ʱ�ȭ
                totalPaddlingForce = _oarDir.z + 1 / oarTriggerStayTime * paddlingForce;
                oarTriggerStayTime = 0f;

                // Local �������� ���� ���Ѵ�.
                _rigidbody.AddRelativeForce(-_oarDir * totalPaddlingForce, ForceMode.Acceleration);
                _rigidbody.AddRelativeTorque(transform.up * totalRotationForce, ForceMode.Acceleration);
                //Debug.Log("���� : " + _rigidbody.velocity);
            }

            else if (_rigidbody.velocity.sqrMagnitude > boatMinVelocity * boatMinVelocity && oarTriggerStayTime > 0.001f && !isTriggered)
            {
                // ��Ʈ�� �̵��� ����� ���� ��ġ ����
                _boatPosDist = new Vector3(0, 0, _currentBoatPos.z - _previousBoatPos.z);
                oarTriggerExitPos = oarTriggerExitPos - _boatPosDist;

                _oarDir = oarTriggerExitPos - oarTriggerEnterPos;

                // �븦 ��Ʈ�� ������ ���⿡��
                if (oarTriggerEnterPos.x > _previousBoatPos.x)
                {
                    Debug.Log("���� ��ġ : " + oarTriggerEnterPos.x + " ���� ��ġ : " + _currentBoatPos.x);
                    // �տ��� �ڷ� ���´ٸ� ��Ʈ�� �ݽð����(����)���� ���� �޴´�.
                    if (_oarDir.z < 0)
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * leftRotationForce;
                        Debug.Log("���� ���� ��Ʈ ������ �ݽð����");
                    }

                    // �ڿ��� �շ� ���´ٸ� ��Ʈ�� �ð����(������)���� ���� �޴´�.
                    else
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * rightRotationForce;
                        Debug.Log("���� ���� ��Ʈ ������ �ð����");
                    }
                }

                // �븦 ��Ʈ�� ���� ���⿡�� ���´ٸ�
                else
                {
                    // �տ��� �ڷ� ���´ٸ� ��Ʈ�� �ð����(������)���� ���� �޴´�.
                    if (_oarDir.z < 0)
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * rightRotationForce;
                        Debug.Log("���� ���� ��Ʈ ���� �ð����");
                    }

                    // �ڿ��� �շ� ���´ٸ� ��Ʈ�� �ݽð����(����)���� ���� �޴´�.
                    else
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * leftRotationForce;
                        Debug.Log("���� ���� ��Ʈ ���� �ݽð����");
                    }
                }

                _oarDir = new Vector3(0, 0, _oarDir.z);

                if (_boatPosDist.z * -_oarDir.z > 0)
                {
                    totalPaddlingForce = _oarDir.z + 1 / oarTriggerStayTime * paddlingForce;
                    oarTriggerStayTime = 0f;
                    _rigidbody.AddRelativeForce(-_oarDir * totalPaddlingForce, ForceMode.Acceleration);
                    //Debug.Log("���� : " + _rigidbody.velocity);
                }

                else
                {
                    // ��Ʈ �̵� ����� ���� ������ �ݴ��� ������ ������ ���� ���� �־ ���ݾ� �����ϰ� �����.
                    totalPaddlingForce = _oarDir.z + 1 / oarTriggerStayTime * reversePaddlingForce;
                    oarTriggerStayTime = 0f;
                    _rigidbody.AddRelativeForce(-_oarDir * totalPaddlingForce, ForceMode.Acceleration);
                    //Debug.Log("���� : " + _rigidbody.velocity);
                }

                _rigidbody.AddRelativeTorque(transform.up * totalRotationForce, ForceMode.Acceleration);
            }

            else if (_rigidbody.velocity.sqrMagnitude > boatMinVelocity * boatMinVelocity && oarTriggerStayTime > 2f && isTriggered)
            {
                _rigidbody.AddRelativeForce(-_rigidbody.velocity * dragForce, ForceMode.Acceleration);

                if (oarTriggerEnterPos.x > _previousBoatPos.x)
                {
                    _rigidbody.AddRelativeTorque(transform.up * rightRotationForce * 0.5f, ForceMode.Acceleration);
                }
                else
                {
                    _rigidbody.AddRelativeTorque(transform.up * leftRotationForce * 0.5f, ForceMode.Acceleration);
                }
                Debug.Log("�극��ũ : " + _rigidbody.velocity);
            }
            coolDownTimer = 0f;
        }
    }
       
}
