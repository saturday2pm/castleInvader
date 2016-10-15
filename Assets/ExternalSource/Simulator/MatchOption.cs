using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator
{
    public class CastleUpgradeInfo
	{
		public int Cost { get; set; }
		public int MaxNum { get; set; }
		public float Radius { get; set; }
		public float IncreaseRatio { get; set; }

		public CastleUpgradeInfo(int cost, int maxNum, float radius, float increaseRatio)
		{
			Cost = cost;
			MaxNum = maxNum;
			Radius = radius;
			IncreaseRatio = increaseRatio;
		}
	}

    public class MatchOption
	{
		public int CastleNum { get; set; }
		//처음 생성시 성 간의 최소 거리
		public float CastleDistance { get; set; }
		//게임 시작시 플레이어간 최소 거리
		public float PlayerDistance { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }

		//성에 처음 있는 유닛 숫자
		public int CastleStartUnitNum { get; set; }

		public float UnitRunRatio { get; set; }


		public float UnitSpeed { get; set; }
		//unit의 공격 범위
		public float UnitAttackRange { get; set; }

		public List<CastleUpgradeInfo> UpgradeInfo { get; set; }
	}
}
