using FM.Core.Application;
using FM.Core.Domain.Models;
using FM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.Core.Usecases
{
    public class WritePatchedImageUseCase
    {
        public List<GameFile> GameFile = new List<GameFile>();

        private FileStream _fs;
        
        public WritePatchedImageUseCase(string file)
        {
            _fs = new FileStream(file, FileMode.Open);
        }

        /// <summary>
        /// Method to patch the Game Image File
        /// </summary>
        /// <returns>1 for success, -1 for failure</returns>
        public int Execute()
        {
            ListDirectories(ref _fs, new[]
            {
                new GameFile
                {
                    Offset = 0xCA20,
                    Name = "",
                    Size = 2048
                }
            });

            foreach (GameFile k in GameFile)
            {
                // Choose which File to use based on the name of the Item in the loop
                string p = k.Name == "SLUS_014.11" ? Configurations.SlusPath : Configurations.WaPath;

                using (FileStream fs2 = new FileStream(p, FileMode.Open))
                {
                    // Filesize is different, abort with error
                    if (k.Size != fs2.Length)
                    {
                        return -1;
                    }

                    _fs.Position = k.Offset + 24;

                    for (int n = 0; n < fs2.Length / 2048L; n++)
                    {
                        _fs.Write(fs2.ExtractPiece(0, 2048), 0, 2048);
                        _fs.Position += 0x130L;
                    }
                }
            }

            _fs.Dispose();
            _fs.Close();

            var output_dir = Configurations.IsoPath.Substring(0, Configurations.IsoPath.LastIndexOf('\\'));

            if (Configurations.LoadedIso)
            {
                File.Copy(Configurations.IsoPath, $"{output_dir}\\{Configurations.patheticFileName}.bin");

            }
            else
            {
                File.Move(Configurations.IsoPath, $"{output_dir}\\{Configurations.patheticFileName}.bin");
            }

            Configurations.IsoPath = $"{output_dir}\\{Configurations.patheticFileName}.bin";
            string[] cueTemplate = { $"FILE \"{Configurations.patheticFileName}.bin\" BINARY", "  TRACK 01 MODE2/2352", "    INDEX 01 00:00:00" };
            File.WriteAllLines($"{output_dir}\\{Configurations.patheticFileName}.cue", cueTemplate);

            return 1;
        }

        private void ListDirectories(ref FileStream fs, IEnumerable<GameFile> iso)
        {
            var fileList = new List<GameFile>();

            foreach (GameFile file in iso)
            {
                using (var ms = new MemoryStream(fs.ExtractPiece(0, 2048, file.Offset)))
                {
                    ms.Position = 120L;

                    for (int j = ms.ReadByte(); j > 0; j = ms.ReadByte())
                    {
                        var tmpFile = new GameFile();
                        byte[] arr = ms.ExtractPiece(0, j - 1);

                        tmpFile.Offset = arr.ExtractInt32(1) * 2352;
                        tmpFile.Size = arr.ExtractInt32(9);
                        tmpFile.IsDirectory = arr[24] == 2;
                        tmpFile.NameSize = arr[31];
                        tmpFile.Name = GetName(ref arr, tmpFile.NameSize);

                        if (tmpFile.IsDirectory)
                        {
                            fileList.Add(tmpFile);
                        }

                        if (tmpFile.NameSize == 13 && tmpFile.Name == "SLUS_014.11")
                        {
                            GameFile.Add(tmpFile);
                        }

                        if (tmpFile.NameSize == 12 && tmpFile.Name == "WA_MRG.MRG")
                        {
                            GameFile.Add(tmpFile);
                        }
                    }
                }
            }
            if (fileList.Count > 0)
            {
                ListDirectories(ref fs, fileList.ToArray());
            }
        }

        private static string GetName(ref byte[] data, int size)
        {
            string text = string.Empty;

            for (int i = 0; i < size; i++)
            {
                char c = Convert.ToChar(data[32 + i]);

                if (c == ';')
                {
                    break;
                }

                text += c.ToString();
            }

            return text;
        }
    }
}
