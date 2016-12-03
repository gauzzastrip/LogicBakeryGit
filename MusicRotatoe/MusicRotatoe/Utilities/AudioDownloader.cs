using MusicRotatoe.Utilities;
using PCLStorage;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace YoutubeExtractor
{
    /// <summary>
    /// Provides a method to download a video and extract its audio track.
    /// </summary>
    public class AudioDownloader : RotatoeDownloader
    {
        private bool isCanceled;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioDownloader"/> class.
        /// </summary>
        /// <param name="video">The video to convert.</param>
        /// <param name="savePath">The path to save the audio.</param>
        /// /// <param name="bytesToDownload">An optional value to limit the number of bytes to download.</param>
        /// <exception cref="ArgumentNullException"><paramref name="video"/> or <paramref name="savePath"/> is <c>null</c>.</exception>
        public AudioDownloader(VideoInfo video, string savePath, int? bytesToDownload = null)
            : base(video, savePath, bytesToDownload)
        { }

        /// <summary>
        /// Occurs when the progress of the audio extraction has changed.
        /// </summary>
        //public event EventHandler<ProgressEventArgs> AudioExtractionProgressChanged;

        /// <summary>
        /// Occurs when the download progress of the video file has changed.
        /// </summary>
       // public event EventHandler<ProgressEventArgs> DownloadProgressChanged;

        /// <summary>
        /// Downloads the video from YouTube and then extracts the audio track out if it.
        /// </summary>
        /// <exception cref="IOException">
        /// The temporary video file could not be created.
        /// - or -
        /// The audio file could not be created.
        /// </exception>
        /// <exception cref="AudioExtractionException">An error occured during audio extraction.</exception>
        /// <exception cref="WebException">An error occured while downloading the video.</exception>
        public override async Task Execute()
        {
            //var documents = FileSystem.Current.LocalStorage;
        
            string tempPath = this.SavePath + Video.VideoExtension;

            await this.DownloadVideo(tempPath);

            if (!this.isCanceled)
            {
                await this.ExtractAudio(tempPath);
            }

            //this.OnDownloadFinished(EventArgs.Empty);
        }

        private async Task DownloadVideo(string path)
        {
            var videoDownloader = new VideoDownloader(this.Video, path, this.BytesToDownload);

            //videoDownloader.DownloadProgressChanged += (sender, args) =>
            //{
            //    if (this.DownloadProgressChanged != null)
            //    {
            //        this.DownloadProgressChanged(this, args);

            //        this.isCanceled = args.Cancel;
            //    }
            //};

            await videoDownloader.Execute();
        }

        private async Task ExtractAudio(string path)
        {
            using (var flvFile = new FlvFile(path, this.SavePath))
            {
                //flvFile.ConversionProgressChanged += (sender, args) =>
                //{
                //    if (this.AudioExtractionProgressChanged != null)
                //    {
                //        this.AudioExtractionProgressChanged(this, new ProgressEventArgs(args.ProgressPercentage));
                //    }
                //};

                await flvFile.ExtractStreams();
            }
        }
    }
    public abstract class RotatoeDownloader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Downloader"/> class.
        /// </summary>
        /// <param name="video">The video to download/convert.</param>
        /// <param name="savePath">The path to save the video/audio.</param>
        /// /// <param name="bytesToDownload">An optional value to limit the number of bytes to download.</param>
        /// <exception cref="ArgumentNullException"><paramref name="video"/> or <paramref name="savePath"/> is <c>null</c>.</exception>
        protected RotatoeDownloader(VideoInfo video, string savePath, int? bytesToDownload = null)
        {
            if (video == null)
                throw new ArgumentNullException("video");

            if (savePath == null)
                throw new ArgumentNullException("savePath");

            this.Video = video;
            this.SavePath = savePath;
            this.BytesToDownload = bytesToDownload;
        }

        /// <summary>
        /// Occurs when the download finished.
        /// </summary>
        public event EventHandler DownloadFinished;

        /// <summary>
        /// Occurs when the download is starts.
        /// </summary>
        public event EventHandler DownloadStarted;

        /// <summary>
        /// Gets the number of bytes to download. <c>null</c>, if everything is downloaded.
        /// </summary>
        public int? BytesToDownload { get; private set; }

        /// <summary>
        /// Gets the path to save the video/audio.
        /// </summary>
        public string SavePath { get; private set; }

        /// <summary>
        /// Gets the video to download/convert.
        /// </summary>
        public VideoInfo Video { get; private set; }

        /// <summary>
        /// Starts the work of the <see cref="Downloader"/>.
        /// </summary>
        public abstract Task Execute();

        protected void OnDownloadFinished(EventArgs e)
        {
            if (this.DownloadFinished != null)
            {
                this.DownloadFinished(this, e);
            }
        }

        protected void OnDownloadStarted(EventArgs e)
        {
            if (this.DownloadStarted != null)
            {
                this.DownloadStarted(this, e);
            }
        }
    }
}