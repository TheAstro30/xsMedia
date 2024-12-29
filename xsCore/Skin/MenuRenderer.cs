/* xsMedia - sxCore
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using xsCore.Utils.UI;

namespace xsCore.Skin
{
    /* Menustrip renderer class
       By: Nick Thissen ©2008 (Modified by Jason James Newland)
       ©2011-2012 - KangaSoft Software, All Rights Reserved */
    public class MenuRenderer : ToolStripRenderer
    {        
        protected override void InitializeItem(ToolStripItem item)
        {
            base.InitializeItem(item);
            item.ForeColor = SkinManager.GetMenuRendererColor("MENU_ITEM_FORECOLOR");
        }

        protected override void Initialize(ToolStrip toolStrip)
        {            
            base.Initialize(toolStrip);
            toolStrip.ForeColor = Color.Black;
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            /* Render horizontal background gradient */
            base.OnRenderToolStripBackground(e);            
            using (var b = new LinearGradientBrush(e.AffectedBounds, SkinManager.GetMenuRendererColor("BG_GRADIENT_TOP"),SkinManager.GetMenuRendererColor("BG_GRADIENT_BOTTOM"), LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(b, e.AffectedBounds);
            }            
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            /* Render Image Margin and gray itembackground */
            base.OnRenderImageMargin(e);
            /* Draw ImageMargin background gradient */            
            using (var b = new LinearGradientBrush(e.AffectedBounds, SkinManager.GetMenuRendererColor("IMAGE_MARGIN_GRADIENT_LEFT"), SkinManager.GetMenuRendererColor("IMAGE_MARGIN_GRADIENT_RIGHT"), LinearGradientMode.Horizontal))
            {
                using (var darkLine = new SolidBrush(SkinManager.GetMenuRendererColor("IMAGE_MARGIN_LINE_DARK")))
                {
                    using (var whiteLine = new SolidBrush(SkinManager.GetMenuRendererColor("IMAGE_MARGIN_LINE_LIGHT")))
                    {
                        var rect = new Rectangle(e.AffectedBounds.Width, 1, 1, e.AffectedBounds.Height);
                        var rect2 = new Rectangle(e.AffectedBounds.Width + 1, 2, 1, e.AffectedBounds.Height);
                        /* Gray background */
                        using (var submenuBGbrush = new LinearGradientBrush(e.AffectedBounds, SkinManager.GetMenuRendererColor("MENU_BG_GRADIENT_TOP"), SkinManager.GetMenuRendererColor("MENU_BG_GRADIENT_BOTTOM"), LinearGradientMode.Vertical))
                        {
                            var rect3 = new Rectangle(0, 0, e.ToolStrip.Width, e.ToolStrip.Height);
                            /* Border */
                            using (var borderPen = new Pen(SkinManager.GetMenuRendererColor("BORDER")))
                            {
                                var rect4 = new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
                                e.Graphics.FillRectangle(submenuBGbrush, rect3);
                                e.Graphics.FillRectangle(b, e.AffectedBounds);
                                e.Graphics.FillRectangle(darkLine, rect);
                                e.Graphics.FillRectangle(whiteLine, rect2);
                                e.Graphics.DrawRectangle(borderPen, rect4);
                            }
                        }
                    }
                }
            }
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            /* Render Checkmark */
            base.OnRenderItemCheck(e);                        
            if (e.Item.Selected)
            {
                var rect = new Rectangle(3, 1, 20, 20);
                var rect2 = new Rectangle(4, 2, 18, 18);
                using (var b = new SolidBrush(SkinManager.GetMenuRendererColor("BUTTON_BORDER")))
                {
                    using (var b2 = new SolidBrush(SkinManager.GetMenuRendererColor("CHECKED_ITEM")))
                    {
                        e.Graphics.FillRectangle(b, rect);
                        e.Graphics.FillRectangle(b2, rect2);
                        e.Graphics.DrawImage(e.Image, new Point(5, 3));
                    }
                }
            }
            else
            {
                var rect = new Rectangle(3, 1, 20, 20);
                var rect2 = new Rectangle(4, 2, 18, 18);
                using (var b = new SolidBrush(SkinManager.GetMenuRendererColor("SELECTED_DROP_BORDER")))
                {
                    using (var b2 = new SolidBrush(SkinManager.GetMenuRendererColor("CHECKED_ITEM")))
                    {
                        e.Graphics.FillRectangle(b, rect);
                        e.Graphics.FillRectangle(b2, rect2);
                        e.Graphics.DrawImage(e.Image, new Point(5, 3));
                    }
                }
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            /* Render separator */
            base.OnRenderSeparator(e);
            using (var darkLine = new SolidBrush(SkinManager.GetMenuRendererColor("SEPARATOR_DARK")))
            {
                using (var whiteLine = new SolidBrush(SkinManager.GetMenuRendererColor("SEPARATOR_LIGHT")))
                {
                    var rect = new Rectangle(32, 3, e.Item.Width - 32, 1);
                    var rect2 = new Rectangle(32, 4, e.Item.Width - 32, 1);
                    e.Graphics.FillRectangle(darkLine, rect);
                    e.Graphics.FillRectangle(whiteLine, rect2);
                }
            }
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            /* Render arrow */             
            base.OnRenderArrow(e);
            if (!e.Item.Enabled) { return; }
            e.ArrowColor = SkinManager.GetMenuRendererColor("MENU_ITEM_FORECOLOR");
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            /* Render Menuitem background: lightblue if selected, darkblue if dropped down */
            base.OnRenderMenuItemBackground(e);
            if (!e.Item.Enabled) { return; }
            Rectangle rect;
            if (!e.Item.IsOnDropDown && e.Item.Selected)
            {
                /* If item is MenuHeader and selected: draw darkblue border */
                rect = new Rectangle(3, 2, e.Item.Width - 6, e.Item.Height - 4);
                using (var b = new LinearGradientBrush(rect, SkinManager.GetMenuRendererColor("SELECTED_GRADIENT_TOP"), SkinManager.GetMenuRendererColor("SELECTED_GRADIENT_BOTTOM"), LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(b, rect);
                    Drawing.DrawRoundedRectangle(e.Graphics, rect.Left - 1, rect.Top - 1, rect.Width, rect.Height + 1, 4, SkinManager.GetMenuRendererColor("BUTTON_BORDER"));
                }
            }
            else if (e.Item.IsOnDropDown && e.Item.Selected)
            {
                /* If item is NOT menuheader (but subitem) and selected: draw gradient border */
                rect = new Rectangle(4, 2, e.Item.Width - 6, e.Item.Height - 4);
                using (var b = new LinearGradientBrush(rect, SkinManager.GetMenuRendererColor("SELECTED_GRADIENT_TOP"), SkinManager.GetMenuRendererColor("SELECTED_GRADIENT_BOTTOM"), LinearGradientMode.Vertical))
                {                    
                    e.Graphics.FillRectangle(b, rect);
                    Drawing.DrawRoundedRectangle(e.Graphics, rect.Left - 1, rect.Top - 1, rect.Width, rect.Height + 1, 4, SkinManager.GetMenuRendererColor("SELECTED_BORDER"));
                }
            }
            /* If item is MenuHeader and menu is dropped down: selection rectangle is now darker */
            if (!((ToolStripMenuItem)e.Item).DropDown.Visible || e.Item.IsOnDropDown)
            {
                return;
            }
            rect = new Rectangle(3, 2, e.Item.Width - 6, e.Item.Height - 4);
            using (var b = new LinearGradientBrush(rect, SkinManager.GetMenuRendererColor("SELECTED_GRADIENT_TOP"), SkinManager.GetMenuRendererColor("SELECTED_BORDER"), LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(b, rect);
                Drawing.DrawRoundedRectangle(e.Graphics, rect.Left - 1, rect.Top - 1, rect.Width, rect.Height + 1, 4, SkinManager.GetMenuRendererColor("SELECTED_DROP_BORDER"));
            }            
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            /* All text coloring gets done here, not in the item background function per version 1.0.2 */
            base.OnRenderItemText(e);
            if (e.Item.Tag != null && e.Item.Tag.ToString() == "ROOT")
            {
                if (((ToolStripMenuItem)e.Item).DropDown.Visible && !e.Item.IsOnDropDown)
                {
                    e.Item.ForeColor = SkinManager.GetMenuRendererColor("SELECTED_FORECOLOR");
                }
                else
                {
                    e.Item.ForeColor = e.Item.Selected ? SkinManager.GetMenuRendererColor("SELECTED_FORECOLOR") : SkinManager.GetMenuRendererColor("MENU_HEADER_FORECOLOR");
                }
            }
            else if (e.Item.Selected)
            {
                e.Item.ForeColor = SkinManager.GetMenuRendererColor("SELECTED_FORECOLOR");
            }
            else
            {
                e.Item.ForeColor = SkinManager.GetMenuRendererColor("MENU_ITEM_FORECOLOR");
            }
        }
    }
}