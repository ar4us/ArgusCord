using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArgusCordInstaller
{
    public partial class MainWindow : Window
    {
        private class DiscordInstall
        {
            public string Name { get; set; } = string.Empty;
            public string Path { get; set; } = string.Empty;
            public string Version { get; set; } = string.Empty; // e.g. app-1.0.9241
            public string ResourcesPath => System.IO.Path.Combine(Path, Version, "resources");
            public bool IsPatched => File.Exists(System.IO.Path.Combine(ResourcesPath, "_app.asar"));
        }

        private List<DiscordInstall> _detectedInstalls = new List<DiscordInstall>();

        public MainWindow()
        {
            InitializeComponent();
            Log("ArgusCord Installer initialized.\n");
            DetectDiscordInstallations();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Log(string message)
        {
            LogTextBlock.Text += message + "\n";
            LogScrollViewer.ScrollToEnd();
        }

        private void DetectDiscordInstallations()
        {
            _detectedInstalls.Clear();
            DiscordBranchComboBox.Items.Clear();

            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (string.IsNullOrEmpty(localAppData))
            {
                Log("[ERROR] LocalAppData folder not found.");
                return;
            }

            var branches = new Dictionary<string, string>
            {
                { "Discord", "Discord Stable" },
                { "DiscordPTB", "Discord PTB" },
                { "DiscordCanary", "Discord Canary" },
                { "DiscordDevelopment", "Discord Development" }
            };

            foreach (var branch in branches)
            {
                string path = System.IO.Path.Combine(localAppData, branch.Key);
                if (Directory.Exists(path))
                {
                    // Find latest app-* directory
                    var appDirs = Directory.GetDirectories(path, "app-*");
                    if (appDirs.Length > 0)
                    {
                        // Sort alphabetically to get the highest version
                        var latestAppDir = appDirs.OrderByDescending(d => d).First();
                        string version = System.IO.Path.GetFileName(latestAppDir);
                        
                        string resourcesPath = System.IO.Path.Combine(latestAppDir, "resources");
                        if (Directory.Exists(resourcesPath))
                        {
                            var install = new DiscordInstall
                            {
                                Name = branch.Value,
                                Path = path,
                                Version = version
                            };
                            _detectedInstalls.Add(install);
                            DiscordBranchComboBox.Items.Add(install.Name);
                            Log($"[INFO] Detected: {install.Name} ({version})");
                        }
                    }
                }
            }

            if (_detectedInstalls.Count > 0)
            {
                DiscordBranchComboBox.SelectedIndex = 0;
            }
            else
            {
                PathDisplayLabel.Text = "No Discord installation found.";
                Log("[WARNING] No Discord installations detected automatically.");
            }
        }

        private void DiscordBranchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = DiscordBranchComboBox.SelectedIndex;
            if (index >= 0 && index < _detectedInstalls.Count)
            {
                var install = _detectedInstalls[index];
                PathDisplayLabel.Text = $"Path: {System.IO.Path.Combine(install.Path, install.Version)}";
                StatusLabel.Text = install.IsPatched ? "Status: Patched" : "Status: Ready to patch";
            }
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            int index = DiscordBranchComboBox.SelectedIndex;
            if (index < 0 || index >= _detectedInstalls.Count)
            {
                MessageBox.Show("Please select a valid Discord installation first.", "ArgusCord Installer", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var install = _detectedInstalls[index];
            InstallProgressBar.Visibility = Visibility.Visible;
            InstallProgressBar.Value = 10;
            StatusLabel.Text = "Installing...";

            try
            {
                // 1. Kill Discord process
                Log($"\n[INFO] Starting installation for {install.Name}...");
                KillDiscordProcesses(install.Name);
                InstallProgressBar.Value = 30;

                // 2. Prepare ArgusCord Assets
                string roamingAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string arguscordDir = System.IO.Path.Combine(roamingAppData, "ArgusCord");
                string distDir = System.IO.Path.Combine(arguscordDir, "dist");
                
                Log("[INFO] Preparing ArgusCord directory...");
                Directory.CreateDirectory(distDir);

                Log("[INFO] Extracting core files...");
                ExtractResource("patcher.js", System.IO.Path.Combine(distDir, "patcher.js"));
                ExtractResource("preload.js", System.IO.Path.Combine(distDir, "preload.js"));
                ExtractResource("renderer.js", System.IO.Path.Combine(distDir, "renderer.js"));
                ExtractResource("renderer.css", System.IO.Path.Combine(distDir, "renderer.css"));
                InstallProgressBar.Value = 60;

                // 3. Patch app.asar
                string resourcesPath = install.ResourcesPath;
                string appAsar = System.IO.Path.Combine(resourcesPath, "app.asar");
                string backupAsar = System.IO.Path.Combine(resourcesPath, "_app.asar");
                string patcherPath = System.IO.Path.Combine(distDir, "patcher.js");

                if (File.Exists(appAsar) && !File.Exists(backupAsar))
                {
                    Log("[INFO] Creating backup of original app.asar...");
                    File.Move(appAsar, backupAsar);
                }
                else if (!File.Exists(appAsar) && !File.Exists(backupAsar))
                {
                    throw new FileNotFoundException("Could not find Discord's app.asar file to patch.");
                }

                Log("[INFO] Injecting loader patch...");
                WriteAppAsar(appAsar, patcherPath);
                InstallProgressBar.Value = 100;

                Log("[SUCCESS] ArgusCord successfully installed!");
                StatusLabel.Text = "Status: Patched successfully!";
                MessageBox.Show("ArgusCord has been successfully installed!\nPlease launch Discord to enjoy the changes.", "ArgusCord Installer", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Log($"[ERROR] Installation failed: {ex.Message}");
                StatusLabel.Text = "Installation failed.";
                MessageBox.Show($"Failed to install ArgusCord:\n{ex.Message}", "ArgusCord Installer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                InstallProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void UninstallButton_Click(object sender, RoutedEventArgs e)
        {
            int index = DiscordBranchComboBox.SelectedIndex;
            if (index < 0 || index >= _detectedInstalls.Count)
            {
                MessageBox.Show("Please select a valid Discord installation first.", "ArgusCord Installer", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var install = _detectedInstalls[index];
            Log($"\n[INFO] Starting uninstallation for {install.Name}...");

            try
            {
                KillDiscordProcesses(install.Name);

                string resourcesPath = install.ResourcesPath;
                string appAsar = System.IO.Path.Combine(resourcesPath, "app.asar");
                string backupAsar = System.IO.Path.Combine(resourcesPath, "_app.asar");

                if (File.Exists(backupAsar))
                {
                    if (File.Exists(appAsar))
                    {
                        File.Delete(appAsar);
                    }
                    Log("[INFO] Restoring original app.asar...");
                    File.Move(backupAsar, appAsar);
                    Log("[SUCCESS] ArgusCord successfully uninstalled.");
                    StatusLabel.Text = "Status: Uninstalled";
                    MessageBox.Show("ArgusCord has been successfully uninstalled.\nDiscord is restored to its original state.", "ArgusCord Installer", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Log("[WARNING] ArgusCord is not currently installed on this version.");
                    MessageBox.Show("ArgusCord was not detected on this Discord installation.", "ArgusCord Installer", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                Log($"[ERROR] Uninstallation failed: {ex.Message}");
                MessageBox.Show($"Failed to uninstall ArgusCord:\n{ex.Message}", "ArgusCord Installer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void KillDiscordProcesses(string name)
        {
            string processName = name.Replace(" ", ""); // DiscordStable -> Discord, DiscordPTB -> DiscordPTB, etc.
            if (processName == "DiscordStable")
            {
                processName = "Discord";
            }

            Log($"[INFO] Checking for running {processName} processes...");
            var processes = Process.GetProcessesByName(processName);
            if (processes.Length > 0)
            {
                Log($"[INFO] Closing {processes.Length} Discord process(es)...");
                foreach (var proc in processes)
                {
                    try
                    {
                        proc.Kill();
                        proc.WaitForExit(3000);
                    }
                    catch (Exception ex)
                    {
                        Log($"[WARNING] Could not close process {proc.Id}: {ex.Message}");
                    }
                }
            }
        }

        private void ExtractResource(string resourceName, string outputPath)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string fullResourceName = $"ArgusCordInstaller.Resources.{resourceName}";
            
            using (var stream = assembly.GetManifestResourceStream(fullResourceName))
            {
                if (stream == null)
                {
                    throw new Exception($"Embedded resource '{fullResourceName}' not found.");
                }
                using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }
            }
            Log($"[INFO] Extracted {resourceName} successfully.");
        }

        private void WriteAppAsar(string outFile, string patcherPath)
        {
            string packageJson = "{\n\t\"name\": \"discord\",\n\t\"main\": \"index.js\"\n}";
            
            // Format require statement: require("C:\\Users\\...\\patcher.js")
            string escapedPath = patcherPath.Replace("\\", "\\\\").Replace("\"", "\\\"");
            string indexJsContents = $"require(\"{escapedPath}\")";

            int indexJsBytes = Encoding.UTF8.GetByteCount(indexJsContents);
            int packageJsonBytes = Encoding.UTF8.GetByteCount(packageJson);

            // Construct header JSON
            string headerJson = $"{{\"files\":{{\"index.js\":{{\"size\":{indexJsBytes},\"offset\":\"0\"}},\"package.json\":{{\"size\":{packageJsonBytes},\"offset\":\"{indexJsBytes}\"}}}}}}";

            byte[] headerBytes = Encoding.UTF8.GetBytes(headerJson);
            uint headerStringSize = (uint)headerBytes.Length;
            uint dataSize = 4;
            uint alignedSize = (headerStringSize + dataSize - 1) & ~(dataSize - 1);
            uint headerSize = alignedSize + 8;
            uint headerObjectSize = alignedSize + dataSize;
            uint diff = alignedSize - headerStringSize;

            byte[] paddedHeaderBytes = new byte[alignedSize];
            Array.Copy(headerBytes, paddedHeaderBytes, headerBytes.Length);
            for (uint i = headerStringSize; i < alignedSize; i++)
            {
                paddedHeaderBytes[i] = 0x30; // '0' padding byte
            }

            byte[] fileContentsBytes = Encoding.UTF8.GetBytes(indexJsContents + packageJson);

            using (var fs = new FileStream(outFile, FileMode.Create, FileAccess.Write))
            using (var bw = new BinaryWriter(fs))
            {
                // ASAR header integers are 32-bit little-endian
                bw.Write((int)dataSize);
                bw.Write((int)headerSize);
                bw.Write((int)headerObjectSize);
                bw.Write((int)headerStringSize);

                bw.Write(paddedHeaderBytes);
                bw.Write(fileContentsBytes);
            }
        }
    }
}