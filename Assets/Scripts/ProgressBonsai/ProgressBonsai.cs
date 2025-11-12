using System.Collections.Generic;
using PierreMizzi.Extensions.Timer;
using UnityEngine;

// 🟥 : After completing a Pomodoro, go to Progress Bonsai and give a bloom animation

// 🟥 : Touch flower for info
// 🟩 : Create foliage
// 🟩 : Find blooming spots inside the foliage
// 🟥 : Flower blooms around the same original blooming spots

// 🟩 : Orbital Camera Controller
// 🟩 : 	- Slow perpetual rotation
// 🟥 : 	- Zoom in and out (clamped)

// 🟥 : TimerUI : Rework animator and make it a proper state machine

// 💡 : Notepad
// 🟥 : Hook it to TimerUI
// 🟥 : Save the inputfield value
// 🟥 : Display a little hint zhen the notepad is empty
// 🟥 : Display a little hint when the notepad is empty at the end
// 🟥 : Display a PopUp "Are you sure you wanna leave this study session notes empty" when notepad is empty

public class ProgressBonsai : MonoBehaviour
{

	#region Behaviour

	[Header("Behaviour")]
	[SerializeField] private ApplicationChannel m_applicationChannel;
	[SerializeField] private Timer m_timer;

	private void CallbackStudyTimeCompleted(StudyTime pomodoro)
	{
		int availableSpotIndex = GetAvailableSpotIndex();

		if (availableSpotIndex == -1)
		{
			return;
		}

		ProgressBonsaiFlowerData data = new ProgressBonsaiFlowerData(pomodoro, availableSpotIndex);
		savedData.flowersData.Add(data);
		Save();

		CreateFlower(data);
	}

	#endregion

	#region Flowers

	[Header("Flowers")]
	[SerializeField] private Transform m_flowersContainer;
	[SerializeField] private ProgressBonsaiFlower m_flowerPrefab;

	protected void CreateFlowersFromData()
	{
		if (savedData == null || savedData.flowersData.Count == 0)
		{
			return;
		}

		foreach (ProgressBonsaiFlowerData flowerData in savedData.flowersData)
		{
			CreateFlower(flowerData);
		}
	}

	protected void CreateFlower(ProgressBonsaiFlowerData data)
	{
		Transform spot = m_bloomingSpotsContainer.GetChild(data.spotChildIndex);

		ProgressBonsaiFlower flower = Instantiate(m_flowerPrefab, m_flowersContainer);
		flower.transform.rotation = spot.rotation;
		flower.transform.position = spot.position;
	}

	#endregion

	#region Blooming Spots

	[Header("Blooming Spots")]
	[SerializeField] private Transform m_bloomingSpotsContainer;

	private List<int> m_availableBloomingSpotIndexes = new List<int>();

	private void InitializeSpotIndexes()
	{
		if (savedData == null || m_availableBloomingSpotIndexes == null)
		{
			return;
		}

		for (int i = 0; i < m_bloomingSpotsContainer.childCount; i++)
		{
			m_availableBloomingSpotIndexes.Add(i);
		}


		foreach (ProgressBonsaiFlowerData flowerData in savedData.flowersData)
		{
			if (flowerData.spotChildIndex == -1)
			{
				continue;
			}

			if (m_availableBloomingSpotIndexes.Contains(flowerData.spotChildIndex))
			{
				m_availableBloomingSpotIndexes.Remove(flowerData.spotChildIndex);
			}
		}

	}

	private int GetAvailableSpotIndex()
	{
		if (m_availableBloomingSpotIndexes.Count == 0)
		{
			return -1;
		}

		int randomIndex = Random.Range(0, m_availableBloomingSpotIndexes.Count);
		int randomSpotIndex = m_availableBloomingSpotIndexes[randomIndex];
		m_availableBloomingSpotIndexes.RemoveAt(randomIndex);

		return randomSpotIndex;
	}

	#endregion

	#region MonoBehaviour

	protected virtual void Start()
	{
		if (m_applicationChannel != null)
			m_applicationChannel.onAppDataLoaded += CallbackAppDataLoaded;

		if (m_timer != null)
			m_timer.onStudyTimeCompleted += CallbackStudyTimeCompleted;

	}

	protected virtual void OnDestroy()
	{
		if (m_applicationChannel != null)
			m_applicationChannel.onAppDataLoaded -= CallbackAppDataLoaded;

		if (m_timer != null)
			m_timer.onStudyTimeCompleted -= CallbackStudyTimeCompleted;
	}

	#endregion

	#region Save Data

	public ProgressBonsaiData savedData
	{
		get
		{
			if (SaveManager.data != null && SaveManager.data.progressBonsaiData != null)
			{
				return SaveManager.data.progressBonsaiData;
			}
			else
				return null;
		}
		set
		{
			if (SaveManager.data == null)
			{
				return;
			}
			else
			{
				SaveManager.data.progressBonsaiData = value;
			}
		}
	}

	private void CallbackAppDataLoaded()
	{
		LoadData();

		InitializeSpotIndexes();
	}

	public void LoadData()
	{
		if (savedData == null)
		{
			SaveManager.data.progressBonsaiData = new ProgressBonsaiData();
		}

		CreateFlowersFromData();

		SaveManager.Save();
	}

	public void Save()
	{
		if (savedData == null)
		{
			return;
		}

		SaveManager.Save();
	}

	[ContextMenu("Call ClearSavedData")]
	public void ClearSavedData()
	{
		savedData = new ProgressBonsaiData();
		SaveManager.Save();
	}

	#endregion

	#region Debug

	[ContextMenu("Call DebugCreateFlow")]
	public void DebugCreateFlow()
	{
		StudyTime newStudyTime = new StudyTime();
		CallbackStudyTimeCompleted(newStudyTime);
	}

	#endregion

}