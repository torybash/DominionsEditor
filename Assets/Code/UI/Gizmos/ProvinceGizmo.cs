using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Entities;
using Data;
using ThisOtherThing.UI.Shapes;
using TMPro;
using UnityEngine;
using Utility.Extensions;

namespace UI.Gizmos
{

	public class ProvinceGizmo : Gizmo
	{
		[SerializeField] private TMP_Text      numLabel;
		[SerializeField] private RectTransform rosterGroup;
		[SerializeField] private Line          borderLineTemplate;
		[SerializeField] private Polygon       borderPolygonTemplate;

		[SerializeField] private GameObject labMarker;
		[SerializeField] private GameObject templeMarker;
		[SerializeField] private GameObject fortMarker;
		[SerializeField] private GameObject throneMarker;

		private List<MonsterGizmo> _monsterGizmos = new List<MonsterGizmo>();
		private List<Line>         _borderLines   = new List<Line>();
		private NationGizmo        _nationGizmo;


		public Province Province { get; private set; }

		private void Awake ()
		{
			borderLineTemplate.gameObject.SetActive(false);
			borderPolygonTemplate.gameObject.SetActive(false);
		}

		public void Initialize (Province province)
		{
			Province = province;

			numLabel.text = Province.ProvinceNumber.ToString();

			var edgePoints = new List<Vector2Int>();
			foreach (var pb in province.ProvinceBorders)
			{
				var minPos = new Vector2Int(pb.X,          pb.Y);
				var maxPos = new Vector2Int(pb.X + pb.Len, pb.Y);
				edgePoints.Add(minPos);
				edgePoints.Add(maxPos);
			}

			//TODO Do this in a smarter way!! Optimize!
			var open         = new List<Vector2Int>(edgePoints);
			var currentPoint = open.First();
			var sortedPoints = new List<Vector2>();
			while (open.Count > 4)
			{
				open.Remove(currentPoint);
				var closestPoint = open.OrderBy(p => Vector2Int.Distance(currentPoint, p)).First();
			
				currentPoint = closestPoint;
			
				sortedPoints.Add(currentPoint);
			}

			var polygon = borderPolygonTemplate.Copy(transform.parent);
			polygon.PointListsProperties.PointListProperties[0].Positions = sortedPoints.ToArray();
			polygon.PointListsProperties.PointListProperties[0].SetPoints();
			polygon.transform.SetParent(transform); //TODO Transform point instead

			foreach (var monster in Province.Monsters)
			{
				switch (monster)
				{
					case Commander commander:
						CreateCommanderGizmo(commander, this);

						foreach (var unit in commander.UnitsUnderCommand)
						{
							CreateUnitGizmo(unit);
						}
						break;
					case Troops unit:
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
			SetOwner(Province.Owner);

			labMarker.SetActive(Province.HasLab);
			templeMarker.SetActive(Province.HasTemple);
			fortMarker.SetActive(Province.HasFort);
			// throneMarker.SetActive(Province.HasThrone);
		}

		public void RemoveElementGizmo (Monster elem)
		{
			var elemGizmo = _monsterGizmos.SingleOrDefault(x => x.MonsterData == elem);
			if (elemGizmo != null)
			{
				Destroy(elemGizmo.gameObject);
			}
		}

		public void SetOwner (Nation nation)
		{
			if (nation == null || nation.Equals(Nation.Independents))
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

			bool isStartLocation = DomEdit.I.MapMan.Map.Players.Any(x => x.CapitalProvinceNum == Province.ProvinceNumber);
			_nationGizmo.ShowCapitalMarker(isStartLocation);
		}

		public void CreateCommanderGizmo (Commander commander, ProvinceGizmo provinceGizmo)
		{
			var commanderGizmo = DomEdit.I.Ui.Create<CommanderGizmo>(rosterGroup);
			commanderGizmo.Initialize(provinceGizmo);
			commanderGizmo.SetData(commander);
			_monsterGizmos.Add(commanderGizmo);
		}

		public void CreateUnitGizmo (Troops troops)
		{
			var unitGizmo = DomEdit.I.Ui.Create<UnitGizmo>(rosterGroup);
			unitGizmo.SetData(troops);
			_monsterGizmos.Add(unitGizmo);

			var ownerCommander = Province.Monsters.OfType<Commander>().SingleOrDefault(x => x.UnitsUnderCommand.Contains(troops));
			if (ownerCommander != null)
			{
				var ownerCommanderGizmo = _monsterGizmos.OfType<CommanderGizmo>().Single(x => x.Data == ownerCommander);
				int commanderIdx        = ownerCommanderGizmo.transform.GetSiblingIndex();
				unitGizmo.transform.SetSiblingIndex(commanderIdx + 1);
			}
		}
	}

}