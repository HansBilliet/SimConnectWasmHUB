
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
            this.groupExecCalcCode = new System.Windows.Forms.GroupBox();
            this.textBoxExecCalcCode = new System.Windows.Forms.TextBox();
            this.textBoxExecCalcCodeFloat = new System.Windows.Forms.TextBox();
            this.labelExecCalcCodeFloat = new System.Windows.Forms.Label();
            this.labelExecCalcCodeInt = new System.Windows.Forms.Label();
            this.textBoxExecCalcCodeInt = new System.Windows.Forms.TextBox();
            this.labelExecCalcCodeString = new System.Windows.Forms.Label();
            this.textBoxExecCalcCodeString = new System.Windows.Forms.TextBox();
            this.buttonExecCalcCodeSend = new System.Windows.Forms.Button();
            this.groupExecCalcCode.SuspendLayout();
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
            this.comboBoxVariables.Location = new System.Drawing.Point(12, 140);
            this.comboBoxVariables.Name = "comboBoxVariables";
            this.comboBoxVariables.Size = new System.Drawing.Size(601, 21);
            this.comboBoxVariables.TabIndex = 2;
            // 
            // textBoxValue
            // 
            this.textBoxValue.Location = new System.Drawing.Point(619, 140);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxValue.TabIndex = 3;
            // 
            // labelVariables
            // 
            this.labelVariables.AutoSize = true;
            this.labelVariables.Location = new System.Drawing.Point(9, 124);
            this.labelVariables.Name = "labelVariables";
            this.labelVariables.Size = new System.Drawing.Size(50, 13);
            this.labelVariables.TabIndex = 4;
            this.labelVariables.Text = "Variables";
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(616, 125);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(34, 13);
            this.labelValue.TabIndex = 5;
            this.labelValue.Text = "Value";
            // 
            // buttonAddVariable
            // 
            this.buttonAddVariable.Location = new System.Drawing.Point(12, 167);
            this.buttonAddVariable.Name = "buttonAddVariable";
            this.buttonAddVariable.Size = new System.Drawing.Size(135, 23);
            this.buttonAddVariable.TabIndex = 6;
            this.buttonAddVariable.Text = "Add Variable";
            this.buttonAddVariable.UseVisualStyleBackColor = true;
            this.buttonAddVariable.Click += new System.EventHandler(this.buttonAddVariable_Click);
            // 
            // buttonRemoveVariable
            // 
            this.buttonRemoveVariable.Location = new System.Drawing.Point(153, 167);
            this.buttonRemoveVariable.Name = "buttonRemoveVariable";
            this.buttonRemoveVariable.Size = new System.Drawing.Size(135, 23);
            this.buttonRemoveVariable.TabIndex = 7;
            this.buttonRemoveVariable.Text = "Remove Variable";
            this.buttonRemoveVariable.UseVisualStyleBackColor = true;
            this.buttonRemoveVariable.Click += new System.EventHandler(this.buttonRemoveVariable_Click);
            // 
            // buttonSetValue
            // 
            this.buttonSetValue.Location = new System.Drawing.Point(619, 167);
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
            this.textResult.Location = new System.Drawing.Point(12, 213);
            this.textResult.Multiline = true;
            this.textResult.Name = "textResult";
            this.textResult.ReadOnly = true;
            this.textResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textResult.Size = new System.Drawing.Size(707, 236);
            this.textResult.TabIndex = 9;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(478, 167);
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
            this.labelResults.Location = new System.Drawing.Point(13, 197);
            this.labelResults.Name = "labelResults";
            this.labelResults.Size = new System.Drawing.Size(42, 13);
            this.labelResults.TabIndex = 11;
            this.labelResults.Text = "Results";
            // 
            // groupExecCalcCode
            // 
            this.groupExecCalcCode.Controls.Add(this.buttonExecCalcCodeSend);
            this.groupExecCalcCode.Controls.Add(this.textBoxExecCalcCodeString);
            this.groupExecCalcCode.Controls.Add(this.labelExecCalcCodeString);
            this.groupExecCalcCode.Controls.Add(this.textBoxExecCalcCodeInt);
            this.groupExecCalcCode.Controls.Add(this.labelExecCalcCodeInt);
            this.groupExecCalcCode.Controls.Add(this.labelExecCalcCodeFloat);
            this.groupExecCalcCode.Controls.Add(this.textBoxExecCalcCodeFloat);
            this.groupExecCalcCode.Controls.Add(this.textBoxExecCalcCode);
            this.groupExecCalcCode.Location = new System.Drawing.Point(12, 42);
            this.groupExecCalcCode.Name = "groupExecCalcCode";
            this.groupExecCalcCode.Size = new System.Drawing.Size(707, 72);
            this.groupExecCalcCode.TabIndex = 12;
            this.groupExecCalcCode.TabStop = false;
            this.groupExecCalcCode.Text = "execute_calculator_code";
            // 
            // textBoxExecCalcCode
            // 
            this.textBoxExecCalcCode.Location = new System.Drawing.Point(6, 19);
            this.textBoxExecCalcCode.Name = "textBoxExecCalcCode";
            this.textBoxExecCalcCode.Size = new System.Drawing.Size(646, 20);
            this.textBoxExecCalcCode.TabIndex = 0;
            // 
            // textBoxExecCalcCodeFloat
            // 
            this.textBoxExecCalcCodeFloat.Location = new System.Drawing.Point(45, 43);
            this.textBoxExecCalcCodeFloat.Name = "textBoxExecCalcCodeFloat";
            this.textBoxExecCalcCodeFloat.Size = new System.Drawing.Size(82, 20);
            this.textBoxExecCalcCodeFloat.TabIndex = 1;
            // 
            // labelExecCalcCodeFloat
            // 
            this.labelExecCalcCodeFloat.AutoSize = true;
            this.labelExecCalcCodeFloat.Location = new System.Drawing.Point(6, 46);
            this.labelExecCalcCodeFloat.Name = "labelExecCalcCodeFloat";
            this.labelExecCalcCodeFloat.Size = new System.Drawing.Size(33, 13);
            this.labelExecCalcCodeFloat.TabIndex = 2;
            this.labelExecCalcCodeFloat.Text = "Float:";
            // 
            // labelExecCalcCodeInt
            // 
            this.labelExecCalcCodeInt.AutoSize = true;
            this.labelExecCalcCodeInt.Location = new System.Drawing.Point(133, 46);
            this.labelExecCalcCodeInt.Name = "labelExecCalcCodeInt";
            this.labelExecCalcCodeInt.Size = new System.Drawing.Size(22, 13);
            this.labelExecCalcCodeInt.TabIndex = 3;
            this.labelExecCalcCodeInt.Text = "Int:";
            // 
            // textBoxExecCalcCodeInt
            // 
            this.textBoxExecCalcCodeInt.Location = new System.Drawing.Point(161, 43);
            this.textBoxExecCalcCodeInt.Name = "textBoxExecCalcCodeInt";
            this.textBoxExecCalcCodeInt.Size = new System.Drawing.Size(82, 20);
            this.textBoxExecCalcCodeInt.TabIndex = 4;
            // 
            // labelExecCalcCodeString
            // 
            this.labelExecCalcCodeString.AutoSize = true;
            this.labelExecCalcCodeString.Location = new System.Drawing.Point(249, 46);
            this.labelExecCalcCodeString.Name = "labelExecCalcCodeString";
            this.labelExecCalcCodeString.Size = new System.Drawing.Size(37, 13);
            this.labelExecCalcCodeString.TabIndex = 5;
            this.labelExecCalcCodeString.Text = "String:";
            // 
            // textBoxExecCalcCodeString
            // 
            this.textBoxExecCalcCodeString.Location = new System.Drawing.Point(292, 43);
            this.textBoxExecCalcCodeString.Name = "textBoxExecCalcCodeString";
            this.textBoxExecCalcCodeString.Size = new System.Drawing.Size(408, 20);
            this.textBoxExecCalcCodeString.TabIndex = 6;
            // 
            // buttonExecCalcCodeSend
            // 
            this.buttonExecCalcCodeSend.Location = new System.Drawing.Point(658, 17);
            this.buttonExecCalcCodeSend.Name = "buttonExecCalcCodeSend";
            this.buttonExecCalcCodeSend.Size = new System.Drawing.Size(42, 23);
            this.buttonExecCalcCodeSend.TabIndex = 7;
            this.buttonExecCalcCodeSend.Text = "Send";
            this.buttonExecCalcCodeSend.UseVisualStyleBackColor = true;
            this.buttonExecCalcCodeSend.Click += new System.EventHandler(this.buttonExecCalcCodeSend_Click);
            // 
            // FormHUB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 461);
            this.Controls.Add(this.groupExecCalcCode);
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
            this.groupExecCalcCode.ResumeLayout(false);
            this.groupExecCalcCode.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupExecCalcCode;
        private System.Windows.Forms.TextBox textBoxExecCalcCode;
        private System.Windows.Forms.Button buttonExecCalcCodeSend;
        private System.Windows.Forms.TextBox textBoxExecCalcCodeString;
        private System.Windows.Forms.Label labelExecCalcCodeString;
        private System.Windows.Forms.TextBox textBoxExecCalcCodeInt;
        private System.Windows.Forms.Label labelExecCalcCodeInt;
        private System.Windows.Forms.Label labelExecCalcCodeFloat;
        private System.Windows.Forms.TextBox textBoxExecCalcCodeFloat;
    }
}

