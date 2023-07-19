using FM.Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.Core.Usecases
{
    public class LoadBinaryFileUseCase
    {
        readonly string fileName;

        public LoadBinaryFileUseCase(string fileName)
        {
            this.fileName = fileName;
        }

        public void Execute()
        {
            var chunker = new BinChunk();
            // List<Task<(string, string, string)>> tasks = new List<Task<(string, string, string)>>
            // {
            //     Task.Run(() => chunker.ExtractBin(this.fileName))
            // };

            // var results = await Task.WhenAll(tasks);

            var (SlusPath, WaPath, IsoPath, fileNameBase) = chunker.ExtractBin(this.fileName);

            Configurations.SlusPath = SlusPath;
            Configurations.WaPath = WaPath;
            Configurations.IsoPath = IsoPath;
            Configurations.patheticFileName = $"{fileNameBase}_final";

            Configurations.LoadedIso = true;
        }


    }
}
