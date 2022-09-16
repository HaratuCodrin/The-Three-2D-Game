using Bravos.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bravos.Tools
{
    public class Sprite
    {
        private Bitmap SPRITE_SHEET = null;
        private Image[,] SpriteArray;

        public int w;
        public int h;

        private int numRows;
        private int numCols;

        private Bitmap subImage;


        public Sprite(Image file, int w, int h, int type)
        {
            this.w = w;
            this.h = h;
            SPRITE_SHEET = (Bitmap)file;

            numCols = SPRITE_SHEET.Width / w;
            numRows = SPRITE_SHEET.Height / h;

            switch (type)
            {
                case 1:
                    SpriteArray = new Image[numRows, numCols];
                    for (int i = 0; i < numRows; i++)
                    {
                        for (int j = 0; j < numCols; j++)
                            // SpriteArray[i, j] = cropImage(SPRITE_SHEET, new Rectangle(i * w, j * h, w, h));
                            SpriteArray[i, j] = cutImage(j, i);
                    }
                    break;
                case 2:
                    SpriteArray = new Image[numCols, numRows];
                    for (int j = 0; j < numCols; j++)
                        for (int i = 0; i < numRows; i++)
                             SpriteArray[j, i] = cutImage(j,i);
                          //  SpriteArray[j, i] = cropImage(i, j);
                    break;

            }
        }

        public Sprite(Image file, int w, int h, int type, int scale)
        {
            this.w = w;
            this.h = h;
            SPRITE_SHEET = (Bitmap)file;

            numCols = SPRITE_SHEET.Width / w;
            numRows = SPRITE_SHEET.Height / h;

            switch (type)
            {
                case 1:
                    SpriteArray = new Image[numRows, numCols];
                    for (int i = 0; i < numRows; i++)
                    {
                        for (int j = 0; j < numCols; j++)
                            // SpriteArray[i, j] = cropImage(SPRITE_SHEET, new Rectangle(i * w, j * h, w, h));
                            SpriteArray[i, j] = GetResizedBlock(cutImage(j, i),scale*w);
                    }
                    break;
                case 2:
                    SpriteArray = new Image[numCols, numRows];
                    for (int j = 0; j < numCols; j++)
                        for (int i = 0; i < numRows; i++)
                            // SpriteArray[j, i] = cropImage(SPRITE_SHEET, new Rectangle(i * w, j * h, w, h));
                            SpriteArray[j, i] = cropImage(i, j);
                    break;

            }
        }

        public Sprite(String file, int w, int h, int type)
        {
            this.w = w;
            this.h = h;
            SPRITE_SHEET = (Bitmap)Image.FromFile(file);

            numCols = SPRITE_SHEET.Width / w;
            numRows = SPRITE_SHEET.Height / h;
            
            switch(type)
            {
                case 1:
                    SpriteArray = new Image[numRows, numCols];
                    for (int i = 0; i < numRows; i++)
                        for (int j = 0; j < numCols; j++)
                            SpriteArray[i, j] = GetSubImage(i, j);
                    break;
                case 2:
                    SpriteArray = new Image[numCols, numRows];
                    for (int j = 0; j < numRows; j++)
                        for (int i = 0; i < numCols; i++)
                            SpriteArray[j, i] = GetSubImage(i, j);
                    break;
                
            }
        }


        public Image[,] GetResizedSpriteArray(int BlockSize)
        {
            Image[,] resized = new Image[numRows,numCols];

            for(int i = 0; i < numCols*numRows; i++)
            {
                resized[i / numCols, i % numCols] = GetResizedBlock(SpriteArray[i / numCols, i % numCols], BlockSize);
            }
            return resized;
        }

        public Image GetResizedBlock(Image image, int BlockSize)
        {
            var destRect = new Rectangle(0, 0, BlockSize, BlockSize);
            var destImage = new Bitmap(BlockSize, BlockSize);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return (Image)destImage;
        }
        private Bitmap GetSubImage(int x, int y)
        {
            return SPRITE_SHEET.Clone(new Rectangle(x * w, y * h, w, h), SPRITE_SHEET.PixelFormat);

        }

        public Image cutImage(int x, int y)
        {
            Rectangle cropRect = new Rectangle(x * w, y * h, w, h);
            Bitmap src = SPRITE_SHEET;
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
            using (Graphics graphics = Graphics.FromImage(target))
            {
                graphics.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
            }


            return target;
        }

        public static Image cutImage(Image img, int x, int y, int width, int height)
        {
            Rectangle cropRect = new Rectangle(x * width, y * height, width, height);
            Bitmap src = (Bitmap)img;
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
            using (Graphics graphics = Graphics.FromImage(target))
            {
                graphics.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
            }


            return target;
        }

        public Image cropImage(int x, int y)
        {
            Bitmap img = SPRITE_SHEET;
            Rectangle cropArea = new Rectangle(x * w, y * h, w, h);
            Bitmap bmp = new Bitmap(cropArea.Width, cropArea.Height);
            bmp.SetResolution(img.HorizontalResolution, img.VerticalResolution);
            using (Graphics gph = Graphics.FromImage(bmp))
            {
               // gph.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), cropArea, GraphicsUnit.Pixel);
                gph.DrawImage(img,-cropArea.X,-cropArea.Y);
            }
            return bmp;
        }

        public Image[] GetColumn(int column)
        {
            CustomArray<Image> imageArray = new CustomArray<Image>();
            return imageArray.GetColumn(SpriteArray, column);
        }

        public Image[] GetRow(int row)
        {
            CustomArray<Image> imageArray = new CustomArray<Image>();
            return imageArray.GetRow(SpriteArray, row);
        }

        public Image[,] GetSpriteArray()
        {
            return SpriteArray;
        }

        public Image GetSpriteSheet()
        {
            return (Image)SPRITE_SHEET;
        }

        public int GetNumRows()
        {
            return numRows;
        }

        public void SetNumRows(int numRows)
        {
            this.numRows = numRows;
        }

        public int GetNumCols()
        {
            return numCols;
        }

        public void SetNumCols(int numCols)
        {
            this.numCols = numCols;
        }

        public static Image[] GetRowFromSheet(Image image, int numCols, int cutWidth, int cutHeight) 
        {
            Image[] images = new Image[numCols];
            for(int i = 0; i < numCols; i++)
            {
                images[i] = cutImage(image, i, 0, cutWidth, cutHeight);
            }

            return images;
        }


    }
}
