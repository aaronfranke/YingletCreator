using Character.Creator;
using Character.Data;
using UnityEngine;


public class SnapshotterDataRepository : MonoBehaviour, ICustomizationSelectedDataRepository
{
    public ObservableCustomizationData CustomizationData { get; private set; }

    public PoseId Pose { get; set; }

    public void Setup(ObservableCustomizationData data)
    {
        CustomizationData = data;
    }
}
