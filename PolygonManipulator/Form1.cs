using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonManipulator
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Polygons = new List<List<Vertex>>();
            highlighted = null;
            MainCanvas.Image = new Bitmap(MainCanvas.Width, MainCanvas.Height);
            FinishedAction = true;
            timer = new Timer();
            add_vertex.Checked = true;
        }
        
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (MovingMode == 0 || highlighted==null) return;

            Point p = MainCanvas.PointToClient(Cursor.Position);
            int dx = p.X - PrevoiusLocation.X;
            int dy = p.Y - PrevoiusLocation.Y;

            if (p.X < 0 || p.Y < 0 || p.X + 9 > MainCanvas.Width || p.Y + 9 > MainCanvas.Height)
            {
                timer.Stop();
                MovingMode = 0;
            }
            PrevoiusLocation = new Point(PrevoiusLocation.X + dx, PrevoiusLocation.Y + dy);

            highlighted.MoveVertex(dx, dy);

            if (MovingMode == 1)
            {
                Vertex temp = highlighted.PreviousVertex;
                while (temp.Relation != null && temp != highlighted)
                {
                    temp.MoveVertex(dx, dy);
                    temp = temp.PreviousVertex;
                }
                if (temp != highlighted && highlighted.Relation!=null)//not the full circle
                {
                    temp = highlighted.NextVertex;
                    while (temp.Relation != null && temp != highlighted)
                    {
                        temp.MoveVertex(dx, dy);
                        temp = temp.NextVertex;
                    }
                    temp.MoveVertex(dx, dy);
                }
            }
            if (MovingMode == 2) //moving whole edge=> highlighted.nextvertex too
            {
                highlighted.NextVertex.MoveVertex(dx, dy);
                Vertex temp = highlighted.PreviousVertex;
                while (temp.Relation != null && temp != highlighted.NextVertex)
                {
                    temp.MoveVertex(dx, dy);
                    temp = temp.PreviousVertex;
                }
                if (temp != highlighted.NextVertex && highlighted.NextVertex.Relation != null)//not the full circle
                {
                    temp = highlighted.NextVertex.NextVertex;
                    while (temp.Relation != null && temp != highlighted)
                    {
                        temp.MoveVertex(dx, dy);
                        temp = temp.NextVertex;
                    }
                    temp.MoveVertex(dx, dy);
                }
            }
            else if (MovingMode == 3)
            {
                Vertex temp = highlighted.NextVertex;
                while (temp != highlighted)
                {
                    temp.MoveVertex(dx, dy);
                    temp = temp.NextVertex;
                }
            }

            ReloadCanvas();
        }

        List<List<Vertex>> Polygons;
        Vertex highlighted;
        bool FinishedAction;
        Timer timer;
        Point PrevoiusLocation;
        int MovingMode; //0-non, 1-vertex, 2-edge, 3-polygon
        public class Vertex
        {
            public PictureBox PictureBox;
            public Vertex PreviousVertex;
            public Vertex NextVertex;
            public EdgeRelation Relation;

            public void MoveVertex(int dx, int dy)
            {
                PictureBox.Location = new Point(PictureBox.Location.X + dx, PictureBox.Location.Y + dy);
                if (Relation != null) Relation.symbol.Location = new Point(Relation.symbol.Location.X + dx, Relation.symbol.Location.Y + dy);
            }
            public Vertex(int x, int y, Vertex prevoius, Vertex next, Control parent)
            {
                PictureBox = new PictureBox();
                PictureBox.Width = PictureBox.Height = 9;
                PictureBox.BackColor = Color.Red;
                PictureBox.Location = new Point(x, y);
                PictureBox.Parent = parent;
                PictureBox.Visible = true;
                PictureBox.Tag = this;
                PreviousVertex = prevoius;
                NextVertex = next;
                Relation = null;
            }
        }

        public class EdgeRelation
        {
            public int Mode;//1-parallel, 2-equall
            public Vertex v;
            public Vertex me;
            public PictureBox symbol;

            public EdgeRelation(int mode, Vertex V1, Vertex Me, Control parent)
            {
                Mode = mode;
                v = V1;
                me = Me;
                symbol = new PictureBox();
                symbol.Size = new Size(30, 30);
                symbol.Parent = parent;
                symbol.Tag = me;
                symbol.Location = new Point((me.PictureBox.Location.X + me.NextVertex.PictureBox.Location.X) / 2 -5, (me.PictureBox.Location.Y + me.NextVertex.PictureBox.Location.Y) / 2 -5);
                if (mode == 1) symbol.Image = PolygonManipulator.Properties.Resources.parallel;
                else symbol.Image = PolygonManipulator.Properties.Resources.equall;
                symbol.Visible = true;
            }

        }

        private int DetermineEdge(Point p)
        {
            for (int i = 0; i < Polygons.Count; i++)
            {
                for (int j = 0; j < Polygons[i].Count; j++)
                {
                    int x0 = Polygons[i][j].PictureBox.Location.X + 3;
                    int y0 = Polygons[i][j].PictureBox.Location.Y + 3;
                    int x1 = Polygons[i][j].NextVertex.PictureBox.Location.X + 3;
                    int y1 = Polygons[i][j].NextVertex.PictureBox.Location.Y + 3;
                    if (radioBresenham.Checked)
                    {
                        int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
                        int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
                        int err = (dx > dy ? dx : -dy) / 2, e2;
                        for (; ; )
                        {
                            if ((p.X == x0 && p.Y == y0) || (p.X == x0 && p.Y == y0 + 1) || (p.X == x0 && p.Y == y0 - 1) || (p.X == x0 + 1 && p.Y == y0) || (p.X == x0 - 1 && p.Y == y0))
                            {
                                UpdateHighlighted(Polygons[i][j]);
                                return i;
                            }

                            if (x0 == x1 && y0 == y1) break;
                            e2 = err;
                            if (e2 > -dx) { err -= dy; x0 += sx; }
                            if (e2 < dy) { err += dx; y0 += sy; }
                        }
                    }
                    else if (radioBressym.Checked)
                    {
                        if (x1 < x0)
                        {
                            int temp = x1;
                            x1 = x0;
                            x0 = temp;

                            temp = y1;
                            y1 = y0;
                            y0 = temp;
                        }
                        int dx = x1 - x0;
                        int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
                        int err = (dx > dy ? dx : -dy) / 2, e2;

                        if ((p.X == x0 && p.Y == y0) || (p.X == x0 && p.Y == y0 + 1) || (p.X == x0 && p.Y == y0 - 1) || (p.X == x0 + 1 && p.Y == y0) || (p.X == x0 - 1 && p.Y == y0))
                        {
                            UpdateHighlighted(Polygons[i][j]);
                            return i;
                        }
                        if ((p.X == x1 && p.Y == y1) || (p.X == x1 && p.Y == y1 + 1) || (p.X == x1 && p.Y == y1 - 1) || (p.X == x1 + 1 && p.Y == y1) || (p.X == x1 - 1 && p.Y == y1))
                        {
                            UpdateHighlighted(Polygons[i][j]);
                            return i;
                        }
                        while (x0 < x1)
                        {
                            e2 = err;
                            if (e2 > -dx)
                            {
                                err -= dy;
                                x0++;
                                x1--;
                            }
                            if (e2 < dy)
                            {
                                err += dx;
                                y0 += sy;
                                y1 -= sy;
                            }
                            if ((p.X == x0 && p.Y == y0) || (p.X == x0 && p.Y == y0 + 1) || (p.X == x0 && p.Y == y0 - 1) || (p.X == x0 + 1 && p.Y == y0) || (p.X == x0 - 1 && p.Y == y0))
                            {
                                UpdateHighlighted(Polygons[i][j]);
                                return i;
                            }
                            if ((p.X == x1 && p.Y == y1) || (p.X == x1 && p.Y == y1 + 1) || (p.X == x1 && p.Y == y1 - 1) || (p.X == x1 + 1 && p.Y == y1) || (p.X == x1 - 1 && p.Y == y1))
                            {
                                UpdateHighlighted(Polygons[i][j]);
                                return i;
                            }
                        }
                        if (y0 > y1)
                        {
                            int temp = y0;
                            y0 = y1;
                            y1 = temp;
                        }
                        while (y1 >= y0)
                        {
                            if ((p.X == x0 && p.Y == y0) || (p.X == x0 && p.Y == y0 + 1) || (p.X == x0 && p.Y == y0 - 1) || (p.X == x0 + 1 && p.Y == y0) || (p.X == x0 - 1 && p.Y == y0))
                            {
                                UpdateHighlighted(Polygons[i][j]);
                                return i;
                            }
                            if ((p.X == x1 && p.Y == y1) || (p.X == x1 && p.Y == y1 + 1) || (p.X == x1 && p.Y == y1 - 1) || (p.X == x1 + 1 && p.Y == y1) || (p.X == x1 - 1 && p.Y == y1))
                            {
                                UpdateHighlighted(Polygons[i][j]);
                                return i;
                            }
                            y0++;
                            y1--;
                        }
                    }
                    else
                    {
                        int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
                        int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
                        int err = (dx > dy ? dx : -dy) / 2, e2;
                        int c1, c2;
                        for (; ; )
                        {
                            if (x0 == x1 && y0 == y1) break;
                            e2 = err;
                            if (e2 > -dx) { err -= dy; x0 += sx; }
                            if (e2 < dy) { err += dx; y0 += sy; }

                            c1 = Convert.ToInt32(255 * (1 - frac(y0)));
                            c2 = Convert.ToInt32(255 * frac(y0));
                            if ((p.X == x0 && p.Y == y0) || (p.X == x0 && p.Y == y0 + 1) || (p.X == x0 && p.Y == y0 - 1))
                            {
                                UpdateHighlighted(Polygons[i][j]);
                                return i;
                            }

                        }
                    }
                }
            }
            return -1;
        }

        private void SetBold(int x0, int y0, Color color)
        {
            if (x0 > 0 && y0 > 0 && x0 < MainCanvas.Image.Width && y0 < MainCanvas.Image.Height) ((Bitmap)MainCanvas.Image).SetPixel(x0, y0, color);
            if (x0 > 0 && y0 + 1 > 0 && x0 < MainCanvas.Image.Width && y0 + 1 < MainCanvas.Image.Height) ((Bitmap)MainCanvas.Image).SetPixel(x0, y0 + 1, color);
            if (x0 > 0 && y0 - 1 > 0 && x0 < MainCanvas.Image.Width && y0 - 1 < MainCanvas.Image.Height) ((Bitmap)MainCanvas.Image).SetPixel(x0, y0 - 1, color);
            if (x0 - 1 > 0 && y0 > 0 && x0 - 1 < MainCanvas.Image.Width && y0 < MainCanvas.Image.Height) ((Bitmap)MainCanvas.Image).SetPixel(x0 - 1, y0, color);
            if (x0 + 1 > 0 && y0 > 0 && x0 + 1 < MainCanvas.Image.Width && y0 < MainCanvas.Image.Height) ((Bitmap)MainCanvas.Image).SetPixel(x0 + 1, y0, color);
        }
        private double frac(double y)
        {
            return y - Math.Floor(y);
        }
        private void DrawLine(int x0, int y0, int x1, int y1, Color color)
        {
            Bitmap canv = ((Bitmap)MainCanvas.Image);
            if (radioBresenham.Checked)
            {
                int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
                int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
                int err = (dx > dy ? dx : -dy) / 2, e2;
                for (; ; )
                {
                    SetBold(x0, y0, color);
                    if (x0 == x1 && y0 == y1) break;
                    e2 = err;
                    if (e2 > -dx) { err -= dy; x0 += sx; }
                    if (e2 < dy) { err += dx; y0 += sy; }
                }
            }
            else if (radioBressym.Checked)
            {
                if (x1 < x0)
                {
                    int temp = x1;
                    x1 = x0;
                    x0 = temp;

                    temp = y1;
                    y1 = y0;
                    y0 = temp;
                }
                int dx = x1 - x0;
                int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
                int err = (dx > dy ? dx : -dy) / 2, e2;

                SetBold(x0, y0, color);
                SetBold(x1, y1, color);
                while (x0 < x1)
                {
                    e2 = err;
                    if (e2 > -dx)
                    {
                        err -= dy;
                        x0++;
                        x1--;
                    }
                    if (e2 < dy)
                    {
                        err += dx;
                        y0 += sy;
                        y1 -= sy;
                    }
                    SetBold(x0, y0, color);
                    SetBold(x1, y1, color);
                }
                if (y0 > y1)
                {
                    int temp = y0;
                    y0 = y1;
                    y1 = temp;
                }
                while (y1 >= y0)
                {
                    SetBold(x0, y0, color);
                    SetBold(x1, y1, color);
                    y0++;
                    y1--;
                }
            }
            else //antialiasing
            {
                int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
                int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
                int err = (dx > dy ? dx : -dy) / 2, e2;
                int c1, c2;
                for (; ; )
                {
                    if (x0 == x1 && y0 == y1) break;
                    e2 = err;
                    if (e2 > -dx) { err -= dy; x0 += sx; }
                    if (e2 < dy) { err += dx; y0 += sy; }

                    c1 = Convert.ToInt32(255 * (1 - frac(y0)));
                    c2 = Convert.ToInt32(255 * frac(y0));
                    canv.SetPixel(x0, y0-1, Color.FromArgb(c1, color));
                    canv.SetPixel(x0, y0,color);
                    canv.SetPixel(x0, y0 + 1, Color.FromArgb(c2, color));

                }
            }
        }
        
        private void MainCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (add_vertex.Checked)
            {
                Vertex v;
                if (!FinishedAction)
                {
                    DrawLine(highlighted.PictureBox.Location.X + 4, highlighted.PictureBox.Location.Y + 4, e.X + 4, e.Y + 4, Color.Purple);
                    v = new Vertex(e.X, e.Y, highlighted, null, (Control)sender);
                    highlighted.NextVertex = v;
                    v.PictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(Vertex_MouseDown);
                    v.PictureBox.MouseUp += PictureBox_MouseUp;
                    Polygons.Last().Add(v);
                }
                else
                {
                    if (((Bitmap)MainCanvas.Image).GetPixel(e.Location.X, e.Location.Y).ToArgb() == Color.Purple.ToArgb()) //add vertex  to edge
                    {
                        int pol = DetermineEdge(e.Location);
                        DeleteRelation(highlighted);
                        v = new Vertex((highlighted.PictureBox.Location.X + highlighted.NextVertex.PictureBox.Location.X) / 2, (highlighted.PictureBox.Location.Y + highlighted.NextVertex.PictureBox.Location.Y) / 2, highlighted, highlighted.NextVertex, (Control)sender);
                        v.PictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(Vertex_MouseDown);
                        v.PictureBox.MouseUp += PictureBox_MouseUp;
                        highlighted.NextVertex.PreviousVertex = v;
                        highlighted.NextVertex = v;
                        Polygons[pol].Add(v);
                    }
                    else
                    {
                        v = new Vertex(e.X, e.Y, null, null, (Control)sender);
                        v.PictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(Vertex_MouseDown);
                        v.PictureBox.MouseUp += PictureBox_MouseUp;
                        Polygons.Add(new List<Vertex> { v });
                        FinishedAction = false;
                    }
                }
                UpdateHighlighted(v);

                MainCanvas.Refresh();
            }
            else if (move_vertex_edge.Checked)
            {
                if (((Bitmap)MainCanvas.Image).GetPixel(e.Location.X, e.Location.Y).ToArgb() == MainCanvas.BackColor.ToArgb()) //not edge => moving Polygon
                {
                    MovingMode = 3;
                }
                else //moving edge
                {
                    MovingMode = 2;
                    DetermineEdge(e.Location);
                }
                PrevoiusLocation = e.Location;
                timer = new Timer();
                timer.Interval = 10;
                timer.Tick += Timer_Tick;
                timer.Start();
            }
            else if (parallel.Checked)
            {
                if (((Bitmap)MainCanvas.Image).GetPixel(e.Location.X, e.Location.Y).ToArgb() == MainCanvas.BackColor.ToArgb()) return;
                Vertex temp = highlighted;
                int ind = DetermineEdge(e.Location);
                if (highlighted.Relation != null)
                {
                    temp.PictureBox.BackColor = Color.Blue;
                    temp.NextVertex.PictureBox.BackColor = Color.Blue;
                    UpdateHighlighted(highlighted);
                    FinishedAction = true;
                    MessageBox.Show("Relation already set");
                    return;
                }
                if (FinishedAction) //first edge in relation
                {
                    FinishedAction = false;
                    highlighted.PictureBox.BackColor = Color.Yellow;
                    highlighted.NextVertex.PictureBox.BackColor = Color.Yellow;
                }
                else//second edge in relation
                {
                    if (temp == highlighted) return;
                    if (temp.NextVertex == highlighted || highlighted.NextVertex == temp)
                    {
                        FinishedAction = true;
                        temp.PictureBox.BackColor = Color.Blue;
                        temp.NextVertex.PictureBox.BackColor = Color.Blue;
                        UpdateHighlighted(highlighted);
                        MessageBox.Show("Incidental edges cannot be parallel");
                        return;
                    }
                    bool already=SetParallel(temp, highlighted);
                    temp.Relation = new EdgeRelation(1, highlighted, temp, MainCanvas);
                    temp.Relation.symbol.MouseDown += Symbol_MouseDown;
                    highlighted.Relation = new EdgeRelation(1, temp, highlighted, MainCanvas);
                    highlighted.Relation.symbol.MouseDown += Symbol_MouseDown;
                    FinishedAction = true;
                    temp.PictureBox.BackColor = Color.Blue;
                    temp.NextVertex.PictureBox.BackColor = Color.Blue;
                    UpdateHighlighted(highlighted);
                    if (!already)
                    {
                        if (!CorrectRelations(highlighted.NextVertex, ind))
                        {
                            DeleteRelation(highlighted);
                            CorrectRelations(highlighted.NextVertex, ind);
                            ReloadCanvas();
                            MessageBox.Show("Relation could not be set");
                        }
                    }
                    ReloadCanvas();
                }
            }
            else if (equall.Checked)
            {
                if (((Bitmap)MainCanvas.Image).GetPixel(e.Location.X, e.Location.Y).ToArgb() == MainCanvas.BackColor.ToArgb()) return;
                Vertex temp = highlighted;
                int ind = DetermineEdge(e.Location);

                if (highlighted.Relation != null)
                {
                    FinishedAction = true;
                    temp.PictureBox.BackColor = Color.Blue;
                    temp.NextVertex.PictureBox.BackColor = Color.Blue;
                    UpdateHighlighted(highlighted);
                    MessageBox.Show("Relation already set");
                    return;
                }
                if (FinishedAction) //first edge in relation
                {
                    FinishedAction = false;
                    highlighted.PictureBox.BackColor = Color.Green;
                    highlighted.NextVertex.PictureBox.BackColor = Color.Green;
                }
                else//second edge in relation
                {
                    if (temp == highlighted) return;
                    bool already=SetEquall(temp, highlighted);
                    temp.Relation = new EdgeRelation(2, highlighted, temp, MainCanvas);
                    temp.Relation.symbol.MouseDown += Symbol_MouseDown;
                    highlighted.Relation = new EdgeRelation(2, temp, highlighted, MainCanvas);
                    highlighted.Relation.symbol.MouseDown += Symbol_MouseDown;
                    temp.PictureBox.BackColor = Color.Blue;
                    temp.NextVertex.PictureBox.BackColor = Color.Blue;
                    FinishedAction = true;
                    UpdateHighlighted(highlighted);
                    if (!already)
                    {
                        if (!CorrectRelations(highlighted.NextVertex, ind))
                        {
                            DeleteRelation(highlighted);
                            CorrectRelations(highlighted.NextVertex, ind);
                            ReloadCanvas();
                            MessageBox.Show("Relation could not be set");
                        }
                    }
                    ReloadCanvas();
                }
            }
            else if (delete_relation.Checked)
            {
                if (((Bitmap)MainCanvas.Image).GetPixel(e.Location.X, e.Location.Y).ToArgb() == MainCanvas.BackColor.ToArgb()) return;
                DetermineEdge(e.Location);
                DeleteRelation(highlighted);
            }
        }

        private void Symbol_MouseDown(object sender, MouseEventArgs e)
        {
            if (delete_relation.Checked)
            {
                Vertex v = (Vertex)((PictureBox)sender).Tag;
                DeleteRelation(v);
            }
        }

        bool CorrectRelations(Vertex v, int ind)
        {
            int counter = 0;
            Vertex temp = v;
            while (temp.Relation != null)
            {
                if (temp.Relation.Mode == 1)
                {
                    if (SetParallel(temp.Relation.v, temp)) return true;
                }
                else
                {
                    if (SetEquall(temp.Relation.v, temp)) return true;
                }
                temp.Relation.symbol.Location= new Point((temp.PictureBox.Location.X + temp.NextVertex.PictureBox.Location.X) / 2-5, (temp.PictureBox.Location.Y + temp.NextVertex.PictureBox.Location.Y) / 2-5);
                temp = temp.NextVertex;
                counter++;
                if (counter > Polygons[ind].Count) return false;
                
            }
            return true;
        }
        bool SetParallel(Vertex v1, Vertex v2)
        {
            int c = (v1.NextVertex.PictureBox.Location.Y - v1.PictureBox.Location.Y) * (v2.NextVertex.PictureBox.Location.X - v2.PictureBox.Location.X);
            int d = (v2.NextVertex.PictureBox.Location.Y - v2.PictureBox.Location.Y) * (v1.NextVertex.PictureBox.Location.X - v1.PictureBox.Location.X);
            if (c == d) return true;
            else //they're not parallel already
            {
                if (c == 0) //first edge horizontal
                {
                    int diff = 0;
                    if (v2.PictureBox.Location.X == v2.NextVertex.PictureBox.Location.X) diff = 30; //second vertical
                    v2.NextVertex.PictureBox.Location = new Point(v2.NextVertex.PictureBox.Location.X + diff, v2.PictureBox.Location.Y);
                    return false;
                }
                if (d == 0) //first edge vertical
                {
                    int diff = 0;
                    if (v2.PictureBox.Location.Y == v2.NextVertex.PictureBox.Location.Y) diff = 30; //second horizontal
                    v2.NextVertex.PictureBox.Location = new Point(v2.PictureBox.Location.X, v2.NextVertex.PictureBox.Location.Y+diff);
                    return false;
                }
                if (Math.Abs(v2.NextVertex.PictureBox.Location.X - v2.PictureBox.Location.X) < Math.Abs(v2.NextVertex.PictureBox.Location.Y - v2.PictureBox.Location.Y))//change x
                {

                    int newx = d / (v1.NextVertex.PictureBox.Location.Y - v1.PictureBox.Location.Y) + v2.PictureBox.Location.X;
                    v2.NextVertex.PictureBox.Location = new Point(newx, v2.NextVertex.PictureBox.Location.Y);
                }
                else
                {
                    int newy=c/(v1.NextVertex.PictureBox.Location.X-v1.PictureBox.Location.X)+v2.PictureBox.Location.Y;
                    v2.NextVertex.PictureBox.Location = new Point(v2.NextVertex.PictureBox.Location.X, newy);
                }
            }
            return false;
        }

        bool SetEquall(Vertex v1, Vertex v2)
        {
            int d= (v1.NextVertex.PictureBox.Location.X - v1.PictureBox.Location.X) * (v1.NextVertex.PictureBox.Location.X - v1.PictureBox.Location.X) + (v1.NextVertex.PictureBox.Location.Y - v1.PictureBox.Location.Y) * (v1.NextVertex.PictureBox.Location.Y - v1.PictureBox.Location.Y);

            if(d== ((v2.NextVertex.PictureBox.Location.X - v2.PictureBox.Location.X) *(v2.NextVertex.PictureBox.Location.X - v2.PictureBox.Location.X) + (v2.NextVertex.PictureBox.Location.Y - v2.PictureBox.Location.Y) * (v2.NextVertex.PictureBox.Location.Y - v2.PictureBox.Location.Y))) return true;
            if (v2.PictureBox.Location.X == v2.NextVertex.PictureBox.Location.X)//vertical edge
            {
                d = Convert.ToInt32(Math.Sqrt(d)); //needed lenght
                if (v2.NextVertex.PictureBox.Location.Y >= v2.PictureBox.Location.Y) v2.NextVertex.PictureBox.Location = new Point(v2.NextVertex.PictureBox.Location.X, d + v2.PictureBox.Location.Y);
                else v2.NextVertex.PictureBox.Location = new Point(v2.NextVertex.PictureBox.Location.X, v2.PictureBox.Location.Y - d);
            }
            else if (v2.PictureBox.Location.Y == v2.NextVertex.PictureBox.Location.Y)//horizontal edge
            {
                d = Convert.ToInt32(Math.Sqrt(d)); //needed lenght
                if (v2.NextVertex.PictureBox.Location.X >= v2.PictureBox.Location.X) v2.NextVertex.PictureBox.Location = new Point(d + v2.PictureBox.Location.X, v2.NextVertex.PictureBox.Location.Y);
                else v2.NextVertex.PictureBox.Location = new Point(v2.PictureBox.Location.X - d, v2.NextVertex.PictureBox.Location.Y);
            }
            else
            {
                double a = Convert.ToDouble(v2.NextVertex.PictureBox.Location.Y - v2.PictureBox.Location.Y) / (v2.NextVertex.PictureBox.Location.X - v2.PictureBox.Location.X);
                d = Convert.ToInt32(Math.Sqrt(d / (a * a + 1)));
                int newx = 0;
                if (v2.NextVertex.PictureBox.Location.X >= v2.PictureBox.Location.X) newx = d + v2.PictureBox.Location.X;
                else newx = v2.PictureBox.Location.X - d;
                v2.NextVertex.PictureBox.Location = new Point(newx,  Convert.ToInt32(a*(newx-v2.PictureBox.Location.X) + v2.PictureBox.Location.Y) );
            }
            return false;
        }
        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            timer.Stop();
            MovingMode = 0;
        }

        private void Vertex_MouseDown(object sender, MouseEventArgs e)
        {
            Vertex v = (Vertex)((Control)sender).Tag;
            if (add_vertex.Checked)
            {
                FinishedAction = true;
                
                if (v.PreviousVertex != null || Polygons.Last().Count < 3) //wrong or not enough vertices
                {
                    if (Polygons.Last().Contains(highlighted))
                    {
                        if (Polygons.Count > 1) UpdateHighlighted (Polygons[Polygons.Count - 2][0]);
                        else UpdateHighlighted( null);
                    }
                    foreach (var item in Polygons.Last()) item.PictureBox.Dispose();
                    Polygons.RemoveAt(Polygons.Count - 1);
                    ReloadCanvas();
                    return;
                }
                DrawLine(highlighted.PictureBox.Location.X + 4, highlighted.PictureBox.Location.Y + 4, ((Control)sender).Location.X + 4, ((Control)sender).Location.Y + 4, Color.Purple);
                v.PreviousVertex = highlighted;
                highlighted.NextVertex = v;
                UpdateHighlighted(v);
                MainCanvas.Refresh();
            }
            else if (delete_vertex.Checked)
            {
                DeleteVertex(v);
                ReloadCanvas();
            }
            else if (move_vertex_edge.Checked)
            {
                MovingMode = 1;
                UpdateHighlighted(v);
                PrevoiusLocation = MainCanvas.PointToClient(Cursor.Position);
                timer = new Timer();
                timer.Interval = 10;
                timer.Tick += Timer_Tick;
                timer.Start();
            }
        }

        private void UpdateHighlighted(Vertex v)
        {
            if (highlighted != null) highlighted.PictureBox.BackColor = Color.Blue;
            highlighted = v;
            if(highlighted!=null) highlighted.PictureBox.BackColor = Color.Red;
        }

        
        public void DeleteVertex(Vertex v)
        {//Removes Vertex (or whole Polygons if vertices count<3)
            DeleteRelation(v);
            DeleteRelation(v.PreviousVertex);
            for (int i = 0; i < Polygons.Count; i++)
            {
                int ind = Polygons[i].IndexOf(v);
                if (ind!=-1)
                {
                    if (Polygons[i].Count <= 3)
                    {
                        foreach (var vertex in Polygons[i])
                        {
                            if (vertex == highlighted)
                            {
                                if (Polygons.Count > 1) UpdateHighlighted(Polygons[Polygons.Count - 2][0]);
                                else UpdateHighlighted(null);
                            }
                            vertex.PictureBox.Dispose();
                        }
                        Polygons.RemoveAt(i);
                    }
                    else
                    {
                        if (v == highlighted) UpdateHighlighted(v.PreviousVertex);
                        if(v.NextVertex!=null)v.NextVertex.PreviousVertex = v.PreviousVertex;
                        if(v.PreviousVertex!=null)v.PreviousVertex.NextVertex = v.NextVertex;
                        v.PictureBox.Dispose();
                        Polygons[i].RemoveAt(ind);
                    }
                    
                    v = null;
                    return;
                }
            }            
        }
        void DeleteRelation(Vertex v)
        {//Draw MainCanvas from Polygons list
            if (v!=null && v.Relation != null)
            {
                v.Relation.v.Relation.symbol.Dispose();
                v.Relation.v.Relation = null;
                v.Relation.symbol.Dispose();
                v.Relation = null;
            } 
        }
        
        private void ReloadCanvas()
        {
            MainCanvas.Image= new Bitmap(MainCanvas.Width, MainCanvas.Height);
            Vertex v;
            foreach (var list in Polygons)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    v = list[i];
                    DrawLine(v.PictureBox.Location.X + 4, v.PictureBox.Location.Y + 4, v.NextVertex.PictureBox.Location.X + 4, v.NextVertex.PictureBox.Location.Y + 4, Color.Purple);
                }
            }
        }
        private void MainCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (MovingMode>0)
            {
                timer.Stop();
                MovingMode = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var p in Polygons)
            {       
                foreach (var v in p)
                {
                    v.PictureBox.Dispose();
                    DeleteRelation(v);
                }
            }
            UpdateHighlighted(null);
            Polygons = new List<List<Vertex>>();
            FinishedAction = true;
            ReloadCanvas();
        }

        private void add_vertex_CheckedChanged(object sender, EventArgs e)
        {
            if (!add_vertex.Checked && !FinishedAction)
            {
                FinishedAction = true;
                foreach (var item in Polygons.Last())
                {
                    item.PictureBox.Dispose();
                    DeleteRelation(item);
                }
                Polygons.RemoveAt(Polygons.Count - 1);
                if (Polygons.Count == 0) UpdateHighlighted(null);
                else UpdateHighlighted(Polygons.Last().Last());
                ReloadCanvas();
            }
        }


        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (add_vertex.Checked)
            {
                FinishedAction = true;
                foreach (var item in Polygons.Last())
                {
                    item.PictureBox.Dispose();
                    DeleteRelation(item);
                }
                Polygons.RemoveAt(Polygons.Count - 1);
                if (Polygons.Count == 0) UpdateHighlighted(null);
                else UpdateHighlighted(Polygons.Last().Last());
            }
            ReloadCanvas();
            
        }

        private void parallel_CheckedChanged(object sender, EventArgs e)
        {
            if (!parallel.Checked)
            {
                if (highlighted != null)
                {
                    highlighted.NextVertex.PictureBox.BackColor = Color.Blue;
                    highlighted.PictureBox.BackColor = Color.Red;
                }
                FinishedAction = true;
            }
        }

        private void equall_CheckedChanged(object sender, EventArgs e)
        {
            if (!equall.Checked)
            {
                if (highlighted != null)
                {
                    highlighted.NextVertex.PictureBox.BackColor = Color.Blue;
                    highlighted.PictureBox.BackColor = Color.Red;
                }
                FinishedAction = true;
            }
        }
    }
}
