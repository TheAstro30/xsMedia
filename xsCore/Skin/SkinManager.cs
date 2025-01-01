/* xsMedia - sxCore
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using xsCore.PlayerControls;
using xsCore.Utils.Serialization;

namespace xsCore.Skin
{
    /*  Skin manager XML class
     *  By Ryan Alexander
     *  (C)Copyright 2011
     *  KangaSoft Software - All Rights Reserved
     */
    public class ResourceData
    {
        public bool Exists { get; set; }

        public string Id { get; set; }
        public string State { get; set; }

        public Bitmap Image { get; set; }
        public Size ImageSize { get; set; }
        public Rectangle Area { get; set; }
        public Size AreaOffset { get; set; }

        public Color RailColor { get; set; }
        public Rectangle RailArea { get; set; }
    }

    public class AvailableSkinsInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public class SkinManager
    {
        public static List<AvailableSkinsInfo> AvailableSkins = new List<AvailableSkinsInfo>();
        public static InternalSkinFormat SkinData = new InternalSkinFormat();

        private static string _xmlFileDirectory;

        /* Resource/image getting method - used for getting image and control data */
        public static ResourceData GetResourceById(string id, string state)
        {
            var rd = new ResourceData();
            try
            {
                var iResource = SkinData.Body.Resources.ResourceList.FirstOrDefault(r => (r.Id.ToLower() == id.ToLower()));
                if (iResource != null)
                {
                    rd.Exists = true;
                    rd.Id = iResource.Id;
                    rd.ImageSize = iResource.ImageSize;
                    rd.Area = iResource.Area;
                    rd.AreaOffset = iResource.AreaOffset;
                    rd.RailArea = iResource.Rail.Area;
                    rd.RailColor = iResource.Rail.Color;
                    /* Get current resource state (down/up/hover) */
                    var st = (!string.IsNullOrEmpty(state) ? state.ToLower() : null);
                    var istate = iResource.States.StateList.FirstOrDefault(r => r.Id.ToLower() == st);
                    if (istate != null)
                    {
                        rd.State = istate.Id;
                        CalculatePhysicalImage(istate, iResource.Id + ":" + istate.Id);
                        rd.Image = istate.PhysicalImage;
                    }
                    else
                    {
                        rd.State = null;
                        CalculatePhysicalImage(iResource, iResource.Id);
                        rd.Image = iResource.PhysicalImage;
                    }
                }
                return rd;
            }
            catch
            {
                return new ResourceData();
            }
        }

        /* Menu renderer color table */
        public static Color GetMenuRendererColor(string id)
        {
            try
            {
                var menu = SkinData.Body.Renderer.RendererData.FirstOrDefault(r => r.Id.ToLower() == id.ToLower());
                return menu != null ? menu.Color : Color.Empty;
            }
            catch
            {
                return Color.Empty;
            }
        }

        public static Color GetPlaylistColor(string id)
        {
            try
            {
                var menu = SkinData.Body.Playlist.RendererData.FirstOrDefault(r => r.Id.ToLower() == id.ToLower());
                return menu != null ? menu.Color : Color.Empty;
            }
            catch
            {
                return Color.Empty;
            }
        }

        /* Read/write skin methods */
        public static void WriteSkin(string filename)
        {
            XmlSerialize<InternalSkinFormat>.Save(filename, SkinData);
        }

        public static bool ReadSkin(string filename)
        {
            if (!File.Exists(filename)) { return false; }
            var f = new FileInfo(filename);
            _xmlFileDirectory = f.DirectoryName;
            return XmlSerialize<InternalSkinFormat>.Load(filename, ref SkinData);
        }

        /* Apply skin to control object */
        public static void ApplySkinLayout(ControlRenderer cr)
        {
            foreach (var pc in cr.PlayerControls)
            {
                var pc1 = pc;
                var tag = (!String.IsNullOrEmpty(pc1.Tag) ? pc1.Tag.ToLower() : null);
                try
                {
                    var iresource = SkinData.Body.Resources.ResourceList.FirstOrDefault(r => (r.Id.ToLower() == tag));
                    pc.SkinStyleChanged();
                    if (iresource != null) { pc.Area = iresource.Area; }
                }
                catch (Exception)
                {
                    break;
                }                
            }
            /* Raise skin style changed event on all control renderer objects */
            cr.SkinStyleChanged();
        }

        /* Private image calculation method */
        private static void CalculatePhysicalImage(ImageData id, string idTag)
        {
            if (id.PhysicalImage != null || id.ImageId == null) { return; }
            var img = SkinData.Images.Image.FirstOrDefault(i => (i.Id.ToLower() == id.ImageId.ToLower()));
            if (img == null)
            {
                throw new Exception("A resource by the name of " + idTag + " requested an image by the name of " +
                                    id.ImageId + " which does not exist");
            }
            if (img.PhysicalImage == null) { img.PhysicalImage = (Bitmap)Image.FromFile(_xmlFileDirectory + @"\" + img.Path); }
            if (id.ImageArea == Rectangle.Empty) { id.PhysicalImage = img.PhysicalImage; }
            else
            {
                id.PhysicalImage = new Bitmap(id.ImageArea.Width, id.ImageArea.Height);
                var g = Graphics.FromImage(id.PhysicalImage);
                g.DrawImage(img.PhysicalImage, new Rectangle(Point.Empty, id.ImageArea.Size), id.ImageArea, GraphicsUnit.Pixel);
            }
        }
    }
}