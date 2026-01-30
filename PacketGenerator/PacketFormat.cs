using System;
using System.Collections.Generic;
using System.Text;

namespace PacketGenerator
{
    class PacketFormat
    {
        // {0} 패킷 등록
        public static string managerFormat =
@"using ServerCore;
using System;
using System.Collections.Generic;
using ServerCore.Serializers;


class PacketManager
{{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance {{ get {{ return _instance; }} }}
	#endregion

	PacketManager()
	{{
		Register();
	}}

	Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>>();
	Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();
		
	public void Register()
	{{
{0}
	}}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
	{{
		ushort packetId = PacketUtility.ReadPacketID(buffer);

		Action<PacketSession, ArraySegment<byte>> action = null;
		if (_onRecv.TryGetValue(packetId, out action))
			action.Invoke(session, buffer);
	}}

	void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
	{{
		T pkt = new T();
		PacketReader reader = new PacketReader(buffer);
		pkt.Serialize(ref reader);
		Action<PacketSession, IPacket> action = null;
		if (_handler.TryGetValue(pkt.Protocol, out action))
			action.Invoke(session, pkt);
	}}
}}";

        // {0} 패킷 이름
        public static string managerRegisterFormat =
@"		_onRecv.Add((ushort)PacketID.{0}, MakePacket<{0}>);
		_handler.Add((ushort)PacketID.{0}, PacketHandler.{0}Handler);";

        // {0} 패킷 이름/번호 목록
        // {1} 패킷 목록
        public static string fileFormat =
@"using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;
using ServerCore.Serializers;

public enum PacketID
{{
	{0}
}}
{1}
";

        // {0} 패킷 이름
        // {1} 패킷 번호
        public static string packetEnumFormat =
@"{0} = {1},";


        // {0} 패킷 이름
        // {1} 멤버 변수들
        // {2} 멤버 변수 Serialize
        public static string packetFormat =
@"
public struct {0} : IPacket
{{
	public {0}(){{}}

	{1}

	public ushort Protocol => _protocol;
	private ushort _protocol = (ushort)PacketID.{0};

    public void Serialize<T>(ref T serializer) where T : struct, IPacketSerializer
	{{
		serializer.Serialize(ref _protocol);
		{2}
		serializer.SerializeOffset();
	}}
}}
";
        public static string dataPacketFormat =
            @"
public struct {0} : IDataPacket
{{
	public {0}(){{}}

	{1}

    public void Serialize<T>(ref T serializer) where T : struct, IPacketSerializer
    {{
        {2}
    }}
}}
";

        // {0} 변수 형식
        // {1} 변수 이름
        public static string memberFormat =
@"public {0} {1} = default;";


        // {0} 변수 형식
        // {1} 변수 이름
        public static string memberListFormat =
@"public List<{0}> {1} = default;";
		/// <summary>
		///{0} 변수 이름 <br/>
		/// </summary>
		/// 
		public static string serializeFormat =
@"serializer.Serialize(ref {0});";
    }
}