﻿using System;
using System.IO;
using System.Threading.Tasks;
using DiscUtils.Iso9660;
using DiscUtils.Streams;
using FM.Core.Application;
using Nito.AsyncEx;

namespace FM.Core.Usecases
{
    public class BinChunk
    {
        public const int SectorLength = 2352;
        private const string CueExtension = ".cue";

        private readonly string _outFileNameBase = $"FM_[{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}]_{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}";
        private string _outFileName;

        public (string, string, string, string) ExtractBin(string cueFileName)
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\" + _outFileNameBase))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\" + _outFileNameBase);
            }

            var save_dir = Directory.GetCurrentDirectory() + @"\" + _outFileNameBase;

            try
            {
                CueFile cueFile;
                try
                {
                    cueFileName = Path.ChangeExtension(cueFileName, CueExtension);
                    cueFile = new CueFile(cueFileName);
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"Could not read CUE {cueFileName}:\n{e.Message}");
                }

                Stream binStream;
                try
                {
                    File.Copy(cueFile.BinFileName, save_dir + @"\"+ _outFileNameBase + ".bin");
                    binStream = File.OpenRead(cueFile.BinFileName);
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"Could not open BIN {cueFile.BinFileName}: {e.Message}");
                }

                Console.WriteLine(Environment.NewLine + "Writing tracks:");
                foreach (Track curTrack in cueFile.TrackList)
                {
                    Console.WriteLine(curTrack.Modes);
                    Console.WriteLine(curTrack.FileExtension);
                    Console.WriteLine(cueFile.TrackList.Count);
                    // Include track number when more than 1 track.
                    
                    if (cueFile.TrackList.Count > 1)
                        _outFileName =
                            $"{_outFileNameBase}{curTrack.TrackNumber:00}.{curTrack.FileExtension.ToString().ToLower()}";
                    else
                        _outFileName = $"{_outFileNameBase}.{curTrack.FileExtension.ToString().ToLower()}";
                    curTrack.Write(binStream, _outFileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("discutils");

            using (FileStream isoStream = File.OpenRead(_outFileName))
            {
                CDReader cd = new CDReader(isoStream, false);
                Console.WriteLine(cd.Root.FullName);

                string[] files = cd.GetFiles(cd.Root.FullName);
                string[] dirs = cd.GetDirectories(cd.Root.FullName);
                bool mrgdone = false;

                foreach (string file in files)
                {
                    Console.WriteLine(file);
                    if (file == @"\SLUS_014.11;1")
                    {
                        FileStream fileStream = File.Create(save_dir + @"\SLUS_014.11");
                        SparseStream isf = cd.OpenFile(file, FileMode.Open);
                        byte[] dat = new byte[isf.Length];

                        int result = AsyncContext.Run(() => isf.ReadAsync(dat, 0, (int)isf.Length));

                        Task task = Task.Run(async () => { await fileStream.WriteAsync(dat, 0, dat.Length); });
                        task.Wait();
                        fileStream.Close();
                    }

                    foreach (string data in cd.GetFiles("\\DATA"))
                    {
                        Console.WriteLine(data);
                        if (data == @"\DATA\WA_MRG.MRG;1" && !mrgdone)
                        {
                            FileStream fs = File.Create(save_dir + @"\WA_MRG.MRG");
                            SparseStream isf = cd.OpenFile(data, FileMode.Open);
                            byte[] dat = new byte[isf.Length];
                            int result = AsyncContext.Run(() => isf.ReadAsync(dat, 0, (int) isf.Length));
                            Task task = Task.Run(async () => { await fs.WriteAsync(dat, 0, dat.Length); });
                            task.Wait();
                            mrgdone = true;
                            fs.Close();
                            break;
                        }
                    }

                    Configurations.SlusPath = save_dir + @"\SLUS_014.11";
                    Configurations.WaPath = save_dir + @"\WA_MRG.MRG";
                    Configurations.IsoPath = save_dir + @"\" + _outFileNameBase + ".bin";

                }
                isoStream.Close();
                File.Delete(_outFileName);

                return (Configurations.SlusPath, Configurations.WaPath, Configurations.IsoPath, _outFileNameBase);
            }
        }
    }
}