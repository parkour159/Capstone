using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class ContinuousMovement : MonoBehaviour
{
    public float speed = 1;
    public float gravity = -9.81f;
    private Vector2 inputAxis;
    public XRNode inputSource;

    public float additionalHeight = 0.2f;
    private XROrigin origin;
    private CapsuleCollider character;
    private Rigidbody rigid;
    private float fallingSpeed;
    public LayerMask groundLayer;

    void Start()
    {
        character = GetComponent<CapsuleCollider>();
        origin = GetComponent<XROrigin>();
        rigid = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -15f, 0);
    }

    void Update()
    {
        // inputSource에 할당된 XRNode(Left Hand)를 device 변수에 받아옴.
        // device 변수에 할당된 InputDevice의 조이스틱 값을 받아서 inputAxis 변수에 저장함.
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();

        // VR Camera Rotation의 y값을 받아옴.
        Quaternion headYaw = Quaternion.Euler(0, origin.Camera.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        // 캐릭터가 바라보는 방향을 정면으로 설정해서 이동하도록 함.
        // 캐릭터의 이동은 물리적인 동작이므로 Time.fixedDeltaTime을 사용함.
        rigid.velocity = direction.normalized * Time.fixedDeltaTime * speed;
        
        //캐릭터가 바닥에 닿아있는 상태인지 체크하여 중력을 적용함
        bool isBoat = CheckIfGrounded();
        if (isBoat)
            rigid.useGravity = false;
        else
            rigid.useGravity = true;
        //character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    /* 캐릭터의 높이를 VR Camera의 높이와 일치시킴. 
     * Controller의 조이스틱 입력 없이 VR Camera만 이동하더라도 캐릭터(Character Controller)가 함께 이동함. */
    void CapsuleFollowHeadset()
    {
        character.height = origin.CameraInOriginSpaceHeight + additionalHeight; // VR Camera의 높이와 캐릭터의 높이 동기화
        Vector3 capsuleCenter = transform.InverseTransformPoint(origin.Camera.transform.position); // VR Camera의 World 좌표를 Local 좌표로 변환하여 저장함.
        character.center = new Vector3(capsuleCenter.x, character.height / 2, capsuleCenter.z); // VR Camera의 높이와 위치 좌표를 이용해서 캐릭터의 Center를 재설정함.
    }

    // 캐릭터가 현재 바닥에 닿아있는지 확인하는 함수
    bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(character.center); // 캐릭터 Center의 Local 좌표를 World 좌표로 변환하여 저장함.
        float rayLength = character.center.y + 0.01f; // 캐릭터 Center의 높이보다 약간 더 길게 Raycast의 길이를 설정함.

        // 캐릭터의 Radius와 같은 두께의 Raycast를 발사함.
        // 충돌이 되면 True를 반환하며 Raycast와 충돌한 오브젝트와의 거리, 위치 등의 정보를 hitInfo 변수에 저장함.
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
