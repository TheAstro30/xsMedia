/* xsMedia - sxCore
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading;
using xsCore.Internal;
using xsCore.Utils;
using xsCore.Utils.Serialization;

/* CDDB (Compact Disc Database) - Modified 2013
 * Written by: Brian Weeres & Jason James Newland
 * Copyright (c) 2004, 2013
 * 
 * Email: bweeres@protegra.com; bweeres@hotmail.com
 * Email: kangasoft@live.com.au
 * 
 * Permission to use, copy, modify, and distribute this software for any
 * purpose is hereby granted, provided that the above
 * copyright notice and this permission notice appear in all copies.
 *
 * If you modify it then please indicate so. 
 *
 * The software is provided "AS IS" and there are no warranties or implied warranties.
 * In no event shall Brian Weeres and/or Protegra Technology Group be liable for any special, 
 * direct, indirect, or consequential damages or any damages whatsoever resulting for any reason 
 * out of the use or performance of this software
 * 
 */

namespace xsCore.CdUtils
{
    internal class CddbDiscInfo
    {
        public CddbDiscInfo(string result)
        {
            Parse(result);
        }

        public string DiscId { get; private set; }
        public string Category { get; private set; }
        public string Title { get; private set; }
        public string Artist { get; private set; }

        public string Genre { get; set; }
        public string Year { get; set; }

        private void Parse(string result)
        {
            result = result.Trim();
            /* Get first white space */
            var index = result.IndexOf(' ');
            if (index <= -1) { return; }
            index++;
            var secondIndex = result.IndexOf(' ', index);
            Category = result.Substring(index, secondIndex - index);
            index = secondIndex;
            index++;
            secondIndex = result.IndexOf(' ', index);
            DiscId = result.Substring(index, secondIndex - index);
            index = secondIndex;
            index++;
            secondIndex = result.IndexOf('/', index);
            Artist = result.Substring(index, secondIndex - index - 1); /* -1 because there is a space at the end of artist */
            index = secondIndex;
            index += 2; /* skip past / and space */
            Title = result.Substring(index);
        }
    }

    public class Cddb
    {
        internal enum CddbResponseCodes
        {
            CddbError = -1,
            CddbExactMatch = 200,
            CddbNoMatch = 202,
            CddbOkMultipleExactMatches = 210,
            CddbInExactMatches = 211,
            CddbNoSiteInformation = 401,
            CddbServerError = 402,
            CddbDatabaseEntryCorrupt = 403,
            CddbNoHandshake = 409,
            CddbInvalidCommand = 500
        }

        private readonly Proxy _proxy;
        private string _computerName;
        private string _version;
        private int _playlistStartPosition;

        private CddbCache _cache;

        public event Action<int> CddbQueryComplete; /* Lookup complete or error */

        public Cddb(Proxy proxy)
        {
            _proxy = proxy;
        }

        public string CddbHost { get; set; }
        public string CddbApplicationName { get; set; }
        public string CddbUserName { get; set; }
        public string CddbCacheFile { get; set; }

        public void CddbQueryLookup(CdDrive drive, IList<CdTrackInfo> tracks, int playlistStartPosition)
        {
            _playlistStartPosition = playlistStartPosition;
            var t = new Thread(() => BackgroundCddbQueryLookup(drive, tracks))
                        {
                            IsBackground = true
                        };
            t.Start();
        }

