using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator
{
    public class Match
    {
		Random r;
        List<Player> players = new List<Player>();
        List<Castle> castles = new List<Castle>();
        List<MatchEvent> eventQueue = new List<MatchEvent>();

        public List<Player> Players { get { return players; } private set { players = value; } }
		public List<Castle> Castles { get { return castles; } private set { castles = value; } }
        public List<MatchEvent> EventQueue { get { return eventQueue; } private set { eventQueue = value; } }

		int waypointId = 0;
        int unitId = 0;

		//unit을 그 unit이 속한 path로 구분해서 기억함. unit은 먼저 출발한 놈이 항상 먼저 도착
		public Dictionary<Path, List<Unit> > Units = new Dictionary<Path, List<Unit> >();

		public int Frame { get; private set; }

		public MatchOption Option { get; private set; }

		public Match(MatchOption option, List<Player> players, int seed)
		{
            r = new Random(seed);
			Option = option;
			Players = players;
		}

		void MakeWaypoints()
		{
			//일단 지금은 성만 생성
			for (int i = 0; i < Option.CastleNum; i++)
			{
				int x = 0; 
				int y = 0;

				do
				{
                    int padding = 50;
					x = r.Next(padding, Option.Width - padding);
					y = r.Next(padding, Option.Height - padding);
				} while (Castles.Any(c => (c.Pos.X - x) * (c.Pos.X - x) + (c.Pos.Y - y) * (c.Pos.Y - y) < Option.CastleDistance * Option.CastleDistance));
                
				Castles.Add(new Castle(waypointId, Option.CastleStartUnitNum, x, y, Option.UnitRunRatio, Option.UnitRunDuration, Option.UpgradeInfo));
				waypointId++;
			}
		}

		public void InitPlayerCastle()
		{
			foreach (var p in Players)
			{
				int castleIndex = 0;

				do
				{
					castleIndex = r.Next(0, Castles.Count);
				} while (Players.Any(player => 
							player.OwnCastles.Any(c => // 기존 플레이어의 모든 성에 대해서 거리 조건 만족하는지 확인
								c.Pos.DistSquare(Castles[castleIndex].Pos) < Option.PlayerDistance * Option.PlayerDistance)));

				Castles[castleIndex].Owner = p;
				p.AddCastle(Castles[castleIndex]);
			}
		}

		public void Init()
		{
			//맵 생성
			MakeWaypoints();

			//플레이어한테 성 할당
			InitPlayerCastle();

			//플레이어별 초기화 동작
			foreach (var player in Players)
			{
				player.Init(this);
			}
		}

		public void Update()
		{
            eventQueue.Clear();

			//각 path에 있는 유닛들에 대한 결과를 처리한다
			foreach (var unitQueue in Units.Values)
			{
				if (unitQueue.Count == 0)
					continue;

				for (int i =0; i < unitQueue.Count;)
				{
					var unit = unitQueue[i];

					switch (unit.Battle(this))
					{
						case BattleResult.AttackCastle:
						case BattleResult.Draw:
						case BattleResult.Lose:
                            {
                                unitQueue.RemoveAt(i);
                                eventQueue.Add(new UnitDeadEvent(unit.Id));
                                break;
                            }
						case BattleResult.Win:
						case BattleResult.NotBattle:
                            {
                                unit.Move(this);
                                i++;
                                break;
                            }
						default:
							throw new NotImplementedException();
					}
				}
			}

			//각 성 현재 상황을 업데이트한다
			foreach (var castle in Castles)
			{
				castle.Update(this);
			}


            //플레이어의 액션에 따른 변화를 반영한다
            foreach (var player in Players)
            {
                //가진 성이 0개, 유닛도 없음 -> 멸망!
                if (player.OwnCastles.Count == 0 && Units.All(pair => pair.Value.All(u => u.Owner != player)))
                {
                    player.IsGoingToEnd = true;
                    eventQueue.Add(new PlayerDeadEvent(player.Id));
                }
                else
                {
                    player.Update(this);
                }
			}
            Players.RemoveAll(p => p.IsGoingToEnd);

            Frame++;
		}

		public bool IsEnd()
		{
			return Players.Count <= 1;
		}

		public Unit FindEnemy(Unit unit, Path path)
		{
			if (!Units.ContainsKey(path.Reverse))
				return null;

			var queue = Units[path.Reverse];

			if (queue.Count == 0)
				return null;

			return queue.FirstOrDefault(u => unit.IsAttackable(u));
		}

		public bool IsMovable(Unit unit)
		{
			//나보다 더 앞에 있는 유닛이 내가 이동할 수 있는 위치 안 쪽이면 못 움직임
			return Units[unit.Road].FirstOrDefault(u => u.CompareTo(unit) > 0 && u.Pos.DistSquare(unit.Pos) < Option.UnitSpeed * Option.UnitSpeed) == null;
		}

		public void CreateUnit(int num, Player owner, float startOffset, Waypoint start, Waypoint end)
		{
			var path = new Path(start, end);
			if (!Units.ContainsKey(path))
				Units.Add(path, new List<Unit>());

			Units[path].Add(new Unit(unitId, num, start.Pos + startOffset * path.Dir, Option.UnitSpeed, Option.UnitAttackRange, owner, start, end));
            unitId++;
		}

		//성이 공격 범위 내에 있는지 계산해서 돌려줌
		public bool IsInRange(Unit unit, Castle castle)
		{
			float range = Option.UnitAttackRange + castle.Radius;
			return unit.Pos.DistSquare(castle.Pos) < range * range;
		}
    }
}
