namespace PolygonManipulator
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.delete_relation = new System.Windows.Forms.RadioButton();
            this.equall = new System.Windows.Forms.RadioButton();
            this.parallel = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.move_vertex_edge = new System.Windows.Forms.RadioButton();
            this.delete_vertex = new System.Windows.Forms.RadioButton();
            this.add_vertex = new System.Windows.Forms.RadioButton();
            this.MainCanvas = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioBresenham = new System.Windows.Forms.RadioButton();
            this.radioBressym = new System.Windows.Forms.RadioButton();
            this.radioantyaliasing = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainCanvas)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.delete_relation);
            this.groupBox1.Controls.Add(this.equall);
            this.groupBox1.Controls.Add(this.parallel);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.move_vertex_edge);
            this.groupBox1.Controls.Add(this.delete_vertex);
            this.groupBox1.Controls.Add(this.add_vertex);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Font = new System.Drawing.Font("Tempus Sans ITC", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(661, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(139, 450);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "OPTIONS";
            // 
            // delete_relation
            // 
            this.delete_relation.AutoSize = true;
            this.delete_relation.Font = new System.Drawing.Font("Tempus Sans ITC", 9.75F, System.Drawing.FontStyle.Bold);
            this.delete_relation.Location = new System.Drawing.Point(17, 227);
            this.delete_relation.Name = "delete_relation";
            this.delete_relation.Size = new System.Drawing.Size(121, 21);
            this.delete_relation.TabIndex = 7;
            this.delete_relation.TabStop = true;
            this.delete_relation.Text = "Delete relation";
            this.delete_relation.UseVisualStyleBackColor = true;
            // 
            // equall
            // 
            this.equall.AutoSize = true;
            this.equall.Font = new System.Drawing.Font("Tempus Sans ITC", 9.75F, System.Drawing.FontStyle.Bold);
            this.equall.Location = new System.Drawing.Point(17, 200);
            this.equall.Name = "equall";
            this.equall.Size = new System.Drawing.Size(96, 21);
            this.equall.TabIndex = 6;
            this.equall.TabStop = true;
            this.equall.Text = "Add Equall";
            this.equall.UseVisualStyleBackColor = true;
            this.equall.CheckedChanged += new System.EventHandler(this.equall_CheckedChanged);
            // 
            // parallel
            // 
            this.parallel.AutoSize = true;
            this.parallel.Font = new System.Drawing.Font("Tempus Sans ITC", 9.75F, System.Drawing.FontStyle.Bold);
            this.parallel.Location = new System.Drawing.Point(17, 173);
            this.parallel.Name = "parallel";
            this.parallel.Size = new System.Drawing.Size(103, 21);
            this.parallel.TabIndex = 5;
            this.parallel.TabStop = true;
            this.parallel.Text = "Add Parallel";
            this.parallel.UseVisualStyleBackColor = true;
            this.parallel.CheckedChanged += new System.EventHandler(this.parallel_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Tempus Sans ITC", 9.75F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(17, 402);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 36);
            this.button1.TabIndex = 4;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // move_vertex_edge
            // 
            this.move_vertex_edge.AutoSize = true;
            this.move_vertex_edge.Font = new System.Drawing.Font("Tempus Sans ITC", 9.75F, System.Drawing.FontStyle.Bold);
            this.move_vertex_edge.Location = new System.Drawing.Point(17, 117);
            this.move_vertex_edge.Name = "move_vertex_edge";
            this.move_vertex_edge.Size = new System.Drawing.Size(62, 21);
            this.move_vertex_edge.TabIndex = 3;
            this.move_vertex_edge.TabStop = true;
            this.move_vertex_edge.Text = "Move";
            this.move_vertex_edge.UseVisualStyleBackColor = true;
            // 
            // delete_vertex
            // 
            this.delete_vertex.AutoSize = true;
            this.delete_vertex.Font = new System.Drawing.Font("Tempus Sans ITC", 9.75F, System.Drawing.FontStyle.Bold);
            this.delete_vertex.Location = new System.Drawing.Point(17, 72);
            this.delete_vertex.Name = "delete_vertex";
            this.delete_vertex.Size = new System.Drawing.Size(110, 21);
            this.delete_vertex.TabIndex = 2;
            this.delete_vertex.TabStop = true;
            this.delete_vertex.Text = "Delete vertex";
            this.delete_vertex.UseVisualStyleBackColor = true;
            // 
            // add_vertex
            // 
            this.add_vertex.AutoSize = true;
            this.add_vertex.Font = new System.Drawing.Font("Tempus Sans ITC", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.add_vertex.Location = new System.Drawing.Point(17, 45);
            this.add_vertex.Name = "add_vertex";
            this.add_vertex.Size = new System.Drawing.Size(96, 21);
            this.add_vertex.TabIndex = 0;
            this.add_vertex.TabStop = true;
            this.add_vertex.Text = "Add vertex";
            this.add_vertex.UseVisualStyleBackColor = true;
            this.add_vertex.CheckedChanged += new System.EventHandler(this.add_vertex_CheckedChanged);
            // 
            // MainCanvas
            // 
            this.MainCanvas.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.MainCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainCanvas.Location = new System.Drawing.Point(0, 0);
            this.MainCanvas.Name = "MainCanvas";
            this.MainCanvas.Size = new System.Drawing.Size(661, 450);
            this.MainCanvas.TabIndex = 2;
            this.MainCanvas.TabStop = false;
            this.MainCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainCanvas_MouseDown);
            this.MainCanvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainCanvas_MouseUp);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioantyaliasing);
            this.groupBox2.Controls.Add(this.radioBressym);
            this.groupBox2.Controls.Add(this.radioBresenham);
            this.groupBox2.Location = new System.Drawing.Point(11, 287);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(127, 100);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Drawing line";
            // 
            // radioBresenham
            // 
            this.radioBresenham.AutoSize = true;
            this.radioBresenham.Checked = true;
            this.radioBresenham.Font = new System.Drawing.Font("Tempus Sans ITC", 8F, System.Drawing.FontStyle.Bold);
            this.radioBresenham.Location = new System.Drawing.Point(6, 23);
            this.radioBresenham.Name = "radioBresenham";
            this.radioBresenham.Size = new System.Drawing.Size(85, 19);
            this.radioBresenham.TabIndex = 0;
            this.radioBresenham.TabStop = true;
            this.radioBresenham.Text = "Bresenham";
            this.radioBresenham.UseVisualStyleBackColor = true;
            // 
            // radioBressym
            // 
            this.radioBressym.AutoSize = true;
            this.radioBressym.Font = new System.Drawing.Font("Tempus Sans ITC", 8F, System.Drawing.FontStyle.Bold);
            this.radioBressym.Location = new System.Drawing.Point(6, 48);
            this.radioBressym.Name = "radioBressym";
            this.radioBressym.Size = new System.Drawing.Size(116, 19);
            this.radioBressym.TabIndex = 1;
            this.radioBressym.Text = "Bresenham sym.";
            this.radioBressym.UseVisualStyleBackColor = true;
            // 
            // radioantyaliasing
            // 
            this.radioantyaliasing.AutoSize = true;
            this.radioantyaliasing.Font = new System.Drawing.Font("Tempus Sans ITC", 8F, System.Drawing.FontStyle.Bold);
            this.radioantyaliasing.Location = new System.Drawing.Point(6, 73);
            this.radioantyaliasing.Name = "radioantyaliasing";
            this.radioantyaliasing.Size = new System.Drawing.Size(121, 19);
            this.radioantyaliasing.TabIndex = 2;
            this.radioantyaliasing.Text = "Antyaliasing WU";
            this.radioantyaliasing.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MainCanvas);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Polygon Manipulator";
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainCanvas)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton delete_vertex;
        private System.Windows.Forms.RadioButton add_vertex;
        private System.Windows.Forms.PictureBox MainCanvas;
        private System.Windows.Forms.RadioButton move_vertex_edge;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton parallel;
        private System.Windows.Forms.RadioButton equall;
        private System.Windows.Forms.RadioButton delete_relation;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioantyaliasing;
        private System.Windows.Forms.RadioButton radioBressym;
        private System.Windows.Forms.RadioButton radioBresenham;
    }
}

