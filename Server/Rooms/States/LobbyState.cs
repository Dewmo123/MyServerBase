using System;
using System.Collections.Generic;

namespace Server.Rooms.States
{
    class LobbyState : GameRoomState
    {
        private Random _rand;
        List<TeamInfoPacket> infos;

        public LobbyState(GameRoom room) : base(room)
        {
            _rand = new Random();
            infos = new List<TeamInfoPacket>();
        }
        public override void Enter()
        {
            base.Enter();
            infos.Clear();
        }
        public override void Update()
        {
            base.Update();
            if (!_room.CanAddPlayer)
                _room.ChangeState("Prepare");
        }
        public override void Exit()
        {
            base.Exit();
            ushort[] teams = GetRamdomTeams();
            var keys = _room.GetSessionKeys();
            for (int i = 0; i < _room.SessionCount; i++)
            {
                infos.Add(new TeamInfoPacket()
                {
                    index = keys[i],
                    team = teams[i]
                });
                Console.WriteLine($"index:{keys[i]}, Team:{teams[i]}");
                
            }
            _room.Broadcast(new S_TeamInfos() { teamInfos = infos });
        }

        private ushort[] GetRamdomTeams()
        {
            ushort[] arr = new ushort[_room.SessionCount];
            if (arr.Length % 2 == 1)
                throw new NullReferenceException();
            for (int i = 1; i <= arr.Length / 2; i++)
            {
                arr[i - 1] = 1;
                arr[arr.Length - i] = 0;
            }
            for (int i = 0; i < 100; i++)
            {
                int idx = _rand.Next(1, arr.Length);
                (arr[0], arr[idx]) = (arr[idx], arr[0]);
            }

            return arr;
        }
    }
}
