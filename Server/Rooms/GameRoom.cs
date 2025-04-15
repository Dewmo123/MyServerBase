using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Rooms
{
	class GameRoom : IJobQueue
	{
		public int MaxSessionCount { get; private set; } = 15;//임의
		public int SessionCount => _sessions.Count;

		List<ClientSession> _sessions = new List<ClientSession>();
		JobQueue _jobQueue = new JobQueue();
		List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
		public void Push(Action job)
		{
			_jobQueue.Push(job);
		}

		public void Flush()
		{
			// N ^ 2
			foreach (ClientSession s in _sessions)
				s.Send(_pendingList);

			Console.WriteLine($"Flushed {_pendingList.Count} items");
			_pendingList.Clear();
		}

		public void Broadcast(IPacket packet)
		{
			_pendingList.Add(packet.Serialize());
		}

		public bool Enter(ClientSession session)
		{
			if (_sessions.Count >= MaxSessionCount)
				return false;
			_sessions.Add(session);
			session.Room = this;
			return true;
		}

		public void Leave(ClientSession session)
		{
			_sessions.Remove(session);
		}
	}
}
