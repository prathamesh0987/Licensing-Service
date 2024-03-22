namespace SimpleCalculatorApplication
{
    partial class SimpleCalculatorCmdDlg
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtNumberA = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNumberB = new System.Windows.Forms.TextBox();
            this.cmdSum = new System.Windows.Forms.Button();
            this.cmdSubtract = new System.Windows.Forms.Button();
            this.cmdMultiply = new System.Windows.Forms.Button();
            this.cmdDivide = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number A:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtNumberA
            // 
            this.txtNumberA.Location = new System.Drawing.Point(76, 10);
            this.txtNumberA.Name = "txtNumberA";
            this.txtNumberA.Size = new System.Drawing.Size(100, 20);
            this.txtNumberA.TabIndex = 1;
            this.txtNumberA.Text = "2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Number B:";
            // 
            // txtNumberB
            // 
            this.txtNumberB.Location = new System.Drawing.Point(76, 36);
            this.txtNumberB.Name = "txtNumberB";
            this.txtNumberB.Size = new System.Drawing.Size(100, 20);
            this.txtNumberB.TabIndex = 1;
            this.txtNumberB.Text = "1";
            // 
            // cmdSum
            // 
            this.cmdSum.Location = new System.Drawing.Point(16, 75);
            this.cmdSum.Name = "cmdSum";
            this.cmdSum.Size = new System.Drawing.Size(75, 23);
            this.cmdSum.TabIndex = 2;
            this.cmdSum.Text = "Sum";
            this.cmdSum.UseVisualStyleBackColor = true;
            this.cmdSum.Click += new System.EventHandler(this.cmdSum_Click);
            // 
            // cmdSubtract
            // 
            this.cmdSubtract.Location = new System.Drawing.Point(97, 75);
            this.cmdSubtract.Name = "cmdSubtract";
            this.cmdSubtract.Size = new System.Drawing.Size(75, 23);
            this.cmdSubtract.TabIndex = 2;
            this.cmdSubtract.Text = "Subtract";
            this.cmdSubtract.UseVisualStyleBackColor = true;
            this.cmdSubtract.Click += new System.EventHandler(this.cmdSubtract_Click);
            // 
            // cmdMultiply
            // 
            this.cmdMultiply.Location = new System.Drawing.Point(16, 104);
            this.cmdMultiply.Name = "cmdMultiply";
            this.cmdMultiply.Size = new System.Drawing.Size(75, 23);
            this.cmdMultiply.TabIndex = 2;
            this.cmdMultiply.Text = "Multiply";
            this.cmdMultiply.UseVisualStyleBackColor = true;
            this.cmdMultiply.Click += new System.EventHandler(this.cmdMultiply_Click);
            // 
            // cmdDivide
            // 
            this.cmdDivide.Location = new System.Drawing.Point(97, 104);
            this.cmdDivide.Name = "cmdDivide";
            this.cmdDivide.Size = new System.Drawing.Size(75, 23);
            this.cmdDivide.TabIndex = 2;
            this.cmdDivide.Text = "Divide";
            this.cmdDivide.UseVisualStyleBackColor = true;
            this.cmdDivide.Click += new System.EventHandler(this.cmdDivide_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Result:";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(76, 142);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(100, 20);
            this.txtResult.TabIndex = 1;
            this.txtResult.Text = "1";
            // 
            // SimpleCalculatorCmdDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.cmdDivide);
            this.Controls.Add(this.cmdMultiply);
            this.Controls.Add(this.cmdSubtract);
            this.Controls.Add(this.cmdSum);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNumberB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNumberA);
            this.Controls.Add(this.label1);
            this.Name = "SimpleCalculatorCmdDlg";
            this.Text = "Simple Calculator";
            this.Load += new System.EventHandler(this.SimpleCalculatorCmdDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNumberA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNumberB;
        private System.Windows.Forms.Button cmdSum;
        private System.Windows.Forms.Button cmdSubtract;
        private System.Windows.Forms.Button cmdMultiply;
        private System.Windows.Forms.Button cmdDivide;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtResult;
    }
}

