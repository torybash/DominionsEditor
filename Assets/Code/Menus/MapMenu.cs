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
		Map.SaveMap();
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
			UnitId = _selectedMonster.Id,
			Amount = 1,
			Commander = (Commander)commanderGizmo.Element
		};

		AddMonster(unitsElem, provinceGizmo);
	}
	
	private void AddMonster (ProvinceDataElement provinceDataElement, ProvinceGizmo provinceGizmo)
	{
		var land = Map.MapElements.OfType<Land>().SingleOrDefault(x => x.ProvinceNum == provinceGizmo.ProvinceNum);
		provinceDataElement.ProvinceNum = provinceGizmo.ProvinceNum;
		Map.AddMapElement(provinceDataElement);
		provinceGizmo.CreateElementGizmo(provinceDataElement);
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
	
	private void RemoveMonster (MonsterGizmo monsterGizmo, ProvinceGizmo provinceGizmo)
	{

		Map.RemoveMapElement(monsterGizmo.Element);
		provinceGizmo.RemoveElementGizmo(monsterGizmo.Element);

		if (monsterGizmo.Element is Commander commander)
		{
			RemoveAllCommanderElements(monsterGizmo, provinceGizmo, commander);
		}
	}
	
	private void RemoveAllCommanderElements (MonsterGizmo monsterGizmo, ProvinceGizmo provinceGizmo, Commander commander)
	{

		foreach (var mapElement in Map.MapElements.ToList())
		{
			if (!(mapElement is IOwnedByCommander ownedByCommander) || ownedByCommander.Commander != commander) continue;

			Map.RemoveMapElement(monsterGizmo.Element);
			provinceGizmo.RemoveElementGizmo(monsterGizmo.Element);
		}
	}

	private void SetProvinceOwner (int numberPressed)
	{
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo);

		if (provinceGizmo == null) return;
		
		var player = Game.GetPlayer(numberPressed);
		if (player == null) return;
		
		int nationNum = player.NationNum;
		
		var startLocation = Map.MapElements.OfType<StartLocation>().SingleOrDefault(x => x.ProvinceNum == provinceGizmo.ProvinceNum);
		if (startLocation != null)
		{
			if (startLocation.NationNum != nationNum)
			{
				startLocation.NationNum = nationNum;
				provinceGizmo.SetOwner(nationNum);
			}
			return;
		}
		
		var provinceOwner = Map.MapElements.OfType<ProvinceOwner>().SingleOrDefault(x => x.ProvinceNum == provinceGizmo.ProvinceNum);
		if (provinceOwner == null)
		{
			provinceOwner = new ProvinceOwner { ProvinceNum = provinceGizmo.ProvinceNum };
			Map.AddMapElement(provinceOwner);
		}
		provinceOwner.NationNum = nationNum;
		provinceGizmo.SetOwner(provinceOwner.NationNum);
	}

	private void ClearProvinceOwner (ProvinceGizmo provinceGizmo)
	{
		var startLocation = Map.MapElements.OfType<StartLocation>().SingleOrDefault(x => x.ProvinceNum == provinceGizmo.ProvinceNum);
		if (startLocation != null)
		{
			Map.RemoveMapElement(startLocation);
		}
		
		var provinceOwner = Map.MapElements.OfType<ProvinceOwner>().SingleOrDefault(x => x.ProvinceNum == provinceGizmo.ProvinceNum);
		if (provinceOwner != null)
		{
			Map.RemoveMapElement(provinceOwner);
		}
		
		provinceGizmo.ClearOwner();
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
		int provinceCount = Map.MapElements.OfType<Terrain>().Max(x => x.ProvinceNum);
		for (int num = 1; num <= provinceCount; num++)
		{
			var pbs = Map.MapElements.OfType<ProvinceBorders>().Where(x => x.ProvinceNum == num).ToList();
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
		var gizmo = Ui.Create<ProvinceGizmo>();
		gizmo.Initialize(provinceNum, centerPos);
	}

	public void SetSelected (MonstersTable.Entry entry)
	{
		_selectedMonster = entry;
		selectedSprite.gameObject.SetActive(true);
		selectedSprite.sprite = entry.Sprite;
	}
}