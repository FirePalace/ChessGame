using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class Menu : Control
{
	public static List<FENPiece> fenPieces = null;
	public static List<Vector2I> castleVectors = new List<Vector2I>();
	private void _on_play_pressed()
	{
		on_fen_input_text_submitted("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
	}
	private void _on_quit_pressed()
	{
		GetTree().Quit();
	}
	private void on_fen_input_text_submitted(string fen)
	{
		Dictionary<char, string> fenToPieces = new Dictionary<char, string>()
		{
			['r'] = "Rook",
			['n'] = "Knight",
			['b'] = "Bishop",
			['q'] = "Queen",
			['k'] = "King",
			['p'] = "Pawn"
		};

		if (IsValidFEN(fen))
		{
			string[] fenSliced = fen.Split(' ');
			string fenOnlyPieces = fenSliced[0];
			string turn = fenSliced[1];
			string castlingRights = fenSliced[2];
			string enPassantSquare = fenSliced[3];
			ChessPiece.halfMoveClock = fenSliced[4].ToInt();
			ChessPiece.fullMoveClock = fenSliced[5].ToInt();
			int file = 0;
			int rank = 0;

			//Is it white's or black's turn?
			if (turn == "b")
			{
				ChessPiece.isWhitesTurn = false;
			}
			else
			{
				ChessPiece.isWhitesTurn = true;
			}

			CastlingRights(castlingRights);

			//Generate a list of all Pieces that need to be placed
			fenPieces = new List<FENPiece>();
			foreach (char c in fenOnlyPieces)
			{

				if (c == '/')
				{
					file = 0;
					rank++;
				}
				else
				{
					if (char.IsDigit(c))
					{
						file += c - '0';
					}
					else
					{
						FENPiece fENPiece = new FENPiece();
						if (char.IsUpper(c))
						{
							fENPiece.pieceType += "white-";
							fENPiece.pieceName += "w";
						}
						else
						{
							fENPiece.pieceType += "black-";
							fENPiece.pieceName += "b";
						}
						fENPiece.pieceType += fenToPieces[char.ToLower(c)].ToLower();
						fENPiece.pieceName += fenToPieces[char.ToLower(c)];
						fENPiece.tilePos = new Vector2I(file, rank);
						fenPieces.Add(fENPiece);

						file++;
					}
				}
			}
			GetTree().ChangeSceneToFile("res://ChessScenes/node_2d.tscn");
		}
		else
		{
			GD.Print("Invalid FEN");
		}

	}
	private bool IsValidFEN(string fen)
	{
		string fenPattern = @"^([rnbqkpRNBQKP1-8]{1,8}/){7}[rnbqkpRNBQKP1-8]{1,8}\s[w|b]\s([K|Q|k|q]{1,4}|-)\s([a-h][1-8]|-)\s\d+\s\d+$";

		return Regex.IsMatch(fen, fenPattern);
	}
	private void CastlingRights(string castlingRights)
	{

		foreach (char c in castlingRights)
		{
			switch (c)
			{
				case 'K':
					castleVectors.Add(new Vector2I(7, 7));
					break;
				case 'Q':
					castleVectors.Add(new Vector2I(0, 7));
					break;
				case 'k':
					castleVectors.Add(new Vector2I(7, 0));
					break;
				case 'q':
					castleVectors.Add(new Vector2I(0, 0));
					break;
			}
		}
	}

}
public struct FENPiece
{
	public string pieceType;
	public string pieceName;
	public Vector2I tilePos;
}
