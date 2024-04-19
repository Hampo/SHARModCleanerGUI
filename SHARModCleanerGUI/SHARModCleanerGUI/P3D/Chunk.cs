namespace P3D
{
    public class Chunk
    {
        public uint ID { get; set; }
        public byte[] Data { get; set; }
        public List<Chunk> SubChunks { get; } = [];

        public int Size => 12 + Data.Length + SubChunks.Sum(x => x.Size);

        public Chunk(uint id, int headerSize, byte[] data)
        {
            ID = id;
            Data = data[..(headerSize - 12)];

            int pos = headerSize - 12;
            while (pos < data.Length)
            {
                uint chunkId = BitConverter.ToUInt32(data, pos);
                pos += sizeof(uint);

                int chunkHeaderSize = BitConverter.ToInt32(data, pos);
                pos += sizeof(uint);

                int chunkSize = BitConverter.ToInt32(data, pos);
                pos += sizeof(uint);

                SubChunks.Add(new(chunkId, chunkHeaderSize, data[pos..(pos + chunkSize - 12)]));
                pos += chunkSize - 12;
            }
        }

        public Chunk(uint id, byte[] data)
        {
            ID = id;
            Data = data;
        }

        public virtual void Write(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(Data.Length + 12);
            bw.Write(Size);
            bw.Write(Data);
            foreach (var chunk in SubChunks)
                chunk.Write(bw);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Chunk chunk2)
                return false;

            if (!Data.SequenceEqual(chunk2.Data))
                return false;

            if (SubChunks.Count != chunk2.SubChunks.Count)
                return false;

            for (int i = 0; i < SubChunks.Count; i++)
            {
                var subChunk = SubChunks[i];
                var subChunk2 = chunk2.SubChunks[i];

                if (!subChunk.Equals(subChunk2))
                    return false;
            }

            return true;
        }

        public override int GetHashCode() => HashCode.Combine(ID, Data, SubChunks);

        public static bool operator ==(Chunk? left, Chunk? right) => EqualityComparer<Chunk>.Default.Equals(left, right);

        public static bool operator !=(Chunk? left, Chunk? right) => !(left == right);
    }
}
