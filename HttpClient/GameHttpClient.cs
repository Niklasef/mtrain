using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Domain.Game;
using Newtonsoft.Json;
using Server;

namespace HttpClient
{
    public class GameHttpClient
    {
        private readonly System.Net.Http.HttpClient httpClient;
        private static readonly Uri CommandApiUri = new Uri("http://localhost:5000/api/command/");
        private static readonly Uri QueryApiUri = new Uri("http://localhost:5000/api/query/");

        public GameHttpClient(System.Net.Http.HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public void CreateGame(Guid gameId) =>
            Send(
                new CreateGameCommand(gameId));

        private void Send(ICommand command)
        {
            var response = httpClient.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Post,
                    CommandApiUri)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(
                            command,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            }),
                        Encoding.UTF8,
                        "application/json")
                }).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.Content?.ReadAsStringAsync()?.Result ?? string.Empty);
            }
        }

        public void JoinGame(
            Guid gameId,
            Guid playerId,
            string playerName
        ) =>
            Send(
                new JoinGameCommand(
                    gameId,
                    playerId,
                    playerName));


        public void StartGame(Guid gameId) =>
            Send(
                new StartGameCommand(gameId));

        public void MakeMove(
            Guid gameId,
            Guid playerId,
            Guid trainId,
            long dominoId
        ) =>
            Send(
                new MakeMoveCommand(
                    gameId,
                    playerId,
                    trainId,
                    dominoId));

        public void PassMove(
            Guid gameId,
            Guid playerId
        ) =>
            Send(
                new PassMoveCommand(
                    gameId,
                    playerId));

        public GameBoard GetGameBoardAsObserver(Guid gameId) =>
            Send<GameBoard>(
                new GetGameBoardAsObserverQuery(gameId));

        public GameBoard GetGameBoard(
            Guid gameId, 
            Guid playerId
        ) =>
            Send<GameBoard>(
                new GetGameBoardQuery(
                    gameId, 
                    playerId));

        public IEnumerable<GameBoard> GetGameBoards() =>
            Send<IEnumerable<GameBoard>>(
                new GetGameBoardsQuery());

        private T Send<T>(IQuery query)
        {
            var response = httpClient.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Post,
                    QueryApiUri)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(
                            query,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            }),
                        Encoding.UTF8,
                        "application/json")
                }).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.Content?.ReadAsStringAsync()?.Result ?? string.Empty);
            }

            return (T)JsonConvert
                .DeserializeObject(
                    response.Content?.ReadAsStringAsync()?.Result,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
        }
    }
}