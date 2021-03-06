// Copyright 2021 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.IO;

namespace Fusion.Build.Haz
{
    [PublicAPI]
    public interface IHazReports : IHazArtifacts
    {
        AbsolutePath ReportDirectory => ArtifactsDirectory / "reports";
    }
}
