using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ProtocolCS
{
    /// <summary>
    /// 클라이언트가 서버로 보내는 매칭 요청
    /// </summary>
    public class JoinQueue : PacketBase
    {
    }

    /// <summary>
    /// [테스트용] 봇게임을 요청한다.
    /// <para />
    /// 이 패킷을 보내면 봇게임으로 매칭된다.
    /// </summary>
    public class JoinBotQueue : JoinQueue
    {
    }

    /// <summary>
    /// 클라이언트가 서버로 보내는 매칭 취소 요청
    /// </summary>
    public class LeaveQueue : PacketBase
    {

    }

    /// <summary>
    /// 매치가 성사되면 서버가 클라에게 보내주는 패킷
    /// </summary>
    public class MatchSuccess : PacketBase
    {
        /// <summary>
        /// 게임 서버 주소
        /// </summary>
        public string gameServerAddress { get; set; }

        /// <summary>
        /// 게임 서버에 연결한 후, 인증하기 위한 토큰
        /// </summary>
        public string matchToken { get; set; }
    }
}
