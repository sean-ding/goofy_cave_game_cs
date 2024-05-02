using static CaveGame.GameSettings;
using static CaveGame.Managers.ChunkManager;

namespace CaveGame;

public static class Viewcast
{
	public static void CastLight(int posY, int posX, int lightStrength)
	{
		
	}

	public static bool[,] GetShadowMask(int posY, int posX, int layer, int rangeY = GAMEVIEW_HEIGHT / 2, int rangeX = GAMEVIEW_WIDTH / 2)
	{
		var viewMask = new bool[rangeY * 2 + 1, rangeX * 2 + 1];
		viewMask[rangeY, rangeX] = true;
		
		for (var i = 0; i < 8; i++)
		{
			var scan = new Scan(posY, posX, layer, i);
			scan.StartScan(viewMask);
		}

		return viewMask;
	}

	private static double GetSlope(double y2, double y1, double x2, double x1, int octet)
	{
		var slope = (y2 - y1) / (x2 - x1);
		if (double.IsPositiveInfinity(slope) || double.IsNegativeInfinity(slope))
		{
			slope = octet switch
			{
				0 or 4 => double.PositiveInfinity,
				3 or 7 => double.NegativeInfinity,
				_ => slope
			};
		}
		else
		{
			slope *= -1;
		}

		return slope;
	}

	private class Scan
	{
		private double _startSlope, _endSlope, _radius = 0;
		private readonly int _octet;
		private readonly int _rootY, _rootX;
		private readonly int _layer;
		private int _lineNumber, _lineIndex;
		private bool _lastWasBlocking, _firstScan = true;

		public Scan(int rootY, int rootX, int layer, int octet, double? startSlope = null, double? endSlope = null, int lineNumber = 0, int lineIndex = 0)
		{
			_rootY = rootY;
			_rootX = rootX;
			_layer = layer;
			_octet = octet;
			_lineNumber = lineNumber;
			_lineIndex = lineIndex;

			if (startSlope == null)
			{
				_startSlope = _octet switch
				{
					0 or 1 or 4 or 5 => 1,
					2 or 3 or 6 or 7 => -1,
					_ => _startSlope
				};
			}
			else
			{
				_startSlope = (double) startSlope;
			}
			
			if (endSlope == null)
			{
				_endSlope = _octet switch
				{
					0 or 4 => double.PositiveInfinity,
					3 or 7 => double.NegativeInfinity,
					1 or 2 or 5 or 6 => 0,
					_ => _endSlope
				};
			}
			else
			{
				_endSlope = (double) endSlope;
			}
		}

		public void StartScan(bool[,] viewMask)
		{
			while (true)
			{
				if (_lineIndex == _lineNumber)
				{
					_lineNumber++;
					_lineIndex = 0;
					_firstScan = true;
				}
				else
				{
					_lineIndex++;
				}

				var currentY = _octet switch
				{
					0 or 7 => _rootY - _lineNumber,
					1 or 6 => _rootY - _lineNumber + _lineIndex,
					2 or 5 => _rootY + _lineNumber - _lineIndex,
					3 or 4 => _rootY + _lineNumber,
					_ => _rootY
				};

				var currentX = _octet switch
				{
					0 or 3 => _rootX + _lineNumber - _lineIndex,
					1 or 2 => _rootX + _lineNumber,
					4 or 7 => _rootX - _lineNumber + _lineIndex,
					5 or 6 => _rootX - _lineNumber,
					_ => _rootX
				};

				var rangeY = viewMask.GetLength(0) / 2;
				var rangeX = viewMask.GetLength(1) / 2;
				
				if (Math.Abs(currentY - _rootY) > rangeY || Math.Abs(currentX - _rootX) > rangeX)
				{
					if (Math.Abs(currentY - _rootY) > rangeY && Math.Abs(currentX - _rootX) > rangeX)
					{
						return;
					}
					continue;
				}

				var slope = GetSlope(currentY, _rootY, currentX, _rootX, _octet);

				switch (_octet)
				{
					case 0 or 2 or 4 or 6 when slope > _endSlope:
					case 1 or 3 or 5 or 7 when slope < _endSlope:
					{
						if (_lastWasBlocking)
						{
							return;
						}
					
						continue;
					}
					case 0 or 2 or 4 or 6 when slope < _startSlope:
					case 1 or 3 or 5 or 7 when slope > _startSlope:
						continue;
				}

				var indexY = _octet switch
				{
					0 or 7 => rangeY - _lineNumber,
					1 or 6 => rangeY - _lineNumber + _lineIndex,
					2 or 5 => rangeY + _lineNumber - _lineIndex,
					3 or 4 => rangeY + _lineNumber,
					_ => rangeY
				};

				var indexX = _octet switch
				{
					0 or 3 => rangeX + _lineNumber - _lineIndex,
					1 or 2 => rangeX + _lineNumber,
					4 or 7 => rangeX - _lineNumber + _lineIndex,
					5 or 6 => rangeX - _lineNumber,
					_ => rangeX
				};
				
				viewMask[indexY, indexX] = true;
				
				var currentTile = GetTile(currentY, currentX, _layer);
				
				if (currentTile.Blocking && !_lastWasBlocking && _lineIndex != 0 && !_firstScan)
				{
					_lastWasBlocking = true;
					
					var slopeY = _octet switch
					{
						0 or 2 or 5 or 7 => currentY + _radius,
						1 or 3 or 4 or 6 => currentY - _radius,
						_ => currentY
					};
					
					var slopeX = _octet switch
					{
						0 or 3 or 5 or 6 => currentX + _radius,
						1 or 2 or 4 or 7 => currentX - _radius,
						_ => currentX
					};

					var newScan = new Scan(_rootY, _rootX, _layer, _octet, _startSlope, GetSlope(slopeY, _rootY, slopeX, _rootX, _octet), _lineNumber + 1, -1);
					newScan.StartScan(viewMask);
					
					if (_lineIndex == _lineNumber)
					{
						return;
					}
				}
				else if (!currentTile.Blocking && _lastWasBlocking)
				{
					_lastWasBlocking = false;
					
					var slopeY = _octet switch
					{
						0 or 1 or 6 or 7 => currentY - 1 + _radius,
						2 or 3 or 4 or 5 => currentY + 1 - _radius,
						_ => currentY
					};
					
					var slopeX = _octet switch
					{
						0 or 1 or 2 or 3 => currentX + 1 - _radius,
						4 or 5 or 6 or 7 => currentX - 1 + _radius,
						_ => currentX
					};

					_startSlope = GetSlope(slopeY, _rootY, slopeX, _rootX, _octet);
				}
				else if (currentTile.Blocking)
				{
					if (_lineIndex == _lineNumber)
					{
						return;
					}
                    
					_lastWasBlocking = true;
				}
				else if (!currentTile.Blocking)
				{
					_lastWasBlocking = false;
				}

				if (_firstScan)
				{
					_firstScan = false;
				}
			}
		}
	}
}