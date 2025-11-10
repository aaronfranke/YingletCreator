using Character.Data;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndividualModUI : MonoBehaviour
{
	[SerializeField] TMP_Text _titleText;
	[SerializeField] TMP_Text _descriptionText;
	[SerializeField] TMP_Text _resourceText;
	[SerializeField] Image _icon;
	public void Setup(ModDefinition mod)
	{
		_titleText.text = $"{mod.Title} by {mod.Author}";
		_descriptionText.text = mod.ShortDescription;
		_icon.sprite = mod.Icon;
		_resourceText.text = BuildResourceString(mod);
	}

	private string BuildResourceString(ModDefinition mod)
	{
		int presetCount = mod.PresetYings.Count();
		int toggleCount = 0;
		int poseCount = 0;
		foreach (var resource in mod.Table.Resources)
		{
			var obj = resource.Object;
			if (obj is CharacterToggleId)
			{
				toggleCount += 1;
			}
			else if (obj is PoseId)
			{
				poseCount += 1;
			}
		}

		List<string> items = new();
		if (presetCount > 0) items.Add(NumberPrefixed(presetCount, "preset"));
		if (toggleCount > 0) items.Add(NumberPrefixed(toggleCount, "toggle"));
		if (poseCount > 0) items.Add(NumberPrefixed(poseCount, "pose"));
		return BuildEnglishList(items);
	}

	string NumberPrefixed(int count, string name)
	{
		var str = $"{count} {name}";
		if (count > 1) str += "s";
		return str;
	}

	string BuildEnglishList(List<string> items)
	{
		int count = items.Count;
		if (count == 0) return "";
		if (count == 1) return items.Single();

		string allButLast = string.Join(", ", items.Take(count - 1));
		return $"{allButLast} and {items.Last()}";
	}
}
