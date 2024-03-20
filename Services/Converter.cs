using System;
using System.Diagnostics;
using VideoLibrary;

namespace YT2mp3.Services
{
    public static class Converter
    {
        public static async Task Convert(string uri, string fileFolder, string fileName, string ffmpegPath)
        {
            var youtube = YouTube.Default;

            var video = youtube.GetVideo(uri);
            byte[] bytes = video.GetBytes();

            File.WriteAllBytes(fileFolder + fileName+".mp4", bytes);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath, 
                Arguments = $"-i {fileFolder}{fileName}.mp4 -vn -ab 128k {fileFolder}{fileName}.mp3", // Extract audio and convert to MP3 with 128kbps
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit();
            }
        }

        public static async Task DeleteFile(string path)
        {

        }
    }
}
