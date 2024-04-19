using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace SHARModCleanerGUI
{
    public partial class FrmMain : Form
    {

        private static readonly HttpClient HttpClient = new();

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            TxtSHARFolder.Text = Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Lucas Stuff\\Lucas' Simpsons Hit & Run Tools", "Game Path", "")?.ToString();
        }

        private void BtnBrowseInputFolder_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog fbd = new()
            {
                ShowNewFolderButton = false,
                ClientGuid = new("2e9d853b-21ed-42df-bf7c-108c54dcb110")
            };
            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            TxtInputFolder.Text = fbd.SelectedPath;
            if (string.IsNullOrWhiteSpace(TxtOutputFolder.Text))
                TxtOutputFolder.Text = $"{fbd.SelectedPath} Minified";
        }

        private void BtnBrowseOutputFolder_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog fbd = new()
            {
                ShowNewFolderButton = true,
                ClientGuid = new("b90a7c63-5660-4e91-9f01-41275b0c9a8b")
            };
            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            TxtOutputFolder.Text = fbd.SelectedPath;
        }

        private void BtnBrowseSHARFolder_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog fbd = new()
            {
                ShowNewFolderButton = false,
                ClientGuid = new("5ea4d233-f584-420b-91b7-122e2e585d76")
            };
            if (!string.IsNullOrWhiteSpace(TxtSHARFolder.Text) && Directory.Exists(TxtSHARFolder.Text))
                fbd.InitialDirectory = TxtSHARFolder.Text;
            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            TxtSHARFolder.Text = fbd.SelectedPath;
        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using SaveFileDialog sfd = new()
            {
                ClientGuid = new("07d5f0d6-931d-474f-9275-78efe833cf59"),
                FileName = $"SHARModCleaner {DateTime.Now:yyyy-MM-dd HH-mm-ss}.log",
                Filter = "Log Files (*.log)|*.log",
                Title = "Choose log file location"
            };
            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                File.WriteAllLines(sfd.FileName, LBLogs.Items.Cast<string>().ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error saving your log file: {ex}", "Error saving logs", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all logs?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            LBLogs.Items.Clear();
        }

        private void LBLogs_KeyDown(object sender, KeyEventArgs e)
        {
            if (LBLogs.SelectedItems.Count == 0)
                return;
            string selectedLogs = string.Join("\r\n", LBLogs.SelectedItems.Cast<string>());
            if (!string.IsNullOrWhiteSpace(selectedLogs) && e.Modifiers.HasFlag(Keys.Control) && e.KeyCode == Keys.C)
                Clipboard.SetText(selectedLogs);
        }

        private async void BtnProcess_Click(object sender, EventArgs e)
        {
            GBInput.Enabled = false;
            CMSLogs.Enabled = false;
            AddLog("Process started...");
            Stopwatch sw = Stopwatch.StartNew();

            await Task.Run(() => Process(TxtInputFolder.Text, TxtOutputFolder.Text, CBRemoveUnchangedFiles.Checked, CBConvertRSDToOGG.Checked, CBReplaceP3DsWithDiffs.Checked, TxtSHARFolder.Text, CBLogCopied.Checked));

            sw.Stop();
            AddLog($"Process finished in {sw.Elapsed:hh\\:mm\\:ss\\.fff}.");
            CMSLogs.Enabled = true;
            GBInput.Enabled = true;
        }

        private void AddLog(string log)
        {
            if (LBLogs.InvokeRequired)
            {
                LBLogs.Invoke(new MethodInvoker(() => AddLog(log)));
                return;
            }

            LBLogs.Items.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {log}");
            LBLogs.SelectedItems.Clear();
            LBLogs.SelectedIndex = LBLogs.Items.Count - 1;
        }

        private void SetProgressBarMax(int max)
        {
            if (PBProgress.InvokeRequired)
            {
                PBProgress.Invoke(new MethodInvoker(() => SetProgressBarMax(max)));
                return;
            }

            PBProgress.Maximum = max;
        }

        private void SetProgressBar(int value)
        {
            if (PBProgress.InvokeRequired)
            {
                PBProgress.Invoke(new MethodInvoker(() => SetProgressBar(value)));
                return;
            }

            PBProgress.Value = value;
        }

        private void IncrementProgressBar()
        {
            if (PBProgress.InvokeRequired)
            {
                PBProgress.Invoke(new MethodInvoker(() => IncrementProgressBar()));
                return;
            }

            PBProgress.Value++;
        }

        private void DownloadFile(string url, string path)
        {
            string? dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(dir))
                Directory.CreateDirectory(dir);

            using FileStream fs = File.OpenWrite(path);
            using Stream s = HttpClient.GetStreamAsync(url).GetAwaiter().GetResult();
            s.CopyTo(fs);
        }

        private void Process(string inputFolder, string outputFolder, bool removeUnchanged, bool convertRSD, bool generateDiffs, string sharFolder, bool logCopied)
        {
            SetProgressBar(0);
            if (string.IsNullOrWhiteSpace(inputFolder))
            {
                AddLog("No input folder specified. Exiting.");
                return;
            }

            if (string.IsNullOrWhiteSpace(outputFolder))
            {
                AddLog("No output folder specified. Exiting.");
                return;
            }

            if (inputFolder == outputFolder)
            {
                AddLog("Input folder and output folder must be different. Exiting.");
                return;
            }

            if (!Directory.Exists(inputFolder))
            {
                AddLog($"Could not find input folder \"{inputFolder}\". Exiting.");
                return;
            }

            if (!removeUnchanged && !convertRSD && !generateDiffs)
            {
                AddLog("You must enable at least one cleanup option. Exiting.");
                return;
            }

            if (generateDiffs)
            {
                if (string.IsNullOrWhiteSpace(sharFolder))
                {
                    AddLog("SHAR folder must be specified when using \"Replace P3Ds with Diffs\". Exiting.");
                    return;
                }

                if (!Directory.Exists(sharFolder))
                {
                    AddLog($"Could not find SHAR folder \"{sharFolder}\". Exiting.");
                    return;
                }
            }

            string metaFile = Path.Combine(inputFolder, "Meta.ini");
            if (!File.Exists(metaFile))
            {
                AddLog($"Could not find meta file \"{metaFile}\". Exiting.");
                return;
            }

            string customFilesDir = Path.Combine(inputFolder, "CustomFiles");
            int customFilesN = customFilesDir.Length;
            if (!Directory.Exists(customFilesDir))
            {
                AddLog($"Could not find CustomFiles folder \"{inputFolder}\". Exiting.");
                return;
            }

            if (Directory.Exists(outputFolder))
            {
                string[] currentDirs = Directory.GetDirectories(outputFolder);
                string[] currentFiles = Directory.GetFiles(outputFolder, "*", SearchOption.TopDirectoryOnly);
                if (currentDirs.Length + currentFiles.Length > 0)
                {
                    AddLog($"Output folder \"{outputFolder}\" already exists.");
                    if (MessageBox.Show("Output directory already exists. It will be overwritten. Do you want to continue?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        AddLog("Chosen not to overwrite. Exiting.");
                        return;
                    }
                    AddLog("Chosen to overwrite. Deleting...");

                    foreach (string subDir in currentDirs)
                        Directory.Delete(subDir, true);

                    foreach (string file in currentFiles)
                        File.Delete(file);
                }
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    AddLog($"Failed to create output folder \"{outputFolder}\": {ex.Message}. Exiting.");
                    return;
                }
            }

            if (convertRSD)
            {
                AddLog("Getting FFmpeg...");
                string ffmpegDir = "FFmpeg";
                FFmpeg.SetExecutablesPath(ffmpegDir);
                FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, ffmpegDir).GetAwaiter().GetResult();
                string jsonFile = Path.Combine(ffmpegDir, "version.json");
                if (File.Exists(jsonFile))
                {
                    JObject jObj = JObject.Parse(File.ReadAllText(jsonFile));
                    AddLog($"Downloaded FFmpeg. Version: {jObj["version"]}");
                }
                else
                    AddLog("Downloaded FFmpeg");
            }

            string[] rootFiles;
            string[] rootFolders;
            string[] customFiles;
            try
            {
                rootFiles = Directory.GetFiles(inputFolder, "*", SearchOption.TopDirectoryOnly);
                rootFolders = Directory.GetDirectories(inputFolder);
                customFiles = Directory.GetFiles(customFilesDir, "*", SearchOption.AllDirectories);

                SetProgressBarMax(rootFiles.Length + rootFolders.Length + customFiles.Length);
            }
            catch (Exception ex)
            {
                AddLog($"Error getting file lists: {ex.Message}. Exiting.");
                return;
            }

            try
            {
                AddLog("Processing root files...");
                foreach (string rootFile in rootFiles)
                {
                    string fileName = Path.GetFileName(rootFile);
                    File.Copy(rootFile, Path.Combine(outputFolder, fileName));
                    if (logCopied)
                        AddLog($"Copied root file: {fileName}");
                    IncrementProgressBar();
                }
            }
            catch (Exception ex)
            {
                AddLog($"Error copying root files: {ex.Message}. Exiting.");
                return;
            }

            try
            {
                AddLog("Processing root folders...");
                foreach (string rootFolder in rootFolders)
                {
                    string folderName = Path.GetFileName(rootFolder);
                    if (folderName == "CustomFiles")
                        continue;
                    CopyFilesRecursively(rootFolder, Path.Combine(outputFolder, folderName));
                    if (logCopied)
                        AddLog($"Copied root folder: {folderName}");
                    IncrementProgressBar();
                }
            }
            catch (Exception ex)
            {
                AddLog($"Error copying root folders: {ex.Message}. Exiting.");
                return;
            }

            try
            {
                AddLog("Processing CustomFiles...");
                long unchangedBytes = 0;
                long unchangedCount = 0;
                long convertedBytes = 0;
                long convertedCount = 0;
                long diffBytes = 0;
                long diffCount = 0;
                StringBuilder customFilesIniSB = new();
                foreach (string customFile in customFiles)
                {
                    string relativePath = customFile[customFilesN..];
                    if (relativePath.StartsWith('\\') || relativePath.StartsWith('/'))
                        relativePath = relativePath[1..];
                    relativePath = relativePath.Replace('/', '\\');

                    string outputFile = Path.Combine(outputFolder, "CustomFiles", relativePath);
                    string? outputDir = Path.GetDirectoryName(outputFile);

                    if (removeUnchanged)
                    {
                        FileInfo fileInfo = new(customFile);
                        using FileStream fileStream = fileInfo.OpenRead();
                        if (Hashes.CompareHash(relativePath, fileStream))
                        {
                            unchangedBytes += fileInfo.Length;
                            unchangedCount++;
                            AddLog($"Skipping unchanged custom file: {relativePath}");

                            IncrementProgressBar();
                            continue;
                        }
                    }

                    if (convertRSD && Path.GetExtension(customFile).Equals(".rsd", StringComparison.OrdinalIgnoreCase))
                    {
                        AddLog($"Converting RSD file: {relativePath}");
                        outputFile = $"{outputFile[..^4]}.ogg";

                        IMediaInfo mediaInfo = FFmpeg.GetMediaInfo(customFile).GetAwaiter().GetResult();
                        if (!mediaInfo.AudioStreams.Any())
                        {
                            AddLog("No audio streams found. Exiting.");
                            return;
                        }

                        IAudioStream audioStream = mediaInfo.AudioStreams.First();
                        audioStream.SetCodec(AudioCodec.libvorbis);

                        IConversion conversion = FFmpeg.Conversions.New();

                        if (!string.IsNullOrWhiteSpace(outputDir))
                            Directory.CreateDirectory(outputDir);
                        conversion.SetOutput(outputFile);
                        conversion.AddStream(audioStream);

                        IConversionResult conversionResult = conversion.Start().GetAwaiter().GetResult();
                        AddLog($"Converted in {conversionResult.Duration:hh\\:mm\\:ss\\.fff}.");

                        FileInfo origFile = new(customFile);
                        FileInfo newFile = new(outputFile);
                        long savedBytes = origFile.Length - newFile.Length;
                        if (savedBytes < 0)
                        {
                            newFile.Delete();
                            AddLog($"Removing converted file as original file was smaller.");
                        }
                        else
                        {
                            convertedBytes += savedBytes;
                            convertedCount++;

                            IncrementProgressBar();
                            continue;
                        }
                    }

                    if (generateDiffs && Path.GetExtension(customFile).Equals(".p3d", StringComparison.OrdinalIgnoreCase))
                    {
                        string baseGameFile = Path.Combine(sharFolder, relativePath);
                        if (File.Exists(baseGameFile))
                        {
                            AddLog($"Generating diff for file: {relativePath}");

                            P3D.File origP3D = new(baseGameFile);
                            P3D.File modifiedP3D = new(customFile);

                            P3D.FileDiff fileDiff = new(origP3D, modifiedP3D);

                            // If all chunks deleted, it's an entirely new file, just copy it
                            if (origP3D.Chunks.Count != fileDiff.DeletedChunks.Count)
                            {
                                outputDir = Path.Combine(outputFolder, "Resources", "P3D_Diffs");
                                outputFile = Path.Combine(outputDir, relativePath);
                                fileDiff.WriteDiff(outputDir, relativePath);

                                FileInfo origFile = new(customFile);
                                FileInfo newFile = new(outputFile);
                                diffBytes += origFile.Length - newFile.Length;
                                diffCount++;

                                string iniPath = relativePath.Replace('\\', '/');
                                customFilesIniSB.AppendLine($"{iniPath}=Resources/P3D_Diffs/{iniPath}.lua");

                                IncrementProgressBar();
                                continue;
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(outputDir))
                        Directory.CreateDirectory(outputDir);
                    File.Copy(customFile, outputFile);
                    if (logCopied)
                        AddLog($"Copied custom file: {relativePath}");
                    IncrementProgressBar();
                }

                if (removeUnchanged)
                    AddLog($"Saved {BytesToString(unchangedBytes)} in {unchangedCount} unchanged files.");

                if (convertRSD)
                    AddLog($"Saved {BytesToString(convertedBytes)} in {convertedCount} converted files.");

                if (generateDiffs)
                {
                    AddLog($"Saved {BytesToString(diffBytes)} in {diffCount} diffed files.");
                    if (diffCount > 0)
                    {
                        try
                        {
                            string P3D2LuaFile = Path.Combine(outputFolder, "Resources", "lib", "P3D2.lua");
                            if (!File.Exists(P3D2LuaFile))
                            {
                                DownloadFile("https://raw.githubusercontent.com/Hampo/LuaP3DLib/master/lib/P3D2.lua", P3D2LuaFile);
                                AddLog("Downloaded P3D2.lua");
                            }

                            string customFilesLua = Path.Combine(outputFolder, "CustomFiles.lua");
                            using StreamWriter sw = new(customFilesLua, true);
                            sw.WriteLine();
                            sw.Write(Properties.Resources.CustomFiles);
                            AddLog("Written CustomFiles.lua");

                            customFilesIniSB.AppendLine("# End generated by SHARModCleanupGUI");
                            string outputCustomFilesFile = Path.Combine(outputFolder, "CustomFiles.ini");
                            if (!File.Exists(outputCustomFilesFile))
                            {
                                customFilesIniSB.Insert(0, "[PathHandlers]\n# Start generated by SHARModCleanupGUI\n");
                                File.WriteAllText(outputCustomFilesFile, customFilesIniSB.ToString());
                                AddLog("Written CustomFiles.ini");
                            }
                            else
                            {
                                string outputCustomFilesIni = File.ReadAllText(outputCustomFilesFile);
                                int index = outputCustomFilesIni.IndexOf("[PathHandlers]");
                                if (index == -1)
                                {
                                    customFilesIniSB.Insert(0, "[PathHandlers]\r\n");
                                    customFilesIniSB.Insert(0, "# Start generated by SHARModCleanupGUI\n");
                                    outputCustomFilesIni += $"\n{customFilesIniSB}";
                                }
                                else
                                {
                                    customFilesIniSB.Insert(0, "# Start generated by SHARModCleanupGUI\n");
                                    outputCustomFilesIni = outputCustomFilesIni.Insert(index + 14, $"\n{customFilesIniSB}");
                                }
                                File.WriteAllText(outputCustomFilesFile, outputCustomFilesIni);
                                AddLog("Updated CustomFiles.ini");
                            }
                        }
                        catch (Exception ex)
                        {
                            AddLog($"Error creating diff handler: {ex.Message}. Exiting.");
                            return;
                        }
                    }
                }

                AddLog($"Saved {BytesToString(unchangedBytes + convertedBytes + diffBytes)} across all settings.");
            }
            catch (Exception ex)
            {
                AddLog($"Error processing CustomFiles: {ex.Message}. Exiting.");
                return;
            }

            if (convertRSD)
            {
                string outputMetaFile = Path.Combine(outputFolder, "Meta.ini");
                string outputMetaIni = File.ReadAllText(outputMetaFile);
                if (!outputMetaIni.Contains("\nRequiredHack=OggVorbisSupport"))
                {
                    int index = outputMetaIni.IndexOf("[Miscellaneous]");
                    if (index == -1)
                    {
                        outputMetaIni += "\n# Start generated by SHARModCleanupGUI\n[Miscellaneous]\nRequiredHack=OggVorbisSupport\n# End generated by SHARModCleanupGUI\n";
                    }
                    else
                    {
                        outputMetaIni = outputMetaIni.Insert(index + 15, "\n# Start generated by SHARModCleanupGUI\nRequiredHack=OggVorbisSupport\n# End generated by SHARModCleanupGUI\n");
                    }
                    File.WriteAllText(outputMetaFile, outputMetaIni);
                    AddLog("Updated Meta.ini");
                }
            }

            // TODO: Modify Meta.ini to add OggVorbisSupport

            // TODO: Modify CustomFiles.ini when P3D diffs are supported
        }

        private static void CopyFilesRecursively(string source, string target)
        {
            if (!Directory.Exists(source))
                return;

            if (!Directory.Exists(target))
                Directory.CreateDirectory(target);

            foreach (string dir in Directory.GetDirectories(source))
            {
                string dirName = Path.GetFileName(dir);
                CopyFilesRecursively(dir, Path.Combine(target, dirName));
            }

            foreach (string file in Directory.GetFiles(source))
            {
                string fileName = Path.GetFileName(file);
                File.Copy(file, Path.Combine(target, fileName));
            }
        }
        private static string BytesToString(long byteCount)
        {
            string[] suf = ["B", "KB", "MB", "GB", "TB", "PB", "EB"]; //Longs run out around EB
            if (byteCount == 0)
                return $"0{suf[0]}";
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 2);
            return $"{Math.Sign(byteCount) * num}{suf[place]}";
        }
    }
}
