﻿#region copyright
// -------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov [https://codestage.net]
// -------------------------------------------------------
#endregion

namespace CodeStage.Maintainer.Issues.Detectors
{
	using Core;
	using Core.Scan;

	internal interface ISettingsAssetBeginIssueDetector : IAssetBeginScanListener<DetectorResults>
	{
		AssetSettingsKind SettingsKind { get; }
	}
}