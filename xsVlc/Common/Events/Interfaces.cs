using System;

namespace xsVlc.Common.Events
{
    public interface IEventBroker
    {
        event EventHandler MediaEnded;
        event EventHandler<MediaPlayerTimeChanged> TimeChanged;
        event EventHandler<MediaPlayerMediaChanged> MediaChanged;
        event EventHandler NothingSpecial;
        event EventHandler PlayerOpening;
        event EventHandler PlayerBuffering;
        event EventHandler PlayerPlaying;
        event EventHandler PlayerPaused;
        event EventHandler PlayerStopped;
        event EventHandler PlayerForward;
        event EventHandler PlayerBackward;
        event EventHandler PlayerEncounteredError;
        event EventHandler<MediaPlayerPositionChanged> PlayerPositionChanged;
        event EventHandler<MediaPlayerSeekableChanged> PlayerSeekableChanged;
        event EventHandler<MediaPlayerPausableChanged> PlayerPausableChanged;
        event EventHandler<MediaPlayerTitleChanged> PlayerTitleChanged;
        event EventHandler<MediaPlayerSnapshotTaken> PlayerSnapshotTaken;
        event EventHandler<MediaPlayerLengthChanged> PlayerLengthChanged;
    }

    public interface IEventProvider
    {
        IntPtr EventManagerHandle { get; }
    }

    public interface IMediaDiscoveryEvents
    {
        event EventHandler MediaDiscoveryStarted;
        event EventHandler MediaDiscoveryEnded;
    }

    public interface IMediaEvents
    {
        event EventHandler<MediaMetaDataChange> MetaDataChanged;
        event EventHandler<MediaNewSubItem> SubItemAdded;
        event EventHandler<MediaDurationChange> DurationChanged;
        event EventHandler<MediaParseChange> ParsedChanged;
        event EventHandler<MediaFree> MediaFreed;
        event EventHandler<MediaStateChange> StateChanged;
    }

    public interface IMediaListEvents
    {
        event EventHandler<MediaListItemAdded> ItemAdded;
        event EventHandler<MediaListWillAddItem> WillAddItem;
        event EventHandler<MediaListItemDeleted> ItemDeleted;
        event EventHandler<MediaListWillDeleteItem> WillDeleteItem;
    }

    public interface IMediaListPlayerEvents
    {
        event EventHandler MediaListPlayerPlayed;
        event EventHandler<MediaListPlayerNextItemSet> MediaListPlayerNextItemSet;
        event EventHandler MediaListPlayerStopped;
    }
}
