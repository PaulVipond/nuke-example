// Copyright 2020 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.IO;

namespace Fusion.Build.Haz
{
    [PublicAPI]
    public interface IHazPackageArtifacts : IHazArtifacts
    {
        AbsolutePath PackageArtifactsDirectory => ArtifactsDirectory / "packages";
    }
}
