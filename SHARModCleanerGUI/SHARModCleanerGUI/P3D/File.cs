namespace P3D
{
    public class File
    {
        public const uint P3DSignature = 0xFF443350;

        public List<Chunk> Chunks { get; } = [];

        public int Size => 12 + Chunks.Sum(x => x.Size);

        public File() { }

        public File(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                throw new ArgumentException($"Could not find the specified file: {filePath}", nameof(filePath));

            byte[] fileBytes;
            using (FileStream fs = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (fs.Length < 12)
                    throw new ArgumentException($"Specified file \"{filePath}\" too short to be a valid P3D.", nameof(filePath));

                using BinaryReader br = new(fs);
                fileBytes = LZR_Compression.DecompressFile(br);
            }

            uint signature = BitConverter.ToUInt32(fileBytes, 0);
            if (signature != P3DSignature)
                throw new ArgumentException($"Specified file \"{filePath}\" has invalid signature \"0x{signature:X}\".", nameof(filePath));

            int pos = 12;
            while (pos < fileBytes.Length)
            {
                uint chunkId = BitConverter.ToUInt32(fileBytes, pos);
                pos += sizeof(uint);

                int chunkHeaderSize = BitConverter.ToInt32(fileBytes, pos);
                pos += sizeof(uint);

                int chunkSize = BitConverter.ToInt32(fileBytes, pos);
                pos += sizeof(uint);

                Chunks.Add(new(chunkId, chunkHeaderSize, fileBytes[pos..(pos+chunkSize-12)]));
                pos += chunkSize - 12;
            }
        }

        public void Write(string filePath)
        {
            using FileStream fs = new(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            using BinaryWriter bw = new(fs);
            bw.Write(P3DSignature);
            bw.Write(12);
            bw.Write(Size);
            foreach (var chunk in Chunks)
                chunk.Write(bw);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not File file2)
                return false;

            if (Chunks.Count != file2.Chunks.Count)
                return false;

            for (int i = 0; i < Chunks.Count; i++)
            {
                var chunk = Chunks[i];
                var chunk2 = file2.Chunks[i];

                if (!chunk.Equals(chunk2))
                    return false;
            }

            return true;
        }

        public override int GetHashCode() => HashCode.Combine(Chunks);

        public static bool operator ==(File? left, File? right) => EqualityComparer<File>.Default.Equals(left, right);

        public static bool operator !=(File? left, File? right) => !(left == right);
    }
}
