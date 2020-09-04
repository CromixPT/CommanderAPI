using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using AutoMapper;
using CommanderAPI.Data;
using CommanderAPI.DTOs;
using CommanderAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CommanderAPI.Controllers
{
    //api/commands
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase //Controller without view support.
    {
        private readonly ICommanderRepository _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/commands
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDTO>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();
            if (commandItems != null)
            {
                return Ok(_mapper.Map<IEnumerable<CommandReadDTO>>(commandItems));
            }
            return NotFound();

        }

        //GET api/commands/id
        [HttpGet("{id}",Name ="GetCommandById")]
        public ActionResult<CommandReadDTO> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommand(id);

            if(commandItem != null)
            {
                return Ok(_mapper.Map<CommandReadDTO>(commandItem)); ;
            }
            return NotFound();
            
        }

        //POST api/commands
        [HttpPost]
        public ActionResult<CommandReadDTO> CreateCommand(CommandCreateDTO newCommand)
        {
            var commandModel = _mapper.Map<Command>(newCommand);
            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();

            var commandReturnValue = _mapper.Map<CommandReadDTO>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReturnValue.Id }, commandReturnValue);
        }

        //PUT api/commands/id
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id,CommandCreateDTO commandUpdateValue)
        {
            var commandFromRepo = _repository.GetCommand(id);
            if(commandFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(commandUpdateValue, commandFromRepo);
            
            //_repository.UpdateCommand(commandFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }


        //PATCH api/commands/id
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id,JsonPatchDocument<CommandCreateDTO> patchDoc)
        {
            var commandFromRepo = _repository.GetCommand(id);
            if (commandFromRepo == null)
            {
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommandCreateDTO>(commandFromRepo);


            //Mapping to ModelState to be able to validate the json
            patchDoc.ApplyTo(commandToPatch, ModelState);

            if(!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            //Maps into the command to have DbContext track changes and update data.
            _mapper.Map(commandToPatch, commandFromRepo);

            _repository.SaveChanges();
            return NoContent();
        }


        //DELETE api/commands/id
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandFromRepo = _repository.GetCommand(id);
            if(commandFromRepo == null)
            {
                return NotFound();
            }
            _repository.DeleteCommand(commandFromRepo);
            _repository.SaveChanges();
            return NoContent();
        }
    }
}
