using Bravos.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bravos.Tiles
{
    public class TileMap
    {
        // type
        public int type { get; set; }
        public const int NORMAL = 0;
        public const int BLOCKED = 1;
        private SolidBrush brush;


        // position
        private Vector Position;

        // bounds
        private int Xmin;
        private int Xmax;
        private int Ymin;
        private int Ymax;

        // for smooth scrolling
        private double Tween;

        // Hashmap for Collisions
        public Dictionary<String, int> blocks = null;

        //map
        private int[,] Map;
        private int TileSize;
        private int Scale;
        private int BlockSize;

        private int NumRows;
        private int NumCols;
        private int Width;
        private int Height;
        String data;

        // tileset
        private Sprite TileSet;
        private Tile[,] Tiles;

        // drawing
        private int RowOffset;
        private int ColOffset;
        private int NumRowsToDraw;
        private int NumColsToDraw;

        public TileManager tm { get; set; }

        public TileMap(int TileSize, int NumCols, int NumRows, Image TileSet, String data, int type, TileManager tm)
        {
            this.tm = tm;
            Tween = 0.5;
            Scale = 2;
            brush = new SolidBrush(Color.Green);
            this.type = type;
            this.Position = new Vector(0, 0);
            this.TileSize = TileSize;
            this.TileSet = new Sprite(TileSet, TileSize, TileSize, 1, Scale);
            this.NumCols = NumCols;
            this.NumRows = NumRows;
            this.data = data;
            this.BlockSize = TileSize * Scale;
            this.Width = NumCols * BlockSize;
            this.Height = NumRows * BlockSize;

            Xmin = GamePanel.WIDTH * GamePanel.SCALE - Width;
            Xmax = 0;
            Ymin = GamePanel.HEIGHT * GamePanel.SCALE - Height;
            Ymax = 0;


            NumRowsToDraw = GamePanel.HEIGHT * GamePanel.SCALE / BlockSize + 2;
            NumColsToDraw = GamePanel.WIDTH * GamePanel.SCALE / BlockSize + 2;
            if (type == 1) blocks = new Dictionary<string, int>();
            Map = new int[NumRows, NumCols];

            String[] block = data.Split(',');
            for (int i = 0; i < (NumRows * NumCols); i++)
            {
                int temp = Int32.Parse(block[i].Replace("\\s+", ""));
                if (temp != 0)
                    Map[i / NumCols, i % NumCols] = temp - 1;
                else
                    Map[i / NumCols, i % NumCols] = 0;

                if (type == 1 && temp != 0)
                    //      blocks.Add((int)(i % NumCols) + "," + (int)(i / NumRows), temp - 1);
                    blocks.Add((int)((i % NumCols) + (i % NumCols) * BlockSize) + "," + (int)((i / NumRows) + (i / NumCols) * BlockSize), temp);

            }

            if (type == 1) tm.BlockMap = Map;

            LoadTiles();


        }
        public TileMap(int TileSize, int NumCols, int NumRows, Image TileSet, String data, int type)
        {
            Tween = 0.7;
            Scale = 2;
            this.type = type;
            this.Position = new Vector(0, 0);
            this.TileSize = TileSize;
            this.TileSet = new Sprite(TileSet, TileSize, TileSize, 1, Scale);
            this.NumCols = NumCols;
            this.NumRows = NumRows;
            this.data = data;
            this.BlockSize = TileSize * Scale;
            this.Width = NumCols * BlockSize;
            this.Height = NumRows * BlockSize;

            Xmin = GamePanel.WIDTH * GamePanel.SCALE - Width;
            Xmax = 0;
            Ymin = GamePanel.HEIGHT * GamePanel.SCALE - Height;
            Ymax = 0;


            NumRowsToDraw = GamePanel.HEIGHT * GamePanel.SCALE / BlockSize + 2;
            NumColsToDraw = GamePanel.WIDTH * GamePanel.SCALE / BlockSize + 2;
            if(type == 1) blocks = new Dictionary<string, int>();
            Map = new int[NumRows, NumCols];

            String[] block = data.Split(',');
            for (int i = 0; i < (NumRows * NumCols); i++)
            {
                int temp = Int32.Parse(block[i].Replace("\\s+", ""));
                if (temp != 0)
                    Map[i / NumCols, i % NumCols] = temp - 1;
                else
                    Map[i / NumCols, i % NumCols] = 0;

                if (type == 1 && temp != 0)
              //      blocks.Add((int)(i % NumCols) + "," + (int)(i / NumRows), temp - 1);
                blocks.Add((int)((i % NumCols) + (i % NumCols) * BlockSize) + "," + (int)((i / NumRows) + (i / NumCols) * BlockSize), temp);

            }

            LoadTiles();

          
        }

        public TileMap(int TileSize, int NumCols, int NumRows, Image TileSet, String data, int type, int scale)
        {
            Tween = 1;
            this.Scale = scale;
            this.type = type;
            this.Position = new Vector(0, 0);
            this.TileSize = TileSize;
            this.TileSet = new Sprite(TileSet, TileSize, TileSize, 1, Scale);
            this.NumCols = NumCols;
            this.NumRows = NumRows;
            this.data = data;
            this.BlockSize = TileSize * Scale;
            this.Width = NumCols * BlockSize;
            this.Height = NumRows * BlockSize;

            Xmin = GamePanel.WIDTH * GamePanel.SCALE - Width;
            Xmax = 0;
            Ymin = GamePanel.HEIGHT * GamePanel.SCALE - Height;
            Ymax = 0;

            NumRowsToDraw = GamePanel.HEIGHT * GamePanel.SCALE / BlockSize + 2;
            NumColsToDraw = GamePanel.WIDTH * GamePanel.SCALE / BlockSize + 2;
            blocks = new Dictionary<string, int>();
            Map = new int[NumRows, NumCols];

            String[] block = data.Split(',');
            for (int i = 0; i < (NumRows * NumCols); i++)
            {
                int temp = Int32.Parse(block[i].Replace("\\s+", ""));
                if (temp != 0)
                    Map[i / NumCols, i % NumCols] = temp - 1;
                else
                    Map[i / NumCols, i % NumCols] = 0;

                if (type == 1 && temp != 0)
                    blocks.Add((int)((i % NumCols) + (i/NumCols) * BlockSize) + "," + (int)((i / NumRows) + (i%NumCols)*BlockSize), temp);
            }

            LoadTiles();

        

        }

        public void LoadTiles()
        {
            Tiles = new Tile[TileSet.GetNumRows(), TileSet.GetNumCols()];
            for (int i = 0; i < TileSet.GetNumRows(); i++)
            {
                for (int j = 0; j < TileSet.GetNumCols(); j++)
                {
                    Tiles[i, j] = new Tile(TileSet.GetSpriteArray()[i, j]);
                }
            }

            //for (int i = 0; i < tileset.getnumcols() * tileset.getnumrows(); i++)
            //{
            //    int k = (int)(i / tileset.getnumcols());
            //    int j = (int)(i % tileset.getnumcols());
            //    tiles[k, j] = new tile(tileset.getspritearray()[k, j]);
            //}

        }



        public void SetPosition(double x, double y)
        {
            this.Position.X += (x - this.Position.X) * Tween;
            this.Position.Y += (y - this.Position.Y) * Tween;

            fixBounds();

            ColOffset = (int)-this.Position.X / BlockSize;
            RowOffset = (int)-this.Position.Y / BlockSize;

        }

        public Vector GetPosition()
        {
            return this.Position;
        }

        public void SetStartPosition(Vector position)
        {
            this.Position = position;
        }

        public Dictionary<String, int> GetBlocks()
        {
            return blocks;
        }

        private void fixBounds()
        {
            if (Position.X < Xmin) Position.X = Xmin;
            if (Position.Y < Ymin) Position.Y = Ymin;
            if (Position.X > Xmax) Position.X = Xmax;
            if (Position.Y > Ymax) Position.Y = Ymax;
        }

        

        public void Render(Graphics g)
        {
            for(int row = RowOffset; row < RowOffset + NumRowsToDraw; row++)
            {
                if (row >= NumRows) break;
                for(int col = ColOffset; col < ColOffset+NumColsToDraw;col++)
                {
                    if (col >= NumCols) break;
                   if (Map[row,col] == 0) continue;

                    int value = Map[row, col];
                    int r = value / TileSet.GetNumCols();
                    int c = value % TileSet.GetNumCols();

                    g.DrawImage(Tiles[r,c].GetImage(),
                               (int)Position.X + col * BlockSize,
                                (int)Position.Y + row * BlockSize);

               //     if (type == 1) g.FillRectangle(brush, new Rectangle((int)Position.X + col * BlockSize,
               //                  (int)Position.Y + row * BlockSize, Tiles[r, c].GetImage().Width, Tiles[r, c].GetImage().Height));

                }
            }
           
          //  g.DrawString(Position.X.ToString() + " " + Position.Y.ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), 30, 30);

        }


    }
}
