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
            Dictionary<Guid, Tuple<Type, IEnumerable<DominoTileEntity>>> trains,
            KeyValuePair<Guid, IEnumerable<DominoTileEntity>> mexicanTrain,
            IEnumerable<DominoTileEntity> hand,
            Dictionary<Guid, Guid> playerTrains,
            Guid playerIdWithTurn,
            Type gameState,
            Guid gameId)
        {
            Players = players;
            Trains = trains;
            MexicanTrain = mexicanTrain;
            Hand = hand;
            PlayerTrains = playerTrains;
            PlayerIdWithTurn = playerIdWithTurn;
            GameState = gameState;
            GameId = gameId;
        }

        public Dictionary<Guid, string> Players { get; }
        public Dictionary<Guid, Tuple<Type, IEnumerable<DominoTileEntity>>> Trains { get; }
        public KeyValuePair<Guid, IEnumerable<DominoTileEntity>> MexicanTrain { get; }
        public IEnumerable<DominoTileEntity> Hand { get; }
        public Dictionary<Guid, Guid> PlayerTrains { get; }
        public Guid PlayerIdWithTurn { get; }
        public Type GameState { get; }
        public Guid GameId { get; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Game State: '{GameState}'");
            stringBuilder.AppendLine($"Player with turn: '{Players.First(p => p.Key == PlayerIdWithTurn).Value}'");

            var trainIndex = 1;
            PlayerTrains
                .ToList()
                .ForEach(pt =>
                {
                    var train = Trains.First(t => t.Key == pt.Value);
                    stringBuilder.AppendLine("");
                    stringBuilder.AppendLine($"Player: '{Players.First(p => p.Key == pt.Key).Value}'");
                    stringBuilder.AppendLine($"train Idx: '{trainIndex}', train id: '{pt.Value}', train state: '{train.Value.Item1}'");
                    stringBuilder.AppendLine(string.Join(",", train.Value.Item2.Reverse()));
                    trainIndex++;
                });

            stringBuilder.AppendLine("");
            stringBuilder.AppendLine($"Mexican train");
            stringBuilder.AppendLine($"Idx: '{trainIndex}'  id: '{MexicanTrain.Key}'");
            stringBuilder.AppendLine(string.Join(",", MexicanTrain.Value.Reverse()));

            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("Hand");
            stringBuilder
                .AppendLine(
                    string.Join(", ", Hand.Select((t, i) => $"{i + 1}: {t} {t.Id}")));

            return stringBuilder.ToString();
        }
    }
}