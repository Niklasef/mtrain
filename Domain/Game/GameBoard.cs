using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DominoTile;
using Domain.Train;

namespace Domain.Game
{
    public class GameBoard
    {
        public GameBoard(
            Dictionary<Guid, string> players,
            Type gameState,
            Guid gameId,
            Dictionary<Guid, KeyValuePair<Guid, IEnumerable<DominoTileEntity>>> playerTrains,
            KeyValuePair<Guid, IEnumerable<DominoTileEntity>> mexicanTrain,
            IEnumerable<DominoTileEntity> hand,
            Guid playerIdWithTurn)
        {
            Players = players ?? throw new ArgumentNullException(nameof(players));
            GameState = gameState ?? throw new ArgumentNullException(nameof(gameState));
            GameId = gameId;
            PlayerTrains = playerTrains ?? throw new ArgumentNullException(nameof(playerTrains));
            MexicanTrain = mexicanTrain;
            Hand = hand ?? throw new ArgumentNullException(nameof(hand));
            PlayerIdWithTurn = playerIdWithTurn;
        }

        public Dictionary<Guid, string> Players { get; }
        public Dictionary<Guid, KeyValuePair<Guid, IEnumerable<DominoTileEntity>>> PlayerTrains { get; }
        public KeyValuePair<Guid, IEnumerable<DominoTileEntity>> MexicanTrain { get; }
        public IEnumerable<DominoTileEntity> Hand { get; }
        public Guid PlayerIdWithTurn { get; }
        public Type GameState { get; }
        public Guid GameId { get; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Game State: '{GameState}'");
            stringBuilder.AppendLine($"Player with turn: '{Players.First(p => p.Key == PlayerIdWithTurn).Value}'");

            stringBuilder.AppendLine("");
            stringBuilder.AppendLine($"Mexican train");

            MexicanTrain
                .Value
                    .ToList()
                    .ForEach(t => stringBuilder.AppendLine(string.Join(",", t)));


            PlayerTrains
                .ToList()
                .ForEach(pt =>
                {
                    stringBuilder.AppendLine("");
                    stringBuilder.AppendLine($"Player: '{Players.First(p => p.Key == pt.Key).Value}'");
                    stringBuilder.AppendLine($"{string.Join(",", pt.Value.Value)}");
                });


            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("Hand");
            stringBuilder
                .AppendLine(
                    string.Join(", ", Hand));

            return stringBuilder.ToString();
        }
    }
}