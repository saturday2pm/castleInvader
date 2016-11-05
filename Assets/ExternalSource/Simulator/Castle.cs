using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator
{
    public class AttackCommand
    {
        public AttackCommand(Waypoint _target, int Duration)
        {
            Target = _target;
            LeftTick = Duration;
        }
        public Waypoint Target { get; set; }
        public int LeftTick { get; set; }
    }

	public class Castle : Waypoint
	{
        Player owner = null;
		public Player Owner { get { return owner; } set { owner = value; } }

        int level = 1;
		public int Level { get { return level; } private set { level = value; } }

		List<CastleUpgradeInfo> UpgradeInfo;
        List<AttackCommand> AttackCommands = new List<AttackCommand>();

        float unitNum;
		float unitRunRatio;
        int unitRunDuration;

		public float UnitIncreaseRatio
		{
			get { return UpgradeInfo[Level - 1].IncreaseRatio; }
		}

		public int UnitNum
		{
			get { return (int)unitNum; }
		}

		public float Radius
		{
			get { return UpgradeInfo[Level - 1].Radius; }
		}

		public int Cost
		{
			get { return UpgradeInfo[Level - 1].Cost; }
		}

		public int MaxNum
		{
			get { return UpgradeInfo[Level - 1].MaxNum; }
		}

		public bool IsUpgradable
		{
			get
			{
				if (Level == UpgradeInfo.Count)
					return false;

				return UnitNum >= Cost;
			}
		}

		public Castle(int id, int startNum, float x, float y, float runRatio, int runDuration, List<CastleUpgradeInfo> info) : base(id)
		{
			Pos = new Point(x, y);
			unitNum = startNum;

			unitRunRatio = runRatio;
            unitRunDuration = runDuration;
			UpgradeInfo = info;
		}

		public Castle(int id, Point pos, float runRatio, int runDuration, List<CastleUpgradeInfo> info) : base(id)
		{
			Pos = pos;

			unitRunRatio = runRatio;
            unitRunDuration = runDuration;
            UpgradeInfo = info;
		}

		public void Update(Match match)
		{
			if (Owner == null)
			{
				return; // 야만인 땅은 걍 가만히 있음
			}

			unitNum += UnitIncreaseRatio;
            int attackNum = (int)(unitNum * unitRunRatio / AttackCommands.Count);

            //unit 일부를 다른 지역으로 파견함
            foreach (var attack in AttackCommands)
            {
                match.CreateUnit(attackNum, Owner, Radius, this, attack.Target);
                unitNum -= attackNum;
                attack.LeftTick--;
            }

            AttackCommands.RemoveAll(x => x.LeftTick < 1);

			if (unitNum > MaxNum)
				unitNum = MaxNum;
		}

		public void Attack(Waypoint point)
		{
			if (point == this)
				return;

            var command = AttackCommands.Find(x => x.Target == point);
            if (command != null)
                command.LeftTick = unitRunDuration;
            else
                AttackCommands.Add(new AttackCommand(point, unitRunDuration));
		}

		public void AddUnit(Unit unit)
		{
			if (unit.Owner != Owner)
				return;

			unitNum += unit.Num;
		}

		public void Attacked(Unit unit)
		{
			if (unit.Owner == Owner)
				return;

			unitNum -= unit.Num;

			if (unitNum <= 0.0f)
			{
				unitNum = -unitNum;

				if(Owner != null)
					Owner.RemoveCastle(this);

				Owner = unit.Owner;
				Owner.AddCastle(this);
			}
		}

		public void Upgrade()
		{
			if (!IsUpgradable)
				return;

			unitNum -= Cost;
			Level++;
		}

        public ProtocolCS.Castle ToProtocolCastle()
        {
            ProtocolCS.Castle castle = new ProtocolCS.Castle();
            castle.id = Id;
            castle.type = (ProtocolCS.CastleType)Level;
            return castle;
        }
    }
}
