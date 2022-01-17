namespace OrhoCommandPaint
{
    public partial class Form1 : Form
    {
        int? initGeneralX = null;
        int? initGeneralY = null;
        bool startPaint = false;

        List<Line> lines = new List<Line>();

        Bitmap bitmap;
        public Form1()
        {
            InitializeComponent();
        }






        private void removePreviewLines()
        {
            foreach (var item in lines.Where(l => l.preview == true).ToList())
            {
                lines.Remove(item);
            }
        }
        private Line drawOrthoLine(int startX, int startY, int endX, int endY, Color color)
        {
            int diffY = endY - startY;
            int diffX = endX - startX;
            double ratio = 0;

            if (Math.Abs(diffY) > Math.Abs(diffX))
            {
                ratio = Math.Abs((double)diffX / diffY);
                //اگر خط عمود رو به پایین باشد
                if (ratio < 0.35)
                {
                    endX = startX;
                }
                else
                {
                    endX = startX + (diffX < 0 ? Math.Abs(diffY) * -1 : Math.Abs(diffY));
                    endY = startY + diffY;
                }
            }
            else
            {
                ratio = Math.Abs((double)diffY / diffX);
                if (ratio < 0.35)
                {
                    endY = startY;
                }
                else
                {
                    endX = startX + (diffX < 0 ? Math.Abs(diffY) * -1 : Math.Abs(diffY));
                    endY = startY + diffY;
                }
            }

            return new Line
            {
                startX = startX,
                startY = startY,
                endX = endX,
                endY = endY,
                color = color,
            };

        }

        private void UpdateDrawPanel()
        {
            bitmap = new Bitmap(drawPanel.Width, drawPanel.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                foreach (var line in lines)
                {
                    Pen p = new Pen(line.color, 4);
                    g.DrawLine(p, new Point(line.startX, line.startY), new Point(line.endX, line.endY));
                }
            }
            drawPanel.Image = bitmap;
        }

        private void drawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (startPaint)
            {
                int startX;
                int startY;
                int endY = e.Y;
                int endX = e.X;
                startX = initGeneralX.Value;
                startY = initGeneralY.Value;

                var line = drawOrthoLine(startX, startY, endX, endY, Color.Black);


                initGeneralX = line.endX;
                initGeneralY = line.endY;


                lines.Add(line);
            }
            else
            {
                startPaint = true;

                initGeneralX = e.X;
                initGeneralY = e.Y;
            }
            UpdateDrawPanel();
        }

        private void drawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (startPaint)
            {
                removePreviewLines();
                int startX;
                int startY;
                int endY = e.Y;
                int endX = e.X;
                startX = initGeneralX.Value;
                startY = initGeneralY.Value;

                var line = drawOrthoLine(startX, startY, endX, endY, Color.Blue);
                line.preview = true;
                lines.Add(line);
            }
            UpdateDrawPanel();
        }
    }
    public class Line
    {
        public int startX;
        public int startY;
        public int endX;
        public int endY;
        public Color color;
        public bool preview = false;
    }
}