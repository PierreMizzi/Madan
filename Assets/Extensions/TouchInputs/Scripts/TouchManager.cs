using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;


public class TouchManager : MonoBehaviour
{
	#region MonoBehaiour


	private void Awake()
	{
		TouchSimulation.Enable();

		m_mainCamera = Camera.main;

		// Taps
		if (m_primaryTapAction != null)
		{
			m_primaryTapAction.action.Enable();
			m_primaryTapAction.action.performed += CallbackPrimaryTap;
		}

		// Swipes
		if (m_primaryPressAction != null)
		{
			m_primaryPressAction.action.Enable();
			m_primaryPressAction.action.started += CallbackPrimaryPressStarted;
			m_primaryPressAction.action.canceled += CallbackPrimaryPressCanceled;
			// m_primaryPressAction.action.performed += CallbackPrimaryPress;
		}

		// foreach (var touch in Touch.activeTouches)
		// {
		// 	Debug.Log($"message : {touch.phase}");
		// }
	}

	void OnEnable()
	{
		EnhancedTouchSupport.Enable();
	}



	public void Update()
	{
		foreach (var touch in Touch.activeTouches)
		{
			// Debug.Log($"{touch.touchId}: {touch.screenPosition},{touch.phase}");
		}

		if (m_hasPrimaryPressStarted)
		{
			UpdatePrimaryPress();
		}

	}

	#endregion

	#region Behaviour

	[Header("Behaviour")]
	private Camera m_mainCamera;

	#endregion

	#region Taps

	[Header("Taps")]
	[SerializeField] private InputActionReference m_primaryTapAction;

	private void CallbackPrimaryTap(InputAction.CallbackContext context)
	{
		if (m_mainCamera == null)
		{
			m_mainCamera = Camera.main;
		}

		if (m_mainCamera == null)
		{
			return;
		}

		if (Touch.activeTouches.Count == 0)
		{
			return;
		}

		Touch primaryTouch = Touch.activeTouches[0];

		Ray ray = m_mainCamera.ScreenPointToRay(primaryTouch.screenPosition);

		if (Physics.Raycast(ray, out RaycastHit hitInfo))
		{
			if (hitInfo.collider.TryGetComponent(out ITappable tappable))
			{
				Debug.Log($"Hit {tappable.gameObject.name} with a TouchInteractable component");
				tappable.TriggerSingleTap(hitInfo);
			}
		}

	}


	#endregion

	#region Swipe

	[Header("Swipe")]

	[SerializeField] private InputActionReference m_primaryPressAction;

	// Duration
	[SerializeField]
	private float m_swipeDuration;
	[SerializeField] private float m_swipeDurationTreshold = 0.5f;

	[SerializeField] private float m_swipeMagnitudeThreshold = 150f; // Minimum swipe length to be considered a swipe

	private bool m_hasPrimaryPressStarted = false;

	private TouchControl pTouch => Touchscreen.current.primaryTouch;
	

	private void CallbackPrimaryPressStarted(InputAction.CallbackContext context)
	{
		m_swipeDuration = 0f;
		m_hasPrimaryPressStarted = true;
	}

	private void UpdatePrimaryPress()
	{

		if (m_swipeDuration > m_swipeDurationTreshold)
		{
			return;
		}

		TouchPhase phase = pTouch.phase.ReadValue();
		if (phase == TouchPhase.Moved || phase == TouchPhase.Stationary)
		{
			m_swipeDuration += Time.deltaTime;
		}
	}

	private void CallbackPrimaryPressCanceled(InputAction.CallbackContext context)
	{
		Vector2 swipeVector = pTouch.position.ReadValue() - pTouch.startPosition.ReadValue();
		if (m_swipeDuration < m_swipeDurationTreshold && swipeVector.magnitude > m_swipeMagnitudeThreshold)
		{
			ResolveSwipe(pTouch, swipeVector);
		}

		m_hasPrimaryPressStarted = false;
	}

	private void ResolveSwipe(TouchControl touchControl, Vector2 swipeDirection)
	{
		Ray ray = m_mainCamera.ScreenPointToRay(touchControl.startPosition.ReadValue());

		if (Physics.Raycast(ray, out RaycastHit hitInfo))
		{
			if (hitInfo.collider.TryGetComponent(out ISwippable swippable))
			{
				swippable.TriggerSwipe(hitInfo, swipeDirection);
			}
		}

	}

	#endregion

	#region Holds

	[SerializeField] private InputActionReference m_primaryHoldAction;

	private void CallbackPrimaryHold(InputAction.CallbackContext context)
	{
		throw new NotImplementedException();
	}

	#endregion
}