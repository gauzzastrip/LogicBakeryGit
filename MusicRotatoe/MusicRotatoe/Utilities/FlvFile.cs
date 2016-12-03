using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRotatoe.Utilities
{
    internal class FlvFile : IDisposable
    {
        private long fileLength;
        private readonly string inputPath;
        private readonly string outputPath;
        private AacAudioExtractor audioExtractor;
        private long fileOffset;
        private Stream fileStream;
       
        /// <summary>
        /// Initializes a new instance of the <see cref="FlvFile"/> class.
        /// </summary>
        /// <param name="inputPath">The path of the input.</param>
        /// <param name="outputPath">The path of the output without extension.</param>
        public FlvFile(string inputPath, string outputPath)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
            
         //   this.fileStream = new IFile()this.inputPath, FileMode.Open, FileAccess.Read, Akavache.Internal.FileShare.Read, 64 * 1024);
            this.fileOffset = 0;
          //  this.fileLength = fileStream.Length;
        }

       // public event EventHandler<ProgressEventArgs> ConversionProgressChanged;

        public bool ExtractedAudio { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);

        }

        /// <exception cref="AudioExtractionException">The input file is not an FLV file.</exception>
        public async Task ExtractStreams()
        {
            await this.Seek(0);

            if (await this.ReadUInt32() != 0x464C5601)
            {
                // not a FLV file
                throw new AudioExtractionException("Invalid input file. Impossible to extract audio track.");
            }

            await this.ReadUInt8();
            uint dataOffset = await this.ReadUInt32();

            await this.Seek(dataOffset);

            await this.ReadUInt32();

            while (fileOffset < fileLength)
            {
                if (!await ReadTag())
                {
                    break;
                }

                if (fileLength - fileOffset < 4)
                {
                    break;
                }

                await this.ReadUInt32();

                double progress = (this.fileOffset * 1.0 / this.fileLength) * 100;

                //if (this.ConversionProgressChanged != null)
                //{
                //    this.ConversionProgressChanged(this, new ProgressEventArgs(progress));
                //}
            }

            this.CloseOutput(false);
        }

        private async void CloseOutput(bool disposing)
        {
            if (this.audioExtractor != null)
            {
                if (disposing && this.audioExtractor.VideoPath != null)
                {
                    try
                    {
                        var documents = FileSystem.Current.LocalStorage;
                        var file = await documents.GetFileAsync(this.audioExtractor.VideoPath);
                        await file.DeleteAsync();
                     
                    }
                    catch { }
                }

                this.audioExtractor.Dispose();
                this.audioExtractor = null;
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.fileStream != null)
                {
                    this.fileStream.Dispose();
                    this.fileStream = null;
                }

                this.CloseOutput(true);
            }
        }


        private AacAudioExtractor GetAudioWriter(uint mediaInfo)
        {
            uint format = mediaInfo >> 4;

            switch (format)
            {
                //case 14:
                //case 2:
                //    return new Mp3AudioExtractor(this.outputPath);

                case 10:
                    return new AacAudioExtractor(this.outputPath);
            }

            string typeStr;

            switch (format)
            {
                case 1:
                    typeStr = "ADPCM";
                    break;

                case 6:
                case 5:
                case 4:
                    typeStr = "Nellymoser";
                    break;

                default:
                    typeStr = "format=" + format;
                    break;
            }

            throw new AudioExtractionException("Unable to extract audio (" + typeStr + " is unsupported).");
        }

        private async Task<byte[]> ReadBytes(int length)
        {
            var buff = new byte[length];
            await UpdateFilestream();
            this.fileStream.Read(buff, 0, length);
            this.fileOffset += length;
         
            return buff;
        }
        private async Task UpdateFilestream()
        {
            if (fileStream == null)
            {
                var documents = FileSystem.Current.LocalStorage;
                var file = await documents.GetFileAsync(inputPath);
                fileStream = await file.OpenAsync(FileAccess.ReadAndWrite);
                this.fileLength = fileStream.Length;
            }

        }
        private async Task<bool> ReadTag()
        {

           
            await UpdateFilestream();

            if (this.fileLength - this.fileOffset < 11)
                return false;

            // Read tag header
            uint tagType = await ReadUInt8();
            uint dataSize = await ReadUInt24();
            uint timeStamp = await ReadUInt24();
            timeStamp |= await this.ReadUInt8() << 24;
            await this.ReadUInt24();

            // Read tag data
            if (dataSize == 0)
                return true;

            if (this.fileLength - this.fileOffset < dataSize)
                return false;

            uint mediaInfo = await this.ReadUInt8();
            dataSize -= 1;
            byte[] data = await this.ReadBytes((int)dataSize);

            if (tagType == 0x8)
            {
                // If we have no audio writer, create one
                if (this.audioExtractor == null)
                {
                    this.audioExtractor = this.GetAudioWriter(mediaInfo);
                    this.ExtractedAudio = this.audioExtractor != null;
                }

                if (this.audioExtractor == null)
                {
                    throw new InvalidOperationException("No supported audio writer found.");
                }

                await this.audioExtractor.WriteChunk(data, timeStamp);
            }

            return true;
        }

        private async Task<uint> ReadUInt24()
        {
            var x = new byte[4];
            await UpdateFilestream();
            this.fileStream.Read(x, 1, 3);
            this.fileOffset += 3;

            return BigEndianBitConverter.ToUInt32(x, 0);
        }

        private async Task<uint> ReadUInt32()
        {
            var x = new byte[4];
            await UpdateFilestream();
            this.fileStream.Read(x, 0, 4);
            this.fileOffset += 4;

            return BigEndianBitConverter.ToUInt32(x, 0);
        }

        private async Task<uint> ReadUInt8()
        {
            await UpdateFilestream();
            this.fileOffset += 1;
            return (uint)this.fileStream.ReadByte();
        }

        private async Task Seek(long offset)
        {
            await UpdateFilestream();
            this.fileStream.Seek(offset, SeekOrigin.Begin);
            this.fileOffset = offset;
        }
    }
}
