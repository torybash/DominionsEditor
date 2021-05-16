using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapMenu : Menu
{
	[SerializeField] private TMP_InputField searchField;
	[SerializeField] private Button clearSearchButton;
	[SerializeField] private Button saveMapButton;
	[SerializeField] private Button runButton;
	[SerializeField] private EntryGizmo selectedEntry;
	[SerializeField] private List<EntryGizmo> previousSelectedEntries;

	private readonly List<SearchResultGizmo> searchGizmos = new List<SearchResultGizmo>();
	private SearchableEntry _selectedEntry;

	private void Awake ()
	{
		searchField.onValueChanged.AddListener(OnSearchChanged);
		clearSearchButton.onClick.AddListener(() => searchField.text = "");
		saveMapButton.onClick.AddListener(OnSaveClicked);
		runButton.onClick.AddListener(OnRunClicked);
		selectedEntry.gameObject.SetActive(false);
		foreach (var previousSelectedEntry in previousSelectedEntries)
		{
			previousSelectedEntry.gameObject.SetActive(false);
		}
	}
	
	private void OnRunClicked ()
	{
		if (Game.Players.Any(x => x.Pretender == null || string.IsNullOrEmpty(x.Pretender.FilePath)))
		{
			Ui.Create<MessageGizmo>().Write("Player missing pretender reference");
			return;
		}
		
		var mapRunner = new MapRunner(Map, Game);
		mapRunner.Run();
	}

	private void OnSaveClicked ()
	{
		Map.SaveMap();
	}

	private void Update ()
	{
		switch (_selectedEntry)
		{
			case MonsterEntry _:
				//TODO // if (_selectedMonster.IS_COMMANDER) 
				// if (Input.GetKeyDown(KeyCode.U))
				// if (Input.GetMouseButtonDown(1))
				// {
				// 	
				// }

				// if (Input.GetKeyDown(KeyCode.C))
				if (Input.GetMouseButtonDown(0))
				{
					if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
					{
						AddUnit();
					} else
					{
						AddCommander();
					}
					
				}
				break;
			
			case ItemEntry _:
				// if (Input.GetKeyDown(KeyCode.I))
				if (Input.GetMouseButtonDown(0))
				{
					AddItem();
				}
				break;
		}
		
		if (Input.GetKeyDown(KeyCode.R))
		{
			Remove();
		}
		
		if (Input.GetKeyDown(KeyCode.L)) ToggleLab();
		if (Input.GetKeyDown(KeyCode.T)) ToggleTemple();		
		if (Input.GetKeyDown(KeyCode.F)) ToggleFort();
		if (Input.GetKeyDown(KeyCode.C)) ToggleCapital();

		
		if (InputHelper.GetKeyNumberPressed(out int numberPressed))
		{
			SetProvinceOwner(numberPressed);
		}
		
		if (Input.mouseScrollDelta.y != 0)
		{
			int change = (int)Input.mouseScrollDelta.y;
			if (InputHelper.IsShiftPressed) change *= 10;
			ChangeUnitCount(change);
		}
	}

	public override void Show ()
	{
		base.Show();
		
		// var mapTex = Man.MapElements.OfType<ImageFile>().Single().GetTexture(); //TODO Fix TGA loading
		// mapImage.texture = mapTex;

		CreateProvinceGizmos();
	}

	private void ChangeUnitCount (int sign)
	{
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo, out var itemGizmo);

		if (!(monsterGizmo is UnitGizmo unitsGizmo)) return;

		unitsGizmo.Data.Amount = Mathf.Clamp(unitsGizmo.Data.Amount + sign, 1, 100000);

		unitsGizmo.SetData(unitsGizmo.Data);
	}

	private void AddCommander ()
	{
		if (!(_selectedEntry is MonsterEntry monsterEntry)) return;
		
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo, out var itemGizmo);

		if (provinceGizmo == null) return;

		var province = provinceGizmo.Province;
		var commander = Commander.Create(monsterEntry.Id, province.Owner);
		
		province.Monsters.Add(commander);
		
		provinceGizmo.CreateCommanderGizmo(commander);;
	}

	private void AddUnit ()
	{
		if (!(_selectedEntry is MonsterEntry monsterEntry)) return;

		RaycastGizmos(out var monsterGizmo, out var provinceGizmo, out var itemGizmo);

		if (provinceGizmo == null) return;

		var province = provinceGizmo.Province;
		var unit = Unit.Create(monsterEntry.Id, 1, province.Owner);

		if (monsterGizmo is CommanderGizmo commanderGizmo)
		{
			commanderGizmo.Data.UnitsUnderCommand.Add(unit);
		} 
		else
		{
			province.Monsters.Add(unit);
		}
		provinceGizmo.CreateUnitGizmo(unit);
	}

	private void AddItem ()
	{
		if (!(_selectedEntry is ItemEntry itemEntry)) return;

		RaycastGizmos(out var monsterGizmo, out var provinceGizmo, out var itemGizmo);

		if (provinceGizmo == null) return;
		if (!(monsterGizmo is CommanderGizmo commanderGizmo)) return;

		var item = Item.Create(itemEntry.Name);
		
		commanderGizmo.Data.Items.Add(item);
		commanderGizmo.SetData(commanderGizmo.Data);
	}
	
	private void ToggleLab ()
	{
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo, out var itemGizmo);

		if (provinceGizmo == null) return;

		provinceGizmo.Province.HasLab = !provinceGizmo.Province.HasLab;
		
		provinceGizmo.Refresh();
	}
	
	private void ToggleTemple ()
	{
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo, out var itemGizmo);

		if (provinceGizmo == null) return;

		provinceGizmo.Province.HasTemple = !provinceGizmo.Province.HasTemple;
		
		provinceGizmo.Refresh();
	}

	private void ToggleFort ()
	{
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo, out var itemGizmo);

		if (provinceGizmo == null) return;

		provinceGizmo.Province.HasFort = !provinceGizmo.Province.HasFort;
		
		provinceGizmo.Refresh();
	}
	
	private void ToggleCapital ()
	{
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo, out var itemGizmo);

		if (provinceGizmo == null) return;

		var playerOwner = Game.Players.SingleOrDefault(x => x.Nation == provinceGizmo.Province.Owner);
		if (playerOwner == null) return;

		var provinceNum = provinceGizmo.Province.ProvinceNumber;
		if (playerOwner.CapitalProvinceNum == provinceNum)
		{
			playerOwner.CapitalProvinceNum = -1;
		} else
		{
			playerOwner.CapitalProvinceNum = provinceNum;
		}

		provinceGizmo.Refresh();
	}
	
	private void Remove ()
	{
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo, out var itemGizmo);

		if (itemGizmo != null)
		{
			RemoveItem(itemGizmo, (CommanderGizmo) monsterGizmo);
		}else if (monsterGizmo != null)
		{
			RemoveMonster(monsterGizmo, provinceGizmo);
		}else if (provinceGizmo != null)
		{
			ClearProvinceOwner(provinceGizmo);
		}
	}
	
	private void RemoveItem (ItemGizmo itemGizmo, CommanderGizmo commanderGizmo)
	{
		commanderGizmo.Data.Items.Remove(itemGizmo.Item);
		commanderGizmo.SetData(commanderGizmo.Data);
	}

	private void RemoveMonster(MonsterGizmo monsterGizmo, ProvinceGizmo provinceGizmo)
	{
		var province = provinceGizmo.Province;
		var ownerCommander = province.Monsters.OfType<Commander>().SingleOrDefault(x => x.UnitsUnderCommand.Contains(monsterGizmo.MonsterData));
		if (ownerCommander != null)
		{
			ownerCommander.UnitsUnderCommand.Remove((Unit)monsterGizmo.MonsterData);
		} 
		else
		{
			province.Monsters.Remove(monsterGizmo.MonsterData);
		}

		provinceGizmo.RemoveElementGizmo(monsterGizmo.MonsterData);

		if (monsterGizmo.MonsterData is Commander commander)
		{
			RemoveAllCommanderElements(provinceGizmo, commander);
		}
	}
	
	private void RemoveAllCommanderElements (ProvinceGizmo provinceGizmo, Commander commander)
	{
		foreach (var unit in commander.UnitsUnderCommand)
		{
			provinceGizmo.RemoveElementGizmo(unit);
		}
	}

	private void SetProvinceOwner (int numberPressed)
	{
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo, out var itemGizmo);

		if (provinceGizmo == null) return;
		
		var player = Game.GetPlayer(numberPressed);
		if (player == null) return;

		var province = provinceGizmo.Province;
		province.Owner = player.Nation;
		provinceGizmo.Refresh();
	}

	private void ClearProvinceOwner (ProvinceGizmo provinceGizmo)
	{
		var province = provinceGizmo.Province;

		province.Owner = Nation.Independents;
		provinceGizmo.Refresh();
	}
	
	private void RaycastGizmos (out MonsterGizmo monsterGizmo, out ProvinceGizmo provinceGizmo, out ItemGizmo itemGizmo)
	{
		var raycaster = GetComponentInParent<GraphicRaycaster>();
		var pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = Input.mousePosition;
		var resultAppendList = new List<RaycastResult>();
		raycaster.Raycast(pointerEventData, resultAppendList);

		monsterGizmo = null;
		provinceGizmo = null;
		itemGizmo = null;
		foreach (var result in resultAppendList)
		{
			var monster = result.gameObject.GetComponentInParent<MonsterGizmo>();
			if (monster != null)
			{
				monsterGizmo = monster;
			}

			var province = result.gameObject.GetComponentInParent<ProvinceGizmo>();
			if (province != null)
			{
				provinceGizmo = province;
			}
			
			var item = result.gameObject.GetComponentInParent<ItemGizmo>();
			if (item != null)
			{
				itemGizmo = item;
			}
		}
	}

	private void OnSearchChanged (string searchText)
	{
		var results = Map.Searcher.Search(searchText);

		foreach (var searchGizmo in searchGizmos) Destroy(searchGizmo.gameObject);
		searchGizmos.Clear();
		
		clearSearchButton.gameObject.SetActive(!string.IsNullOrEmpty(searchText));
		
		if (!string.IsNullOrEmpty(searchText)) CreateSearchGizmos(searchText, results);
		
	}
	
	private void CreateSearchGizmos (string searchText, List<SearchableEntry> results)
	{
		foreach (var searchResult in results)
		{
			var searchResultGizmo = Ui.Create<SearchResultGizmo>();
			searchResultGizmo.Initialize(searchResult, searchText);
			searchGizmos.Add(searchResultGizmo);
		}
	}
	
	private void CreateProvinceGizmos ()
	{
		var mapPicture = Ui.Get<MapPicture>();
		foreach (var province in Map.ProvinceMap.Values)
		{
			var gizmo = Ui.Create<ProvinceGizmo>(mapPicture.MapImage.transform);
			gizmo.Initialize(province);
		}
	}

	public void SetSelected (SearchableEntry entry)
	{
		_selectedEntry = entry;
		selectedEntry.gameObject.SetActive(true);
		selectedEntry.SetEntry(entry);
	}
}