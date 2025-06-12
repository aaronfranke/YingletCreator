using UnityEngine;

namespace Snapshotter
{
	public class AnimatorForceForSnapshot : MonoBehaviour, ISnapshottableComponent
	{
		public void PrepareForSnapshot()
		{
			this.GetComponent<Animator>().Update(0.2f);
		}
	}
}