        private void BackgroundCddbQueryLookup(CdDrive drive, IList<CdTrackInfo> tracks)
        {
            Debug.Print("CDDB lookup started");
            /* Get DiscID */
            var discId = drive.GetDiscId();
            if (string.IsNullOrEmpty(discId))
            {
                if (CddbQueryComplete != null)
                {
                    CddbQueryComplete(_playlistStartPosition);
                }
                return;
            }
            /* Check the CDDB entry isn't already in the cache */
            if (CheckCache(discId, tracks))
            {
                /* Raise complete event as list has been filled out from cache */
                if (CddbQueryComplete != null)
                {
                    CddbQueryComplete(_playlistStartPosition);
                }
                return;
            }
            Debug.Print("CDDB: Disc ID = " + discId);
            /* Get the fully-qualifed domain name of our computer */
            uint dwSize = 0;
            Win32.GetComputerNameEx(Win32.ComputerNameFormat.ComputerNameDnsFullyQualified, null, ref dwSize);
            var sb = new StringBuilder((int)dwSize);
            Win32.GetComputerNameEx(Win32.ComputerNameFormat.ComputerNameDnsFullyQualified, sb, ref dwSize);
            _computerName = sb.ToString();
            _version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            /* Build query */
            var query = new StringBuilder();
            query.AppendFormat("cddb+query+{0}", discId);
            var result = Query(query.ToString(), CddbHost);
            if (result.Count <= 0) { return; }
            var code = GetCode(result[0]);
            switch (code)
            {
                case CddbResponseCodes.CddbError:
                case CddbResponseCodes.CddbNoMatch:
                case CddbResponseCodes.CddbInExactMatches:
                case CddbResponseCodes.CddbInvalidCommand:
                case CddbResponseCodes.CddbNoHandshake:
                case CddbResponseCodes.CddbServerError:
                case CddbResponseCodes.CddbDatabaseEntryCorrupt:
                    if (CddbQueryComplete != null)
                    {
                        CddbQueryComplete(_playlistStartPosition);
                    }
                    return;

                case CddbResponseCodes.CddbExactMatch:
                    /* Exact match */
                    var info = new CddbDiscInfo(result[0]);
                    /* Resolve track names */
                    ResolveTrackNames(info, tracks);
                    /* Build and save cache */
                    if (_cache != null)
                    {
                        var data = new CddbCacheData
                                       {
                                           DiscId = discId,
                                           Tracks = new List<CdTrackInfo>(tracks)
                                       };
                        _cache.Entry.Add(data);
                        XmlSerialize<CddbCache>.Save(CddbCacheFile, _cache);
                    }
                    /* CDDB complete */
                    if (CddbQueryComplete != null)
                    {
                        CddbQueryComplete(_playlistStartPosition);
                    }
                    return;
            }
        }

        private bool CheckCache(string discId, IList<CdTrackInfo> tracks)
        {
            if (string.IsNullOrEmpty(CddbCacheFile))
            {
                return false;
            }
            _cache = new CddbCache();
            if (XmlSerialize<CddbCache>.Load(CddbCacheFile, ref _cache))
            {
                var list = _cache.GetEntry(discId);
                if (list != null)
                {
                    /* You can't use out or ref params in anonymous method bodies; eg. Threading - this is by design so this is why
                     * we iterate the second list and manually manipulate the first instead of tracks = new List<CdTrackInfo>(list); */
                    var count = 0;
                    foreach (var title in list)
                    {
                        tracks[count].Name = title.Name;
                        tracks[count].Artist = title.Artist;
                        tracks[count].Album = title.Album;
                        tracks[count].Genre = title.Genre;
                        tracks[count].Year = title.Year;
                        count++;
                    }
                    Debug.Print("Pulled from cache");
                    return true;
                }
                return false;
            }
            if (_cache == null)
            {
                _cache = new CddbCache(); /* Make sure it isn't null as above Xml method will return a null object if it fails */
            }
            return false;
        }

