using Character.Creator;

namespace Snapshotter
{
    public sealed class SnapshotterParams
    {

        public SnapshotterParams(SnapshotterCameraPosition camPos, ObservableCustomizationData data)
        {
            CamPos = camPos; ;
            Data = data;
        }

        public SnapshotterCameraPosition CamPos { get; }
        public ObservableCustomizationData Data { get; }
    }
}
