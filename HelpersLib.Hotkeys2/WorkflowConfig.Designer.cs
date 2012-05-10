﻿namespace HelpersLib.Hotkeys2
{
    partial class WindowWorkflow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lvActivitiesAll = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvActivitiesUser = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(8, 8);
            this.txtDescription.MaxLength = 100;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(656, 20);
            this.txtDescription.TabIndex = 0;
            this.txtDescription.Text = "New workflow";
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(288, 128);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(96, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add ==>";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(288, 232);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(96, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "<== Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(584, 392);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(74, 22);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(504, 392);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 22);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lvActivitiesAll
            // 
            this.lvActivitiesAll.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvActivitiesAll.FullRowSelect = true;
            this.lvActivitiesAll.GridLines = true;
            this.lvActivitiesAll.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvActivitiesAll.Location = new System.Drawing.Point(8, 40);
            this.lvActivitiesAll.Name = "lvActivitiesAll";
            this.lvActivitiesAll.Size = new System.Drawing.Size(264, 336);
            this.lvActivitiesAll.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvActivitiesAll.TabIndex = 1;
            this.lvActivitiesAll.UseCompatibleStateImageBehavior = false;
            this.lvActivitiesAll.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "All Activities";
            this.columnHeader1.Width = 250;
            // 
            // lvActivitiesUser
            // 
            this.lvActivitiesUser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lvActivitiesUser.FullRowSelect = true;
            this.lvActivitiesUser.GridLines = true;
            this.lvActivitiesUser.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvActivitiesUser.Location = new System.Drawing.Point(400, 40);
            this.lvActivitiesUser.Name = "lvActivitiesUser";
            this.lvActivitiesUser.Size = new System.Drawing.Size(264, 336);
            this.lvActivitiesUser.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvActivitiesUser.TabIndex = 4;
            this.lvActivitiesUser.UseCompatibleStateImageBehavior = false;
            this.lvActivitiesUser.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "User Activities";
            this.columnHeader2.Width = 250;
            // 
            // WindowWorkflow
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(682, 427);
            this.Controls.Add(this.lvActivitiesUser);
            this.Controls.Add(this.lvActivitiesAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "WindowWorkflow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WindowWorkflow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView lvActivitiesAll;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView lvActivitiesUser;
        private System.Windows.Forms.ColumnHeader columnHeader2;

    }
}