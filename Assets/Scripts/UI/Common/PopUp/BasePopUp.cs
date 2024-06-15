using UnityEngine;
using UnityEngine.Events;

public class BasePopUp : MonoBehaviour
{

	private Animator m_animator;

	private const string k_displayed = "Displayed";

	#region Behaviour

	public UnityEvent<string> onClick;

	public virtual void Display()
	{
		m_animator.SetBool(k_displayed, true);
	}

	public virtual void Close()
	{
		m_animator.SetBool(k_displayed, false);
	}

	public virtual void OnClickButton(string option)
	{
		onClick.Invoke(option);
	}

	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		onClick = new UnityEvent<string>();
		m_animator = GetComponent<Animator>();
	}

	#endregion

}