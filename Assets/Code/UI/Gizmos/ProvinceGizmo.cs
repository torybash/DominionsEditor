using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ProvinceGizmo : Gizmo
{
	[SerializeField] private TMP_Text numLabel;
	[SerializeField] private RectTransform rosterGroup;
	[SerializeField] private GameObject labMarker;
	[SerializeField] private GameObject templeMarker;
	[SerializeField] private GameObject fortMarker;

	private List<MonsterGizmo> monsterGizmos = new List<MonsterGizmo>();
	private NationGizmo _nationGizmo;
	
	
	public Province Province { get; private set; }

	public void Initialize (Province province)
	{
		Province = province;
		
		foreach (var monster in Province.Monsters)
		{
			switch (monster)
			{
				case Commander commander:
					CreateCommanderGizmo(commander);

					foreach (var unit in commander.UnitsUnderCommand)
					{
						CreateUnitGizmo(unit);
					}
					break;
				case Unit unit:
					CreateUnitGizmo(unit);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(monster));

			}
		}
		
		Refresh();
	}

	public void Refresh ()
	{
		numLabel.text = Province.ProvinceNumber.ToString();
		RectTrans.anchoredPosition = Province.CenterPos;
		
		SetOwner(Province.Owner);

		labMarker.SetActive(Province.HasLab);
		templeMarker.SetActive(Province.HasTemple);
		fortMarker.SetActive(Province.HasFort);
	}

	public void RemoveElementGizmo (Monster elem)
	{
		var elemGizmo = monsterGizmos.SingleOrDefault(x => x.MonsterData == elem);
		if (elemGizmo != null)
		{
			Destroy(elemGizmo.gameObject);
		}
	}
	
	public void SetOwner (Nation nation)
	{
		if (nation.Equals(Nation.Independents))
		{
			if (_nationGizmo != null)
			{
				Destroy(_nationGizmo.gameObject);
			}
			return;
		}

		if (_nationGizmo == null)
		{
			_nationGizmo = DomEdit.I.Ui.Create<NationGizmo>(transform);
		}
			
		_nationGizmo.SetNation(nation);

		bool isStartLocation = DomEdit.I.PlayerSetup.Players.Any(x => x.CapitalProvinceNum == Province.ProvinceNumber);
		_nationGizmo.ShowCapitalMarker(isStartLocation);
	}

	public void CreateCommanderGizmo (Commander commander)
	{
		var commanderGizmo = DomEdit.I.Ui.Create<CommanderGizmo>(rosterGroup);
		commanderGizmo.SetData(commander);
		monsterGizmos.Add(commanderGizmo);
	}
	
	public void CreateUnitGizmo (Unit unit)
	{
		var unitGizmo = DomEdit.I.Ui.Create<UnitGizmo>(rosterGroup);
		unitGizmo.SetData(unit);
		monsterGizmos.Add(unitGizmo);

		var ownerCommander = Province.Monsters.OfType<Commander>().SingleOrDefault(x => x.UnitsUnderCommand.Contains(unit));
		if (ownerCommander != null)
		{
			var ownerCommanderGizmo = monsterGizmos.OfType<CommanderGizmo>().Single(x => x.Data == ownerCommander);
			int commanderIdx = ownerCommanderGizmo.transform.GetSiblingIndex();
			unitGizmo.transform.SetSiblingIndex(commanderIdx + 1);
		}
	}
}