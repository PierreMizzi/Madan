using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*

-	Trial button is activated
----> Click on Trial Button in MainMenu

	-> Victory PopUp

		FadeIn + Scale down de la PopUp

	-> Fail PopUp

*/

public class TrialMenu : ApplicationScreen
{

	#region Behaviour


	private enum TrialState
	{
		None,
		Idle,
		Ongoing,
		Success,
		Failed,
	}

	private TrialState m_state;

	private ApplicationData data { get { return data; } }

	private List<WordData> dailyWords { get { return data.dailyWords; } }

	private int m_currentIndex;

	private WordData m_currentWord
	{
		get
		{
			int index = data.dailyWords.Count - 1 - m_currentIndex;
			return data.dailyWords[index];
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
		m_backButton.gameObject.SetActive(true);
		m_progressContainer.gameObject.SetActive(true);

		m_state = TrialState.Ongoing;
		m_currentIndex = 0;
		SetUpWord();
	}

	private void TrialSuccess()
	{
		m_state = TrialState.Success;
		m_currentIndex = 0;

		// Saving
		data.trial.hasBeenPassed = true;
		data.trial.passDate = DateTime.Now;

		data.userLevel++;

		SaveManager.Save();

		DisplaySuccessPopUp();

		Debug.Log("Trial is successful");
	}

	private void TrialFailed()
	{
		m_state = TrialState.Failed;
		m_currentIndex = 0;
		Debug.Log("Trial is failed");
	}

	private void NextWord()
	{
		m_currentIndex++;
		SetUpWord();
	}

	private void SetUpWord()
	{
		m_dateLabel.text = m_currentWord.dateChosen.ToLongDateString();
		m_progressLabel.text = $"{m_currentIndex + 1}/{data.dailyWords.Count}";
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
			m_inputField.text = "";

			// Check is last index
			if (m_currentIndex == data.dailyWords.Count - 1)
				TrialSuccess();
			else
				NextWord();
		}
		else
			TrialFailed();
	}

	#endregion

	#region Callbacks

	// TODO : Refacto with same method, set-up for testing reasons
	// Change also in Editor
	public void CallbackEndEdit(string input)
	{
		// Debug.Log($"On EndEdit : {input}");
		if (m_state == TrialState.Ongoing)
			CheckInputText();
	}

	public void CallbackDeselect(string input)
	{
		// Debug.Log($"On Deselect : {input}");
		if (m_state == TrialState.Ongoing)
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
	[SerializeField] private RectTransform m_progressContainer;
	[SerializeField] private TextMeshProUGUI m_progressLabel;
	[SerializeField] private Button m_backButton;

	public void OnClickValidate()
	{
		CheckInputText();
	}

	public void OnClickBack()
	{
		m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.MainMenu);
	}

	#endregion

	#region Success PopUp

	[Header("Success PopUp")]

	[SerializeField] private TextMeshProUGUI m_successMessage;

	[SerializeField] private BasePopUp m_successPopUp;

	private void DisplaySuccessPopUp()
	{
		// UI
		m_backButton.gameObject.SetActive(false);
		m_progressContainer.gameObject.SetActive(false);

		// Date ordinal
		int count = data.dailyWords.Count;
		string daysCount = count + GetOrdinalSuffix(count);

		// TODO : NTA : Random success message everytime
		// Full SuccessMessage
		m_successMessage.text = $"It's your {daysCount} day in a row ! You've reached level {data.userLevel}!";

		m_successPopUp.Display();
	}

	/// <summary> 
	///	Called by TrialMenu -> SucccessPopUp -> GreatButton
	/// </summary>
	public void CloseSuccessPopUp()
	{
		m_successPopUp.Close();
		m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.MainMenu);
	}

	static string GetOrdinalSuffix(int number)
	{
		int lastDigit = number % 10;
		int lastTwoDigits = number % 100;

		if (lastTwoDigits >= 11 && lastTwoDigits <= 13)
		{
			return "th";
		}

		return lastDigit switch
		{
			1 => "st",
			2 => "nd",
			3 => "rd",
			_ => "th",
		};
	}

	#endregion

	#region Failed PopUp

	[Header("Failed PopUp")]
	[SerializeField] private TextMeshProUGUI m_failedMessage;

	[SerializeField] private BasePopUp m_failedPopUp;

	private const string k_failedMessage = "Last word is wrong unfortunately, ";

	private void DisplayFailedPopUp()
	{

		m_failedPopUp.Display();

		data.trial.hasBeenPassed = true;
		data.trial.passDate = DateTime.Now;

		data.userLevel--;

		data.dailyWords.RemoveAt(data.dailyWords.Count - 1);

		SaveManager.Save();


		// m_failedMessage =
	}

	#endregion

	#region Correct Feedback

	[SerializeField] private Animator m_correctFeedbackAnimator;

	private void PlayCorrectFeedback()
	{
		m_correctFeedbackAnimator.SetTrigger("Play");
	}

	#endregion

}