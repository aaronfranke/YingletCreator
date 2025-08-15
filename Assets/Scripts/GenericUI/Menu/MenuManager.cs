using Reactivity;
using UnityEngine;

/// <summary>
/// Handles which "menu" is being shown
/// </summary>
public interface IMenuManager
{
	MenuType OpenMenu { get; set; }
}

public class MenuManager : MonoBehaviour, IMenuManager
{
	Observable<MenuType> _openMenu = new Observable<MenuType>(null);
	public MenuType OpenMenu { get => _openMenu.Val; set => _openMenu.Val = value; }
}
