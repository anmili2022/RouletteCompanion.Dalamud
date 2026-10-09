using System;
using System.IO;

namespace RouletteCompanion.Helpers;

internal static class LegacyDataMigrator
{
    private const string LegacyInternalName = "RouletteBuddy";
    private const string LegacyConfigFileName = "RouletteBuddy.json";
    private const string CurrentConfigFileName = "RouletteCompanion.json";

    public static void MigrateIfNeeded(string configDirectory)
    {
        try
        {
            var configRoot = Path.GetDirectoryName(configDirectory);
            if (configRoot is null)
            {
                return;
            }

            var legacyConfigDir = Path.Combine(configRoot, LegacyInternalName);
            var currentConfigDir = configDirectory;

            Directory.CreateDirectory(currentConfigDir);

            var legacyConfigFile = Path.Combine(configRoot, LegacyConfigFileName);
            var currentConfigFile = Path.Combine(configRoot, CurrentConfigFileName);
            if (File.Exists(legacyConfigFile) && !File.Exists(currentConfigFile))
            {
                File.Copy(legacyConfigFile, currentConfigFile);
            }

            if (!Directory.Exists(legacyConfigDir))
            {
                return;
            }

            foreach (var fileName in new[] { "data.json", "task_history.json", "risui.json", "data_pending.json", "data.csv" })
            {
                var legacyFile = Path.Combine(legacyConfigDir, fileName);
                var currentFile = Path.Combine(currentConfigDir, fileName);
                if (File.Exists(legacyFile) && !File.Exists(currentFile))
                {
                    File.Copy(legacyFile, currentFile);
                }
            }
        }
        catch (Exception e)
        {
            Plugin.PluginLog.Warning(e, "Failed to migrate legacy data");
        }
    }
}
