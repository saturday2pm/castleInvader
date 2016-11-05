using System;
using Simulator;
using System.Linq;

public class AIPlayerObject : Player
{
    static Random r = new Random();

    public override void Init(Match match)
    {
    }

    public override void Update(Match match)
    {
        if (0 != r.Next(0, 30))
            return;

        var selected = OwnCastles.OrderBy(_ => r.Next()).Take(r.Next(1, 1 + (int)(OwnCastles.Count * 0.5)));
        foreach (var castle in selected)
        {
            CastleUpdate(match, castle);
        }
    }

    void CastleUpdate(Match match, Castle castle)
    {
        //전략
        //업그레이드 가능한 성은 코스트 *1.2보다 유닛 수가 많으면 업그레이드 한다.
        //자신이 소유한 모든 성에 대해서, 가장 가까운 빈 성 한 곳에 쳐들어간다.
        //빈 성이 없다면, 자신보다 병력이 약한 적 성 중에서 가장 가까운 성에 쳐들어간다.
        //가장 가까운 적 성과의 거리가 일정 이상 떨어져있다면, 가장 가까운 아군 성에 병력을 보낸다.
        //모든 적 성의 병력이 자신보다 높다면, 가만히 있는다.
        //우선순위는 위에서부터 적은 순서대로고, 반드시 한 번에 한 곳의 성만 쳐들어간다.
        //만약 지금 수행하고 있는 일보다 더 높은 우선순위의 경우가 발생한다면 기존 행동을 중단하고 더 높은 우선순위의 행동으로 변경한다.

        if (castle.IsUpgradable && castle.UnitNum > castle.Cost * 1.2)
            castle.Upgrade();

        Castle emptyCastle = null;
        float emptyValue = 987654321;
        Castle weekCastle = null;
        float weekValue = 987654321;
        Castle allyCastle = null;
        float allyValue = 987654321;

        float maxDist = 400.0f;

        foreach (var c in match.Castles)
        {
            if (c == castle)
                continue;

            var dist = (float)Math.Sqrt(c.Pos.DistSquare(castle.Pos));
            float value = c.UnitNum + dist;

            if (c.Owner == null && dist < maxDist && emptyValue > dist)
            {
                emptyCastle = c;
                emptyValue = dist;
            }

            if (c.Owner != castle.Owner && dist < maxDist && weekValue > value)
            {
                weekCastle = c;
                weekValue = value;
            }

            if (c.Owner == castle.Owner && dist < maxDist && c.UnitNum < castle.UnitNum && allyValue < value)
            {
                allyCastle = c;
                allyValue = value;
            }
        }

        if (emptyCastle != null)
        {
            Attack(castle, emptyCastle);
            return;
        }

        if (weekCastle != null)
        {
            Attack(castle, weekCastle);
            return;
        }

        if (allyCastle != null)
        {
            Attack(castle, allyCastle);
            return;
        }
    }

    void Attack(Castle castle, Castle end)
    {
        castle.Attack(end);
    }
}

