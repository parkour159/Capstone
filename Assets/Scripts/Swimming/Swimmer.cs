using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Swimmer : MonoBehaviour
{
	[Header("Values")]
	[SerializeField] float swimForce = 2f;
	/* Swim Force
     * 얼마나 빨리 수영할 것인가
     * 팔로 젓는 힘을 나타내는 변수
     */
	[SerializeField] float dragForce = 1f;
	/* Drag Force
     * 물의 저항력
     * 우리가 수영을 멈추고 가만히 있으면
     * 점점 느려지다가 멈추거나, 약간의 속도로 표류돼야 함.
     */
	[SerializeField] float minForce;
	[SerializeField] float minTimeBetweenStrokes;
	/* 게임 플레이를 위한 약간의 제한사항
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

		// 너무 빠르게 연속해서 젓지 못하도록 젓는 사이에 최소의 텀을 둠.
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