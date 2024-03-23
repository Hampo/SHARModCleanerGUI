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

        public void WriteDiff(string dir, string path)
        {
            string filePath = Path.Combine(dir, path);
            string? fileDir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(fileDir))
                Directory.CreateDirectory(fileDir);

            File diffFile = new();
            StringBuilder luaFile = new();

            luaFile.Append("local DeletedChunks = {");
            foreach (int i in DeletedChunks)
                luaFile.Append($"{i + 1},");
            luaFile.AppendLine("}");

            luaFile.Append("local AddedChunks = {");
            foreach ((int, Chunk) addedChunk in AddedChunks)
            {
                diffFile.Chunks.Add(addedChunk.Item2);
                luaFile.Append($"{{{addedChunk.Item1 + 1},{diffFile.Chunks.Count}}},");
            }
            luaFile.AppendLine("}");
            luaFile.AppendLine();

            string luaPath = path.Replace('\\', '/');
            luaFile.AppendLine($"local OrigP3D = P3D.P3DFile(\"/GameData/{luaPath}\")");
            luaFile.AppendLine($"local ModifiedP3D = P3D.P3DFile(GetModPath() .. \"/Resources/P3D_Diffs/{luaPath}\")");
            luaFile.AppendLine("ProcessP3DDiff(OrigP3D, ModifiedP3D, DeletedChunks, AddedChunks)");
            luaFile.AppendLine();

            luaFile.AppendLine("OrigP3D:Output()");

            System.IO.File.WriteAllText($"{filePath}.lua", luaFile.ToString());
            diffFile.Write(filePath);
        }
    }
}
