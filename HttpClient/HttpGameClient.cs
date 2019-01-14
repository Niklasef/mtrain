using System;
using System.Collections.Generic;
using Domain.Game;

namespace HttpClient
{
    public class HttpGameClient
    {
        public void CreateGame(Guid gameId) { }
        public void JoinGame(Guid gameId, Guid playerId, string playerName) { }
        public void StartGame(Guid gameId) { }
        public void MakeMove(Guid gameId, Guid playerId, Guid trainId, long dominoId) { }
        public void PassMove(Guid gameId, Guid playerId) { }
        public GameBoard GetGameBoard(Guid gameId) {throw new NotImplementedException(); }
        public GameBoard GetGameBoardAsObserver(Guid playerId) {throw new NotImplementedException(); }
        public IEnumerable<GameBoard> GetGameBoards() {throw new NotImplementedException(); }
    }
}