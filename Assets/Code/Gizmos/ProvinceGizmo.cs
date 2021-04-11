using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
public class ProvinceGizmo : Gizmo
{
	[SerializeField] private TMP_Text numLabel;
	[SerializeField] private RectTransform rosterGroup;

	private List<MonsterGizmo> monsterGizmos = new List<MonsterGizmo>();
	private NationGizmo _nationGizmo;
	public int ProvinceNum { get; private set; }
	
	public void Initialize (int provinceNum, Vector2 centerPos)
	{
		ProvinceNum = provinceNum;
		numLabel.text = provinceNum.ToString();
		RectTrans.anchoredPosition = centerPos;

		// Debug.Log($"Province {provinceNum}, centerPos {centerPos}");

		var provinceElements = Man.MapElements.OfType<ProvinceDataElement>().Where(x => x.ProvinceNum == provinceNum);
		foreach (var elem in provinceElements)
		{
			CreateElementGizmo(elem);
		}
		
		var specStart = Man.MapElements.OfType<SpecStart>().SingleOrDefault(x => x.ProvinceNum == provinceNum);
		if (specStart != null)
		{
			CreateNationGizmo(specStart);
		}
	}

	public MonsterGizmo CreateElementGizmo (ProvinceDataElement elem)
	{
		MonsterGizmo gizmo = null;
		switch (elem)
		{
			case Commander commander:
				var commanderGizmo = Ui.Create<CommanderGizmo>(rosterGroup);
				commanderGizmo.Initialize(commander);
				gizmo = commanderGizmo;
				break;
			case Units units:
				var unitsGizmo = Ui.Create<UnitsGizmo>(rosterGroup);
				unitsGizmo.Initialize(units);
				gizmo = unitsGizmo;

				var ownerCommanderGizmo = monsterGizmos.Single(x => x.Element == units.Commander);
				int commanderIdx = ownerCommanderGizmo.transform.GetSiblingIndex();
				unitsGizmo.transform.SetSiblingIndex(commanderIdx + 1);
					
				break;
		}
		if (gizmo == null)
		{
			Debug.LogWarning($"No gizmo for element {elem}");
			return null;
		}
		monsterGizmos.Add(gizmo);
		
		return gizmo;
	}

	public void RemoveElementGizmo (ProvinceDataElement elem)
	{
		var elemGizmo = monsterGizmos.SingleOrDefault(x => x.Element == elem);
		if (elemGizmo != null)
		{
			Destroy(elemGizmo.gameObject);
		}
	}
	
	private void CreateNationGizmo (SpecStart specStart)
	{
		_nationGizmo = Ui.Create<NationGizmo>(transform);
		_nationGizmo.SetPlayerNumber(specStart.NationNum);
	}
}