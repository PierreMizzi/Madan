using System;
using System.Collections.Generic;

[Serializable]
public class ProgressBonsaiData
{
	public List<ProgressBonsaiFlowerData> flowersData = new List<ProgressBonsaiFlowerData>();

	public override string ToString()
	{
		string data = "ProgressBonsaiData : \n";
		data += $"flowersData.Count : {flowersData.Count}";
		return data;
	}
}