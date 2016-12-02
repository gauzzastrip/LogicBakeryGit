using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicRotatoe.Interfaces;
using MusicRotatoe.Models;
using MusicRotatoe.Utilities;
using Akavache;
using System.Reactive.Linq;

namespace MusicRotatoe.DataAccess
{
    public class MusicRotatoeDao : IMusicRotatoeDao
    {
        public MusicRotatoeDao()
        {

        }
        public async Task<List<Rotatoe>> GetAllRotatoes()
        {
            // Make sure you set the application name before doing any inserts or gets
            BlobCache.ApplicationName = Settings.ApplicationName;

            var results = await BlobCache.UserAccount.GetAllObjects<Rotatoe>();

            return results.ToList();
        }

        public async Task SaveRotatoe(Rotatoe rotatoe)
        {
            BlobCache.ApplicationName = Settings.ApplicationName;
            if (rotatoe != null && rotatoe.Songs.Count() > 0)
            {
                await BlobCache.UserAccount.InsertObject(rotatoe.RotatoeId.ToString(), rotatoe);
            }
        }
        public async Task DeleteRotatoe(Rotatoe rotatoe)
        {
            BlobCache.ApplicationName = Settings.ApplicationName;
            if (rotatoe != null)
            {
                await BlobCache.UserAccount.Invalidate(rotatoe.RotatoeId.ToString());
            }
        }
    }
}
