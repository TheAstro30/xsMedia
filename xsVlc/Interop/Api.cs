//    nVLC
//    
//    Author:  Roman Ginzburg
//
//    nVLC is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    nVLC is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//     
// ========================================================================

using System;
using System.Runtime.InteropServices;

namespace xsVlc.Interop
{
    public static class Api
    {
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_errmsg(); /* LPstr */

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_clearerr();

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_new(int argc, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] argv);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_release(IntPtr libvlcInstanceT);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_retain(IntPtr pInstance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_add_intf(IntPtr pInstance, [MarshalAs(UnmanagedType.LPArray)] byte[] name);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_set_exit_handler(IntPtr pInstance, IntPtr callback, IntPtr opaque);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_wait(IntPtr pInstance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_set_user_agent(IntPtr pInstance, [MarshalAs(UnmanagedType.LPArray)] byte[] name, [MarshalAs(UnmanagedType.LPArray)] byte[] http);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_get_version();

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_get_compiler();

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_get_changeset();

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_event_attach(IntPtr pEventManager, LibvlcEventE iEventType, IntPtr fCallback, IntPtr userData);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_event_detach(IntPtr pEventManager, LibvlcEventE iEventType, IntPtr fCallback, IntPtr userData);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]        
        public static extern IntPtr libvlc_event_type_name(LibvlcEventE eventType); /* AnsiBStr */

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 libvlc_get_log_verbosity(IntPtr libvlcInstanceT);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_set_log_verbosity(IntPtr libvlcInstanceT, UInt32 level);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_log_open(IntPtr libvlcInstanceT);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_log_close(IntPtr libvlcLogT);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 libvlc_log_count(IntPtr libvlcLogT);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_log_clear(IntPtr libvlcLogT);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_log_get_iterator(IntPtr libvlcLogT);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_log_iterator_free(IntPtr libvlcLogIteratorT);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 libvlc_log_iterator_has_next(IntPtr libvlcLogIteratorT);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_log_iterator_next(IntPtr libvlcLogIteratorT, ref LibvlcLogMessageT pBuffer);

        [MinimalLibVlcVersion("1.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_audio_filter_list_get(IntPtr pInstance);

        [MinimalLibVlcVersion("1.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_video_filter_list_get(IntPtr pInstance);

        [MinimalLibVlcVersion("1.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_module_description_list_release(IntPtr pList);

        [MinimalLibVlcVersion("1.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 libvlc_clock();

        [MinimalLibVlcVersion("1.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 libvlc_delay(Int64 pts);

        [MinimalLibVlcVersion("2.1.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_log_subscribe(ref LibvlcLogSubscriber sub, IntPtr callback, IntPtr data);

        [MinimalLibVlcVersion("2.1.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_log_subscribe_file(ref LibvlcLogSubscriber sub, IntPtr file);

        [MinimalLibVlcVersion("2.1.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_log_unsubscribe(ref LibvlcLogSubscriber sub);


        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_new_location(IntPtr pInstance, [MarshalAs(UnmanagedType.LPArray)] byte[] pszMrl);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_new_path(IntPtr pInstance, [MarshalAs(UnmanagedType.LPArray)] byte[] pszMrl);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_new_as_node(IntPtr pInstance, [MarshalAs(UnmanagedType.LPArray)] byte[] pszMrl);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_add_option(IntPtr libvlcMediaInst, [MarshalAs(UnmanagedType.LPArray)] byte[] ppszOptions);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_add_option_flag(IntPtr pMd, [MarshalAs(UnmanagedType.LPArray)] byte[] ppszOptions, int iFlags);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_retain(IntPtr pMd);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_release(IntPtr libvlcMediaInst);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)] /* Changed */
        public static extern IntPtr libvlc_media_get_mrl(IntPtr pMd);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_duplicate(IntPtr pMd);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]        
        public static extern IntPtr libvlc_media_get_meta(IntPtr pMd, LibvlcMetaT eMeta); /* Changed */

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_set_meta(IntPtr pMd, LibvlcMetaT eMeta, [MarshalAs(UnmanagedType.LPArray)] byte[] pszValue);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_save_meta(IntPtr pMd);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern LibvlcStateT libvlc_media_get_state(IntPtr pMetaDesc);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_get_stats(IntPtr pMd, out LibvlcMediaStatsT pStats);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_subitems(IntPtr pMd);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_event_manager(IntPtr pMd);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 libvlc_media_get_duration(IntPtr pMd);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_parse(IntPtr media);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_parse_async(IntPtr media);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool libvlc_media_is_parsed(IntPtr pMd);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_set_user_data(IntPtr pMd, IntPtr pNewUserData);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_get_user_data(IntPtr pMd);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_get_tracks_info(IntPtr media, out IntPtr tracks);


        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_discoverer_new_from_name(IntPtr pInst, [MarshalAs(UnmanagedType.LPArray)] byte[] pszName);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_discoverer_release(IntPtr pMdis);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]        
        public static extern IntPtr libvlc_media_discoverer_localized_name(IntPtr pMdis);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_discoverer_media_list(IntPtr pMdis);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_discoverer_event_manager(IntPtr pMdis);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_discoverer_is_running(IntPtr pMdis);


        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_library_new(IntPtr pInstance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_library_release(IntPtr pMlib);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_library_retain(IntPtr pMlib);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_library_load(IntPtr pMlib);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_library_media_list(IntPtr pMlib);


        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_player_new(IntPtr pLibvlcInstance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_player_new_from_media(IntPtr libvlcMedia);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_release(IntPtr libvlcMediaplayer);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_retain(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_player_get_media(IntPtr libvlcMediaplayer);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_set_media(IntPtr libvlcMediaPlayerT, IntPtr libvlcMediaT);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_player_event_manager(IntPtr libvlcMediaPlayerT);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_player_is_playing(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_play(IntPtr libvlcMediaplayer);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_set_pause(IntPtr mp, int doPause);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_pause(IntPtr libvlcMediaplayer);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_stop(IntPtr libvlcMediaplayer);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_callbacks(IntPtr mp, IntPtr @lock, IntPtr unlock, IntPtr display, IntPtr opaque);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_format(IntPtr mp, [MarshalAs(UnmanagedType.LPArray)] byte[] chroma, int width, int height, int pitch);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_set_hwnd(IntPtr libvlcMediaplayer, IntPtr libvlcDrawable);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_player_get_hwnd(IntPtr libvlcMediaplayer);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 libvlc_media_player_get_length(IntPtr libvlcMediaplayer);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 libvlc_media_player_get_time(IntPtr libvlcMediaplayer);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_set_time(IntPtr libvlcMediaplayer, Int64 time);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern float libvlc_media_player_get_position(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_set_position(IntPtr pMi, float fPos);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_set_chapter(IntPtr pMi, int iChapter);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_player_get_chapter(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_player_get_chapter_count(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool libvlc_media_player_will_play(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_player_get_chapter_count_for_title(IntPtr pMi, int iTitle);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_set_title(IntPtr pMi, int iTitle);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_player_get_title(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_player_get_title_count(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_previous_chapter(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_next_chapter(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern float libvlc_media_player_get_rate(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_player_set_rate(IntPtr pMi, float rate);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern LibvlcStateT libvlc_media_player_get_state(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern float libvlc_media_player_get_fps(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_player_has_vout(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool libvlc_media_player_is_seekable(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool libvlc_media_player_can_pause(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_next_frame(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_track_description_release(IntPtr pTrackDescription);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_toggle_fullscreen(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_set_fullscreen(IntPtr pMi, bool bFullscreen);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool libvlc_get_fullscreen(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_key_input(IntPtr pMi, bool on);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_mouse_input(IntPtr pMi, bool on);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_get_size(IntPtr pMi, uint num, out uint px, out uint py);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_get_height(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_get_width(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_get_cursor(IntPtr pMi, uint num, out int px, out int py);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern float libvlc_video_get_scale(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_scale(IntPtr pMi, float fFactor);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_video_get_aspect_ratio(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_aspect_ratio(IntPtr pMi, [MarshalAs(UnmanagedType.LPArray)] byte[] pszAspect);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_get_spu(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_get_spu_count(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_video_get_spu_description(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_set_spu(IntPtr pMi, int iSpu);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_set_subtitle_file(IntPtr pMi, [MarshalAs(UnmanagedType.LPArray)] byte[] pszSubtitle);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_video_get_title_description(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_video_get_chapter_description(IntPtr pMi, int iTitle);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]        
        public static extern IntPtr libvlc_video_get_crop_geometry(IntPtr pMi); /* AnsiBStr */

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_crop_geometry(IntPtr pMi, [MarshalAs(UnmanagedType.LPArray)] byte[] pszGeometry);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_get_teletext(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_teletext(IntPtr pMi, int iPage);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_toggle_teletext(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_get_track_count(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_video_get_track_description(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_get_track(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_set_track(IntPtr pMi, int iTrack);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_take_snapshot(IntPtr pMi, uint stream, [MarshalAs(UnmanagedType.LPArray)] byte[] filePath, UInt32 iWidth, UInt32 iHeight);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_deinterlace(IntPtr pMi, [MarshalAs(UnmanagedType.LPArray)] byte[] pszMode);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_get_marquee_int(IntPtr pMi, LibvlcVideoMarqueeOptionT option);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]        
        public static extern IntPtr libvlc_video_get_marquee_string(IntPtr pMi, LibvlcVideoMarqueeOptionT option); /* AnsiBStr */

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_marquee_int(IntPtr pMi, LibvlcVideoMarqueeOptionT option, int iVal);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_marquee_string(IntPtr pMi, LibvlcVideoMarqueeOptionT option, [MarshalAs(UnmanagedType.LPArray)] byte[] pszText);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_get_logo_int(IntPtr pMi, LibvlcVideoLogoOptionT option);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_logo_int(IntPtr pMi, LibvlcVideoLogoOptionT option, int value);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_logo_string(IntPtr pMi, LibvlcVideoLogoOptionT option, [MarshalAs(UnmanagedType.LPArray)] byte[] pszValue);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_get_adjust_int(IntPtr pMi, LibvlcVideoAdjustOptionT option);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_adjust_int(IntPtr pMi, LibvlcVideoAdjustOptionT option, int value);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern float libvlc_video_get_adjust_float(IntPtr pMi, LibvlcVideoAdjustOptionT option);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_adjust_float(IntPtr pMi, LibvlcVideoAdjustOptionT option, float value);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_audio_output_list_get(IntPtr pInstance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_audio_output_list_release(IntPtr pList);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_audio_output_set(IntPtr pMi, [MarshalAs(UnmanagedType.LPArray)] byte[] pszName);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_audio_output_device_count(IntPtr pInstance, [MarshalAs(UnmanagedType.LPArray)] byte[] pszAudioOutput);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_audio_output_device_list_get(IntPtr pInstance, [MarshalAs(UnmanagedType.LPArray)] byte[] pszAudioOutput);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_audio_output_device_set(IntPtr pMi, [MarshalAs(UnmanagedType.LPArray)] byte[] pszAudioOutput, [MarshalAs(UnmanagedType.LPArray)] byte[] pszDeviceId);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern LibvlcAudioOutputDeviceTypesT libvlc_audio_output_get_device_type(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_audio_output_set_device_type(IntPtr pMi, LibvlcAudioOutputDeviceTypesT deviceType);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_audio_toggle_mute(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_audio_get_volume(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_audio_set_volume(IntPtr pMi, int volume);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_audio_set_mute(IntPtr pMi, bool mute);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool libvlc_audio_get_mute(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_audio_get_track_count(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_audio_get_track_description(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_audio_get_track(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_audio_set_track(IntPtr pMi, int iTrack);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern LibvlcAudioOutputChannelT libvlc_audio_get_channel(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_audio_set_channel(IntPtr pMi, LibvlcAudioOutputChannelT channel);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 libvlc_audio_get_delay(IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_audio_set_delay(IntPtr pMi, Int64 iDelay);

        [MinimalLibVlcVersion("1.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_navigate(IntPtr pMi, LibvlcNavigateModeT navigate);

        [MinimalLibVlcVersion("1.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_format_callbacks(IntPtr pMi, IntPtr setup, IntPtr cleanup);

        [MinimalLibVlcVersion("1.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_audio_set_callbacks(IntPtr pMi, IntPtr play, IntPtr pause, IntPtr resume, IntPtr flush, IntPtr drain, IntPtr opaque);

        [MinimalLibVlcVersion("1.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_audio_set_volume_callback(IntPtr pMi, IntPtr volume);

        [MinimalLibVlcVersion("1.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_audio_set_format_callbacks(IntPtr pMi, IntPtr setup, IntPtr cleanup);

        [MinimalLibVlcVersion("1.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_audio_set_format(IntPtr pMi, [MarshalAs(UnmanagedType.LPArray)] byte[] format, int rate, int channels);

        /* Media list */
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_list_new(IntPtr pInstance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_list_release(IntPtr pMl);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_list_retain(IntPtr pMl);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_list_set_media(IntPtr pMl, IntPtr pMd);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_list_media(IntPtr pMl);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_list_add_media(IntPtr pMl, IntPtr pMd);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_list_insert_media(IntPtr pMl, IntPtr pMd, int iPos);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_list_remove_index(IntPtr pMl, int iPos);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_list_count(IntPtr pMl);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_list_item_at_index(IntPtr pMl, int iPos);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_list_index_of_item(IntPtr pMl, IntPtr pMd);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_list_is_readonly(IntPtr pMl);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_list_lock(IntPtr pMl);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_list_unlock(IntPtr pMl);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_list_event_manager(IntPtr pMl);


        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_list_player_new(IntPtr pInstance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_list_player_release(IntPtr pMlp);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_list_player_event_manager(IntPtr pMlp);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_list_player_set_media_player(IntPtr pMlp, IntPtr pMi);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_list_player_set_media_list(IntPtr pMlp, IntPtr pMlist);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_list_player_play(IntPtr pMlp);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_list_player_pause(IntPtr pMlp);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_list_player_is_playing(IntPtr pMlp);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern LibvlcStateT libvlc_media_list_player_get_state(IntPtr pMlp);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_list_player_play_item_at_index(IntPtr pMlp, int iIndex);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_list_player_play_item(IntPtr pMlp, IntPtr pMd);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_list_player_stop(IntPtr pMlp);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_list_player_next(IntPtr pMlp);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_list_player_previous(IntPtr pMlp);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_list_player_set_playback_mode(IntPtr pMlp, LibvlcPlaybackModeT eMode);


        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_vlm_release(IntPtr pInstance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_add_broadcast(IntPtr pInstance,
                                               [MarshalAs(UnmanagedType.LPArray)] byte[] pszName,
                                               [MarshalAs(UnmanagedType.LPArray)] byte[] pszInput,
                                               [MarshalAs(UnmanagedType.LPArray)] byte[] pszOutput,
                                               int iOptions,
                                               [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] ppszOptions,
                                               int bEnabled,
                                               int bLoop);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_add_vod(IntPtr pInstance,
                                         [MarshalAs(UnmanagedType.LPArray)] byte[] pszName,
                                         [MarshalAs(UnmanagedType.LPArray)] byte[] pszInput,
                                         int iOptions,
                                         [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] ppszOptions,
                                         int bEnabled,
                                         [MarshalAs(UnmanagedType.LPArray)] byte[] pszMux);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_del_media(IntPtr pInstance, [MarshalAs(UnmanagedType.LPArray)] byte[] pszName);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_set_enabled(IntPtr pInstance,
                                             [MarshalAs(UnmanagedType.LPArray)] byte[] pszName,
                                             int bEnabled);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_set_output(IntPtr pInstance,
                                            [MarshalAs(UnmanagedType.LPArray)] byte[] pszName,
                                            [MarshalAs(UnmanagedType.LPArray)] byte[] pszOutput);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_set_input(IntPtr pInstance,
                                           [MarshalAs(UnmanagedType.LPArray)] byte[] pszName,
                                           [MarshalAs(UnmanagedType.LPArray)] byte[] pszInput);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_add_input(IntPtr pInstance,
                                           [MarshalAs(UnmanagedType.LPArray)] byte[] pszName,
                                           [MarshalAs(UnmanagedType.LPArray)] byte[] pszInput);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_set_loop(IntPtr pInstance,
                                          [MarshalAs(UnmanagedType.LPArray)] byte[] pszName,
                                          int bLoop);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_set_mux(IntPtr pInstance,
                                         [MarshalAs(UnmanagedType.LPArray)] byte[] pszName,
                                         [MarshalAs(UnmanagedType.LPArray)] byte[] pszMux);


        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_change_media(IntPtr pInstance,
                                              [MarshalAs(UnmanagedType.LPArray)] byte[] pszName,
                                              [MarshalAs(UnmanagedType.LPArray)] byte[] pszInput,
                                              [MarshalAs(UnmanagedType.LPArray)] byte[] pszOutput,
                                              int iOptions,
                                              [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] ppszOptions,
                                              int bEnabled,
                                              int bLoop);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_play_media(IntPtr pInstance,
                                             [MarshalAs(UnmanagedType.LPArray)] byte[] pszName);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_stop_media(IntPtr pInstance,
                                             [MarshalAs(UnmanagedType.LPArray)] byte[] pszName);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_pause_media(IntPtr pInstance,
                                             [MarshalAs(UnmanagedType.LPArray)] byte[] pszName);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_seek_media(IntPtr pInstance,
                                            [MarshalAs(UnmanagedType.LPArray)] byte[] pszName,
                                            float fPercentage);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_vlm_show_media(IntPtr pInstance,
                                                          [MarshalAs(UnmanagedType.LPArray)] byte[] pszName); /* LPStr */

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern float libvlc_vlm_get_media_instance_position(IntPtr pInstance,
                                                               [MarshalAs(UnmanagedType.LPArray)] byte[] pszName,
                                                               int iInstance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_get_media_instance_time(IntPtr pInstance,
                                                         [MarshalAs(UnmanagedType.LPArray)] byte[] pszName,
                                                         int iInstance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_get_media_instance_length(IntPtr pInstance,
                                                           [MarshalAs(UnmanagedType.LPArray)] byte[] pszName,
                                                           int iInstance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_get_media_instance_rate(IntPtr pInstance,
                                                         [MarshalAs(UnmanagedType.LPArray)] byte[] pszName,
                                                         int iInstance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_get_media_instance_title(IntPtr pInstance,
                                                          [MarshalAs(UnmanagedType.LPArray)] byte[] pszName, int iInstance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_get_media_instance_chapter(IntPtr pInstance,
                                                            [MarshalAs(UnmanagedType.LPArray)] byte[] pszName, int iInstance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_vlm_get_media_instance_seekable(IntPtr pInstance,
                                                             [MarshalAs(UnmanagedType.LPArray)] byte[] pszName, int iInstance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_vlm_get_event_manager(IntPtr pInstance);

        /* Equalizer */
        [MinimalLibVlcVersion("2.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_audio_equalizer_get_preset_count();

        [MinimalLibVlcVersion("2.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_audio_equalizer_get_preset_name(int uIndex);

        [MinimalLibVlcVersion("2.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_audio_equalizer_get_band_count();

        [MinimalLibVlcVersion("2.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern float libvlc_audio_equalizer_get_band_frequency(int uIndex);

        [MinimalLibVlcVersion("2.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_audio_equalizer_new();

        [MinimalLibVlcVersion("2.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_audio_equalizer_new_from_preset(int uIndex);

        [MinimalLibVlcVersion("2.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_audio_equalizer_release(IntPtr pEqualizer);

        [MinimalLibVlcVersion("2.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_audio_equalizer_set_preamp(IntPtr pEqualizer, float fPreamp);

        [MinimalLibVlcVersion("2.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern float libvlc_audio_equalizer_get_preamp(IntPtr pEqualizer);

        [MinimalLibVlcVersion("2.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_audio_equalizer_set_amp_at_index(IntPtr pEqualizer, float fAmp, int uBand);

        [MinimalLibVlcVersion("2.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern float libvlc_audio_equalizer_get_amp_at_index(IntPtr pEqualizer, int uBand);

        [MinimalLibVlcVersion("2.2.0")]
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_player_set_equalizer(IntPtr pMi, IntPtr pEqualizer);
    }
}
