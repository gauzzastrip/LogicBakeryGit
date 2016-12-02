using MusicRotatoe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRotatoe.Interfaces
{
    public interface IMusicRotatoeService
    {
        Task DeleteRotatoe(Rotatoe rotatoe);
        Task SaveRotatoe(Rotatoe rotatoe);
        Task<List<Rotatoe>> GetAllRotatoes();
        Task<List<string>> GetAllGenres();
        Task<Rotatoe> RefreshMusic(Rotatoe rotatoe);
    }
}
