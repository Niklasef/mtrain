using System;

namespace Server
{
    public class GameServer
    {
        public object ExecuteQuery(IQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return query.Execute();
        }

        public void ExecuteCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.Execute();
        }
    }
}
