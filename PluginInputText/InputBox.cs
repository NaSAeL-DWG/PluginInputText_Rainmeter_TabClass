﻿/* Copyright (C) 2013 Rainmeter Project Developers
 *
 * This Source Code Form is subject to the terms of the GNU General Public
 * License; either version 2 of the License, or (at your option) any later
 * version. If a copy of the GPL was not distributed with this file, You can
 * obtain one at <https://www.gnu.org/licenses/gpl-2.0.html>. */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace InputText
{
    public partial class InputBox : Form
    {
        private SkinWindow parent = null;

        private bool _FocusDismiss = true;
        private bool _Numeric = false;
        private string _TextValue = string.Empty;

        public InputBox(SkinWindow parent)
        {
            this.parent = parent;

            InitializeComponent();
        }

        // This event will make sure we have focus when the inputbox is shown ot the user.
        // it will only cause focus if the window itself has focus (i.e. doesn't steal).
        private void InputBox_Shown(object sender, EventArgs e)
        {
            this.Activate();
            this.BringToFront();
            this.Activate();
            this.BringToFront();
            this.txtInput.Focus();
        }

        private void InputBox_Load(object sender, EventArgs e)
        {
            this.Activate();
            this.BringToFront();
            this.Activate();
            this.BringToFront();
        }


        private void txtInput_KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (_Numeric)
            {
                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) 
                    && (e.KeyChar != '.') && (e.KeyChar != '-'))
                {
                    e.Handled = true;
                }

                // only allow one decimal point
                if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }

                // only allow minus sign at the beginning
                if (e.KeyChar == '-' && (sender as TextBox).SelectionStart != 0)
                {
                    e.Handled = true;
                }
            }
        }

        // All exceptions are swallowed for the sake of this example.  Since the majority
        // (if not all) of these are rather simple in nature, debugging without errors 
        // should be fairly simple anyway.
        //
        // However, if you wanted to log errors, a simple way would be to add something like
        // this:
        //
        //      try
        //      {
        //           ... // code you are testing
        //      }
        //      catch (Exception ex)
        //      {
        //           API.Log(API.LogType.Warning, "InputText :: exception :: " + ex.GetType().ToString() + ": " + ex.Message);
        //      }

        #region TextValue -- returns the inputted text
        public string TextValue
        {
            get { return this._TextValue; }
        }
        #endregion
        #region ChangeFontFace(string) -- changes the text's font name
        public void ChangeFontFace(string sFont)
        {
            try
            {
                this.txtInput.Font = new Font(sFont, this.txtInput.Font.Size);
            }
            catch { }
        }
        #endregion
        #region ChangeFontColor(string) -- changes the font's color, supports 6 and 8-digit hex or a series of 3 or 4 comma-separated numbers
        public void ChangeFontColor(string sColor)
        {
            try
            {
                sColor = sColor.Trim();

                if (sColor.Contains(",")) // #,#,# or #,#,#,# format
                {
                    string[] sParts = sColor.Trim().Replace(" ", string.Empty).Split(new char[] { ',' }, StringSplitOptions.None);
                    this.txtInput.ForeColor = Color.FromArgb(int.Parse(sParts[0]), int.Parse(sParts[1]), int.Parse(sParts[2]));
                    this.txtInput.ForeColor = Color.FromArgb(int.Parse(sParts[3]), int.Parse(sParts[0]), int.Parse(sParts[1]), int.Parse(sParts[2]));
                }
                else
                {
                    if (sColor.Length == 6) // 6-digit hex (no alpha transparency)
                    {
                        uint iColor = uint.Parse(sColor, System.Globalization.NumberStyles.HexNumber);
                        // Since it was omitted, force full opacity (not transparent at all)
                        this.txtInput.ForeColor = Color.FromArgb((int)(0xFF000000 | iColor));
                    }
                    else if (sColor.Length == 8) // 8-digit hex (with alpha transparency)
                    {
                        uint iColor = uint.Parse(sColor, System.Globalization.NumberStyles.HexNumber);
                        // This swaps RRGGBBAA for AARRGGBB, which Color.FromArgb() wants
                        iColor = ((iColor & 0xFFFFFF00) >> 8) | ((iColor & 0x000000FF) << 24);
                        this.txtInput.ForeColor = Color.FromArgb((int)iColor);
                    }
                }
            }
            catch { }
        }
        #endregion
        #region ChangeBackColor(string) -- changes the background color, supports 6 and 8-digit hex or a series of 3 or 4 comma-separated numbers
        public void ChangeBackColor(string sColor)
        {
            try
            {
                sColor = sColor.Trim();

                if (sColor.Contains(",")) // #,#,# or #,#,#,# format
                {
                    string[] sParts = sColor.Trim().Replace(" ", string.Empty).Split(new char[] { ',' }, StringSplitOptions.None);
                    this.txtInput.BackColor = Color.FromArgb(int.Parse(sParts[0]), int.Parse(sParts[1]), int.Parse(sParts[2]));

                    // Change 0-255 transparency to a 0.0-1.0 transparency range
                    double dTrans = (double)int.Parse(sParts[3]);
                    dTrans /= 255.0;
                    this.Opacity = dTrans;
                }
                else
                {
                    if (sColor.Length == 6) // 6-digit hex (no alpha transparency)
                    {
                        uint iColor = uint.Parse(sColor, System.Globalization.NumberStyles.HexNumber);
                        this.txtInput.BackColor = Color.FromArgb((int)(0xFF000000 | iColor));
                        // Since it was omitted, force full opacity (not transparent at all)
                        this.Opacity = 1.0;
                    }
                    else if (sColor.Length == 8) // 8-digit hex (with alpha transparency)
                    {
                        uint iColor = uint.Parse(sColor, System.Globalization.NumberStyles.HexNumber);
                        // This swaps RRGGBBAA for AARRGGBB, which Color.FromArgb() wants
                        iColor = ((iColor & 0xFFFFFF00) >> 8) | ((iColor & 0x000000FF) << 24);
                        this.txtInput.BackColor = Color.FromArgb((int)((0xFF000000) | (0x00FFFFFF & iColor)));

                        // Change 0-255 transparency to a 0.0-1.0 transparency range
                        double dTrans = (double)((iColor & 0xFF000000) >> 24);
                        dTrans /= 255.0;
                        this.Opacity = dTrans;
                    }
                }
            }
            catch { }
        }
        #endregion
        #region ChangeStringAlign(string) -- Changes text horizontal alignment
        public void ChangeStringAlign(string sAlign)
        {
            try
            {
                sAlign = sAlign.ToUpper().Trim();

                if (sAlign == "LEFT")
                    this.txtInput.TextAlign = HorizontalAlignment.Left;
                else if (sAlign == "CENTER")
                    this.txtInput.TextAlign = HorizontalAlignment.Center;
                else if (sAlign == "RIGHT")
                    this.txtInput.TextAlign = HorizontalAlignment.Right;
            }
            catch { }
        }
        #endregion
        #region ChangeFontSize(string) -- changes the font's point size (supports floating-point values)
        public void ChangeFontSize(string sSize, double scale)
        {
            try
            {
                this.txtInput.Font = new Font(this.txtInput.Font.Name, (float)(float.Parse(sSize) * scale));
            }
            catch { }
        }
        #endregion
        #region MakeTopmost() -- forces the form to have the 'Always On Top' property
        public void MakeTopmost()
        {
            this.TopMost = true;
        }
        #endregion
        #region MakePassword(bool) -- causes the textbox to be password style
        public void MakePassword(bool bPass)
        {
            this.txtInput.PasswordChar = bPass ? '*' : '\0';
        }
        #endregion
        #region ChangeFontStringStyle() -- changes the font's bold/italic styles
        public void ChangeFontStringStyle(string sStyle)
        {
            try
            {
                sStyle = sStyle.ToUpper().Trim();

                if (sStyle == "NORMAL")
                    this.txtInput.Font = new Font(this.txtInput.Font, FontStyle.Regular);
                else if (sStyle == "BOLD")
                    this.txtInput.Font = new Font(this.txtInput.Font, FontStyle.Bold);
                else if (sStyle == "ITALIC")
                    this.txtInput.Font = new Font(this.txtInput.Font, FontStyle.Italic);
                else if (sStyle == "BOLDITALIC")
                    this.txtInput.Font = new Font(this.txtInput.Font, FontStyle.Bold | FontStyle.Italic);
            }
            catch { }
        }
        #endregion
        #region ChangeW(string) -- changes the width of the input textbox
        public void ChangeW(string sWidth, double scale)
        {
            try
            {
                txtInput.Width = (int)(int.Parse(sWidth) * scale); // 将宽度乘以 Scale
            }
            catch
            {

            }
        }
        #endregion
        #region ChangeH(string) -- changes the height of the input textbox
        public void ChangeH(string sHeight, double scale)
        {
            try
            {
                txtInput.Height = (int)(int.Parse(sHeight) * scale); // 将高度乘以 Scale
            }
            catch
            {
                // 忽略异常
            }
        }
        #endregion
        #region ChangeX(string) -- changes the X (horizontal) position of the input textbox, relative to its parent skin
        public void ChangeX(string sX, double scale)
        {
            try
            {
                if (base.StartPosition != FormStartPosition.Manual)
                {
                    base.StartPosition = FormStartPosition.Manual;
                }
                int scaledX = (int)(int.Parse(sX) * scale); // 将 X 坐标乘以 Scale
                base.Location = new Point(parent.X + scaledX, base.Location.Y);
            }
            catch
            {
                // 忽略异常
            }
        }
        #endregion
        #region ChangeY(string) -- changes the Y (vertical) position of the input textbox, relative to its parent skin
        public void ChangeY(string sY, double scale)
        {
            try
            {
                if (base.StartPosition != FormStartPosition.Manual)
                {
                    base.StartPosition = FormStartPosition.Manual;
                }
                int scaledY = (int)(int.Parse(sY) * scale); // 将 Y 坐标乘以 Scale
                base.Location = new Point(base.Location.X, parent.Y + scaledY);
            }
            catch
            {
                // 忽略异常
            }
        }
        #endregion
        #region DefaultValue(string) -- sets the default text to appear in the input textbox
        public void DefaultValue(string val)
        {
            this.txtInput.Text = val;
        }
        #endregion
        #region MakeFocusDismiss(bool) -- dismisses the input textbox if it loses cursor/window focus
        public void MakeFocusDismiss(bool bDismissing)
        {
            this._FocusDismiss = bDismissing;
        }
		#endregion
        #region TextMaxLength(int length)
        public void TextMaxLength(string length)
        {
            try
            {
                this.txtInput.MaxLength = int.Parse(length);
            }
            catch { }
        }
        #endregion
        #region MakeNumeric(bool numeric) //ES_NUMBER
        public void MakeNumeric(bool numeric)
        {
            _Numeric = numeric;
        }
        #endregion

        #region ShowInputBox() -- shows text input textbox

        private DialogResult drBackup = DialogResult.None;

        public bool ShowInputBox()
        {
            if (this.drBackup != DialogResult.None)
                return false;

            if (this._FocusDismiss)
            {
                this.Show(this.parent);

                while (this.DialogResult == DialogResult.None &&
                    this.drBackup == DialogResult.None)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(44);
                }
            }
            else
            {
                this.ShowDialog(this.parent);
                Application.DoEvents();
            }

            if (this.drBackup != DialogResult.None)
            {
                if (this.drBackup != DialogResult.OK)
                    return false;
            }
            else if (this.DialogResult != DialogResult.None)
            {
                if (this.DialogResult != DialogResult.OK)
                    return false;
            }

            return true;
        }

        public void Abort()
        {
            this.drBackup = DialogResult.Abort;
            this.Close();
        }

        private void txtInput_Leave(object sender, EventArgs e)
        {
            if (this._FocusDismiss)
            {
                this.drBackup = DialogResult.Cancel;
                this.Close();
            }
        }

        private void InputBox_Leave(object sender, EventArgs e)
        {
            if (this._FocusDismiss)
            {
                this.drBackup = DialogResult.Cancel;
                this.Close();
            }
        }

        private void InputBox_Deactivate(object sender, EventArgs e)
        {
            if (this._FocusDismiss)
            {
                this.drBackup = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this._FocusDismiss = false;
            this._TextValue = this.txtInput.Text.Trim();
            this.drBackup = DialogResult.OK;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._FocusDismiss = false;
            this.drBackup = DialogResult.Cancel;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }
}
