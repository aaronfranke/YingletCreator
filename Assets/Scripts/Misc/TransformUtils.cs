using System.Collections.Generic;
using UnityEngine;

public static class TransformUtils
{
	public static Transform FindChildRecursive(Transform parent, string childName)
	{
		foreach (Transform child in parent)
		{
			if (child.name == childName)
				return child;

			Transform found = FindChildRecursive(child, childName);
			if (found != null)
				return found;
		}
		return null;
	}


	public static List<Transform> FindChildrenByPrefix(Transform parent, string prefix)
	{
		List<Transform> matchingChildren = new List<Transform>();
		FindChildrenByPrefixRecursive(parent);
		return matchingChildren;

		void FindChildrenByPrefixRecursive(Transform parent)
		{
			foreach (Transform child in parent)
			{
				if (child.name.StartsWith(prefix))
				{
					matchingChildren.Add(child);
				}
				FindChildrenByPrefixRecursive(child);
			}
		}
	}
}
