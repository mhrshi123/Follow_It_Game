using System;
using System.Diagnostics.Eventing.Reader;

namespace FollowIt
{
    public enum GameState { PlayerPlay, ReachedTarget, Lerping, Pathing, MoveTarget, GameOver };
    public partial class Form1 : Form
    {
        int score;
        int highestScore;
        const string HighestScoreFile = "highestscore.txt";
        const int _cellWidth = 75;
        const int _cellHeight = 75;
        const int _gridX = 300;
        const int _gridY = 150;
        int _maxRows = 8;
        int _maxCols = 8;
        TableLayoutPanel mainGrid;
        Cell[,] gridArray;
        Player player;
        Target target;
        List<Cell> targetPath;
        List<Cell> pathToMove;
        List<Cell> playerPath;
        TableLayoutPanel nextGrid;
        Cell[,] nextGridArray;
        Player nextPlayer;
        Target nextTarget;
        GameState gameState = GameState.Pathing;
        Random random = new Random();
        bool moveVerified = false;

        public Form1()
        {
            InitializeComponent();

            LoadHighestScore();

            lblScore.Text = $"{score}";
            lblhgScore.Text = $"👑 {highestScore}";
            mainGrid = CreateTableLayoutPanel(1, 2, _cellWidth, _cellHeight, ref gridArray);
            player = new Player(0, 0);
            player.Add(mainGrid);
            target = new Target(0, 0);
            target.Add(mainGrid);
            mainGrid.Location = new Point(_gridX, _gridY);
            this.Controls.Add(mainGrid);

            nextGrid = CreateTableLayoutPanel(random.Next(Math.Min(2 + score / 3, 3), Math.Min(4 + score / 3, _maxRows)), random.Next(Math.Min(2 + score / 3, 4), Math.Min(4 + score / 3, _maxCols)), _cellWidth, _cellHeight, ref nextGridArray);
            nextPlayer = new Player(random.Next(nextGrid.RowCount), random.Next(nextGrid.ColumnCount));
            nextPlayer.Add(nextGrid);
            nextTarget = new Target(nextPlayer.row, nextPlayer.col);
            nextTarget.Add(nextGrid);

            timer1.Start();

        }

        private void LoadHighestScore()
        {
            if (File.Exists(HighestScoreFile))
            {
                using (StreamReader reader = new StreamReader(HighestScoreFile))
                {
                    string file = File.ReadAllText(HighestScoreFile);
                    if (!int.TryParse(file, out highestScore))
                    {
                        highestScore = 0;
                    }
                }

            }
            else
            {
                highestScore = 0;
                using (StreamWriter writer = new StreamWriter(HighestScoreFile, false))
                {
                    writer.WriteLine(highestScore);
                }
            }
        }

        private void SaveScore()
        {
            File.WriteAllText(HighestScoreFile, highestScore.ToString());
        }

        private TableLayoutPanel CreateTableLayoutPanel(int rows, int cols, int cellWidth, int cellHeight, ref Cell[,] gridArray)
        {
            TableLayoutPanel grid = new TableLayoutPanel
            {
                RowCount = rows,
                ColumnCount = cols,
                AutoSize = false,
                Size = new Size(cols * cellWidth, rows * cellHeight),
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };

            gridArray = new Cell[rows, cols];
            grid.RowStyles.Clear();
            grid.ColumnStyles.Clear();

            for (int i = 0; i < rows; i++)
            {
                grid.RowStyles.Add(new RowStyle(SizeType.Absolute, cellHeight));
                for (int j = 0; j < cols; j++)
                {
                    grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, cellWidth));

                    Panel cellPanel = new Panel
                    {
                        Size = new Size(cellWidth, cellHeight),
                        BackColor = Color.BurlyWood,
                        Margin = new Padding(1)
                    };
                    grid.Controls.Add(cellPanel, j, i);

                    gridArray[i, j] = new Cell(i, j, cellPanel);
                }
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {

                    if (i + 1 < rows) gridArray[i, j].cellsToMove.Add(gridArray[i + 1, j]);
                    if (i - 1 >= 0) gridArray[i, j].cellsToMove.Add(gridArray[i - 1, j]);
                    if (j + 1 < cols) gridArray[i, j].cellsToMove.Add(gridArray[i, j + 1]);
                    if (j - 1 >= 0) gridArray[i, j].cellsToMove.Add(gridArray[i, j - 1]);
                }
            }

            return grid;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            int newRow = player.row;
            int newCol = player.col;

            switch (keyData)
            {
                case Keys.Up:
                    newRow--;
                    break;
                case Keys.Down:
                    newRow++;
                    break;
                case Keys.Left:
                    newCol--;
                    break;
                case Keys.Right:
                    newCol++;
                    break;
            }

