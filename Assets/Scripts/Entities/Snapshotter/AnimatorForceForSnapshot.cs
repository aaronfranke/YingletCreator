using UnityEngine;

namespace Snapshotter
{
    public class AnimatorForceForSnapshot : MonoBehaviour, ISnapshottable
    {
        public void PrepareForSnapshot()
        {
            this.GetComponent<Animator>().Update(0.2f);
        }
    }
}
