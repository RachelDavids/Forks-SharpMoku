using System;
using System.Collections.Generic;

namespace SharpMoku.Board
{
	[Serializable]
	public class Board
	{
		public int[,] Matrix { get; set; }

		/*
         * dicWhiteStone stores WhiteStone position
         * dicBlackStone stores BlackStone position
         * dicNieghbor stores position of the stone next to both white and black stone
         * These 3 dictionaries are used by Minimax evaluate function.
         */
		public Dictionary<string, Position> WhiteStones { get; private set; } = [];
		public Dictionary<string, Position> BlackStones { get; private set; } = [];
		public Dictionary<string, Position> Neighbours { get; private set; } = [];

		public int BoardSize { get; private set; }

		public Turn CurrentTurn { get; private set; } = Turn.Black;
		public CellValue CurrentTurnCellValue => CurrentTurn == Turn.Black ? CellValue.Black : CellValue.White;
		public Board(int boardSize)
		{
			if (boardSize is not 9 and not 15)
			{
				throw new ArgumentException($"Board size is invalid {boardSize}, program only accept 9 and 15 as valid value");
			}
			BoardSize = boardSize;
			Matrix = new int[BoardSize, BoardSize];
		}

		public Board(Board board)
		{

			Matrix = new int[board.Matrix.GetLength(0), board.Matrix.GetLength(1)];
			WhiteStones = [];
			BlackStones = [];
			Neighbours = [];

			Matrix = board.Matrix.Clone() as int[,];
			WhiteStones = new Dictionary<string, Position>(board.WhiteStones);
			BlackStones = new Dictionary<string, Position>(board.BlackStones);
			Neighbours = new Dictionary<string, Position>(board.Neighbours);
			ListHistory = new List<Position>(board.ListHistory);
			BoardSize = board.BoardSize;
			CurrentTurn = board.CurrentTurn;

		}
		public Board Clone() => new(this);

		private Dictionary<string, Position> GetHshByCellValue(CellValue cellValue)
		{
			return cellValue == CellValue.White ? WhiteStones : BlackStones;
		}
		public bool IsThereAnyOneWon()
		{
			throw new NotSupportedException($"{nameof(Board)}.{nameof(IsThereAnyOneWon)}");
		}

		public void PutStone(int pRow, int pCol)
		{

			CellValue cellValue = CurrentTurnCellValue;
			Matrix[pRow, pCol] = (int)cellValue;
			Position newPosition = new(pRow, pCol);

			GetHshByCellValue(cellValue).Add(newPosition.PositionString(), newPosition);
			ListHistory.Add(newPosition);
		}
		public List<Position> GetListNeighborPosition(Position position) => GetListNeighborPosition(position, 1);
		public List<Position> GetListNeighborPosition(Position position, int radius)
		{

			List<Position> listResult = [];
			int i;
			int j;
			for (i = -radius; i <= radius; i++)
			{
				for (j = -radius; j <= radius; j++)
				{
					if (i == 0 && j == 0)
					{
						continue;
					}
					Position neighborPosition = new(i + position.Row, j + position.Col);
					if (!IsValidPosition(neighborPosition))
					{
						continue;
					}

					listResult.Add(neighborPosition);
				}
			}
			return listResult;
		}
		private void AddEmptyNeighborOf(Position position)
		{

			if (Neighbours.ContainsKey(position.PositionString()))
			{
				Neighbours.Remove(position.PositionString());
			}

			List<Position> listNieghbor = GetListNeighborPosition(position);
			foreach (Position neighborPosition in listNieghbor)
			{

				if (Neighbours.ContainsKey(neighborPosition.PositionString()))
				{
					continue;
				}

				if (Matrix[neighborPosition.Row, neighborPosition.Col] != (int)CellValue.Empty)
				{
					continue;
				}
				Neighbours.Add(neighborPosition.PositionString(), neighborPosition);

			}
		}
		public void AdjustEmptyNeighborOf(Position position)
		{
			List<Position> listNieghbor = GetListNeighborPosition(position);
			HashSet<string> hshCannotRemove = [];
			bool isNeedtoAddNeighborForPostion = false;
			foreach (Position neighborPosition in listNieghbor)
			{
				bool IsThisNeighborNotEmpty = false;
				if (Matrix[neighborPosition.Row, neighborPosition.Col] != (int)CellValue.Empty)
				{
					IsThisNeighborNotEmpty = true;
					isNeedtoAddNeighborForPostion = true;

					if (!hshCannotRemove.Contains(neighborPosition.PositionString()))
					{
						hshCannotRemove.Add(neighborPosition.PositionString());
					}
				}

				List<Position> listNighborOfNeighbor = GetListNeighborPosition(neighborPosition);

				foreach (Position neighborOfneighborPosition in listNighborOfNeighbor)
				{
					if (IsThisNeighborNotEmpty)
					{
						if (!hshCannotRemove.Contains(neighborPosition.PositionString()))
						{
							hshCannotRemove.Add(neighborPosition.PositionString());
						}
						break;
					}

					if (Matrix[neighborOfneighborPosition.Row, neighborOfneighborPosition.Col] != (int)CellValue.Empty)
					{
						if (!hshCannotRemove.Contains(neighborPosition.PositionString()))
						{
							hshCannotRemove.Add(neighborPosition.PositionString());
						}
						break;
					}
				}
			}
			foreach (Position neighborPosition in listNieghbor)
			{
				if (hshCannotRemove.Contains(neighborPosition.PositionString()))
				{
					continue;
				}
				Neighbours.Remove(neighborPosition.PositionString());
			}
			if (isNeedtoAddNeighborForPostion)
			{
				Neighbours.Add(position.PositionString(), position);

			}
		}

