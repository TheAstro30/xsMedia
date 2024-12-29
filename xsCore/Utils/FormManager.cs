/* xsMedia - sxCore
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace xsCore.Utils
{
    public static class FormManager
    {
        private static readonly List<Form> FormCol = new List<Form>();

        public static Form Open(Form form, IWin32Window owner)
        {
            var f = GetForm(form.Name);
            if (f != null)
            {
                f.BringToFront();
                f.Focus();
                return f;
            }
            form.FormClosed += OnFormClosed;
            FormCol.Add(form);
            form.Show(owner);
            return form;
        }

        public static Form GetForm(string formName)
        {
            return FormCol.FirstOrDefault(f => f.Name == formName);
        }

        /* Callback */
        private static void OnFormClosed(object sender, EventArgs e)
        {
            var f = (Form)sender;
            f.FormClosed -= OnFormClosed;
            foreach (var form in FormCol.Where(form => form.Name == f.Name))
            {
                FormCol.Remove(form);
                return;
            }            
        }
    }
}
