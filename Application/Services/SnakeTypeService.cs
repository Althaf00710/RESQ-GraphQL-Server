using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Generic;
using Core.Models;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;

namespace Application.Services
{
    internal class SnakeTypeService : Service<SnakeType>, ISnakeTypeService
    {
        private readonly ISnakeTypeRepository _repository;
        public SnakeTypeService(ISnakeTypeRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
