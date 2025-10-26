using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NumberPicker : MonoBehaviour, IEndDragHandler
{

	[SerializeField] private ScrollRect m_scrollRect;
	[SerializeField] private int m_numbers;

	[SerializeField] private GameObject m_numberPrefab;

	public Action<int> onNumberPicked;

	public float test;

	#region Behaviour

	public void Populate()
	{
		// Compute size of button
		Rect viewportSize = m_scrollRect.viewport.rect;

		// Create buttons
		for (int i = 0; i < m_numbers; i++)
		{
			GameObject numberObject = Instantiate(m_numberPrefab, m_scrollRect.content.transform);

			RectTransform numberRect = numberObject.GetComponent<RectTransform>();
			numberRect.sizeDelta = new Vector2(viewportSize.width, viewportSize.height / 3f);

			Button numberButton = numberObject.GetComponent<Button>();
			int index = i;
			numberButton.onClick.AddListener(() => { CallbackNumberClicked(index); });

			TextMeshProUGUI numberText = numberObject.GetComponentInChildren<TextMeshProUGUI>();
			numberText.text = i.ToString();
		}

		// Create header
		GameObject header = new GameObject("Header", typeof(RectTransform));
		RectTransform headerRect = header.GetComponent<RectTransform>();
		headerRect.SetParent(m_scrollRect.content.transform);
		headerRect.SetAsFirstSibling();
		headerRect.sizeDelta = new Vector2(viewportSize.width, viewportSize.height / 3f);

		// Create header
		GameObject footer = new GameObject("Footer", typeof(RectTransform));
		RectTransform footerRect = footer.GetComponent<RectTransform>();
		footerRect.SetParent(m_scrollRect.content.transform);
		footerRect.SetAsLastSibling();
		footerRect.sizeDelta = new Vector2(viewportSize.width, viewportSize.height / 3f);
	}

	private void ScrollValueToNumberValue()
	{
		float value = m_scrollRect.normalizedPosition.y;
	}

	private void CallbackNumberClicked(int i)
	{
		SetNumber(i);
		onNumberPicked.Invoke(i);
	}

	#endregion

	#region MonoBehaviour

	public void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log($"OnEndDrag");
		Debug.Log($"VALUE : {m_scrollRect.normalizedPosition}");
	}

	#endregion

	#region Snapping

	public void SetNumber(int index)
	{
		float value;

		if (index == 0)
		{
			value = 0;
		}
		else if (index == m_numbers - 1)
		{
			value = 1;
		}
		else
		{
			value = (float)index / ((float)m_numbers - 1f);
		}

		m_scrollRect.normalizedPosition = new Vector2(0, 1 - value);
	}

	#endregion

}