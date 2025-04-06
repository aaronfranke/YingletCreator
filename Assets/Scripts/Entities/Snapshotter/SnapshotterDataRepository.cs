using Character.Creator;
using UnityEngine;


public class SnapshotterDataRepository : MonoBehaviour, ICustomizationSelectedDataRepository
{
    public ObservableCustomizationData CustomizationData { get; private set; }

    public void Setup(ObservableCustomizationData data)
    {
        CustomizationData = data;
    }
}
