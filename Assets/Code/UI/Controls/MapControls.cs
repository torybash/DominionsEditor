using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapControls : MonoBehaviour
{
	private void Update ()
	{
		switch (DomEdit.I.Ui.Get<MapMenu>().ActiveEntry)
		{
			case MonsterEntry monsterEntry:
				//TODO // if (_selectedMonster.IS_COMMANDER) 
				if (Input.GetMouseButtonDown(0))
				{
					if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
					{
						AddUnit(monsterEntry);
					} else
					{
						AddCommander(monsterEntry);
					}

				}
				break;

			case ItemEntry itemEntry:
				if (Input.GetMouseButtonDown(0))
				{
					AddItem(itemEntry);
				}
				break;

			case MagicEntry magicEntry:
				if (Input.GetMouseButtonDown(0))
				{
					AddMagic(magicEntry);
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
		// if (Input.GetKeyDown(KeyCode.Y)) ToggleThrone();


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
		RaycastGizmos(out var gizmo);

		if (!(gizmo is UnitGizmo unitsGizmo)) return;

		unitsGizmo.Data.Amount = Mathf.Clamp(unitsGizmo.Data.Amount + sign, 1, 100000);

		unitsGizmo.SetData(unitsGizmo.Data);
	}

	private void AddCommander (MonsterEntry monsterEntry)
	{
		RaycastGizmos(out var gizmo);

		if (!(gizmo is ProvinceGizmo provinceGizmo)) return;

		var province  = provinceGizmo.Province;
		var commander = Commander.Create(monsterEntry.Id, province.Owner);

		province.Monsters.Add(commander);

		provinceGizmo.CreateCommanderGizmo(commander, provinceGizmo);
	}

	private void AddUnit (MonsterEntry monsterEntry)
	{
		RaycastGizmos(out var gizmo);

		if (gizmo is ProvinceGizmo provinceGizmo)
		{
			var province = provinceGizmo.Province;
			var unit     = Unit.Create(monsterEntry.Id, 1, province.Owner);

			province.Monsters.Add(unit);
			provinceGizmo.CreateUnitGizmo(unit);
		} else if (gizmo is CommanderGizmo commanderGizmo)
		{
			var unit = Unit.Create(monsterEntry.Id, 1, commanderGizmo.OwnerProvince.Province.Owner);

			commanderGizmo.Data.UnitsUnderCommand.Add(unit);
			commanderGizmo.OwnerProvince.CreateUnitGizmo(unit);
		}
	}

	private void AddItem (ItemEntry itemEntry)
	{
		RaycastGizmos(out var gizmo);

		if (!(gizmo is CommanderGizmo commanderGizmo)) return;

		var item = Item.Create(itemEntry.Name);

		commanderGizmo.Data.Items.Add(item);
		commanderGizmo.SetData(commanderGizmo.Data);
	}

	private void AddMagic (MagicEntry magicEntry)
	{
		RaycastGizmos(out var gizmo);

		if (!(gizmo is CommanderGizmo commanderGizmo)) return;

		var magicOverride = commanderGizmo.Data.MagicOverrides.SingleOrDefault(x => x.Path == magicEntry.magicPath);
		if (magicOverride == null)
		{
			magicOverride = new MagicOverride(magicEntry.magicPath, 0);
			commanderGizmo.Data.MagicOverrides.Add(magicOverride);
		}
		magicOverride.MagicValue++;

		commanderGizmo.SetData(commanderGizmo.Data);
	}

	private void ToggleLab ()
	{
		RaycastGizmos(out var gizmo);

		if (!(gizmo is ProvinceGizmo provinceGizmo)) return;

		provinceGizmo.Province.HasLab = !provinceGizmo.Province.HasLab;

		provinceGizmo.Refresh();
	}

	private void ToggleTemple ()
	{
		RaycastGizmos(out var gizmo);

		if (!(gizmo is ProvinceGizmo provinceGizmo)) return;

		provinceGizmo.Province.HasTemple = !provinceGizmo.Province.HasTemple;

		provinceGizmo.Refresh();
	}

	private void ToggleFort ()
	{
		RaycastGizmos(out var gizmo);

		if (!(gizmo is ProvinceGizmo provinceGizmo)) return;

		provinceGizmo.Province.HasFort = !provinceGizmo.Province.HasFort;

		provinceGizmo.Refresh();
	}
	
	// private void ToggleThrone ()
	// {
	// 	RaycastGizmos(out var gizmo);
	//
	// 	if (!(gizmo is ProvinceGizmo provinceGizmo)) return;
	//
	// 	provinceGizmo.Province.HasThrone = !provinceGizmo.Province.HasThrone;
	//
	// 	provinceGizmo.Refresh();
	// }

	private void ToggleCapital ()
	{
		RaycastGizmos(out var gizmo);

		if (!(gizmo is ProvinceGizmo provinceGizmo)) return;

		var playerOwner = DomEdit.I.MapMan.Map.Players.SingleOrDefault(x => x.Nation == provinceGizmo.Province.Owner);
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
		RaycastGizmos(out var gizmo);

		if (gizmo is ItemGizmo itemGizmo)
		{
			RemoveItem(itemGizmo);
		} else if (gizmo is MonsterGizmo monsterGizmo)
		{
			RemoveMonster(monsterGizmo);
		} else if (gizmo is MagicGizmo magicGizmo)
		{
			RemoveMagic(magicGizmo);
		} else if (gizmo is ProvinceGizmo provinceGizmo)
		{
			ClearProvinceOwner(provinceGizmo);
		}
	}

	private void RemoveItem (ItemGizmo itemGizmo)
	{
		var commanderGizmo = itemGizmo.OwnerGizmo;
		commanderGizmo.Data.Items.Remove(itemGizmo.Item);
		commanderGizmo.SetData(commanderGizmo.Data);
	}
	
	private void RemoveMagic (MagicGizmo magicGizmo)
	{
		var commanderGizmo = magicGizmo.OwnerGizmo;
		commanderGizmo.Data.MagicOverrides.Remove(magicGizmo.Magic);
		commanderGizmo.SetData(commanderGizmo.Data);
	}

	private void RemoveMonster (MonsterGizmo monsterGizmo)
	{
		var province       = monsterGizmo.OwnerProvince.Province;
		var ownerCommander = province.Monsters.OfType<Commander>().SingleOrDefault(x => x.UnitsUnderCommand.Contains(monsterGizmo.MonsterData));
		if (ownerCommander != null)
		{
			ownerCommander.UnitsUnderCommand.Remove((Unit)monsterGizmo.MonsterData);
		} else
		{
			province.Monsters.Remove(monsterGizmo.MonsterData);
		}

		monsterGizmo.OwnerProvince.RemoveElementGizmo(monsterGizmo.MonsterData);

		if (monsterGizmo.MonsterData is Commander commander)
		{
			RemoveAllCommanderElements(monsterGizmo.OwnerProvince, commander);
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
		RaycastGizmos(out var gizmo);

		if (!(gizmo is ProvinceGizmo provinceGizmo)) return;

		var player = DomEdit.I.MapMan.GetPlayer(numberPressed);
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

	private void RaycastGizmos (out Gizmo gizmo)
	{
		var pointerEventData = new PointerEventData(EventSystem.current)
		{
			position = Input.mousePosition
		};

		var resultAppendList = new List<RaycastResult>();
		DomEdit.I.Ui.Raycaster.Raycast(pointerEventData, resultAppendList);

		gizmo = null;
		// monsterGizmo  = null;
		// provinceGizmo = null;
		// itemGizmo     = null;

		foreach (var result in resultAppendList)
		{
			var raycastedGizmo = result.gameObject.GetComponentInParent<Gizmo>();
			if (raycastedGizmo != null)
			{
				Debug.Log($"Clicking {raycastedGizmo}");
				gizmo = raycastedGizmo;
				return;
			}
			// var monster = result.gameObject.GetComponentInParent<Gizmo>();
			// if (monster != null)
			// {
			// 	monsterGizmo = monster;
			// }
			//
			// var province = result.gameObject.GetComponentInParent<ProvinceGizmo>();
			// if (province != null)
			// {
			// 	provinceGizmo = province;
			// }
			//
			// var item = result.gameObject.GetComponentInParent<ItemGizmo>();
			// if (item != null)
			// {
			// 	itemGizmo = item;
			// }
		}
	}

}