            if (newRow >= 0 && newRow < mainGrid.RowCount &&
                newCol >= 0 && newCol < mainGrid.ColumnCount &&
                gameState == GameState.PlayerPlay &&
                moveVerified
                )
            {
                player.row = newRow;
                player.col = newCol;
                player.Add(mainGrid);
                moveVerified = false;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (gameState)
            {
                case GameState.PlayerPlay:
                    if (player.row == target.row && player.col == target.col)
                    {
                        gameState = GameState.ReachedTarget;
                        score++;
                        highestScore = Math.Max(score, highestScore);
                        lblScore.Text = $"{score}";
                        lblhgScore.Text = $"👑 {highestScore}";
                    }
                    if (playerPath.Count == 0 || !(playerPath[playerPath.Count - 1] == gridArray[player.row, player.col]))
                    {
                        playerPath.Add(gridArray[player.row, player.col]);
                    }
                    moveVerified = targetPath.Take(playerPath.Count).SequenceEqual(playerPath);

                    if (!moveVerified) gameState = GameState.GameOver;
                    break;

                case GameState.ReachedTarget:
                    moveVerified = false;
                    int cellx = mainGrid.Location.X + (player.col * _cellWidth);
                    int celly = mainGrid.Location.Y + (player.row * _cellHeight);

                    this.Controls.Remove(mainGrid);
                    nextGrid.Location = new Point(cellx, celly);
                    this.Controls.Add(nextGrid);
                    mainGrid = nextGrid;
                    player = nextPlayer;
                    gridArray = nextGridArray;
                    target = nextTarget;

                    nextGrid = CreateTableLayoutPanel(random.Next(Math.Min(2 + score / 3, 3), Math.Min(4 + score / 3, _maxRows)), random.Next(Math.Min(2 + score / 3, 4), Math.Min(4 + score / 3, _maxCols)), _cellWidth, _cellHeight, ref nextGridArray);
                    nextPlayer = new Player(random.Next(nextGrid.RowCount), random.Next(nextGrid.ColumnCount));
                    nextPlayer.Add(nextGrid);
                    nextTarget = new Target(nextPlayer.row, nextPlayer.col);
                    nextTarget.Add(nextGrid);

                    cellx = mainGrid.Location.X - (player.col * _cellWidth);
                    celly = mainGrid.Location.Y - (player.row * _cellHeight);
                    mainGrid.Location = new Point(cellx, celly);
                    gameState = GameState.Lerping;
                    break;

                case GameState.Lerping:
                    LERP(mainGrid, _gridX, _gridY, GameState.Pathing, 0.35f);
                    break;

                case GameState.Pathing:
                    var current = gridArray[target.row, target.col];
                    current.visited = true;
                    targetPath = new List<Cell>();
                    playerPath = new List<Cell>();

                    targetPath.Add(current);

                    int pathLength = score == 0 ? 1 : 3 + (score - 1) / 3;

                    for (int i = 0; i < pathLength; i++)
                    {
                        var available = current.cellsToMove.Where(c => c.visited == false).ToList();
                        if (available.Count > 0)
                        {
                            var nextCell = available[random.Next(available.Count)];
                            targetPath.Add(nextCell);
                            current = nextCell;
                            nextCell.visited = true;
                        }
                    }
                    pathToMove = new List<Cell>(targetPath);
                    gameState = GameState.MoveTarget;
                    break;

                case GameState.MoveTarget:

                    var finalTarget = pathToMove[pathToMove.Count - 1];
                    var next = pathToMove[0];

                    if (gridArray[target.row, target.col].panel.Controls.Contains(target.panel))
                    {
                        mainGrid.Controls.Remove(target.panel);
                        target.panel.Location = new Point(mainGrid.Location.X + (target.col * _cellWidth) + ((_cellWidth - target.panel.Width) / 2), mainGrid.Location.Y + (target.row * _cellHeight) + ((_cellHeight - target.panel.Height) / 2));
                        this.Controls.Add(target.panel);
                        target.panel.BringToFront();
                    }

                    int nextCellX = mainGrid.Location.X + (next.col * _cellWidth) + ((_cellWidth - target.panel.Width) / 2);
                    int nextCellY = mainGrid.Location.Y + (next.row * _cellHeight) + ((_cellHeight - target.panel.Height) / 2);

                    if (LERP(target.panel, nextCellX, nextCellY, GameState.MoveTarget, 0.65f))
                    {

                        target.row = next.row;
                        target.col = next.col;
                        target.Add(mainGrid);

                        pathToMove.RemoveAt(0);
                        if (target.row == finalTarget.row && target.col == finalTarget.col)
                        {
                            gameState = GameState.PlayerPlay;
                        }
                    }
                    break;

                case GameState.GameOver:
                    SaveScore();
                    timer1.Stop();
                    if (highestScore == score)
                    {
                        MessageBox.Show("Congratulationss!! New High score", "👑 NEW HIGHSCORE!");
                    }
                    MessageBox.Show($"Game Over!! \nFINAL SCORE: {score} \nHIGHEST SCORE: {highestScore}", "☠ GAME OVER");
                    Close();
                    break;
            }
        }

        private bool LERP(Control control, int finalX, int finalY, GameState nextState, float speed)
        {
            Point currentPosition = control.Location;

            int newX = (int)(currentPosition.X + (finalX - currentPosition.X) * speed);
            int newY = (int)(currentPosition.Y + (finalY - currentPosition.Y) * speed);

            control.Location = new Point(newX, newY);

            if (Math.Abs(finalX - newX) < 5 && Math.Abs(finalY - newY) < 5)
            {
                control.Location = new Point(finalX, finalY);
                gameState = nextState;
                return true;
            }

            return false;
        }


    }
}
