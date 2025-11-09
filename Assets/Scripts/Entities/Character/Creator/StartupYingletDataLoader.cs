using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Creator
{

	internal interface IStartupYingletDataLoader
	{
		IEnumerable<CachedYingletReference> LoadInitialYingData(CustomizationYingletGroup group);
	}
	internal class StartupYingletDataLoader : MonoBehaviour, IStartupYingletDataLoader
	{
		public IEnumerable<CachedYingletReference> LoadInitialYingData(CustomizationYingletGroup group)
		{
			var data = group switch
			{
				CustomizationYingletGroup.Custom => LoadCustomYingData(),
				CustomizationYingletGroup.Preset => LoadPresetYingData(),
				_ => throw new System.NotImplementedException()
			};
			var dataList = data
				.Where(reference => reference.CachedData != null) // In case we got a corrupt yingsave file
				.ToList();
			dataList.Sort((a, b) => DateTime.Compare(a.CachedData.CreationTime, b.CachedData.CreationTime));
			return dataList;
		}

		IEnumerable<CachedYingletReference> LoadCustomYingData()
		{
			var diskIO = this.GetComponent<ICustomizationDiskIO>();
			return diskIO.LoadInitialCustomYingData();
		}

		IEnumerable<CachedYingletReference> LoadPresetYingData()
		{
			return Enumerable.Empty<CachedYingletReference>();
		}
	}
}
