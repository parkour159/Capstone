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
        // inputSource�� �Ҵ�� XRNode(Left Hand)�� device ������ �޾ƿ�.
        // device ������ �Ҵ�� InputDevice�� ���̽�ƽ ���� �޾Ƽ� inputAxis ������ ������.
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();

        // VR Camera Rotation�� y���� �޾ƿ�.
        Quaternion headYaw = Quaternion.Euler(0, origin.Camera.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        // ĳ���Ͱ� �ٶ󺸴� ������ �������� �����ؼ� �̵��ϵ��� ��.
        // ĳ������ �̵��� �������� �����̹Ƿ� Time.fixedDeltaTime�� �����.
        rigid.velocity = direction.normalized * Time.fixedDeltaTime * speed;
        
        //ĳ���Ͱ� �ٴڿ� ����ִ� �������� üũ�Ͽ� �߷��� ������
        bool isBoat = CheckIfGrounded();
        if (isBoat)
            rigid.useGravity = false;
        else
            rigid.useGravity = true;
        //character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    /* ĳ������ ���̸� VR Camera�� ���̿� ��ġ��Ŵ. 
     * Controller�� ���̽�ƽ �Է� ���� VR Camera�� �̵��ϴ��� ĳ����(Character Controller)�� �Բ� �̵���. */
    void CapsuleFollowHeadset()
    {
        character.height = origin.CameraInOriginSpaceHeight + additionalHeight; // VR Camera�� ���̿� ĳ������ ���� ����ȭ
        Vector3 capsuleCenter = transform.InverseTransformPoint(origin.Camera.transform.position); // VR Camera�� World ��ǥ�� Local ��ǥ�� ��ȯ�Ͽ� ������.
        character.center = new Vector3(capsuleCenter.x, character.height / 2, capsuleCenter.z); // VR Camera�� ���̿� ��ġ ��ǥ�� �̿��ؼ� ĳ������ Center�� �缳����.
    }

    // ĳ���Ͱ� ���� �ٴڿ� ����ִ��� Ȯ���ϴ� �Լ�
    bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(character.center); // ĳ���� Center�� Local ��ǥ�� World ��ǥ�� ��ȯ�Ͽ� ������.
        float rayLength = character.center.y + 0.01f; // ĳ���� Center�� ���̺��� �ణ �� ��� Raycast�� ���̸� ������.

        // ĳ������ Radius�� ���� �β��� Raycast�� �߻���.
        // �浹�� �Ǹ� True�� ��ȯ�ϸ� Raycast�� �浹�� ������Ʈ���� �Ÿ�, ��ġ ���� ������ hitInfo ������ ������.
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