        private void ResolveTrackNames(CddbDiscInfo discInfo, IList<CdTrackInfo> tracks)
        {
            var query = new StringBuilder();
            query.AppendFormat("cddb+read+{0}+{1}", discInfo.Category, discInfo.DiscId);
            /* Send query */
            var result = Query(query.ToString(), CddbHost);
            /* check if results came back */
            if (result.Count <= 0) { return; }
            var code = GetCode(result[0]);
            switch (code)
            {
                case CddbResponseCodes.CddbNoMatch:
                case CddbResponseCodes.CddbInExactMatches:
                case CddbResponseCodes.CddbInvalidCommand:
                case CddbResponseCodes.CddbNoHandshake:
                case CddbResponseCodes.CddbServerError:
                case CddbResponseCodes.CddbDatabaseEntryCorrupt:
                    return;

                case CddbResponseCodes.CddbExactMatch:
                case CddbResponseCodes.CddbOkMultipleExactMatches: /* Good */
                    result.RemoveAt(0); /* Remove the 210 */
                    var track = 0;
                    foreach (var s in result)
                    {
                        Debug.Print("CDDB: result = " + s);
                        /* Parse title information */
                        string[] sp;
                        if (s.StartsWith("DYEAR", StringComparison.CurrentCultureIgnoreCase))
                        {
                            sp = s.Split('=');
                            if (sp.Length > 1) { discInfo.Year = sp[1].Trim(); }
                        }
                        if (s.StartsWith("DGENRE", StringComparison.CurrentCultureIgnoreCase))
                        {
                            sp = s.Split('=');
                            if (sp.Length > 1) { discInfo.Genre = sp[1].Trim(); }
                        }
                        if (!s.StartsWith("TTITLE", StringComparison.InvariantCultureIgnoreCase)) { continue; }
                        sp = s.Split('=');
                        if (sp.Length > 1)
                        {
                            var titleArtist = sp[1].Split('/');
                            if (titleArtist.Length > 1)
                            {
                                tracks[track].Artist = titleArtist[0].Trim();
                                tracks[track].Name = titleArtist[1].Trim();
                            }
                            else
                            {
                                tracks[track].Artist = discInfo.Artist;
                                tracks[track].Name = sp[1].Trim();
                            }
                            tracks[track].Album = discInfo.Title;
                            tracks[track].Genre = discInfo.Genre;
                            tracks[track].Year = discInfo.Year;
                        }
                        track++;
                    }
                    return;
            }
        }

        private List<string> Query(string commandIn, string url)
        {
            StreamReader reader = null;
            HttpWebResponse response = null;
            var retResponse = new List<string>();
            bool success;
            try
            {
                /* Create our HttpWebRequest which we use to call the freedb server */
                var proxyProvider = new ProxyProvider(_proxy);
                var req = proxyProvider.BeginRequest(url);
                req.ContentType = "text/plain";
                /* We are using th POST method of calling the http server. We could have also used the GET method */
                req.Method = "POST";
                /* Add the hello and proto commands to the request */
                var command = BuildCommand(string.Format("cmd={0}", commandIn));
                /* Using Unicode */
                var byteArray = Encoding.UTF8.GetBytes(command);
                /* Get our request stream */
                using (var newStream = req.GetRequestStream())
                {
                    newStream.Write(byteArray, 0, byteArray.Length);
                    newStream.Close();
                }
                /* Send the query. Note this is a synchronous call (which is why it's done in a background thread) */
                response = (HttpWebResponse)req.GetResponse();
                /* Put the results into a StreamReader */
                using (var resStream = response.GetResponseStream())
                {
                    if (resStream != null)
                    {
                        reader = new StreamReader(resStream, Encoding.UTF8);
                        /* Add each line to the retReponse until we get the terminator */
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.StartsWith("."))
                            {
                                break;
                            }
                            Debug.Print("CDDB: data = " + line);
                            retResponse.Add(line);
                        }
                    }
                }
                success = true;
            }
            catch
            {
                success = false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            return success ? retResponse : new List<string>();
        }

        private string BuildCommand(string command)
        {
            var query = new StringBuilder();
            query.AppendFormat("{0}&{1}&proto=6", command, Hello());
            return query.ToString();
        }

        public string Hello()
        {
            var query = new StringBuilder();
            query.AppendFormat("hello={0}+{1}+{2}+{3}", CddbUserName, _computerName, CddbApplicationName, _version);
            return query.ToString();
        }

        private static CddbResponseCodes GetCode(string line)
        {
            var code = line.Trim();
            /* Find first white space after start */
            var index = code.IndexOf(' ');
            if (index != -1)
            {
                int outCode;
                return int.TryParse(code.Substring(0, index), out outCode)
                           ? (CddbResponseCodes)outCode
                           : CddbResponseCodes.CddbError;
            }
            return CddbResponseCodes.CddbError;
        }
    }
}
