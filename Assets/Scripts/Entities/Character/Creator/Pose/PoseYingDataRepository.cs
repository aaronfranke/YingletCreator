using Character.Creator;
using UnityEngine;

public interface IPoseYingDataRepository
{
	void Setup(CachedYingletReference reference);
}

public class PoseYingDataRepository : MonoBehaviour, IPoseYingDataRepository, ICustomizationSelectedDataRepository
{
	public ObservableCustomizationData CustomizationData { get; private set; }

	public void Setup(CachedYingletReference reference)
	{
		CustomizationData = new ObservableCustomizationData(reference.CachedData);
	}
}
