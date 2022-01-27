using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapControls : MonoBehaviour
{
	private void Update ()
	{
		switch (DomEdit.I.Ui.Get<MapMenu>().SelectedEntry)
		{
			case MonsterEntry _:
				//TODO // if (_selectedMonster.IS_COMMANDER) 
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
				if (Input.GetMouseButtonDown(0))
				{
					AddItem();
				}
				break;
		}
		
		if (Input.GetMouseButtonDown(1))
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
			int change                             = (int)Input.mouseScrollDelta.y;
			if (InputHelper.IsShiftPressed) change *= 10;
			ChangeUnitCount(change);
		}
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
		if (!(DomEdit.I.Ui.Get<MapMenu>().SelectedEntry is MonsterEntry monsterEntry)) return;
		
		RaycastGizmos(out var monsterGizmo, out var provinceGizmo, out var itemGizmo);

		if (provinceGizmo == null) return;

		var province = provinceGizmo.Province;
		var commander = Commander.Create(monsterEntry.Id, province.Owner);
		
		province.Monsters.Add(commander);
		
		provinceGizmo.CreateCommanderGizmo(commander);;
	}

	private void AddUnit ()
	{
		if (!(DomEdit.I.Ui.Get<MapMenu>().SelectedEntry is MonsterEntry monsterEntry)) return;

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
		if (!(DomEdit.I.Ui.Get<MapMenu>().SelectedEntry is ItemEntry itemEntry)) return;

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

		var playerOwner = DomEdit.I.PlayerSetup.Players.SingleOrDefault(x => x.Nation == provinceGizmo.Province.Owner);
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
		
		var player = DomEdit.I.PlayerSetup.GetPlayer(numberPressed);
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
		var pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = Input.mousePosition;
		var resultAppendList = new List<RaycastResult>();
		DomEdit.I.Ui.Raycaster.Raycast(pointerEventData, resultAppendList);

		monsterGizmo  = null;
		provinceGizmo = null;
		itemGizmo     = null;
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

}