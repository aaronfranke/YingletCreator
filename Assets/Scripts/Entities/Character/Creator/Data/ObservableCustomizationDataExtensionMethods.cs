using Character.Data;
using Reactivity;
using System.Linq;
using UnityEngine;

namespace Character.Creator
{
	public static class ObservableCustomizationDataExtensionMethods
	{

		public static bool GetToggle(this ObservableCustomizationToggleData data, CharacterToggleId id)
		{
			return data.Toggles.Contains(id);
		}

		public static void FlipToggle(this ObservableCustomizationToggleData data, CharacterToggleId id)
		{

			using var suspender = new ReactivitySuspender();
			bool exists = data.GetToggle(id);

			if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
			{
				// Easter egg: Ignore rules if holding both down
				if (exists) data.Toggles.Remove(id);
				else data.Toggles.Add(id);
				return;
			}

			if (exists)
			{
				if (id.Group && id.Group.MustHaveOne)
				{
					bool anotherExists = data.Toggles.Any(other => other != id && other.Group == id.Group);
					if (!anotherExists) return;
				}
				data.Toggles.Remove(id);
			}
			else
			{
				data.Toggles.Add(id);

				if (id.Group)
				{
					var togglesToRemove = data.Toggles
						.Where(toggle => toggle.Group == id.Group && toggle != id)
						.ToList();
					foreach (var toggleToRemove in togglesToRemove)
					{
						data.Toggles.Remove(toggleToRemove);
					}

				}
			}
		}

		public static int GetInt(this ObservableCustomizationNumberData data, CharacterIntId id)
		{
			if (data.IntValues.TryGetValue(id, out Observable<int> value))
			{
				return value.Val;
			}
			return 0;
		}
	}
}
