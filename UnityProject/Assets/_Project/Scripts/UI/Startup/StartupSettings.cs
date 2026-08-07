using System;
using System.Collections.Generic;
using System.Linq;

namespace LastHost.Prototype.UI.Startup
{
    public enum StartupDisplayMode
    {
        ExclusiveFullScreen = 0,
        FullScreenWindow = 1,
        MaximizedWindow = 2,
        Windowed = 3
    }

    public readonly struct StartupResolution : IEquatable<StartupResolution>
    {
        public StartupResolution(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }
        public int Height { get; }
        public bool IsValid => Width > 0 && Height > 0;
        public bool IsSixteenByNine => IsValid && (long)Width * 9L == (long)Height * 16L;
        public long PixelCount => IsValid ? (long)Width * Height : 0L;

        public bool Equals(StartupResolution other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            return obj is StartupResolution other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Width * 397) ^ Height;
            }
        }

        public override string ToString()
        {
            return $"{Width}x{Height}";
        }

        public static bool operator ==(StartupResolution left, StartupResolution right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StartupResolution left, StartupResolution right)
        {
            return !left.Equals(right);
        }
    }

    public sealed class StartupSettings : IEquatable<StartupSettings>
    {
        public const int CurrentSchemaVersion = 1;

        public StartupSettings(
            StartupLanguage language,
            StartupDisplayMode displayMode,
            StartupResolution resolution,
            int vSyncCount)
            : this(CurrentSchemaVersion, language, displayMode, resolution, vSyncCount)
        {
        }

        public StartupSettings(
            int schemaVersion,
            StartupLanguage language,
            StartupDisplayMode displayMode,
            StartupResolution resolution,
            int vSyncCount)
        {
            SchemaVersion = schemaVersion;
            Language = language;
            DisplayMode = displayMode;
            Resolution = resolution;
            VSyncCount = vSyncCount;
        }

        public int SchemaVersion { get; }
        public StartupLanguage Language { get; }
        public StartupDisplayMode DisplayMode { get; }
        public StartupResolution Resolution { get; }
        public int VSyncCount { get; }

        public bool Equals(StartupSettings other)
        {
            return other != null &&
                   SchemaVersion == other.SchemaVersion &&
                   Language == other.Language &&
                   DisplayMode == other.DisplayMode &&
                   Resolution == other.Resolution &&
                   VSyncCount == other.VSyncCount;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StartupSettings);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = SchemaVersion;
                hashCode = (hashCode * 397) ^ (int)Language;
                hashCode = (hashCode * 397) ^ (int)DisplayMode;
                hashCode = (hashCode * 397) ^ Resolution.GetHashCode();
                hashCode = (hashCode * 397) ^ VSyncCount;
                return hashCode;
            }
        }
    }

    public sealed class StartupSettingsDraft
    {
        public StartupSettingsDraft(StartupSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            ReplaceWith(settings);
        }

        public StartupLanguage Language { get; private set; }
        public StartupDisplayMode DisplayMode { get; private set; }
        public StartupResolution Resolution { get; private set; }
        public int VSyncCount { get; private set; }

        public void SetLanguage(StartupLanguage language)
        {
            Language = language;
        }

        public void SetDisplayMode(StartupDisplayMode displayMode)
        {
            DisplayMode = displayMode;
        }

        public void SetResolution(StartupResolution resolution)
        {
            Resolution = resolution;
        }

        public void SetVSyncCount(int vSyncCount)
        {
            VSyncCount = vSyncCount;
        }

        public void ReplaceWith(StartupSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Language = settings.Language;
            DisplayMode = settings.DisplayMode;
            Resolution = settings.Resolution;
            VSyncCount = settings.VSyncCount;
        }

        public StartupSettings ToSettings()
        {
            return new StartupSettings(Language, DisplayMode, Resolution, VSyncCount);
        }
    }

    public static class StartupSettingsDefaults
    {
        public static readonly StartupResolution PreferredResolution = new StartupResolution(1920, 1080);

        public const StartupLanguage DefaultLanguage = StartupLanguage.Korean;
        public const StartupDisplayMode DefaultDisplayMode = StartupDisplayMode.FullScreenWindow;
        public const int DefaultVSyncCount = 1;

        public static StartupSettings Create(
            IEnumerable<StartupResolution> supportedResolutions,
            StartupResolution currentResolution)
        {
            return new StartupSettings(
                DefaultLanguage,
                DefaultDisplayMode,
                SelectResolution(supportedResolutions, currentResolution),
                DefaultVSyncCount);
        }

        public static StartupResolution SelectResolution(
            IEnumerable<StartupResolution> supportedResolutions,
            StartupResolution currentResolution)
        {
            var supported = NormalizeSupportedResolutions(supportedResolutions);
            if (supported.Contains(PreferredResolution))
            {
                return PreferredResolution;
            }

            var sixteenByNine = supported.Where(resolution => resolution.IsSixteenByNine).ToArray();
            if (sixteenByNine.Length > 0)
            {
                return SelectHighest(sixteenByNine);
            }

            if (supported.Count > 0)
            {
                return SelectHighest(supported);
            }

            return currentResolution.IsValid ? currentResolution : PreferredResolution;
        }

        public static bool IsValid(
            StartupSettings settings,
            IEnumerable<StartupResolution> supportedResolutions,
            StartupResolution currentResolution)
        {
            if (settings == null ||
                settings.SchemaVersion != StartupSettings.CurrentSchemaVersion ||
                !Enum.IsDefined(typeof(StartupLanguage), settings.Language) ||
                !Enum.IsDefined(typeof(StartupDisplayMode), settings.DisplayMode) ||
                !settings.Resolution.IsValid ||
                (settings.VSyncCount != 0 && settings.VSyncCount != 1))
            {
                return false;
            }

            var supported = NormalizeSupportedResolutions(supportedResolutions);
            if (supported.Count > 0)
            {
                return supported.Contains(settings.Resolution);
            }

            var effectiveCurrent = currentResolution.IsValid ? currentResolution : PreferredResolution;
            return settings.Resolution == effectiveCurrent;
        }

        public static IReadOnlyList<StartupResolution> NormalizeSupportedResolutions(
            IEnumerable<StartupResolution> supportedResolutions)
        {
            if (supportedResolutions == null)
            {
                return Array.Empty<StartupResolution>();
            }

            return supportedResolutions
                .Where(resolution => resolution.IsValid)
                .Distinct()
                .OrderBy(resolution => resolution.Width)
                .ThenBy(resolution => resolution.Height)
                .ToArray();
        }

        private static StartupResolution SelectHighest(IEnumerable<StartupResolution> resolutions)
        {
            return resolutions
                .OrderByDescending(resolution => resolution.PixelCount)
                .ThenByDescending(resolution => resolution.Width)
                .ThenByDescending(resolution => resolution.Height)
                .First();
        }
    }
}
