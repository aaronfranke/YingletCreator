using Character.Creator;
using UnityEngine;

public interface IPoseYingDataRepository
{
	CachedYingletReference Reference { get; }
	void Setup(CachedYingletReference reference);
}

public class PoseYingDataRepository : MonoBehaviour, IPoseYingDataRepository, ICustomizationSelectedDataRepository
{
	public CachedYingletReference Reference { get; private set; }
	public ObservableCustomizationData CustomizationData { get; private set; }

	public void Setup(CachedYingletReference reference)
	{
		Reference = reference;
		CustomizationData = new ObservableCustomizationData(reference.CachedData);
	}
}
