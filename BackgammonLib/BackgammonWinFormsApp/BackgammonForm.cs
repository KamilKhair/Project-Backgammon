using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using BackgammonLib;
using BackgammonWinFormsApp.Properties;

namespace BackgammonWinFormsApp
{
    public partial class BackgammonForm : Form
    {
        private readonly PictureBox[] _triangles = new PictureBox[28];
        private readonly Backgammon _game;
        private bool _firstClick;
        private int _fromTriangle;
        private bool _fromDeadBar;
        public BackgammonForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
            _game = new Backgammon();
            _turnLabel.Text = Resources.BackgammonForm_BackgammonForm_Turn__Black;
            _firstClick = true;
            _game.GameFinished += GameFinishedListener;
            _game.NoAvailableMoves += NoAvailableMovesListener;
        }

        private void DrawBoard()
        {
            for (var i = 0; i < 24; ++i)
            {

                var triangle = _game.Board.AllTriangles.ElementAt(i);
                DrawTriangle(triangle, i + 1);
            }

            var blackDeadBar = _game.BlackDeadCheckersBar;
            DrawBar(blackDeadBar, 25);
            var blackOutsideBar = _game.BlackOutSideCheckersBar;
            DrawBar(blackOutsideBar, 26);
            var whiteDeadBar = _game.WhiteDeadCheckersBar;
            DrawBar(whiteDeadBar, 0);
            var whiteOutsideBar = _game.WhiteOutSideCheckersBar;
            DrawBar(whiteOutsideBar, 27);
            if (_game.Turn == CheckerType.Black)
            {
                _turnLabel.Text = Resources.BackgammonForm_BackgammonForm_Turn__Black;
                _turnLabel.ForeColor = Color.Black;
            }
            else
            {
                _turnLabel.Text = Resources.BackgammonForm_BackgammonForm_Turn__White;
                _turnLabel.ForeColor = Color.White;
            }

        }

        private void DrawBar(IEnumerable<Checker> bar, int pixtureBox)
        {
            var graphics = _triangles[pixtureBox].CreateGraphics();
            var start = 0;
            int ratio = 30;
            var enumerable = bar as Checker[] ?? bar.ToArray();
            if (enumerable.Length != 0 && enumerable.Length * 30 > 223)
            {
                ratio = 223 / enumerable.Length;
            }
            for (var i = 0; i < enumerable.Length; ++i)
            {
                var brush = enumerable.ElementAt(i).CheckerType == CheckerType.Black ? new HatchBrush(HatchStyle.DarkVertical, Color.Black) : new HatchBrush(HatchStyle.DarkVertical, Color.White);
                if (pixtureBox < 12)
                {
                    graphics.FillEllipse(brush, 5, 193 - start, 30, 30);
                }
                else
                {
                    graphics.FillEllipse(brush, 5, 0 + start, 30, 30);
                }
                start += ratio;
            }
        }

        private void DrawTriangle(Triangle triangle, int pixtureBox)
        {
            var graphics = _triangles[pixtureBox].CreateGraphics();
            var start = 0;
            int ratio = 30;
            if (triangle.Checkers.Count() != 0 && triangle.Checkers.Count() * 30 > 223)
            {
                ratio = 223/triangle.Checkers.Count();
            }
            for (var i = 0; i < triangle.Checkers.Count(); ++i)
            {
                var brush = triangle.TriangleType == CheckerType.Black ? new HatchBrush(HatchStyle.DarkVertical, Color.Black) : new HatchBrush(HatchStyle.DarkVertical, Color.White);
                if (pixtureBox < 13)
                {
                    graphics.FillEllipse(brush, 10, 193 - start, 30, 30);
                }
                else
                {
                    graphics.FillEllipse(brush, 10, 0 + start, 30, 30);
                }
                start += ratio;
            }
        }

        private void ClearBoard()
        {
            for (int i = 0; i < 28; i++)
            {
                _triangles[i].CreateGraphics().Clear(Color.Aquamarine);
            }
        }

