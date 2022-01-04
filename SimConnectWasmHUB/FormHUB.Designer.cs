
namespace SimConnectWasmHUB
{
    partial class FormHUB
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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.comboBoxVariables = new System.Windows.Forms.ComboBox();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.labelVariables = new System.Windows.Forms.Label();
            this.labelValue = new System.Windows.Forms.Label();
            this.buttonAddVariable = new System.Windows.Forms.Button();
            this.buttonRemoveVariable = new System.Windows.Forms.Button();
            this.buttonSetValue = new System.Windows.Forms.Button();
            this.textResult = new System.Windows.Forms.TextBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.labelResults = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(12, 12);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(102, 12);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(75, 23);
            this.buttonDisconnect.TabIndex = 1;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // comboBoxVariables
            // 
            this.comboBoxVariables.FormattingEnabled = true;
            this.comboBoxVariables.Location = new System.Drawing.Point(12, 63);
            this.comboBoxVariables.Name = "comboBoxVariables";
            this.comboBoxVariables.Size = new System.Drawing.Size(601, 21);
            this.comboBoxVariables.TabIndex = 2;
            // 
            // textBoxValue
            // 
            this.textBoxValue.Location = new System.Drawing.Point(619, 63);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxValue.TabIndex = 3;
            // 
            // labelVariables
            // 
            this.labelVariables.AutoSize = true;
            this.labelVariables.Location = new System.Drawing.Point(9, 47);
            this.labelVariables.Name = "labelVariables";
            this.labelVariables.Size = new System.Drawing.Size(50, 13);
            this.labelVariables.TabIndex = 4;
            this.labelVariables.Text = "Variables";
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(616, 48);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(34, 13);
            this.labelValue.TabIndex = 5;
            this.labelValue.Text = "Value";
            // 
            // buttonAddVariable
            // 
            this.buttonAddVariable.Location = new System.Drawing.Point(12, 90);
            this.buttonAddVariable.Name = "buttonAddVariable";
            this.buttonAddVariable.Size = new System.Drawing.Size(135, 23);
            this.buttonAddVariable.TabIndex = 6;
            this.buttonAddVariable.Text = "Add Variable";
            this.buttonAddVariable.UseVisualStyleBackColor = true;
            this.buttonAddVariable.Click += new System.EventHandler(this.buttonAddVariable_Click);
            // 
            // buttonRemoveVariable
            // 
            this.buttonRemoveVariable.Location = new System.Drawing.Point(153, 90);
            this.buttonRemoveVariable.Name = "buttonRemoveVariable";
            this.buttonRemoveVariable.Size = new System.Drawing.Size(135, 23);
            this.buttonRemoveVariable.TabIndex = 7;
            this.buttonRemoveVariable.Text = "Remove Variable";
            this.buttonRemoveVariable.UseVisualStyleBackColor = true;
            this.buttonRemoveVariable.Click += new System.EventHandler(this.buttonRemoveVariable_Click);
            // 
            // buttonSetValue
            // 
            this.buttonSetValue.Location = new System.Drawing.Point(619, 90);
            this.buttonSetValue.Name = "buttonSetValue";
            this.buttonSetValue.Size = new System.Drawing.Size(100, 23);
            this.buttonSetValue.TabIndex = 8;
            this.buttonSetValue.Text = "Set Value";
            this.buttonSetValue.UseVisualStyleBackColor = true;
            this.buttonSetValue.Click += new System.EventHandler(this.buttonSetValue_Click);
            // 
            // textResult
            // 
            this.textResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textResult.Location = new System.Drawing.Point(12, 138);
            this.textResult.Multiline = true;
            this.textResult.Name = "textResult";
            this.textResult.ReadOnly = true;
            this.textResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textResult.Size = new System.Drawing.Size(707, 311);
            this.textResult.TabIndex = 9;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(478, 90);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(135, 23);
            this.buttonClear.TabIndex = 10;
            this.buttonClear.Text = "Clear Results";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // labelResults
            // 
            this.labelResults.AutoSize = true;
            this.labelResults.Location = new System.Drawing.Point(13, 120);
            this.labelResults.Name = "labelResults";
            this.labelResults.Size = new System.Drawing.Size(42, 13);
            this.labelResults.TabIndex = 11;
            this.labelResults.Text = "Results";
            // 
            // FormHUB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 461);
            this.Controls.Add(this.labelResults);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.textResult);
            this.Controls.Add(this.buttonSetValue);
            this.Controls.Add(this.buttonRemoveVariable);
            this.Controls.Add(this.buttonAddVariable);
            this.Controls.Add(this.labelValue);
            this.Controls.Add(this.labelVariables);
            this.Controls.Add(this.textBoxValue);
            this.Controls.Add(this.comboBoxVariables);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.buttonConnect);
            this.MaximumSize = new System.Drawing.Size(747, 1000);
            this.MinimumSize = new System.Drawing.Size(747, 500);
            this.Name = "FormHUB";
            this.Text = "SimConnect + WASM HUB";
            this.Shown += new System.EventHandler(this.FormHUB_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.ComboBox comboBoxVariables;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.Label labelVariables;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.Button buttonAddVariable;
        private System.Windows.Forms.Button buttonRemoveVariable;
        private System.Windows.Forms.Button buttonSetValue;
        private System.Windows.Forms.TextBox textResult;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Label labelResults;
    }
}

