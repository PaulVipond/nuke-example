using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.Json;
using Nuke.Common;
using Nuke.Common.Execution;

namespace Fusion.Build
{
    public class CiStats
    {
        public DateTime buildStart { get; set; }
        public DateTime buildCompleted { get; set; }
        public string solutionName { get; set; }
        public string configuration{ get; set; }
        public bool buildSucceeded { get; set; }
        public List<BuildStep> skippedTargets { get; }
        public List<BuildStep> failedTargets { get; }
        public List<BuildStep> succeededTargets { get; }
        public List<BuildStep> abortedTargets { get; }
        public String timeZone { get; set; }
        public String requestedBy { get; set; }
        public String ciBuildId { get; set; }
        public String machineName { get; set; }
        public String osVersion { get; set; }
        public String osDescription { get; set; }
        public String osSpMajor { get; set; }
        public String osSpMinor { get; set; }
        public bool is64BitOs { get; set; }
        public bool is64BitProcess { get; set; }
        public int physicalMemory { get; set; }
        public String processor { get; set; }
        public int cores { get; set; }
        public int physicalProcessors { get; set; }
        public int logicalProcessors { get; set; }
        public String ciDiskType { get; set; }
        public String ciDiskMedium { get; set; }
        public String ciDriveLetter { get; set; }
        public String ciDriveFormat { get; set; }
        public int ciDiskSize { get; set; }
        public int ciTotalDiskSpace { get; set; }
        public int ciAvailableDiskSpace { get; set; }

        public CiStats(string solutionName, string configuration)
        {
            try
            {
                this.solutionName = solutionName;
                this.configuration = configuration;
                buildStart = DateTime.UtcNow;
                skippedTargets = new List<BuildStep>();
                failedTargets = new List<BuildStep>();
                succeededTargets = new List<BuildStep>();
                abortedTargets = new List<BuildStep>();

                timeZone = System.TimeZoneInfo.Local.ToString();
                requestedBy = Environment.UserName;
                machineName = Environment.MachineName;
                osVersion = Environment.OSVersion.ToString();
                is64BitOs = Environment.Is64BitOperatingSystem;
                is64BitProcess = Environment.Is64BitProcess;

                var gcMemoryInfo = GC.GetGCMemoryInfo();
                var installedMemory = gcMemoryInfo.TotalAvailableMemoryBytes;
                physicalMemory = (int) Math.Round((double)installedMemory / (1048576.0 * 1024.0));
                logicalProcessors = Environment.ProcessorCount;

                GetStatsItemsFromWinQuery();
                GetDriveDetails();
            }
            catch (Exception ex)
            {
                Logger.Warn($"The build will continue, but the build stats collection step failed with the following error: {ex}");
            }
        }

        void GetStatsItemsFromWinQuery()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            var info = searcher.Get().Cast<ManagementObject>().FirstOrDefault();
            var caption = info.Properties["Caption"].Value.ToString();
            var version = info.Properties["Version"].Value.ToString();
            var spMajorVersion = info.Properties["ServicePackMajorVersion"].Value.ToString();
            var spMinorVersion = info.Properties["ServicePackMinorVersion"].Value.ToString();
            osDescription = $"{caption} {version}";
            osSpMajor = spMajorVersion;
            osSpMinor = spMinorVersion;

            cores = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                cores += int.Parse(item["NumberOfCores"].ToString());
                processor = item["name"].ToString();
            }

            physicalProcessors = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                physicalProcessors += Convert.ToInt32(item["NumberOfProcessors"]);
            }
        }

        void GetDriveDetails()
        {
            ciDriveLetter = Path.GetPathRoot(System.Reflection.Assembly.GetEntryAssembly().Location);
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true && d.Name == ciDriveLetter)
                {
                    ciDriveFormat = d.DriveFormat;
                    ciDiskType = d.DriveType.ToString();
                    ciDiskSize = Convert.ToInt32(d.TotalSize / (1024 * 1024 * 1024));
                    ciTotalDiskSpace = Convert.ToInt32(d.TotalFreeSpace / (1024 * 1024 * 1024));
                    ciAvailableDiskSpace = Convert.ToInt32(d.TotalFreeSpace / (1024 * 1024 * 1024));
                }
            }

            ManagementScope scope = new ManagementScope(@"\\.\root\microsoft\windows\storage");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM MSFT_PhysicalDisk");
            scope.Connect();
            searcher.Scope = scope;

            foreach (ManagementObject queryObj in searcher.Get())
            {
                switch (Convert.ToInt16(queryObj["MediaType"]))
                {
                    case 1:
                        ciDiskMedium = "Unspecified";
                        break;

                    case 3:
                        ciDiskMedium = "HDD";
                        break;

                    case 4:
                        ciDiskMedium = "SSD";
                        break;

                    case 5:
                        ciDiskMedium = "SCM";
                        break;

                    default:
                        ciDiskMedium = "Unspecified";
                        break;
                }
            }
            searcher.Dispose();
        }
        override public string ToString()
        {
            return JsonSerializer.Serialize<CiStats>(this);
        }

        public void PopulateTargetOutcomesFromNukeTargets(IReadOnlyCollection<ExecutableTarget> nukeTargets, List<BuildStep> buildSteps)
        {
            foreach (ExecutableTarget target in nukeTargets)
            {
                buildSteps.Add(new BuildStep { name = target.Name, duration = (long) target.Duration.TotalSeconds });
            }
        }
    }
}