        private void InitializeCustomComponents()
        {
            _triangles[0] = pictureBox0;
            _triangles[1] = pictureBox1;
            _triangles[2] = pictureBox2;
            _triangles[3] = pictureBox3;
            _triangles[4] = pictureBox4;
            _triangles[5] = pictureBox5;
            _triangles[6] = pictureBox6;
            _triangles[7] = pictureBox7;
            _triangles[8] = pictureBox8;
            _triangles[9] = pictureBox9;
            _triangles[10] = pictureBox10;
            _triangles[11] = pictureBox11;
            _triangles[12] = pictureBox12;
            _triangles[13] = pictureBox13;
            _triangles[14] = pictureBox14;
            _triangles[15] = pictureBox15;
            _triangles[16] = pictureBox16;
            _triangles[17] = pictureBox17;
            _triangles[18] = pictureBox18;
            _triangles[19] = pictureBox19;
            _triangles[20] = pictureBox20;
            _triangles[21] = pictureBox21;
            _triangles[22] = pictureBox22;
            _triangles[23] = pictureBox23;
            _triangles[24] = pictureBox24;
            _triangles[25] = pictureBox25;
            _triangles[26] = pictureBox26;
            _triangles[27] = pictureBox27;

            for (var i = 0; i < 28; ++i)
            {
                _triangles[i].BackColor = Color.Aquamarine;
                _triangles[i].Click += pictureBox_Click;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.BackgammonForm_aboutToolStripMenuItem_Click_By__Kamil_Khair);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void RollButton_Click(object sender, EventArgs e)
        {
            if (_game.Turn == CheckerType.Black)
            {
                _game.BlackPlayerRoll();
            }
            else
            {
                _game.WhitePlayerRoll();
            }
            switch (_game.Dice.FirstCube)
            {
                case 1:
                    _firstCubePictueBox.Image = Resources._1;
                    break;
                case 2:
                    _firstCubePictueBox.Image = Resources._2;
                    break;
                case 3:
                    _firstCubePictueBox.Image = Resources._3;
                    break;
                case 4:
                    _firstCubePictueBox.Image = Resources._4;
                    break;
                case 5:
                    _firstCubePictueBox.Image = Resources._5;
                    break;
                case 6:
                    _firstCubePictueBox.Image = Resources._6;
                    break;
                default:
                    _firstCubePictueBox.Image = Resources.x;
                    break;
            }
            switch (_game.Dice.SecondCube)
            {
                case 1:
                    _secondCubePictueBox.Image = Resources._1;
                    break;
                case 2:
                    _secondCubePictueBox.Image = Resources._2;
                    break;
                case 3:
                    _secondCubePictueBox.Image = Resources._3;
                    break;
                case 4:
                    _secondCubePictueBox.Image = Resources._4;
                    break;
                case 5:
                    _secondCubePictueBox.Image = Resources._5;
                    break;
                case 6:
                    _secondCubePictueBox.Image = Resources._6;
                    break;
                default:
                    _secondCubePictueBox.Image = Resources.x;
                    break;
            }
            DrawBoard();
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            var triangle = sender as PictureBox;
            var graphics = triangle?.CreateGraphics();
            var name = triangle?.Name;
            int index = 0;
            if (name != null && !int.TryParse(name[name.Length - 2].ToString() + name[name.Length - 1].ToString(), out index))
            {
                index = int.Parse(name[name.Length - 1].ToString());
            }
            if (_firstClick)
            {
                if (_game.Turn == CheckerType.Black)
                {
                    if (index == 0)
                    {
                        return;
                    }
                    else if (index == 25)
                    {
                        if (!_game.BlackDeadCheckersBar.Any())
                        {
                            return;
                        }
                    }
                    else if (index != 0 && index != 26 && index != 27 &&
                             (_game.Board.AllTriangles.ElementAt(index - 1).TriangleType == CheckerType.White ||
                             _game.Board.AllTriangles.ElementAt(index - 1).TriangleType == CheckerType.None))
                    {
                        return;
                    }
                }
                else
                {
                    if (index == 25)
                    {
                        return;
                    }
                    else if (index == 0)
                    {
                        if (!_game.WhiteDeadCheckersBar.Any())
                        {
                            return;
                        }
                    }
                    else if (index != 25 && index != 26 && index != 27 &&
                             (_game.Board.AllTriangles.ElementAt(index - 1).TriangleType == CheckerType.Black ||
                             _game.Board.AllTriangles.ElementAt(index - 1).TriangleType == CheckerType.None))
                    {
                        return;
                    }
                }
            }
            if (_game.Turn == CheckerType.Black)
            {
                if (_firstClick)
                {
                    if (index == 25)
                    {
                        var pen = new Pen(Color.Red, 5);
                        graphics?.DrawRectangle(pen, 0, 0, 42, 223);
                    }
                    else if (index != 26 && index != 27 && index != 0)
                    {
                        var pen = new Pen(Color.Red, 5);
                        graphics?.DrawRectangle(pen, 0, 0, 50, 223);
                    }
                    var firstchoice = index - _game.Dice.FirstCube;
                    int secondChoice = index - _game.Dice.SecondCube;
                    if (firstchoice >= 1 && _game.Dice.FirstCube > 0 && firstchoice <= 24 &&
                        (_game.Board.AllTriangles.ElementAt(firstchoice - 1).TriangleType == CheckerType.Black ||
                        _game.Board.AllTriangles.ElementAt(firstchoice - 1).TriangleType == CheckerType.None ||
                        (_game.Board.AllTriangles.ElementAt(firstchoice - 1).TriangleType == CheckerType.White &&
                        _game.Board.AllTriangles.ElementAt(firstchoice - 1).Checkers.Count() == 1)))
                    {
                        var graphics2 = _triangles[firstchoice].CreateGraphics();
                        var pen2 = new Pen(Color.Green, 5);
                        graphics2.DrawRectangle(pen2, 0, 0, 50, 223);
                    }
                    if (secondChoice >= 1 && _game.Dice.SecondCube > 0 && secondChoice <= 24 &&
                        (_game.Board.AllTriangles.ElementAt(secondChoice - 1).TriangleType == CheckerType.Black ||
                        _game.Board.AllTriangles.ElementAt(secondChoice - 1).TriangleType == CheckerType.None ||
                        (_game.Board.AllTriangles.ElementAt(secondChoice - 1).TriangleType == CheckerType.White &&
                        _game.Board.AllTriangles.ElementAt(secondChoice - 1).Checkers.Count() == 1)))
                    {
                        var graphics2 = _triangles[secondChoice].CreateGraphics();
                        var pen3 = new Pen(Color.Green, 5);
                        graphics2.DrawRectangle(pen3, 0, 0, 50, 223);
                    }
                    _fromTriangle = index;
                    if (index == 25)
                    {
                        _fromDeadBar = true;
                    }
                    _firstClick = false;
                }
                else
                {
                    if (_fromDeadBar)
                    {
                        _game.MoveBlackFromDeadBar(index);
                        _fromDeadBar = false;
                    }
                    else
                    {
                        if (index == 26)
                        {
                            if (_fromTriangle <= _game.Dice.FirstCube)
                            {
                                _game.MoveBlack(_fromTriangle, _game.Dice.FirstCube);
                            }
                            else if (_fromTriangle <= _game.Dice.SecondCube)
                            {
                                _game.MoveBlack(_fromTriangle, _game.Dice.SecondCube);
                            }
                        }
                        else
                        {
                            _game.MoveBlack(_fromTriangle, _fromTriangle - index);
                        }
                    }
                    ClearBoard();
                    DrawBoard();
                    _firstClick = true;
                }
            }
            else if (_game.Turn == CheckerType.White)
            {
                if (_firstClick)
                {
                    if (index == 0)
                    {
                        var pen = new Pen(Color.Red, 5);
                        graphics?.DrawRectangle(pen, 0, 0, 42, 223);
                    }
                    else if(index != 26 && index != 27 && index != 25)
                    {
                        var pen = new Pen(Color.Red, 5);
                        graphics?.DrawRectangle(pen, 0, 0, 50, 223);
                    }
                    var firstchoice = index + _game.Dice.FirstCube;
                    int secondChoice = index + _game.Dice.SecondCube;
                    if (firstchoice <= 24 && _game.Dice.FirstCube > 0 && firstchoice >= 1 &&
                        (_game.Board.AllTriangles.ElementAt(firstchoice - 1).TriangleType == CheckerType.White ||
                        _game.Board.AllTriangles.ElementAt(firstchoice - 1).TriangleType == CheckerType.None ||
                        (_game.Board.AllTriangles.ElementAt(firstchoice - 1).TriangleType == CheckerType.Black &&
                        _game.Board.AllTriangles.ElementAt(firstchoice - 1).Checkers.Count() == 1)))
                    {
                        var graphics2 = _triangles[firstchoice].CreateGraphics();
                        var pen2 = new Pen(Color.Green, 5);
                        graphics2.DrawRectangle(pen2, 0, 0, 50, 223);
                    }
                    if (secondChoice <= 24 && _game.Dice.SecondCube > 0 && secondChoice >= 1 &&
                        (_game.Board.AllTriangles.ElementAt(secondChoice - 1).TriangleType == CheckerType.White ||
                        _game.Board.AllTriangles.ElementAt(secondChoice - 1).TriangleType == CheckerType.None ||
                        (_game.Board.AllTriangles.ElementAt(secondChoice - 1).TriangleType == CheckerType.Black &&
                        _game.Board.AllTriangles.ElementAt(secondChoice - 1).Checkers.Count() == 1)))
                    {
                        var graphics2 = _triangles[secondChoice].CreateGraphics();
                        var pen3 = new Pen(Color.Green, 5);
                        graphics2.DrawRectangle(pen3, 0, 0, 50, 223);
                    }
                    _fromTriangle = index;
                    if (index == 0)
                    {
                        _fromDeadBar = true;
                    }
                    _firstClick = false;
                }
                else
                {
                    if (_fromDeadBar)
                    {
                        _game.MoveWhiteFromDeadBar(index);
                        _fromDeadBar = false;
                    }
                    else
                    {
                        if (index == 27)
                        {
                            if (_fromTriangle + _game.Dice.FirstCube >= 25)
                            {
                                _game.MoveWhite(_fromTriangle, _game.Dice.FirstCube);
                            }
                            else if (_fromTriangle + _game.Dice.SecondCube >= 25)
                            {
                                _game.MoveWhite(_fromTriangle, _game.Dice.SecondCube);
                            }
                        }
                        else
                        {
                            _game.MoveWhite(_fromTriangle, index - _fromTriangle);
                        }
                    }
                    ClearBoard();
                    DrawBoard();
                    _firstClick = true;
                }
            }
        }

        private void GameFinishedListener(object o, EventArgs e)
        {
            var winner = _game.Winner == CheckerType.Black ? "Black Player" : "White Player";
            var dialogResult = MessageBox.Show($"The winner is {winner}!, Start a new game?", @"Game Finished!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Restart();
            }
            else if (dialogResult == DialogResult.No)
            {
                Application.Exit();
            }
        }

        private void NoAvailableMovesListener(object sender, EventArgs e)
        {
            MessageBox.Show(@"No Available Moves !");
        }
    }
}
