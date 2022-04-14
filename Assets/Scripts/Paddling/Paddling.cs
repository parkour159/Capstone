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
        // 보트의 각도에 변화가 생기면 플레이어의 rotation의 y축과 보트의 rotation의 y축을 일치시키는 코드
        //if (_rigidbody.angularVelocity.sqrMagnitude > boatMinAngularVelocity * boatMinAngularVelocity)
        //{
        //    player.rotation = Quaternion.Euler(new Vector3(player.rotation.x, transform.localRotation.eulerAngles.y, player.rotation.z));
        //    Debug.Log("Rotation : " + player.rotation.eulerAngles + ", " + transform.rotation.eulerAngles);
        //}

    }

    void FixedUpdate()
    {
        coolDownTimer += Time.fixedDeltaTime;
        if(coolDownTimer > minTermBetweentPaddling && twoHandGrabTime > 0.5f) // 0.2초 이상 텀을 두고 저어야 함. + 반드시 노를 양손으로 잡고 0.5초 이후에 저어야 함.
        {
            if (_rigidbody.velocity.sqrMagnitude < boatMinVelocity * boatMinVelocity && oarTriggerStayTime > 0.001f && !isTriggered)
            {
                // 방향 설정
                _oarDir = oarTriggerExitPos - oarTriggerEnterPos;


                // 노를 보트의 오른쪽 방향에서
                if (oarTriggerEnterPos.x > this.transform.InverseTransformDirection(transform.position).x)
                {
                    // 앞에서 뒤로 젓는다면 보트는 반시계방향(왼쪽)으로 힘을 받는다.
                    if (_oarDir.z < 0)
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * leftRotationForce;
                        Debug.Log("보트 오른쪽 반시계방향");
                    }

                    // 뒤에서 앞으로 젓는다면 보트는 시계방향(오른쪽)으로 힘을 받는다.
                    else
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * rightRotationForce;
                        Debug.Log("보트 오른쪽 시계방향");
                    }
                }

                // 노를 보트의 왼쪽 방향에서 젓는다면
                else
                {
                    // 앞에서 뒤로 젓는다면 보트는 시계방향(오른쪽)으로 힘을 받는다.
                    if (_oarDir.z < 0)
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * rightRotationForce;
                        Debug.Log("보트 왼쪽 시계방향");
                    }

                    // 뒤에서 앞으로 젓는다면 보트는 반시계방향(왼쪽)으로 힘을 받는다.
                    else
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * leftRotationForce;
                        Debug.Log("보트 왼쪽 반시계방향");
                    }
                }

                _oarDir = new Vector3(0, 0, _oarDir.z);

                // 힘 설정 및 시간 초기화
                totalPaddlingForce = _oarDir.z + 1 / oarTriggerStayTime * paddlingForce;
                oarTriggerStayTime = 0f;

                // Local 방향으로 힘을 가한다.
                _rigidbody.AddRelativeForce(-_oarDir * totalPaddlingForce, ForceMode.Acceleration);
                _rigidbody.AddRelativeTorque(transform.up * totalRotationForce, ForceMode.Acceleration);
                //Debug.Log("시작 : " + _rigidbody.velocity);
            }

            else if (_rigidbody.velocity.sqrMagnitude > boatMinVelocity * boatMinVelocity && oarTriggerStayTime > 0.001f && !isTriggered)
            {
                // 보트의 이동을 고려한 노의 위치 설정
                _boatPosDist = new Vector3(0, 0, _currentBoatPos.z - _previousBoatPos.z);
                oarTriggerExitPos = oarTriggerExitPos - _boatPosDist;

                _oarDir = oarTriggerExitPos - oarTriggerEnterPos;

                // 노를 보트의 오른쪽 방향에서
                if (oarTriggerEnterPos.x > _previousBoatPos.x)
                {
                    Debug.Log("노의 위치 : " + oarTriggerEnterPos.x + " 배의 위치 : " + _currentBoatPos.x);
                    // 앞에서 뒤로 젓는다면 보트는 반시계방향(왼쪽)으로 힘을 받는다.
                    if (_oarDir.z < 0)
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * leftRotationForce;
                        Debug.Log("진행 중인 보트 오른쪽 반시계방향");
                    }

                    // 뒤에서 앞로 젓는다면 보트는 시계방향(오른쪽)으로 힘을 받는다.
                    else
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * rightRotationForce;
                        Debug.Log("진행 중인 보트 오른쪽 시계방향");
                    }
                }

                // 노를 보트의 왼쪽 방향에서 젓는다면
                else
                {
                    // 앞에서 뒤로 젓는다면 보트는 시계방향(오른쪽)으로 힘을 받는다.
                    if (_oarDir.z < 0)
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * rightRotationForce;
                        Debug.Log("진행 중인 보트 왼쪽 시계방향");
                    }

                    // 뒤에서 앞로 젓는다면 보트는 반시계방향(왼쪽)으로 힘을 받는다.
                    else
                    {
                        totalRotationForce = _oarDir.x * 0.6f + _oarDir.z * 0.4f + 1 / oarTriggerStayTime * leftRotationForce;
                        Debug.Log("진행 중인 보트 왼쪽 반시계방향");
                    }
                }

                _oarDir = new Vector3(0, 0, _oarDir.z);

                if (_boatPosDist.z * -_oarDir.z > 0)
                {
                    totalPaddlingForce = _oarDir.z + 1 / oarTriggerStayTime * paddlingForce;
                    oarTriggerStayTime = 0f;
                    _rigidbody.AddRelativeForce(-_oarDir * totalPaddlingForce, ForceMode.Acceleration);
                    //Debug.Log("가속 : " + _rigidbody.velocity);
                }

                else
                {
                    // 보트 이동 방향과 노의 방향이 반대라면 기존의 힘보다 약한 힘을 주어서 조금씩 감속하게 만든다.
                    totalPaddlingForce = _oarDir.z + 1 / oarTriggerStayTime * reversePaddlingForce;
                    oarTriggerStayTime = 0f;
                    _rigidbody.AddRelativeForce(-_oarDir * totalPaddlingForce, ForceMode.Acceleration);
                    //Debug.Log("감속 : " + _rigidbody.velocity);
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
                Debug.Log("브레이크 : " + _rigidbody.velocity);
            }
            coolDownTimer = 0f;
        }
    }
       
}
