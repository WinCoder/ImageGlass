﻿/*
ImageGlass Project - Image viewer for Windows
Copyright (C) 2010 - 2023 DUONG DIEU PHAP
Project homepage: https://imageglass.org

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using ImageGlass.Base;
using ImageGlass.Base.WinApi;
using System.ComponentModel;

namespace ImageGlass.UI;


/// <summary>
/// Dialog form with dark mode and backdrop support.
/// </summary>
public partial class DialogForm : ModernForm
{
    private bool _showAcceptButton = true;
    private bool _showCancelButton = true;
    private bool _showApplyButton = false;


    // Public properties
    #region Public properties

    /// <summary>
    /// Gets, sets <see cref="BtnAccept"/>'s text.
    /// </summary>
    public string AcceptButtonText
    {
        get => BtnAccept.Text;
        set => BtnAccept.Text = value;
    }


    /// <summary>
    /// Shows or hides the Shield icon for the <see cref="BtnAccept"/>.
    /// </summary>
    [DefaultValue(false)]
    public bool ShowAcceptButtonShieldIcon
    {
        get => BtnAccept.SystemIcon == SHSTOCKICONID.SIID_SHIELD;
        set => BtnAccept.SystemIcon = value ? SHSTOCKICONID.SIID_SHIELD : null;
    }


    /// <summary>
    /// Gets, sets visibility value of the <see cref="BtnAccept"/>.
    /// </summary>
    [DefaultValue(true)]
    public bool ShowAcceptButton
    {
        get => _showAcceptButton;
        set
        {
            _showAcceptButton = value;
            BtnAccept.Visible = value;
        }
    }


    /// <summary>
    /// Gets, sets <see cref="BtnCancel"/>'s text.
    /// </summary>
    public string CancelButtonText
    {
        get => BtnCancel.Text;
        set => BtnCancel.Text = value;
    }


    /// <summary>
    /// Gets, sets visibility value of the <see cref="BtnCancel"/>.
    /// </summary>
    [DefaultValue(true)]
    public bool ShowCancelButton
    {
        get => _showCancelButton;
        set
        {
            _showCancelButton = value;
            BtnCancel.Visible = value;
        }
    }


    /// <summary>
    /// Gets, sets <see cref="BtnApply"/>'s text.
    /// </summary>
    public string ApplyButtonText
    {
        get => BtnApply.Text;
        set => BtnApply.Text = value;
    }


    /// <summary>
    /// Gets, sets visibility value of the <see cref="BtnApply"/>.
    /// </summary>
    [DefaultValue(false)]
    public bool ShowApplyButton
    {
        get => _showApplyButton;
        set
        {
            _showApplyButton = value;
            BtnApply.Visible = value;
        }
    }

    public override Keys CloseFormHotkey => Keys.Escape;

    #endregion // Public properties


    /// <summary>
    /// Initializes the new instance of <see cref="DialogForm"/>. This appends
    /// an action bar with <see cref="BtnAccept"/> and <see cref="BtnCancel"/>
    /// to the bottom of the form.
    /// </summary>
    public DialogForm() : base()
    {
        InitializeComponent();

        AppendActionBar();
    }


    // Action bar codes
    #region Action bar codes

    public TableLayoutPanel TableActions = new TableLayoutPanel();
    public ModernButton BtnAccept = new ModernButton();
    public ModernButton BtnCancel = new ModernButton();
    public ModernButton BtnApply = new ModernButton();


    /// <summary>
    /// Adds action bar with OK and Cancel buttons to the form.
    /// </summary>
    private void AppendActionBar()
    {
        this.TableActions.SuspendLayout();
        this.SuspendLayout();
        // 
        // TableActions
        // 
        this.TableActions.AutoSize = true;
        this.TableActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        this.TableActions.ColumnCount = 4;
        this.TableActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        this.TableActions.ColumnStyles.Add(new ColumnStyle());
        this.TableActions.ColumnStyles.Add(new ColumnStyle());
        this.TableActions.Controls.Add(this.BtnAccept, 1, 0);
        this.TableActions.Controls.Add(this.BtnCancel, 2, 0);
        this.TableActions.Controls.Add(this.BtnApply, 3, 0);
        this.TableActions.Dock = DockStyle.Bottom;
        this.TableActions.Location = new Point(0, 367);
        this.TableActions.Margin = new Padding(0);
        this.TableActions.Padding = this.ScaleToDpi(new Padding(16));
        this.TableActions.Name = "TableActions";
        this.TableActions.RowCount = 1;
        this.TableActions.RowStyles.Add(new RowStyle());
        this.TableActions.Size = new Size(800, 83);
        this.TableActions.TabIndex = 2;
        // 
        // BtnAccept
        // 
        this.BtnAccept.AutoSize = true;
        this.BtnAccept.DarkMode = false;
        this.BtnAccept.ImagePadding = 2;
        this.BtnAccept.Location = new Point(500, 20);
        this.BtnAccept.Margin = this.ScaleToDpi(new Padding(8, 0, 0, 0));
        this.BtnAccept.Name = "BtnAccept";
        this.BtnAccept.Text = "[OK]";
        this.BtnAccept.Size = this.ScaleToDpi(new Size(90, 30));
        this.BtnAccept.SystemIcon = null;
        this.BtnAccept.TabIndex = 1;
        this.BtnAccept.Text = AcceptButtonText;
        this.BtnAccept.TextImageRelation = TextImageRelation.ImageBeforeText;
        this.BtnAccept.Visible = _showAcceptButton;
        this.BtnAccept.Click += BtnAccept_Click;
        // 
        // BtnCancel
        // 
        this.BtnCancel.AutoSize = true;
        this.BtnCancel.DarkMode = false;
        this.BtnCancel.ImagePadding = 2;
        this.BtnCancel.Location = new Point(650, 20);
        this.BtnCancel.Margin = this.ScaleToDpi(new Padding(8, 0, 0, 0));
        this.BtnCancel.Name = "BtnCancel";
        this.BtnCancel.Text = "[Cancel]";
        this.BtnCancel.Size = this.ScaleToDpi(new Size(90, 30));
        this.BtnCancel.SystemIcon = null;
        this.BtnCancel.TabIndex = 2;
        this.BtnCancel.Text = CancelButtonText;
        this.BtnCancel.TextImageRelation = TextImageRelation.ImageBeforeText;
        this.BtnCancel.Visible = _showCancelButton;
        this.BtnCancel.Click += BtnCancel_Click;
        // 
        // BtnApply
        // 
        this.BtnApply.AutoSize = true;
        this.BtnApply.DarkMode = false;
        this.BtnApply.ImagePadding = 2;
        this.BtnApply.Location = new Point(800, 20);
        this.BtnApply.Margin = this.ScaleToDpi(new Padding(8, 0, 0, 0));
        this.BtnApply.Name = "BtnApply";
        this.BtnApply.Text = "[Apply]";
        this.BtnApply.Size = this.ScaleToDpi(new Size(90, 30));
        this.BtnApply.SystemIcon = null;
        this.BtnApply.TabIndex = 3;
        this.BtnApply.Text = ApplyButtonText;
        this.BtnApply.TextImageRelation = TextImageRelation.ImageBeforeText;
        this.BtnApply.Visible = _showApplyButton;
        this.BtnApply.Click += BtnApply_Click;
        

        this.TableActions.ResumeLayout(false);
        this.TableActions.PerformLayout();

        this.Controls.Add(this.TableActions);
        this.AcceptButton = BtnAccept;
        this.CancelButton = BtnCancel;
        this.ResumeLayout(false);
        this.PerformLayout();
    }


    private void BtnAccept_Click(object? sender, EventArgs e)
    {
        OnAcceptButtonClicked();
    }


    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        OnCancelButtonClicked();
    }


    private void BtnApply_Click(object? sender, EventArgs e)
    {
        OnApplyButtonClicked();
    }

    #endregion // Action bar codes


    // Override / Virtual methods
    #region Override / Virtual methods

    protected override void ApplyTheme(bool darkMode, BackdropStyle? style = null)
    {
        SuspendLayout();
        EnableTransparent = darkMode;


        TableActions.BackColor = darkMode
            ? Color.White.WithAlpha(30)
            : Color.Black.WithAlpha(10);

        if (!darkMode)
        {
            BackColor = Color.White;
        }


        base.ApplyTheme(darkMode, style);
        ResumeLayout();
    }


    protected override void OnLoad(EventArgs e)
    {
        // update size and padding
        this.BtnAccept.Size =
            this.BtnCancel.Size =
            this.BtnApply.Size = this.ScaleToDpi(new Size(90, 30));

        this.TableActions.Padding = this.ScaleToDpi(new Padding(16));
        this.BtnAccept.Margin =
            this.BtnCancel.Margin =
            this.BtnApply.Margin = this.ScaleToDpi(new Padding(8, 0, 0, 0));

        var buttonPadding = this.ScaleToDpi(new Padding(
            ModernButton.DefaultPadding.Left * 2,
            ModernButton.DefaultPadding.Top,
            ModernButton.DefaultPadding.Right * 2,
            ModernButton.DefaultPadding.Bottom));
        this.BtnAccept.Padding = buttonPadding;
        this.BtnCancel.Padding = buttonPadding;
        this.BtnApply.Padding = buttonPadding;


        // adjust form size
        _ = OnUpdateHeight();

        base.OnLoad(e);

        this.TableActions.BringToFront();

        // enable free form moving
        EnableFormFreeMoving(this);
    }


    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);

        DisableFormFreeMoving(this);
    }


    protected override void CloseFormByKeys()
    {
        // Closes the form and returns <see cref="DialogResult.Abort"/> code.
        DialogResult = DialogResult.Abort;

        base.CloseFormByKeys();
    }


    /// <summary>
    /// Recalculates and updates window height on <see cref="Form.OnLoad(EventArgs)"/> event.
    /// </summary>
    protected virtual int OnUpdateHeight(bool performUpdate = true)
    {
        // calculate form height
        var formNonClientHeight = Padding.Vertical;
        var contentHeight = TableActions.Height + TableActions.Padding.Vertical;
        var formHeight = formNonClientHeight + contentHeight;

        if (performUpdate)
        {
            Height = formHeight;
        }

        return formHeight;
    }


    /// <summary>
    /// Closes the form and returns <see cref="DialogResult.OK"/> code.
    /// </summary>
    protected virtual void OnAcceptButtonClicked()
    {
        DialogResult = DialogResult.OK;
        Close();
    }


    /// <summary>
    /// Closes the form and returns <see cref="DialogResult.Cancel"/> code.
    /// </summary>
    protected virtual void OnCancelButtonClicked()
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }


    /// <summary>
    /// Occurs when <see cref="BtnApply"/> is clicked.
    /// </summary>
    protected virtual void OnApplyButtonClicked()
    {
        //
    }

    #endregion // Override / Virtual methods


}
