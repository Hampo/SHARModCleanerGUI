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

            List<(uint, byte[], byte[])> originalChunks = [];
            int pos = 12;
            while (pos < originalBytes.Length)
            {
                uint chunkId = BitConverter.ToUInt32(originalBytes, pos);

                int chunkHeaderSize = BitConverter.ToInt32(originalBytes, pos + sizeof(uint));

                int chunkSize = BitConverter.ToInt32(originalBytes, pos + sizeof(uint) + sizeof(uint));

                originalChunks.Add((chunkId, originalBytes[(pos + 12)..(pos + chunkHeaderSize)], originalBytes[pos..(pos + chunkSize)]));
                pos += chunkHeaderSize;
            }

            Diff = new();

            foreach (var chunk in modifiedP3D.Chunks)
            {
                P3D.Chunk c;

                var index = originalChunks.FindIndex(x => x.Item1 == chunk.ID && x.Item3.SequenceEqual(chunk.Bytes));
                if (index != -1)
                {
                    AllNew = false;
                    c = new(0x73737373, BitConverter.GetBytes(index + 1));
                }
                else
                {
                    index = originalChunks.FindIndex(x => x.Item1 == chunk.ID && x.Item2.SequenceEqual(chunk.Data));
                    if (index != -1)
                    {
                        AllNew = false;
                        c = new(0x69696969, BitConverter.GetBytes(index + 1));
                    }
                    else
                    {
                        c = new(chunk.ID, chunk.Data);
                    }

                    ProcessChunks(originalChunks, chunk.SubChunks, Diff, c);
                }

                Diff.Chunks.Add(c);
            }
        }

        private void ProcessChunks(List<(uint, byte[], byte[])> originalChunks, List<P3D.Chunk> modifiedChunks, P3D.File diffP3D, P3D.Chunk parent)
        {
            foreach (var chunk in modifiedChunks)
            {
                P3D.Chunk c;

                var index = originalChunks.FindIndex(x => x.Item1 == chunk.ID && x.Item3.SequenceEqual(chunk.Bytes));
                if (index != -1)
                {
                    AllNew = false;
                    c = new(0x73737373, BitConverter.GetBytes(index + 1));
                }
                else
                {
                    index = originalChunks.FindIndex(x => x.Item1 == chunk.ID && x.Item2.SequenceEqual(chunk.Data));
                    if (index != -1)
                    {
                        AllNew = false;
                        c = new(0x69696969, BitConverter.GetBytes(index + 1));
                    }
                    else
                    {
                        c = new(chunk.ID, chunk.Data);
                    }

                    ProcessChunks(originalChunks, chunk.SubChunks, diffP3D, c);
                }

                parent.SubChunks.Add(c);
            }
        }
    }
}
