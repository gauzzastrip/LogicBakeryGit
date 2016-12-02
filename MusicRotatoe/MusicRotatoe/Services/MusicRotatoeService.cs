using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicRotatoe.Interfaces;
using MusicRotatoe.DataAccess;
using MusicRotatoe.Models;
using System.IO;
using VideoLibrary;
using Flavor;
using PCLStorage;
using System.Net;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using MusicRotatoe.Utilities;
using Xamarin.Forms;

namespace MusicRotatoe.Services
{
    public class MusicRotatoeService : IMusicRotatoeService
    {
        private IMusicRotatoeDao musicRotatoeDao;
        private SpotifyWebAPI spotifyApi;
        private YouTubeService youtubeService;
        public MusicRotatoeService() : this(new MusicRotatoeDao())
        {

        }
        public MusicRotatoeService(IMusicRotatoeDao _musicRotatoeDao)
        {
            musicRotatoeDao = _musicRotatoeDao;
            InitializeAPIs();
        }
        public async Task<List<string>> GetAllGenres()
        {
            var genres = await spotifyApi.GetRecommendationSeedsGenresAsync();

            return genres.Genres;
        }
        public async Task<List<Rotatoe>> GetAllRotatoes()
        {
            return await musicRotatoeDao.GetAllRotatoes();
        }
        public async Task SaveRotatoe(Rotatoe rotatoe)
        {
            await musicRotatoeDao.SaveRotatoe(rotatoe);
        }
        public async Task DeleteRotatoe(Rotatoe rotatoe)
        {
            await musicRotatoeDao.DeleteRotatoe(rotatoe);
        }
        public async Task<Rotatoe> RefreshMusic(Rotatoe rotatoe)
        {
            var recommendations = new Recommendations();
            if (spotifyApi == null)
            {
                InitializeAPIs();
            }
            try
            {
                TuneableTrack minTuneable = new TuneableTrack();

                TuneableTrack maxTuneable = new TuneableTrack();
                minTuneable.Popularity = (rotatoe.MinPopularity.LimitToRange(0, 100));
                maxTuneable.Popularity = (rotatoe.MaxPopularity.LimitToRange(0, 100));
                minTuneable.Energy = ((float)rotatoe.MinEnergy.LimitToRange(0, 100) / 100);
                maxTuneable.Energy = ((float)rotatoe.MaxEnergy.LimitToRange(0, 100) / 100);
                recommendations = await spotifyApi.GetRecommendationsAsync(null,
                    rotatoe.Genres,
                    null,
                    null,
                    minTuneable,
                    maxTuneable,
                    rotatoe.TotalSongs * 2,
                    null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            if (recommendations.Tracks.Count() > 0)
            {
                var keepers = recommendations.Tracks.OrderBy(x => Guid.NewGuid()).Take(rotatoe.TotalSongs);
                var previousSongs = rotatoe.Songs;
                rotatoe.Songs = rotatoe.Songs.Where(w => w.Keep).ToList(); //keep the keepers
                var tasks = new List<Task>();
                tasks.Add(Task.Run(async () =>
                {
                    foreach (var song in keepers)
                    {
                        var songTitle = string.Format("{0} - {1}", song.Artists.FirstOrDefault().Name, song.Name);
                        if (!rotatoe.Songs.Any(w => w.Title == songTitle))
                        {
                            var youtubeId = await GetYoutubeId(songTitle);

                            if (!(string.IsNullOrEmpty(youtubeId)))
                            {
                                var file = await GetSong(youtubeId);

                                rotatoe.Songs.Add(new Song()
                                {
                                    File = file,
                                    Title = songTitle,
                                    YoutubeId = youtubeId
                                });
                            }
                        }
                    }
                }));
                Task.WaitAll(tasks.ToArray());
                await SaveRotatoe(rotatoe);
                var allRotatoes = await GetAllRotatoes();
                var removers = previousSongs.Where(w => !allRotatoes.Any(r => r.Songs.Contains(w)));
                foreach (var song in removers)
                {
                    await RemoveSong(song.File);
                }

            }

            return rotatoe;
        }
        private async Task RemoveSong(string fileName)
        {
            var documents = FileSystem.Current.LocalStorage;
            var fileExists = await documents.CheckExistsAsync(fileName);
            if (fileExists == ExistenceCheckResult.FileExists)
            {
                var file = await documents.GetFileAsync(fileName);
                file.DeleteAsync();
            }
        }
        private async Task<string> GetYoutubeId(string songTitle)
        {
            try
            {
                var searchListRequest = youtubeService.Search.List("snippet");

                searchListRequest.Q = songTitle;
                searchListRequest.MaxResults = 3;
                var searchListResponse = await searchListRequest.ExecuteAsync();
                var videos = searchListResponse.Items.Where(w => w.Id.Kind == "youtube#video");

                return videos.FirstOrDefault().Id.VideoId;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return string.Empty;
        }
        private async Task<string> GetSong(string youtubeId)
        {
            try
            {
                var youtubeLink = string.Format("https://www.youtube.com/watch?v={0}", youtubeId);
                var documents = FileSystem.Current.LocalStorage;
                var fileName = string.Format("{0}.mp4", youtubeId);
                var fileExists = await documents.CheckExistsAsync(fileName);
                if (fileExists == ExistenceCheckResult.NotFound)
                {
                    var youTube = YouTube.Default; // starting point for YouTube actions

                    var allVideos = await youTube.GetAllVideosAsync(youtubeLink); // gets a Video object with info about the video\
                    var video = allVideos.Where(w => w.Format == VideoFormat.Mp4).FirstOrDefault();
                    if (video != null && !(string.IsNullOrEmpty(video.Title)))
                    {

                        var videoBytes = await video.GetBytesAsync();

                        // create a file, overwriting any existing file


                        var file = await documents.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                        using (var stream = await file.OpenAsync(FileAccess.ReadAndWrite))
                        {
                            var videoBuffer = await video.GetBytesAsync();
                            await stream.WriteAsync(videoBuffer, 0, videoBuffer.Length);
                        }

                    }
                }
                return fileName;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return string.Empty;
        }
        private void InitializeAPIs()
        {
            try
            {
                var auth = new ClientCredentialsAuth()
                {
                    //Your client Id
                    ClientId = Settings.SpotifyClientId,
                    //Your client secret UNSECURE!!
                    ClientSecret = Settings.SpotifyClientSecret,
                    //How many permissions we need?
                    Scope = Scope.None,
                };
                //With this token object, we now can make calls
                Token token = auth.DoAuth();
                spotifyApi = new SpotifyWebAPI()
                {
                    TokenType = token.TokenType,
                    AccessToken = token.AccessToken,

                    UseAuth = true
                };

                youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = Settings.YoutubeApiKey,
                    ApplicationName = this.GetType().ToString()
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

        }
    }
}
