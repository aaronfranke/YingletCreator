using UnityEngine;

namespace CharacterCompositor
{
	/// <summary>
	/// Stub class to make it easier to look up components
	/// </summary>
	public sealed class CompositedYingletRoot : MonoBehaviour
	{
	}

	public static class CompositedYingletRootExtensionMethods
	{
		/// <summary>
		/// Returns the first component under the parent composited yinglet root
		/// </summary>
		public static T GetCompositedYingletComponent<T>(this MonoBehaviour mb)
		{
			var type = typeof(T);
			var root = mb.GetComponentInParent<CompositedYingletRoot>();
			if (root == null)
			{
				Debug.LogWarning($"Failed to get singleton component of type {type}; could not find composited yinglet root");
				return default(T);
			}

			var component = root.GetComponentInChildren<T>();
			if (component == null)
			{
				Debug.LogWarning($"Failed to get singleton component of type {type}; could not find a component");
				return default(T);
			}
			return component;
		}
	}

}

