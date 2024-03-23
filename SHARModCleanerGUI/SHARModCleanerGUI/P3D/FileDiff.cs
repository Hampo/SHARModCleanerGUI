using System.Text;

namespace P3D
{
    public class FileDiff
    {
        public List<int> DeletedChunks { get; } = [];
        public List<(int, Chunk)> AddedChunks { get; } = [];

        public FileDiff(File originalFile, File modifiedFile)
        {
            if (originalFile.Equals(modifiedFile))
                return;

            List<Chunk> tmp = new(modifiedFile.Chunks);
            for (int i = 0; i < originalFile.Chunks.Count; i++)
            {
                int index = tmp.IndexOf(originalFile.Chunks[i]);
                if (index == -1)
                    DeletedChunks.Add(i);
                else
                    tmp.RemoveAt(index);
            }

            List<Chunk> tmp2 = new(modifiedFile.Chunks);
            tmp.Reverse();
            foreach (Chunk chunk in tmp)
            {
                int index = tmp2.LastIndexOf(chunk);
                AddedChunks.Add((index, chunk));
                tmp2.RemoveAt(index);
            }
            AddedChunks.Reverse();
        }

        public void WriteDiff(string dir, string fileName)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File diffFile = new();
            StringBuilder luaFile = new();

            luaFile.AppendLine("local DeletedChunks = {");
            foreach (int i in DeletedChunks)
                luaFile.AppendLine($"\t{i + 1},");
            luaFile.AppendLine("}");
            luaFile.AppendLine();

            luaFile.AppendLine("local AddedChunks = {");
            foreach ((int, Chunk) addedChunk in AddedChunks)
            {
                diffFile.Chunks.Add(addedChunk.Item2);
                luaFile.AppendLine($"\t{{{addedChunk.Item1 + 1},{diffFile.Chunks.Count}}},");
            }
            luaFile.AppendLine("}");
            luaFile.AppendLine();

            luaFile.AppendLine("return DeletedChunks, AddedChunks");

            System.IO.File.WriteAllText(Path.Combine(dir, $"{fileName}.lua"), luaFile.ToString());
            diffFile.Write(Path.Combine(dir, fileName));
        }
    }
}
