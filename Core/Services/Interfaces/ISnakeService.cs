using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO;
using Core.Models;
using Core.Services.Generic;

namespace Core.Services.Interfaces
{
    public interface ISnakeService : IService<Snake>
    {
        Task<Snake> Add(SnakeCreateInput dto, IFile? picture);
        Task<Snake> Update(int id, SnakeUpdateInput dto, IFile? picture);
        Task<SnakePredicted> SnakePredictor(IFile file);
    }
}
