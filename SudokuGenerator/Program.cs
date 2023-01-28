internal class Program
{
    private static void Main(string[] args)
    {
        //int[,] blankGrid = new int[10, 10];
        //printGrid(blankGrid);
        Console.WriteLine("Welcome to Sudoku Grid Generator!!!");
        Console.WriteLine();
        Console.WriteLine("Press Enter to Continue.");
        Console.ReadLine();
        // Creates empty grid
        // I use a 10x10 instead of a 9x9 so the indices are easier to deal with
        int[,] grid = new int[10, 10];
        int count = 0;
        // loop to try to generate working sudoku grid
        // and count how many times it takes to generate
        // working sudoku grid
        while (isBlank(grid))
        {
            count++;
            grid = generateGrid();
        }
        printGrid(grid);
        bool valid = isValidGrid(grid);
        Console.WriteLine($"Is this a valid grid?: {valid}");
        Console.WriteLine($"It took {count} attempts to make a suduko grid");
        Console.ReadLine();

    }

    // Prints the sudoku grid
    static void printGrid(int[,] grid)
    {
        int[] arr = { 3, 6, 9 };
        Console.WriteLine("+---+---+---+");
        for(int row = 1; row < 10; row++)
        {
            Console.Write("|");
            for(int col = 1; col < 10; col++)
            {
                Console.Write(grid[row, col]);
                if (arr.Contains(col))
                    Console.Write("|");
            }
            Console.WriteLine();
            if(arr.Contains(row))
                Console.WriteLine("+---+---+---+");
        }
    }

    // attempts to generate a working sudoku grid
    // but if it fails it returns a grid of zeros
    static int[,] generateGrid()
    {
        int[,] grid = new int[10, 10];
        List<int> emptySpaces = new List<int>();
        Random rnd = new Random();
        // loop through numbers 1-9
        for(int num = 1; num <= 9; num++)
        {
            // loop through rows
            for (int row = 1; row < 10; row++)
            {
                // get possible columns in row that can contain a number
                emptySpaces = getEmptySpaces(grid, num, row);
                // if no possibilities end function and return grid full of 0s
                if (emptySpaces.Count == 0)
                {
                    int[,] blankGrid = new int[10, 10];
                    return blankGrid;
                }
                // otherwise pick a random space from the empty spaces
                // and change that grid cell to num
                int emptyIndex = rnd.Next(emptySpaces.Count);
                int col = emptySpaces[emptyIndex];
                emptySpaces.Clear();
                grid[row, col] = num;
            }
        }
        return grid;
    }

    // returns a list of all the possible spaces of row that
    // num can be in without violating the sudoku rules
    // I use a list because I do not know what size it will be
    static List<int> getEmptySpaces(int[,] grid, int num, int row)
    {
        int sec;
        List<int> emptySpaces = new List<int>();
        for(int col = 1; col < 10; col++)
        {
            // remove the column space if that column already contains that number
            if (getCol(grid, col).Contains(num))
                continue;
            sec = getSecFromRowCol(row, col);
            // remove the column space if that section already contain that number
            if (getSec(grid, sec).Contains(num))
                continue;
            // makes sure the cell doesn't already contain a number before adding to the list
            // this check is probably not needed
            if (grid[row, col] == 0)
                emptySpaces.Add(col);
        }
        return emptySpaces;
    }

    // returns an array of all numbers in that row of the grid
    static int[] getRow(int[,] grid, int row)
    {
        int[] fullRow =
        {
            grid[row, 1],
            grid[row, 2],
            grid[row, 3],
            grid[row, 4],
            grid[row, 5],
            grid[row, 6],
            grid[row, 7],
            grid[row, 8],
            grid[row, 9]
        };
        return fullRow;
    }

    // returns an array of all numbers in that column of the grid
    static int[] getCol(int[,] grid, int col)
    {
        int[] fullCol =
        {
            grid[1, col],
            grid[2, col],
            grid[3, col],
            grid[4, col],
            grid[5, col],
            grid[6, col],
            grid[7, col],
            grid[8, col],
            grid[9, col]
        };
        return fullCol;
    }

    // returns an array of all numbers in that section of the grid
    static int[] getSec(int[,] grid, int sec)
    {
        int row = ((sec - 1) / 3) * 3 + 1;
        int col = ((sec - 1) % 3) * 3 + 1;

        int[] fullSec =
        {
            grid[row, col],
            grid[row, col + 1],
            grid[row, col + 2],
            grid[row + 1, col],
            grid[row + 1, col + 1],
            grid[row + 1, col + 2],
            grid[row + 2, col],
            grid[row + 2, col + 1],
            grid[row + 2, col + 2]
        };
        return fullSec;
    }

    // returns which section of the sudoku grid the cell is in
    // sections as below:
    // 1 | 2 | 3
    // 4 | 5 | 6
    // 7 | 8 | 9
    static int getSecFromRowCol(int row, int col)
    {
        int[] first = { 1, 2, 3 };
        int[] second = { 4, 5, 6 };
        int[] third = { 7, 8, 9 };
        if (first.Contains(row) && first.Contains(col))
            return 1;
        if (first.Contains(row) && second.Contains(col))
            return 2;
        if (first.Contains(row) && third.Contains(col))
            return 3;
        if (second.Contains(row) && first.Contains(col))
            return 4;
        if (second.Contains(row) && second.Contains(col))
            return 5;
        if (second.Contains(row) && third.Contains(col))
            return 6;
        if (third.Contains(row) && first.Contains(col))
            return 7;
        if (third.Contains(row) && second.Contains(col))
            return 8;
        if (third.Contains(row) && third.Contains(col))
            return 9;
        else
            return 0;

    }

    // returns true if the grid is full of 0s
    // otherwise returns false
    static bool isBlank(int[,] grid)
    {
        for(int row = 0; row < 10; row++)
        {
            for(int col = 0; col < 10; col++)
            {
                if (grid[row, col] != 0)
                    return false;
            }
        }
        return true;
    }

    // Checks if the grid is a valid sudoku grid
    static bool isValidGrid(int[,] grid)
    {
        for(int i = 1; i < 10; i++)
        {
            for(int j = 1; j < 10; j++)
            {
                if (!getRow(grid, i).Contains(j))
                    return false;
                if (!getCol(grid, i).Contains(j))
                    return false;
                if (!getSec(grid, i).Contains(j))
                    return false;
            }
        }
        return true;
    }
}