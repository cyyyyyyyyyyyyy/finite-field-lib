using finite_fields;

namespace crc_lib
{
	public class CheckSum
	{
		private const int _mpl = 4; // main polynomial length
		private const int _ffc = 2; //finite field characteristic
		private readonly RPolyn<FiniteFieldElement> _mainpolyn;
		public CheckSum(byte[] bytes) 
		{
			if (bytes.Length != _mpl) throw new ArgumentException("");

			var gf256 = new FiniteField(_ffc, new int[] { 0 }); //tmp - factor polyn
			var ElementPolyn = new FiniteFieldElement[_mpl];
			for (int i = 0; i < bytes.Length; i++)
			{
				ElementPolyn[i] = gf256.Get(new byte[] { bytes[i] } ); // overload for byte and not byte[]
			}
			_mainpolyn = new RPolyn<FiniteFieldElement>(_ffc, ElementPolyn);
		}

		public byte[] CalculateCheckSum() { return new byte[] { }; }
		public bool CheckCheckSum(byte[] bytes) { return false;  }

	}
}