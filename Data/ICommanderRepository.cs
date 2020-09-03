using CommanderAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommanderAPI.Data
{
    public interface ICommanderRepository
    {
        IEnumerable<Command> GetAllCommands();

        Command GetCommand(int id);

    }
}
