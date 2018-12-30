using System;

namespace Server
{
    public class GameServer
    {
        public TResult ExecuteQuery<TResult>(IQuery<TResult> query)
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
