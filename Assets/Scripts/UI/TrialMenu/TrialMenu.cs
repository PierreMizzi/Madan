using System;
using System.Collections.Generic;
using PierreMizzi.Useful;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Events;
using UnityEngine.UI;

/*

-	Trial button is activated
----> Click on Trial Button in MainMenu


[ 13/06 ] [ 14/06 ]
	

*/

public class TrialMenu : ApplicationScreen
{

	#region Behaviour

	private List<WordData> m_dailyWords { get { return SaveManager.data.dailyWords; } }

	private int m_currentIndex;

	private WordData m_currentWord
	{
		get
		{
			int index = m_dailyWords.Count - 1 - m_currentIndex;
			return SaveManager.data.dailyWords[index];
		}
	}

	private void CallbackDisplayScreen(ApplicationScreenType type, string[] options)
	{
		if (type == m_type)
		{
			StartTrial();
		}
	}

	private void StartTrial()
	{
		m_currentIndex = 0;
		SetUpWord();
	}

	private void NextWord()
	{
		m_currentIndex++;
		SetUpWord();
	}

	private void SetUpWord()
	{
		m_dateLabel.text = m_currentWord.dateChosen.ToLongDateString();
		m_progressLabel.text = $"{m_currentIndex + 1}/{m_dailyWords.Count}";
	}

	/// <summary> 
	///	Checks if m_inputField.text matches the currentWord
	/// </summary>
	private void CheckInputText()
	{
		Debug.Log($"m_inputField.text = {m_inputField.text}");
		if (m_inputField.text == m_currentWord.kanji)
		{
			Debug.Log("Correct !");
			PlayCorrectFeedback();
			NextWord();
			m_inputField.text = "";
		}
		else
		{
			Debug.Log("Incorrect !");
		}
	}



	#endregion

	#region Callbacks

	// TODO : Refacto with same method, set-up for testing reasons
	// Change also in Editor

	public void CallbackEndEdit(string input)
	{
		// Debug.Log($"On EndEdit : {input}");
		CheckInputText();
	}

	public void CallbackDeselect(string input)
	{
		// Debug.Log($"On Deselect : {input}");
		CheckInputText();
	}

	// TODO : Check for forbidden characters as we type
	public char CallbackValidateInput(string text, int charIndex, char addedChar)
	{
		// Debug.Log($"On Validate Input : {text}");
		return addedChar;
	}

	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		if (m_applicationChannel != null)
			m_applicationChannel.onDisplayScreen += CallbackDisplayScreen;

		// m_inputField.onEndEdit.AddListener(CallbackEndEdit);
		m_inputField.onValidateInput += CallbackValidateInput;

	}

	private void OnDestroy()
	{
		if (m_applicationChannel != null)
			m_applicationChannel.onDisplayScreen -= CallbackDisplayScreen;
	}

	#endregion

	#region UI

	[Header("UI")]
	[SerializeField] private TMP_InputField m_inputField;
	[SerializeField] private TextMeshProUGUI m_dateLabel;
	[SerializeField] private TextMeshProUGUI m_progressLabel;

	public void OnClickValidate()
	{
		CheckInputText();
	}

	public void OnClickBack()
	{
		m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.MainMenu);
	}

	#endregion

	#region Correct Feedback

	[SerializeField] private TextMeshProUGUI m_correctLabel;

	private void InitializeCorrectFeedback()
	{
		m_correctLabel.color = UtilsClass.Transparent;
	}

	private void PlayCorrectFeedback()
	{
		
	}

	#endregion
}