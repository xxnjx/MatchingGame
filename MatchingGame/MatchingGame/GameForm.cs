using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class GameForm : Form
    {
        Timer playTimer = new Timer();
        Bitmap backImage;
        Bitmap[] cardImages;
        Bitmap[] cardImagesDarken;
        Button first = null, second = null;

        public GameForm()
        {
            InitializeComponent();
            backImage = new Bitmap("../../Resources/Back.png");
            loadCardImages();
            initTable();
            playTimer.Interval = 300;
            playTimer.Tick += playTimer_Tick;
        }

        private void loadCardImages()
        {
            cardImages = new Bitmap[10];
            Size newSize = new Size(96, 128);

            for (int index = 0; index < cardImages.Length; index++)
            {
                string card = Model.getCardName(index);
                string filePath = "../../Resources/" + card + ".png";

                if(File.Exists(filePath))
                {
                    Image originalImage = Image.FromFile(filePath);
                    cardImages[index] = ResizeImage(originalImage, newSize);
                }
            }

            cardImagesDarken = new Bitmap[10];
            for(int index = 0; index < 10; index++)
            {
                cardImagesDarken[index] = DarkenImage(cardImages[index]);
            }
        }

        private Bitmap ResizeImage(Image originalImage, Size newSize)
        {
            Bitmap resizedImage = new Bitmap(newSize.Width, newSize.Height);

            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalImage, 0, 0, newSize.Width, newSize.Height);
            }

            return resizedImage;
        }

        private Bitmap DarkenImage(Bitmap original)
        {
            Bitmap darkImage = (Bitmap)original.Clone();

            using (Graphics g = Graphics.FromImage(darkImage))
            {
                ColorMatrix matrix = new ColorMatrix
                {
                    Matrix00 = 0.5f,
                    Matrix11 = 0.5f,
                    Matrix22 = 0.5f,
                };
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix);
                g.DrawImage(darkImage, new Rectangle(0, 0, darkImage.Width, darkImage.Height),
                    0, 0, darkImage.Width, darkImage.Height, GraphicsUnit.Pixel, attributes);
            }
            return darkImage;
        }

        private void initTable()
        {
            Model.init();
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                Button btn = c as Button;
                if(btn != null)
                {
                    btn.Image = backImage;
                    CardTag cardTag = Model.cardTags[tableLayoutPanel1.Controls.IndexOf(c)];
                    btn.Tag = cardTag;
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            CardTag tag = btn.Tag as CardTag;

            if(Model.isClickable(btn, first))
            {
                return;
            }

            if(tag != null && !tag.isCorrect)
            {
                btn.Image = cardImages[tag.cardId % 10];
                Model.increaseClickCount();
                labelClickCount.Text = "Click Count : " + Model.getClickCount();

                if(first == null)
                {
                    first = btn;
                    return;
                }

                second = btn;
                CardTag firstTag = first.Tag as CardTag;
                CardTag secondTag = second.Tag as CardTag;

                if (firstTag.cardId % 10 == secondTag.cardId % 10)
                {
                    firstTag.markAsCorrect();
                    secondTag.markAsCorrect();
                    DarkenCard(first);
                    DarkenCard(second);
                    first = null;
                    second = null;

                    Model.increaseCorrectCount();
                    labelCorrect.Text = "Correct# : " + Model.getCorrectCount();

                    if (Model.getCorrectCount() >= 10)
                    {
                        DialogResult res = MessageBox.Show("모두 맞췄습니다.", "Finish!!", MessageBoxButtons.YesNo);
                        if (res == DialogResult.Yes)
                        {
                            initTable();
                            labelClickCount.Text = "총클릭수";
                            labelCorrect.Text = "Correct#";
                            labelWrong.Text = "Wrong#";
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }
                else
                {
                    Model.increaseWrongCount();
                    labelWrong.Text = "Wrong# : " + Model.getWrongCount();
                    playTimer.Start();
                }
            }
        }

        private void playTimer_Tick(object sender, EventArgs e)
        {
            playTimer.Stop();
            if (first != null) first.Image = backImage;
            if (second != null) second.Image = backImage;
            first = null;
            second = null;
        }

        private void DarkenCard(Button btn)
        {
            CardTag tag = btn.Tag as CardTag;
            if (tag != null)
            {
                int imageIndex = tag.cardId % 10;
                btn.Image = cardImagesDarken[imageIndex];
            }
        }

    }
}
