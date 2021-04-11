using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

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
		Man.SaveMap();
		Man.RunMap();
		
		
	}

	private void OnSaveClicked ()
	{
		Man.SaveMap();
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
			RemoveMonster();
		}

		if (Input.mouseScrollDelta.y != 0)
		{
			int change = (int)Input.mouseScrollDelta.y;
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) change *= 10;
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

		if (!(monsterGizmo is UnitsGizmo unitsGizmo)) return;

		unitsGizmo.UnitsElement.Amount = Mathf.Clamp(unitsGizmo.UnitsElement.Amount + sign, 1, 100000);
		unitsGizmo.Initialize(unitsGizmo.UnitsElement);
	}

	private void AddCommander ()
	{
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo);

		if (provinceGizmo == null) return;

		var commandElem = new Commander
		{
			Man = Man,
			UnitId = _selectedMonster.Id
		};
		AddMonster(commandElem, provinceGizmo);
	}


	private void AddUnit ()
	{
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo);

		if (!(monsterGizmo is CommanderGizmo commanderGizmo)) return;

		var unitsElem = new Units()
		{
			Man = Man,
			UnitId = _selectedMonster.Id,
			Amount = 1,
			Commander = (Commander)commanderGizmo.Element
		};

		AddMonster(unitsElem, provinceGizmo);
	}
	
	private void AddMonster (ProvinceDataElement provinceDataElement, ProvinceGizmo provinceGizmo)
	{
		var land = Man.MapElements.OfType<Land>().SingleOrDefault(x => x.ProvinceNum == provinceGizmo.ProvinceNum);
		provinceDataElement.ProvinceNum = provinceGizmo.ProvinceNum;
		Man.AddMapElement(provinceDataElement);
		provinceGizmo.CreateElementGizmo(provinceDataElement);
	}

	private void RemoveMonster ()
	{
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo);

		if (monsterGizmo == null) return;
		
		Man.RemoveMapElement(monsterGizmo.Element);
		provinceGizmo.RemoveElementGizmo(monsterGizmo.Element);

		if (!(monsterGizmo.Element is Commander commander)) return;
		
		foreach (var mapElement in Man.MapElements.ToList())
		{
			if (!(mapElement is IOwnedByCommander ownedByCommander) || ownedByCommander.Commander != commander) continue;
			
			Man.RemoveMapElement(monsterGizmo.Element);
			provinceGizmo.RemoveElementGizmo(monsterGizmo.Element);
		}
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
		var results = Man.Searcher.Search(searchText);

		foreach (var searchGizmo in searchGizmos) Destroy(searchGizmo.gameObject);
		searchGizmos.Clear();
		
		clearSearchButton.gameObject.SetActive(!string.IsNullOrEmpty(searchText));
		
		if (!string.IsNullOrEmpty(searchText)) CreateSearchGizmos(searchText, results);
		
	}
	private void CreateSearchGizmos (string searchText, List<MonstersTable.Entry> results)
	{
		foreach (var searchResult in results)
		{
			var searchResultGizmo = Man.Create<SearchResultGizmo>();
			searchResultGizmo.Initialize(searchResult, searchText);
			searchGizmos.Add(searchResultGizmo);
		}
	}
	

	private void CreateProvinceGizmos ()
	{
		int provinceCount = Man.MapElements.OfType<Terrain>().Max(x => x.ProvinceNum);
		for (int num = 1; num <= provinceCount; num++)
		{
			var pbs = Man.MapElements.OfType<ProvinceBorders>().Where(x => x.ProvinceNum == num).ToList();
			if (pbs.Count == 0) continue;

			var centerPos = Vector2.zero;
			foreach (var pb in pbs)
			{
				centerPos.x += pb.X;
				centerPos.y += pb.Y;
			}
			centerPos /= pbs.Count();

			CreateProvinceGizmo(num, centerPos);
		}
	}
	
	private void CreateProvinceGizmo (int provinceNum, Vector2 centerPos)
	{
		var gizmo = Man.Create<ProvinceGizmo>();
		gizmo.Initialize(provinceNum, centerPos);
	}

	public void SetSelected (MonstersTable.Entry entry)
	{
		_selectedMonster = entry;
		selectedSprite.gameObject.SetActive(true);
		selectedSprite.sprite = entry.Sprite;
	}
}