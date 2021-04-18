using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapMenu : Menu
{
	[SerializeField] private RawImage mapImage;
	[SerializeField] private TMP_InputField searchField;
	[SerializeField] private Button clearSearchButton;
	[SerializeField] private Button saveMapButton;
	[SerializeField] private Button runButton;
	[SerializeField] private Image selectedSprite;

	private readonly List<SearchResultGizmo> searchGizmos = new List<SearchResultGizmo>();
	private MonstersTable.Entry _selectedMonster;

	private void Awake ()
	{
		searchField.onValueChanged.AddListener(OnSearchChanged);
		clearSearchButton.onClick.AddListener(() => searchField.text = "");
		saveMapButton.onClick.AddListener(OnSaveClicked);
		runButton.onClick.AddListener(OnRunClicked);
		selectedSprite.gameObject.SetActive(false);
	}
	
	private void OnRunClicked ()
	{
		Game.RunMap();
	}

	private void OnSaveClicked ()
	{
		Map.SaveMap();
	}

	private void Update ()
	{
		//TODO // if (_selectedMonster.IS_COMMANDER) 
		if (Input.GetKeyDown(KeyCode.U) && _selectedMonster != null)
		{
			AddUnit();
		}

		if (Input.GetKeyDown(KeyCode.C) && _selectedMonster != null)
		{
			AddCommander();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			Remove();
		}

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

		mapImage.gameObject.SetActive(true);
		CreateProvinceGizmos();
	}
	
	private void ChangeUnitCount (int sign)
	{
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo);

		if (!(monsterGizmo is UnitGizmo unitsGizmo)) return;

		unitsGizmo.Data.Amount = Mathf.Clamp(unitsGizmo.Data.Amount + sign, 1, 100000);

		unitsGizmo.Initialize(unitsGizmo.Data);
	}

	private void AddCommander ()
	{
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo);

		if (provinceGizmo == null) return;

		var province = provinceGizmo.Province;
		var commander = Commander.Create(_selectedMonster.Id, province.Owner);
		
		province.Monsters.Add(commander);
		
		provinceGizmo.CreateCommanderGizmo(commander);;
	}

	private void AddUnit ()
	{
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo);

		if (provinceGizmo == null) return;

		var province = provinceGizmo.Province;
		var unit = Unit.Create(_selectedMonster.Id, 1, province.Owner);

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
	
	private void Remove ()
	{
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo);

		if (monsterGizmo != null)
		{
			RemoveMonster(monsterGizmo, provinceGizmo);
		}else if (provinceGizmo != null)
		{
			ClearProvinceOwner(provinceGizmo);
		}
	}
	
	private void RemoveMonster(MonsterGizmo monsterGizmo, ProvinceGizmo provinceGizmo)
	{
		provinceGizmo.Province.Monsters.Remove(monsterGizmo.MonsterData);
		
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
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo);

		if (provinceGizmo == null) return;
		
		var player = Game.GetPlayer(numberPressed);
		if (player == null) return;

		var province = provinceGizmo.Province;
		province.Owner = player.NationNum;
		provinceGizmo.Refresh();
	}

	private void ClearProvinceOwner (ProvinceGizmo provinceGizmo)
	{
		var province = provinceGizmo.Province;

		province.Owner = Nation.Independents;
		provinceGizmo.Refresh();
	}
	
	private void RaycastGizmos (out MonsterGizmo monsterGizmo, out ProvinceGizmo provinceGizmo)
	{
		var raycaster = GetComponentInParent<GraphicRaycaster>();
		var pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = Input.mousePosition;
		var resultAppendList = new List<RaycastResult>();
		raycaster.Raycast(pointerEventData, resultAppendList);

		monsterGizmo = null;
		provinceGizmo = null;
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
	
	private void CreateSearchGizmos (string searchText, List<MonstersTable.Entry> results)
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
		foreach (var province in Map.ProvinceMap.Values)
		{
			var gizmo = Ui.Create<ProvinceGizmo>();
			gizmo.Initialize(province);
		}
	}

	public void SetSelected (MonstersTable.Entry entry)
	{
		_selectedMonster = entry;
		selectedSprite.gameObject.SetActive(true);
		selectedSprite.sprite = entry.Sprite;
	}
}