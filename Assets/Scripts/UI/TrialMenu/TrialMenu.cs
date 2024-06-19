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

	#region MonoBehaviour

	protected override void Awake()
	{
		base.Awake();

		// m_inputField.onEndEdit.AddListener(CallbackEndEdit);
		m_inputField.onValidateInput += CallbackValidateInput;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		m_inputField.onValidateInput -= CallbackValidateInput;
	}

	#endregion

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

	private ApplicationData data { get { return SaveManager.data; } }

	private int m_currentIndex;

	private WordData m_currentWord
	{
		get
		{
			int index = data.dailyWords.Count - 1 - m_currentIndex;
			return data.dailyWords[index];
		}
	}

	protected override void CallbackDisplayScreen(ApplicationScreenType type, string[] options)
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
		m_inputField.text = "";

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
		m_inputField.text = "";

		// SaveManager
		data.trial.hasBeenPassed = true;
		data.trial.passDate = DateTime.Now;

		if (data.userLevel > 1)
		{
			data.userLevel--;
			data.dailyWords.RemoveAt(0);
			m_applicationChannel.onChangeDailyWords.Invoke();
		}

		SaveManager.Save();

		DisplayFailedPopUp();
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

		if (String.IsNullOrEmpty(m_inputField.text))
			return;

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
		// UI
		m_backButton.gameObject.SetActive(false);
		m_progressContainer.gameObject.SetActive(false);

		m_failedMessage.text =
		$"Last word is wrong, you failed today's trial. \n We deleted the oldest word in the list of Daily Words.\n This means you're level {data.userLevel} now.";

		m_failedPopUp.Display();
	}

	/// <summary> 
	///	Called by TrialMenu -> FailedPopUp -> CloseButton
	/// </summary>
	public void CloseFailedPopUp()
	{
		m_failedPopUp.Close();
		m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.MainMenu);
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