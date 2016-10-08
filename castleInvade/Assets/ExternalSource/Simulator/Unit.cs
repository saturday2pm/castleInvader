using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator
{
	public enum BattleResult
	{
		NotBattle, // 아무하고도 안 마주침
		AttackCastle, //성 공격(무조건 뒤짐)
		Win, //상대와 싸워이김(살았음)
		Draw, //비김(같은 숫자) - 둘다 죽음
		Lose, //상대와 싸워서 졌음(죽었음)
	}

	public class Point
	{
		public float X { get; set; }
		public float Y { get; set; }

		public Point()
		{
			X = 0;
			Y = 0;
		}

		public Point(float x, float y)
		{
			X = x;
			Y = y;
		}

		public float DistSquare(Point other)
		{
			float xDist = X - other.X;
			float yDist = Y - other.Y;

			return xDist * xDist + yDist * yDist;
		}

		public static Point operator *(float multiply, Point pos)
		{
			return new Point(pos.X * multiply, pos.Y * multiply);
		}

		public static Point operator +(Point lhs, Point rhs)
		{
			return new Point(lhs.X + rhs.X, lhs.Y + rhs.Y);
		}
	}

	public class Unit : IComparable<Unit>
	{
		//유닛의 현재 좌표
		public Point Pos { get; private set; }
		//해당 유닛의 병력 크기
		public int Num { get; private set; }

		public Player Owner { get; private set; }

		//이 유닛이 있는 경로 - 전투 판단할 때 쓰임
		public Path Road { get; private set; }

		float unitSpeed;
		float attackRange;

		public Unit(int num, float x, float y, float speed, float range, Player owner, Waypoint startPoint, Waypoint endPoint)
		{
			Num = num;

			Pos.X = x;
			Pos.Y = y;

			unitSpeed = speed;
			attackRange = range;

			Owner = owner;

			Road = new Path(startPoint, endPoint);
		}

		public Unit(int num, Point pos, float speed, float range, Player owner, Waypoint startPoint, Waypoint endPoint)
		{
			Num = num;

			Pos = pos;

			unitSpeed = speed;
			attackRange = range;

			Owner = owner;

			Road = new Path(startPoint, endPoint);
		}

		public bool IsAttackable(Unit other)
		{
			return Pos.DistSquare(other.Pos) < attackRange * attackRange;
		}

		public BattleResult Battle(Match match)
		{
			// 반대편 전투 결과 반영
			if (Num == 0)
			{
				return BattleResult.Draw;
			}
			else if (Num < 0)
			{
				return BattleResult.Lose;
			}

			var enemy = match.FindEnemy(this, Road);

			//적이 없음
			if (enemy == null)
			{
				//waypoint에 도달했는지 확인 - TODO : 나중에 waypoint에 castle말고 딴 거 추가되면 그 쪽 처리해야함
				var castle = Road.End as Castle;

				if (castle == null)
					throw new NotImplementedException();

				if (match.IsInRange(this, castle))
				{
					if (castle.Owner == Owner)
					{
						castle.AddUnit(this);
					}
					else
					{
						castle.Attacked(this);
					}

					return BattleResult.AttackCastle;
				}

				//적도 없고 waypoint에도 도착 안함 -> 아예 전투가 없었음
				return BattleResult.NotBattle;
			}

			//적이 있는 경우 적과 싸운다

			//적 유닛이 더 많다 -> 나의 패배
			if (enemy.Num > Num)
			{
				enemy.Num -= Num;
				return BattleResult.Lose;
			}
			else if (enemy.Num == Num)
			{
				enemy.Num = 0;
				return BattleResult.Draw;
			}
			else
			{
				Num -= enemy.Num;
				enemy.Num = -1;
				return BattleResult.Win;
			}
		}

		//속도에 맞게 지정된 경로 따라 이동함
		public void Move(Match match)
		{
			if(match.IsMovable(this))
				Pos += unitSpeed * Road.Dir;
		}

		//endPoint에 가까울 수록 더 큰 값(뒤에 오는 값)으로 취급(SortedSet)

		public int CompareTo(Unit other)
		{
			var dist = Road.End.Pos.DistSquare(Pos);
			var otherDist = other.Road.End.Pos.DistSquare(other.Pos);

			if (dist < otherDist)
				return 1; //내가 더 가까우므로 내가 더 큼
			else if (dist == otherDist)
				return 0;
			else
				return -1;
		}
	}
}
