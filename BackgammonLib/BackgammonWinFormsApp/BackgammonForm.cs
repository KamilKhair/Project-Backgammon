using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly IBackgammon _game;
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
                DrawTriangle(_game.Board.Triangles.ElementAt(i), i + 1);
            }

            DrawBar(_game.BlackDeadCheckersBar, 25);
            DrawBar(_game.BlackOutSideCheckersBar, 26);
            DrawBar(_game.WhiteDeadCheckersBar, 0);
            DrawBar(_game.WhiteOutSideCheckersBar, 27);

            DrawTurn();
        }

        private void DrawTurn()
        {
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

        private void DrawBar(IEnumerable<IChecker> bar, int pixtureBox)
        {
            var graphics = _triangles[pixtureBox].CreateGraphics();
            var start = 0;
            var ratio = 30;
            var enumerable = bar.ToArray();
            if (enumerable.Length != 0 && enumerable.Length * 30 > 223)
            {
                ratio = 223 / enumerable.Length;
            }
            for (var i = 0; i < enumerable.Length; ++i)
            {
                start = DrawChecker(pixtureBox, enumerable, graphics, ratio, start, i);
            }
        }

        private static int DrawChecker(int pixtureBox, IEnumerable<IChecker> enumerable, Graphics graphics, int ratio, int start, int i)
        {
            var brush = enumerable.ElementAt(i).Type == CheckerType.Black
                ? new HatchBrush(HatchStyle.DarkVertical, Color.Black)
                : new HatchBrush(HatchStyle.DarkVertical, Color.White);
            if (pixtureBox < 13)
            {
                graphics.FillEllipse(brush, 5, 193 - start, 30, 30);
            }
            else
            {
                graphics.FillEllipse(brush, 5, 0 + start, 30, 30);
            }
            start += ratio;
            return start;
        }

        private void DrawTriangle(ITriangle triangle, int pixtureBox)
        {
            var graphics = _triangles[pixtureBox].CreateGraphics();
            var start = 0;
            var ratio = 30;
            if (triangle.CheckersStack.Count() != 0 && triangle.CheckersStack.Count() * 30 > 223)
            {
                ratio = 223/triangle.CheckersStack.Count();
            }
            for (var i = 0; i < triangle.CheckersStack.Count(); ++i)
            {
                start = DrawChecker(pixtureBox, triangle.CheckersStack, graphics, ratio, start, i);
            }
        }

        private void ClearBoard()
        {
            for (var i = 0; i < 28; i++)
            {
                _triangles[i].CreateGraphics().Clear(SystemColors.ActiveCaption);
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
                _triangles[i].BackColor = SystemColors.ActiveCaption;
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
            Roll();
            DisplayFirstCube();
            DisplaySecondCube();
            DrawBoard();
        }

        private void DisplaySecondCube()
        {
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
        }

        private void DisplayFirstCube()
        {
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
        }

        private void Roll()
        {
            if (_game.Turn == CheckerType.Black)
            {
                _game.BlackPlayerRoll();
            }
            else
            {
                _game.WhitePlayerRoll();
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            var triangle = sender as PictureBox;
            var graphics = triangle?.CreateGraphics();
            var name = triangle?.Name;
            var index = GetIndexOfTheCurrentClickedOnPtictureBox(name);
            if (FirstClickIsIllegal(index))
            {
                return;
            }
            if (_game.Turn == CheckerType.Black)
            {
                DrawBlackBoundingRectangleOrPerformBlackMove(index, graphics);
            }
            else
            {
                DrawWhiteBoundingRectangleOrPerformWhiteMove(index, graphics);
            }
        }

        private void DrawWhiteBoundingRectangleOrPerformWhiteMove(int index, Graphics graphics)
        {
            if (_firstClick)
            {
                DrawWhiteBoundingRectangleAndAvailavleMoves(index, graphics);
            }
            else
            {
                PerformWhiteMove(index);
                ClearBoard();
                DrawBoard();
                _firstClick = true;
            }
        }

        private void DrawBlackBoundingRectangleOrPerformBlackMove(int index, Graphics graphics)
        {
            if (_firstClick)
            {
                DrawBlackBoundingRectangleAndAvailavleMoves(index, graphics);
            }
            else
            {
                PerformBlackMove(index);
                ClearBoard();
                DrawBoard();
                _firstClick = true;
            }
        }

        private bool FirstClickIsIllegal(int index)
        {
            if (_firstClick)
            {
                if (_game.Turn == CheckerType.Black)
                {
                    if (DrawingBoundingTriangleOnBlackTriangleIsIllegal(index))
                    {
                        return true;
                    }
                }
                else
                {
                    if (DrawingBoundingTriangleOnWhiteTriangleIsIllegal(index))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void PerformWhiteMove(int index)
        {
            if (_fromDeadBar)
            {
                _game.MoveWhiteFromDeadBar(index);
                _fromDeadBar = false;
            }
            else
            {
                MoveWhite(index);
            }
        }

        private void MoveWhite(int index)
        {
            if (index == 27)
            {
                MoveWhiteToOutSide();
            }
            else
            {
                _game.MoveWhite(_fromTriangle, index - _fromTriangle);
            }
        }

        private void MoveWhiteToOutSide()
        {
            if (_fromTriangle == 25 - _game.Dice.FirstCube)
            {
                _game.MoveWhite(_fromTriangle, _game.Dice.FirstCube);
            }
            else if (_fromTriangle == 25 - _game.Dice.SecondCube)
            {
                _game.MoveWhite(_fromTriangle, _game.Dice.SecondCube);
            }
            else if (_fromTriangle + _game.Dice.FirstCube >= 25)
            {
                _game.MoveWhite(_fromTriangle, _game.Dice.FirstCube);
            }
            else if (_fromTriangle + _game.Dice.SecondCube >= 25)
            {
                _game.MoveWhite(_fromTriangle, _game.Dice.SecondCube);
            }
        }

        private void DrawWhiteBoundingRectangleAndAvailavleMoves(int index, Graphics graphics)
        {
            DrawWhiteBoundingRectangle(index, graphics);
            var firstchoice = index + _game.Dice.FirstCube;
            
            TryDrawFirstChoiceOfWhiteBoundingRectangle(firstchoice);

            TryDrawSecondChoiceOfWhiteBoundingRectangle(index);
            _fromTriangle = index;
            if (index == 0)
            {
                _fromDeadBar = true;
            }
            _firstClick = false;
        }

        private void TryDrawSecondChoiceOfWhiteBoundingRectangle(int index)
        {
            var secondChoice = index + _game.Dice.SecondCube;
            if (secondChoice <= 24 && _game.Dice.SecondCube > 0 && secondChoice >= 1 &&
                (_game.Board.Triangles.ElementAt(secondChoice - 1).Type == CheckerType.White ||
                 _game.Board.Triangles.ElementAt(secondChoice - 1).Type == CheckerType.None ||
                 (_game.Board.Triangles.ElementAt(secondChoice - 1).Type == CheckerType.Black &&
                  _game.Board.Triangles.ElementAt(secondChoice - 1).CheckersStack.Count() == 1)))
            {
                var graphics2 = _triangles[secondChoice].CreateGraphics();
                var pen3 = new Pen(Color.Green, 5);
                graphics2.DrawRectangle(pen3, 0, 0, 50, 223);
            }
        }

        private void TryDrawFirstChoiceOfWhiteBoundingRectangle(int firstchoice)
        {
            if (firstchoice <= 24 && _game.Dice.FirstCube > 0 && firstchoice >= 1 &&
                (_game.Board.Triangles.ElementAt(firstchoice - 1).Type == CheckerType.White ||
                 _game.Board.Triangles.ElementAt(firstchoice - 1).Type == CheckerType.None ||
                 (_game.Board.Triangles.ElementAt(firstchoice - 1).Type == CheckerType.Black &&
                  _game.Board.Triangles.ElementAt(firstchoice - 1).CheckersStack.Count() == 1)))
            {
                var graphics2 = _triangles[firstchoice].CreateGraphics();
                var pen2 = new Pen(Color.Green, 5);
                graphics2.DrawRectangle(pen2, 0, 0, 50, 223);
            }
        }

        private static void DrawWhiteBoundingRectangle(int index, Graphics graphics)
        {
            if (index == 0)
            {
                var pen = new Pen(Color.Red, 5);
                graphics?.DrawRectangle(pen, 0, 0, 42, 223);
            }
            else if (index != 26 && index != 27 && index != 25)
            {
                var pen = new Pen(Color.Red, 5);
                graphics?.DrawRectangle(pen, 0, 0, 50, 223);
            }
        }

        private void PerformBlackMove(int index)
        {
            if (_fromDeadBar)
            {
                _game.MoveBlackFromDeadBar(index);
                _fromDeadBar = false;
            }
            else
            {
                MoveBlack(index);
            }
        }

        private void MoveBlack(int index)
        {
            if (index == 26)
            {
                MoveBlackToOutside();
            }
            else
            {
                _game.MoveBlack(_fromTriangle, _fromTriangle - index);
            }
        }

        private void MoveBlackToOutside()
        {
            if (_fromTriangle == _game.Dice.FirstCube)
            {
                _game.MoveWhite(_fromTriangle, _game.Dice.FirstCube);
            }
            else if (_fromTriangle == _game.Dice.SecondCube)
            {
                _game.MoveWhite(_fromTriangle, _game.Dice.SecondCube);
            }
            if (_fromTriangle <= _game.Dice.FirstCube)
            {
                _game.MoveBlack(_fromTriangle, _game.Dice.FirstCube);
            }
            else if (_fromTriangle <= _game.Dice.SecondCube)
            {
                _game.MoveBlack(_fromTriangle, _game.Dice.SecondCube);
            }
        }

        private void DrawBlackBoundingRectangleAndAvailavleMoves(int index, Graphics graphics)
        {
            DrawBlackBoundingRectangle(index, graphics);

            TryDrawFirstChoiceOfBlackBoundingRectangle(index);

            TryDrawSecondChoiceOfBlackBoundingRectangle(index);
            _fromTriangle = index;
            if (index == 25)
            {
                _fromDeadBar = true;
            }
            _firstClick = false;
        }

        private static void DrawBlackBoundingRectangle(int index, Graphics graphics)
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
        }

        private void TryDrawSecondChoiceOfBlackBoundingRectangle(int index)
        {
            var secondChoice = index - _game.Dice.SecondCube;
            if (secondChoice >= 1 && _game.Dice.SecondCube > 0 && secondChoice <= 24 &&
                (_game.Board.Triangles.ElementAt(secondChoice - 1).Type == CheckerType.Black ||
                 _game.Board.Triangles.ElementAt(secondChoice - 1).Type == CheckerType.None ||
                 (_game.Board.Triangles.ElementAt(secondChoice - 1).Type == CheckerType.White &&
                  _game.Board.Triangles.ElementAt(secondChoice - 1).CheckersStack.Count() == 1)))
            {
                var graphics2 = _triangles[secondChoice].CreateGraphics();
                var pen3 = new Pen(Color.Green, 5);
                graphics2.DrawRectangle(pen3, 0, 0, 50, 223);
            }
        }

        private void TryDrawFirstChoiceOfBlackBoundingRectangle(int index)
        {
            var firstchoice = index - _game.Dice.FirstCube;
            if (firstchoice >= 1 && _game.Dice.FirstCube > 0 && firstchoice <= 24 &&
                (_game.Board.Triangles.ElementAt(firstchoice - 1).Type == CheckerType.Black ||
                 _game.Board.Triangles.ElementAt(firstchoice - 1).Type == CheckerType.None ||
                 (_game.Board.Triangles.ElementAt(firstchoice - 1).Type == CheckerType.White &&
                  _game.Board.Triangles.ElementAt(firstchoice - 1).CheckersStack.Count() == 1)))
            {
                var graphics2 = _triangles[firstchoice].CreateGraphics();
                var pen2 = new Pen(Color.Green, 5);
                graphics2.DrawRectangle(pen2, 0, 0, 50, 223);
            }
        }

        private bool DrawingBoundingTriangleOnWhiteTriangleIsIllegal(int index)
        {
            if (index == 25)
            {
                return true;
            }
            else if (index == 0)
            {
                if (!_game.WhiteDeadCheckersBar.Any())
                {
                    return true;
                }
            }
            else if (index != 25 && index != 26 && index != 27 &&
                     (_game.Board.Triangles.ElementAt(index - 1).Type == CheckerType.Black ||
                      _game.Board.Triangles.ElementAt(index - 1).Type == CheckerType.None))
            {
                return true;
            }
            return false;
        }

        private bool DrawingBoundingTriangleOnBlackTriangleIsIllegal(int index)
        {
            if (index == 0)
            {
                return true;
            }
            else if (index == 25)
            {
                if (!_game.BlackDeadCheckersBar.Any())
                {
                    return true;
                }
            }
            else if (index != 0 && index != 26 && index != 27 &&
                     (_game.Board.Triangles.ElementAt(index - 1).Type == CheckerType.White ||
                      _game.Board.Triangles.ElementAt(index - 1).Type == CheckerType.None))
            {
                return true;
            }
            return false;
        }

        private static int GetIndexOfTheCurrentClickedOnPtictureBox(string name)
        {
            var index = 0;
            if (name != null && !int.TryParse(name[name.Length - 2].ToString() + name[name.Length - 1].ToString(), out index))
            {
                index = int.Parse(name[name.Length - 1].ToString());
            }
            return index;
        }

        private void GameFinishedListener(object o, GameFinishedEventArgs e)
        {
            var winner = e.Winner == CheckerType.Black ? "Black Player" : "White Player";
            var dialogResult = MessageBox.Show($"The winner is {winner}!, Start a new game?", @"Game Finished!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Restart();
            }
            else if (dialogResult == DialogResult.No)
            {
                Application.Exit();
            }
            else
            {
                throw new InvalidEnumArgumentException();
            }
        }

        private void NoAvailableMovesListener(object sender, NoAvailableMovesEventArgs e)
        {
            DrawBoard();
            DisplayFirstCubeFromEventArgs(e);
            DisplaySecondCubeFromEventArgs(e);
            MessageBox.Show(@"No Available Moves !");
        }

        private void DisplaySecondCubeFromEventArgs(NoAvailableMovesEventArgs e)
        {
            switch (e.SecondCube)
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
        }

        private void DisplayFirstCubeFromEventArgs(NoAvailableMovesEventArgs e)
        {
            switch (e.FirstCube)
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
        }
    }
}
