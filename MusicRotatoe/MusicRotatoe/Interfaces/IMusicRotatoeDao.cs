using MusicRotatoe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRotatoe.Interfaces
{
    public interface IMusicRotatoeDao
    {
        Task<List<Rotatoe>> GetAllRotatoes();
        Task SaveRotatoe(Rotatoe rotatoe);
        Task DeleteRotatoe(Rotatoe rotatoe);
        Task DeleteAllRotatoes();
    }
}