		public void PutStone(int pRow, int pCol, CellValue cellValue)
		{
			/* 1.Assign value into the matrix
             * 2.Add the postion value into Hash
             * 3.Add postion into history
             * 4.Add Empty nex
             */
			Matrix[pRow, pCol] = (int)cellValue;
			Position newPosition = new(pRow, pCol);
			GetHshByCellValue(cellValue).Add(newPosition.PositionString(), newPosition);
			ListHistory.Add(newPosition);
			AddEmptyNeighborOf(newPosition);
		}

		public void PutStone(Position position) => PutStone(position.Row, position.Col, CurrentTurnCellValue);
		public void PutStone(Position position, CellValue cellValue) => PutStone(position.Row, position.Col, cellValue);
		public bool IsValidPosition(Position pos) => IsValidPosition(pos.Row, pos.Col);

		public bool IsValidPosition(int pRow, int pCol)
		{
			return pRow >= 0
				   && pRow < BoardSize
				   && pCol >= 0
				   && pCol < BoardSize;
		}

		public void PutStone(int pRow, int pCol, int value)
		{
			if (!IsValidPosition(pRow, pCol))
			{
				throw new ArgumentException($"Position is not valid {pRow},{pCol} ");
			}
			if (!Enum.IsDefined(typeof(CellValue), value))
			{
				throw new ArgumentException($"Value is not valid {value}");
			}
			PutStone(pRow, pCol, (CellValue)value);
		}

		public void PutStoneAndSwitchTurn(Position pos)
		{
			PutStone(pos.Row, pos.Col, CurrentTurnCellValue);
			SwitchTurn();
		}
		public void PutStoneAndSwitchTurn(Position pos, CellValue cellValue)
		{
			PutStone(pos.Row, pos.Col, cellValue);
			SwitchTurn();
		}
		public void PutStoneAndSwitchTurn(int pRow, int pCol, CellValue cellValue)
		{
			PutStone(pRow, pCol, cellValue);
			SwitchTurn();
		}
		public void SwitchTurn()
		{
			CurrentTurn = CurrentTurn == Turn.Black ? Turn.White : Turn.Black;
		}
		public bool IsThere5InRow(Position pos)
		{
			int i;
			int j;
			int cellValue = Matrix[pos.Row, pos.Col];
			if (cellValue == 0)
			{
				return false;
			}

			for (i = -1; i <= 1; i++)
			{

				for (j = -1; j <= 1; j++)
				{
					if (i == 0 && j == 0)
					{
						continue;
					}
					int sameCellValueInDirectionCount = 0;
					for (int noOfRepeating = 1; noOfRepeating <= 4; noOfRepeating++)
					{
						Position checkPosition = new(pos.Row + (i * noOfRepeating),
													 pos.Col + (j * noOfRepeating));
						if (checkPosition.Row < 0
							|| checkPosition.Row > BoardSize - 1
							|| checkPosition.Col < 0
							|| checkPosition.Col > BoardSize - 1)
						{
							break;
						}

						if (Matrix[checkPosition.Row, checkPosition.Col] != cellValue)
						{
							break;
						}
						sameCellValueInDirectionCount++;
						if (sameCellValueInDirectionCount >= 4)
						{
							return true;
						}
					}

					// Position posCheck

				}
			}
			return false;
		}
		public bool IsEmpty => BlackStones.Count + WhiteStones.Count == 0;
		public bool IsFull => BlackStones.Count + WhiteStones.Count == BoardSize * BoardSize;
		public WinStatus CheckWinStatus()
		{
			foreach (Position pos in WhiteStones.Values)
			{

				if (IsThere5InRow(pos))
				{
					return WinStatus.WhiteWon;
				}
			}
			foreach (Position pos in BlackStones.Values)
			{
				if (IsThere5InRow(pos))
				{
					return WinStatus.BlackWon;
				}
			}
			return IsFull ? WinStatus.Draw : WinStatus.NotDecidedYet;
		}

