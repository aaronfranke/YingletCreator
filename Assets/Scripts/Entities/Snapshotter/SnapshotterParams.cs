namespace Snapshotter
{
    public sealed class SnapshotterParams
    {

        public SnapshotterParams(SnapshotterCameraPosition camPos)
        {
            CamPos = camPos; ;
        }

        public SnapshotterCameraPosition CamPos { get; }
    }
}
