/* xsMedia - xsPlaylist
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using xsCore.Utils;

namespace xsPlaylist.Utils
{
    public static class Filters
    {
        static Filters()
        {
            /* Open media filters */
            OpenFilters = new FilterMasks
                              {
                                  AllowAllFiles = true,
                                  AllFiles = @"All supported media types"
                              };
            OpenFilters.AddRange(new[]
                                     {
                                         new FileMasks("Video Files",
                                                       "*.avi;*.qt;*.mov;*.mpg;*.mpeg;*.m1v;*.mp4;*.mkv;*.rmvb;*.ogm;*.wmv;*.flv;*.m4v")
                                         ,
                                         new FileMasks("Audio Files",
                                                       "*.wav;*.mpa;*.mp2;*.mp3;*.au;*.aif;*.aiff;*.snd;*.ogg;*.wma;*.m4a"),
                                         new FileMasks("Tracker Modules", "*.mod;*.xm;*.s3m;*.it;*.ned;*.mo3;*.mtm;*.umx"),
                                         new FileMasks("MIDI Files", "*.mid;*.midi"),
                                         new FileMasks("ASX Files", "*.asx")
                                     });
            /* Playlist filters */
            OpenPlaylistFilters = new FilterMasks
                                      {
                                          AllowAllFiles = true,
                                          AllFiles = @"All supported playlist files"
                                      };
            OpenPlaylistFilters.AddRange(new[]
                                             {
                                                 new FileMasks("xsMedia playlist files", "*.xspl"),
                                                 new FileMasks("Vlc playlist files", "*.xspf"),
                                                 new FileMasks("Windows media playlist files", "*.wpl"),
                                                 new FileMasks("Winamp playlist files", "*.m3u"),
                                                 new FileMasks("Winamp playlist files UTF-8", "*.m3u8"),
                                                 new FileMasks("Playlist files", "*.pls")
                                             });

            SavePlaylistFilters = new FilterMasks(OpenPlaylistFilters, false, null);
            /* Meta data editor filters */
            CoverArtFilters = new FilterMasks
                                  {
                                      AllowAllFiles = true,
                                      AllFiles = @"All supported image formats"
                                  };
            CoverArtFilters.AddRange(new[]
                                         {
                                             new FileMasks("JPG Images", "*.jpg;*.jpeg"),
                                             new FileMasks("Bitmap Images", "*.bmp"),
                                             new FileMasks("PNG Images", "*.png")
                                         });
            /* Subtitle filters */
            SubtitleFilters = new FilterMasks
                                  {
                                      AllowAllFiles = false
                                  };
            SubtitleFilters.Add(new FileMasks("Subtitle Files", "*.cdg;*.idx;*.srt;*.sub;*.utf;*.ass;*.ssa;*.aqt;*.jss;*.psb;*.rt;*.smi;*.txt;*.smil;*.stl;*.usf;*.dks;*.pjs;*.mpl2;*.mks"));
        }

        public static FilterMasks OpenFilters { get; private set; }
        public static FilterMasks OpenPlaylistFilters { get; private set; }
        public static FilterMasks SavePlaylistFilters { get; private set; }
        public static FilterMasks CoverArtFilters { get; private set; }
        public static FilterMasks SubtitleFilters { get; private set; }
    }
}
