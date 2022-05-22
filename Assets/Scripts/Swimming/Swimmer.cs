using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Swimmer : MonoBehaviour
{
	[Header("Values")]
	[SerializeField] float swimForce = 2f;
	/* Swim Force
     * �󸶳� ���� ������ ���ΰ�
     * �ȷ� ���� ���� ��Ÿ���� ����
     */
	[SerializeField] float dragForce = 1f;
	/* Drag Force
     * ���� ���׷�
     * �츮�� ������ ���߰� ������ ������
     * ���� �������ٰ� ���߰ų�, �ణ�� �ӵ��� ǥ���ž� ��.
     */
	[SerializeField] float minForce;
	[SerializeField] float minTimeBetweenStrokes;
	/* ���� �÷��̸� ���� �ణ�� ���ѻ���
     * Minimum Force
     * Time Between Strokes
     */
	[SerializeField] private float gravityScale; // 0.02

	private const float globalGravity = -9.81f;

	[SerializeField] XRController leftController;
	[SerializeField] XRController rightController;
	[SerializeField] Transform trackingReference;

	Rigidbody _rigidbody;
	float _cooldownTimer;
	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.useGravity = false;
		_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
	}

	void FixedUpdate()
	{
		_cooldownTimer += Time.fixedDeltaTime;

		bool leftGrip = false;
		bool rightGrip = false;

		// �ʹ� ������ �����ؼ� ���� ���ϵ��� ���� ���̿� �ּ��� ���� ��.
		if (_cooldownTimer > minTimeBetweenStrokes)
        {
			if(leftController.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out leftGrip) && leftGrip
			&& rightController.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out rightGrip) && rightGrip)
			{
				InputDevices.GetDeviceAtXRNode(leftController.controllerNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 leftHandVelocity);
				InputDevices.GetDeviceAtXRNode(rightController.controllerNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 rightHandVelocity);
				Vector3 localVelocity = leftHandVelocity + rightHandVelocity;
				localVelocity *= -1;
				if (localVelocity.sqrMagnitude > minForce * minForce)
				{
					Vector3 worldVelocity = trackingReference.TransformDirection(localVelocity);
					_rigidbody.AddForce(worldVelocity * swimForce, ForceMode.Acceleration);
					_cooldownTimer = 0f;
				}
			}
		}

		if (_rigidbody.velocity.sqrMagnitude > 0.01f)
		{
			_rigidbody.AddForce(-_rigidbody.velocity * dragForce, ForceMode.Acceleration);
		}
        else
        {
			Vector3 gravity = globalGravity * gravityScale * Vector3.up;
			_rigidbody.AddForce(gravity, ForceMode.Acceleration);
		}
	}
}