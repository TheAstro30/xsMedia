namespace xsVlc.Interop
{
    public enum LibvlcStateT
    {
        LibvlcNothingSpecial = 0,
        LibvlcOpening = 1,
        LibvlcBuffering = 2,
        LibvlcPlaying = 3,
        LibvlcPaused = 4,
        LibvlcStopped = 5,
        LibvlcEnded = 6,
        LibvlcError = 7
    }

    public enum LibvlcLogMessateTSeverity
    {
        Info = 0,
        Err = 1,
        Warn = 2,
        Dbg = 3
    }

    public enum LibvlcLogLevel
    {
        LibvlcDebug = 0,   /**< Debug message */
        LibvlcNotice = 2,  /**< Important informational message */
        LibvlcWarning = 3, /**< Warning (potential error) message */
        LibvlcError = 4    /**< Error message */
    }

    public enum LibvlcEventE
    {
        LibvlcMediaMetaChanged = 0,
        LibvlcMediaSubItemAdded,
        LibvlcMediaDurationChanged,
        LibvlcMediaParsedChanged,
        LibvlcMediaFreed,
        LibvlcMediaStateChanged,
        LibvlcMediaPlayerMediaChanged = 0x100,
        LibvlcMediaPlayerNothingSpecial,
        LibvlcMediaPlayerOpening,
        LibvlcMediaPlayerBuffering,
        LibvlcMediaPlayerPlaying,
        LibvlcMediaPlayerPaused,
        LibvlcMediaPlayerStopped,
        LibvlcMediaPlayerForward,
        LibvlcMediaPlayerBackward,
        LibvlcMediaPlayerEndReached,
        LibvlcMediaPlayerEncounteredError,
        LibvlcMediaPlayerTimeChanged,
        LibvlcMediaPlayerPositionChanged,
        LibvlcMediaPlayerSeekableChanged,
        LibvlcMediaPlayerPausableChanged,
        LibvlcMediaPlayerTitleChanged,
        LibvlcMediaPlayerSnapshotTaken,
        LibvlcMediaPlayerLengthChanged,
        LibvlcMediaListItemAdded = 0x200,
        LibvlcMediaListWillAddItem,
        LibvlcMediaListItemDeleted,
        LibvlcMediaListWillDeleteItem,
        LibvlcMediaListViewItemAdded = 0x300,
        LibvlcMediaListViewWillAddItem,
        LibvlcMediaListViewItemDeleted,
        LibvlcMediaListViewWillDeleteItem,
        LibvlcMediaListPlayerPlayed = 0x400,
        LibvlcMediaListPlayerNextItemSet,
        LibvlcMediaListPlayerStopped,
        LibvlcMediaDiscovererStarted = 0x500,
        LibvlcMediaDiscovererEnded,
        LibvlcVlmMediaAdded = 0x600,
        LibvlcVlmMediaRemoved,
        LibvlcVlmMediaChanged,
        LibvlcVlmMediaInstanceStarted,
        LibvlcVlmMediaInstanceStopped,
        LibvlcVlmMediaInstanceStatusInit,
        LibvlcVlmMediaInstanceStatusOpening,
        LibvlcVlmMediaInstanceStatusPlaying,
        LibvlcVlmMediaInstanceStatusPause,
        LibvlcVlmMediaInstanceStatusEnd,
        LibvlcVlmMediaInstanceStatusError,
    }

    public enum LibvlcPlaybackModeT
    {
        LibvlcPlaybackModeDefault = 0,
        LibvlcPlaybackModeLoop = 1,
        LibvlcPlaybackModeRepeat = 2
    }

    public enum LibvlcMetaT
    {
        LibvlcMetaTitle = 1,
        LibvlcMetaArtist = 2,
        LibvlcMetaGenre = 3,
        LibvlcMetaCopyright = 4,
        LibvlcMetaAlbum = 5,
        LibvlcMetaTrackNumber = 6,
        LibvlcMetaDescription = 7,
        LibvlcMetaRating = 8,
        LibvlcMetaDate = 9,
        LibvlcMetaSetting = 10,
        LibvlcMetaUrl = 11,
        LibvlcMetaLanguage = 12,
        LibvlcMetaNowPlaying = 13,
        LibvlcMetaPublisher = 14,
        LibvlcMetaEncodedBy = 15,
        LibvlcMetaArtworkUrl = 16,
        LibvlcMetaTrackId = 17
    }

    public enum LibvlcTrackTypeT
    {
        LibvlcTrackUnknown = -1,
        LibvlcTrackAudio = 0,
        LibvlcTrackVideo = 1,
        LibvlcTrackText = 2,
    }

    public enum LibvlcVideoMarqueeOptionT
    {
        LibvlcMarqueeEnable = 0,
        LibvlcMarqueeText = 1,
        LibvlcMarqueeColor = 2,
        LibvlcMarqueeOpacity = 3,
        LibvlcMarqueePosition = 4,
        LibvlcMarqueeRefresh = 5,
        LibvlcMarqueeSize = 6,
        LibvlcMarqueeTimeout = 7,
        LibvlcMarqueeX = 8,
        LibvlcMarqueeY = 9
    }

    public enum LibvlcVideoLogoOptionT
    {
        LibvlcLogoEnable = 0,
        LibvlcLogoFile = 1,
        LibvlcLogoX = 2,
        LibvlcLogoY = 3,
        LibvlcLogoDelay = 4,
        LibvlcLogoRepeat = 5,
        LibvlcLogoOpacity = 6,
        LibvlcLogoPosition = 7,
    }

    public enum LibvlcVideoAdjustOptionT
    {
        LibvlcAdjustEnable = 0,
        LibvlcAdjustContrast = 1,
        LibvlcAdjustBrightness = 2,
        LibvlcAdjustHue = 3,
        LibvlcAdjustSaturation = 4,
        LibvlcAdjustGamma = 5,
    }

    public enum LibvlcAudioOutputDeviceTypesT
    {
        LibvlcAudioOutputDeviceError = -1,
        LibvlcAudioOutputDeviceMono = 1,
        LibvlcAudioOutputDeviceStereo = 2,
        LibvlcAudioOutputDevice_2F2R = 4,
        LibvlcAudioOutputDevice_3F2R = 5,
        LibvlcAudioOutputDevice51 = 6,
        LibvlcAudioOutputDevice61 = 7,
        LibvlcAudioOutputDevice71 = 8,
        LibvlcAudioOutputDeviceSpdif = 10
    }

    public enum LibvlcAudioOutputChannelT
    {
        LibvlcAudioChannelError = -1,
        LibvlcAudioChannelStereo = 1,
        LibvlcAudioChannelRStereo = 2,
        LibvlcAudioChannelLeft = 3,
        LibvlcAudioChannelRight = 4,
        LibvlcAudioChannelDolbys = 5
    }

    public enum LibvlcNavigateModeT
    {
        LibvlcNavigateActivate = 0,
        LibvlcNavigateUp = 1,
        LibvlcNavigateDown = 2,
        LibvlcNavigateLeft = 3,
        LibvlcNavigateRight = 4
    }
}
