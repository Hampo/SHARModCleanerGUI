using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace P3D
{
    public class FileDiffNew
    {
        public P3D.File Diff { get; }
        public bool AllNew { get; protected set; } = true;

        public FileDiffNew(string originalFile, string modifiedFile)
        {
            if (!System.IO.File.Exists(originalFile))
                throw new ArgumentException($"Could not find the specified file: {originalFile}", nameof(originalFile));

            if (!System.IO.File.Exists(modifiedFile))
                throw new ArgumentException($"Could not find the specified file: {modifiedFile}", nameof(modifiedFile));

            byte[] originalBytes;
            using (FileStream fs = new(originalFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (fs.Length < 12)
                    throw new ArgumentException($"Specified file \"{originalFile}\" too short to be a valid P3D.", nameof(originalFile));

                using BinaryReader br = new(fs);
                originalBytes = P3D.LZR_Compression.DecompressFile(br);
            }

            uint signature = BitConverter.ToUInt32(originalBytes, 0);
            if (signature != P3D.File.P3DSignature)
                throw new ArgumentException($"Specified file \"{originalFile}\" has invalid signature \"0x{signature:X}\".", nameof(originalFile));

            P3D.File modifiedP3D = new(modifiedFile);

            List<(uint, byte[])> originalChunks = [];
            int pos = 12;
            while (pos < originalBytes.Length)
            {
                uint chunkId = BitConverter.ToUInt32(originalBytes, pos);
                pos += sizeof(uint);

                int chunkHeaderSize = BitConverter.ToInt32(originalBytes, pos);
                pos += sizeof(uint);

                int chunkSize = BitConverter.ToInt32(originalBytes, pos);
                pos += sizeof(uint);

                originalChunks.Add((chunkId, originalBytes[pos..(pos + chunkHeaderSize - 12)]));
                pos += chunkHeaderSize - 12;
            }

            Diff = new();

            foreach (var chunk in modifiedP3D.Chunks)
            {
                var index = originalChunks.FindIndex(x => x.Item1 == chunk.ID && x.Item2.SequenceEqual(chunk.Data));
                P3D.Chunk c;
                if (index != -1)
                {
                    AllNew = false;
                    c = new(0x69696969, BitConverter.GetBytes(index + 1));
                }
                else
                {
                    c = new(chunk.ID, chunk.Data);
                }

                if (ProcessChunks(originalChunks, chunk.SubChunks, Diff, c) && index != -1)
                {
                    c.ID = 0x73737373;
                    c.SubChunks.Clear();
                }

                Diff.Chunks.Add(c);
            }
        }

        private bool ProcessChunks(List<(uint, byte[])> originalChunks, List<P3D.Chunk> modifiedChunks, P3D.File diffP3D, P3D.Chunk parent)
        {
            bool unchanged = true;
            foreach (var chunk in modifiedChunks)
            {
                var index = originalChunks.FindIndex(x => x.Item1 == chunk.ID && x.Item2.SequenceEqual(chunk.Data));
                P3D.Chunk c;
                if (index != -1)
                {
                    AllNew = false;
                    c = new(0x69696969, BitConverter.GetBytes(index + 1));
                }
                else
                {
                    unchanged = false;
                    c = new(chunk.ID, chunk.Data);
                }
                unchanged = ProcessChunks(originalChunks, chunk.SubChunks, diffP3D, c) && unchanged;
                parent.SubChunks.Add(c);
            }
            return unchanged;
        }
    }
}
