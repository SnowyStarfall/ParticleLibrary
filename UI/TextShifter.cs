namespace ParticleLibrary.UI
{
	public class TextShifter
	{
		public float Progress { get; private set; }

		public int StartTime { get; set; }
		public int MoveTime { get; set; }
		public int EndTime { get; set; }

		private int _startCounter;
		private int _moveCounter;
		private int _endCounter;

		public TextShifter(int startTime = 60, int moveTime = 120, int endTime = 60)
		{
			StartTime = startTime;
			MoveTime = moveTime;
			EndTime = endTime;
		}

		public void Update()
		{
			if (_moveCounter >= MoveTime && _endCounter < EndTime)
			{
				_endCounter++;
			}

			if (_startCounter >= StartTime && _moveCounter < MoveTime)
			{
				_moveCounter++;
			}

			if (_startCounter < StartTime)
			{
				_startCounter++;
			}

			Progress = _moveCounter / (float)MoveTime;

			if (_endCounter >= EndTime)
			{
				_startCounter = 0;
				_moveCounter = 0;
				_endCounter = 0;
			}
		}

		public void Reset()
		{
			_startCounter = 0;
			_moveCounter = 0;
			_endCounter = 0;
		}
	}
}
