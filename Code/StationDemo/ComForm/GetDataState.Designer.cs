namespace StationDemo
{
    partial class GetDataState
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
            this.NozzleStates = new System.Windows.Forms.GroupBox();
            this.SocketState = new System.Windows.Forms.GroupBox();
            this.txtNozzleStates = new System.Windows.Forms.TextBox();
            this.txtSoketStates = new System.Windows.Forms.TextBox();
            this.NozzleStates.SuspendLayout();
            this.SocketState.SuspendLayout();
            this.SuspendLayout();
            // 
            // NozzleStates
            // 
            this.NozzleStates.Controls.Add(this.txtNozzleStates);
            this.NozzleStates.Location = new System.Drawing.Point(12, 12);
            this.NozzleStates.Name = "NozzleStates";
            this.NozzleStates.Size = new System.Drawing.Size(369, 438);
            this.NozzleStates.TabIndex = 0;
            this.NozzleStates.TabStop = false;
            this.NozzleStates.Text = "吸嘴状态";
            // 
            // SocketState
            // 
            this.SocketState.Controls.Add(this.txtSoketStates);
            this.SocketState.Location = new System.Drawing.Point(419, 12);
            this.SocketState.Name = "SocketState";
            this.SocketState.Size = new System.Drawing.Size(369, 438);
            this.SocketState.TabIndex = 1;
            this.SocketState.TabStop = false;
            this.SocketState.Text = "Socket(治具）状态";
            // 
            // txtNozzleStates
            // 
            this.txtNozzleStates.Location = new System.Drawing.Point(6, 20);
            this.txtNozzleStates.Multiline = true;
            this.txtNozzleStates.Name = "txtNozzleStates";
            this.txtNozzleStates.Size = new System.Drawing.Size(357, 406);
            this.txtNozzleStates.TabIndex = 0;
            // 
            // txtSoketStates
            // 
            this.txtSoketStates.Location = new System.Drawing.Point(0, 20);
            this.txtSoketStates.Multiline = true;
            this.txtSoketStates.Name = "txtSoketStates";
            this.txtSoketStates.Size = new System.Drawing.Size(357, 406);
            this.txtSoketStates.TabIndex = 1;
            // 
            // GetDataState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SocketState);
            this.Controls.Add(this.NozzleStates);
            this.Name = "GetDataState";
            this.Text = "GetDataState";
            this.Load += new System.EventHandler(this.GetDataState_Load);
            this.NozzleStates.ResumeLayout(false);
            this.NozzleStates.PerformLayout();
            this.SocketState.ResumeLayout(false);
            this.SocketState.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox NozzleStates;
        private System.Windows.Forms.GroupBox SocketState;
        private System.Windows.Forms.TextBox txtNozzleStates;
        private System.Windows.Forms.TextBox txtSoketStates;
    }
}