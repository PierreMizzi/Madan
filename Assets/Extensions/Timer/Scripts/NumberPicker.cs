using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NumberPicker : ScrollRect
{
	
	[SerializeField] private int m_numbers;

	[SerializeField] private GameObject m_numberPrefab;

	public float m_fraction => 1f / m_numbers + 1f;

	public float test;

	#region Behaviour

	private void Populate()
	{
		for (int i = 0; i <= m_numbers; i++)
		{
			GameObject numberObject = Instantiate(m_numberPrefab, content.transform);

			TextMeshProUGUI numberText = numberObject.GetComponentInChildren<TextMeshProUGUI>();
			numberText.text = i.ToString();
		}
	}

	#endregion

	#region MonoBehaviour

	protected override void Start()
	{
		base.Start();
		Populate();
	}


	#endregion

	#region Scroll Rect

	public override void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log($"OnEndDrag");
		base.OnEndDrag(eventData);
	}
		
	#endregion
}