using MusicRotatoe.Models;
using MusicRotatoe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRotatoeConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => MainAsync(args)).Wait();
            Console.ReadKey();
        }

        static async void MainAsync(string[] args)
        {
            Console.WriteLine("Starting");
            var service = new MusicRotatoeService();
            //await service.DeleteAllRotatoes();
            var allGenres = await service.GetAllGenres();
            var allRotatoes = await service.GetAllRotatoes();
            Rotatoe rotatoe = allRotatoes.FirstOrDefault();
            if (rotatoe == null)
            {
                rotatoe = new Rotatoe()
                {
                    Title = "Super upbeat",
                    Genres = new List<string>() { "rock","punk"},
                    Interval = 1000,
                    StartDate = DateTime.Now,
                    SongsToDownload = 3,
                    MinEnergy = 90
                };
            }

            Console.WriteLine("Refreshing Music");
            var results = await service.RefreshMusic(rotatoe);
            if (results.Songs.Count() > 0)
            {
                results.Songs.OrderBy(o => Guid.NewGuid()).FirstOrDefault().Keep = true;

                Console.WriteLine("Keeping 1 Song");
                await service.SaveRotatoe(results);
                Console.WriteLine("Refreshing 1 Song");
                results = await service.RefreshSong(rotatoe, results.Songs.OrderBy(o => o.Keep).LastOrDefault().SongId);
                Console.WriteLine("DONE");
            }
            else
            {
                Console.WriteLine("NO SONGS FOUND");
            }
            var rotatoeSadObscure = new Rotatoe()
            {
                Title = "Sad Obscure",
                Genres = new List<string>() { "emo"},
                Interval = 1000,
                StartDate = DateTime.Now,
                SongsToDownload = 3,
                MaxEnergy = 50,
                MinPopularity= 75
            };
            Console.WriteLine("Refreshing Music");
            var resultssad = await service.RefreshMusic(rotatoeSadObscure);
        }
    }
}
