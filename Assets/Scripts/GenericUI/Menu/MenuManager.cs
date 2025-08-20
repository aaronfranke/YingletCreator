using Reactivity;
using UnityEngine;

/// <summary>
/// Handles which "menu" is being shown
/// </summary>
public interface IMenuManager
{
	Observable<MenuType> OpenMenu { get; }
}

public class MenuManager : MonoBehaviour, IMenuManager
{
	Observable<MenuType> _openMenu = new Observable<MenuType>(null);
	public Observable<MenuType> OpenMenu => _openMenu;
}