		public List<Position> ListHistory { get; } = [];
		public bool CanUndo => ListHistory is { Count: > 0 };
		public Position LastPositionPut => ListHistory == null || ListHistory.Count == 0
			? new Position(-1, -1)
			: ListHistory[^1];

		public List<Position> generateNeighbourMoves()
		{
			return generateNeighbourMoves(1);
		}

		public List<Position> generateNeighbourMoves(int radius)
		{
			//List<Position> moveList = [];
			bool IsUsedicNeighbor = radius == 1;

			if (IsUsedicNeighbor)
			{
				return [.. Neighbours.Values];
			}

			HashSet<string> hsh = [];
			List<Position> listResult = [];
			foreach (string key in BlackStones.Keys)
			{
				List<Position> list = GetListNeighborPosition(BlackStones[key], radius);
				foreach (Position pos in list)
				{
					if (hsh.Contains(pos.PositionString()))
					{
						continue;
					}
					if (BlackStones.ContainsKey(pos.PositionString()))
					{
						continue;
					}
					if (WhiteStones.ContainsKey(pos.PositionString()))
					{
						continue;
					}
					hsh.Add(pos.PositionString());
					listResult.Add(pos);
				}
			}

			foreach (string key in WhiteStones.Keys)
			{
				List<Position> list = GetListNeighborPosition(WhiteStones[key], radius);
				foreach (Position pos in list)
				{
					if (hsh.Contains(pos.PositionString()))
					{
						continue;
					}
					if (BlackStones.ContainsKey(pos.PositionString()))
					{
						continue;
					}
					if (WhiteStones.ContainsKey(pos.PositionString()))
					{
						continue;
					}
					hsh.Add(pos.PositionString());
					listResult.Add(pos);
				}
			}

			return listResult;

		}
		/*
        public List<Position> generateNeighbourMovesBK()
        {



            List<Position> moveList = new List<Position>();
            int boardSize =Matrix.GetLength(0);

            // Look for cells that has at least one stone in an adjacent cell.
            int iCount = 0;


            var listPosition = dicBlackStone.Values.ToList();
            listPosition.AddRange(dicWhiteStone.Values.ToList());


            NewGetMove(moveList, listPosition, this.Matrix, boardSize);

            iCount++;
            return moveList;

        }
        */
		/*
        private void NewGetMove(List<Position> moveList, List<SharpMoku.Position> listCheckPosition, int[,] pboarMatrix, int boardSize)
        {
            HashSet<String> hshPosition = new HashSet<String>();
            foreach (SharpMoku.Position checkPosition in listCheckPosition)
            {
                for (int Row = checkPosition.Row - 1; Row <= checkPosition.Row + 1; Row++)
                {
                    for (int Col = checkPosition.Col - 1; Col <= checkPosition.Col + 1; Col++)
                    {
                        Boolean IsOwnCell = (Row == checkPosition.Row && Col == checkPosition.Col);
                        Boolean IsInvalidRange = Row < 0 ||
                            Row >= boardSize ||
                            Col < 0 ||
                            Col >= boardSize;


                        if (IsOwnCell || IsInvalidRange)
                        {
                            continue;
                        }

                        Boolean IsNotEmpty = this.Matrix[Row, Col] != 0;

                        if (IsNotEmpty)
                        {
                            continue;
                        }
                        SharpMoku.Position newPosition = new SharpMoku.Position(Row, Col);
                        Boolean IsAlreadyExist = hshPosition.Contains(newPosition.PositionString());
                        if (IsAlreadyExist)
                        {
                            continue;
                        }

                        moveList.Add(newPosition);
                        hshPosition.Add(newPosition.PositionString());
                    }
                }
            }

        }
        */

		public void Undo()
		{
			int LastIndex = ListHistory.Count - 1;
			Position pos = ListHistory[LastIndex];
			ListHistory.RemoveAt(LastIndex);
			RemoveStone(pos);
			AdjustEmptyNeighborOf(pos);
			SwitchTurn();

		}

		public void RemoveStone(Position position)
		{

			if (BlackStones.ContainsKey(position.PositionString()))
			{
				BlackStones.Remove(position.PositionString());
			}
			else
			{
				if (WhiteStones.ContainsKey(position.PositionString()))
				{
					WhiteStones.Remove(position.PositionString());
				}
				else
				{
					throw new InvalidOperationException($"Position is not correct {position.Row},{position.Col}");
				}
			}

			Matrix[position.Row, position.Col] = 0;

		}
		public void RemoveStone(int posX, int posY)
		{
			RemoveStone(new Position(posX, posY));
		}
	}
}
