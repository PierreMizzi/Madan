using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notepad : MonoBehaviour
{

	#region Behaviour

	private void CallbackStartEditing()
	{
		m_inputField.interactable = true;
		m_inputField.ActivateInputField();
		m_inputField.Select();

		m_closeButton.gameObject.SetActive(false);
		m_startEditingButton.gameObject.SetActive(false);
		m_stopEditingButton.gameObject.SetActive(true);
	}

	private void CallbackStopEditing()
	{
		m_inputField.interactable = false;
		m_inputField.DeactivateInputField();

		m_closeButton.gameObject.SetActive(true);
		m_startEditingButton.gameObject.SetActive(true);
		m_stopEditingButton.gameObject.SetActive(false);
	}

	#endregion

	#region UI

	[SerializeField] private Button m_openButton;
	[SerializeField] private Button m_closeButton;
	[SerializeField] private Button m_startEditingButton;
	[SerializeField] private Button m_stopEditingButton;

	[SerializeField] private TMP_InputField m_inputField;
	[SerializeField] private TMP_Text m_durationText;

	public Button OpenButton => m_openButton;
	public Button CloseButton => m_closeButton;
	public TMP_InputField InputField => m_inputField;

	public void RefreshDurationTime(string durationTimeText)
	{
		m_durationText.text = durationTimeText;
	}

	#endregion

	#region MonoBehaviour

	protected void Awake()
	{
		m_startEditingButton?.onClick.AddListener(CallbackStartEditing);
		m_stopEditingButton?.onClick.AddListener(CallbackStopEditing);

		CallbackStopEditing();
	}

	protected void OnDestroy()
	{
		m_startEditingButton?.onClick.RemoveListener(CallbackStartEditing);
		m_stopEditingButton?.onClick.RemoveListener(CallbackStopEditing);
	}



	#endregion


}
