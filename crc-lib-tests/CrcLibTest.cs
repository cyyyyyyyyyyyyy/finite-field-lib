using crc_lib;
using finite_fields;

namespace crc_lib_tests
{
	[TestClass]
	public class CrcLibTest
	{
		[TestMethod]
		public void Crc_CalculateTest1()
		{
			var gf256 = new FiniteField(2, new int[] { 1, 0, 1, 1, 1, 0, 0, 0, 1 });
			var mainpolyn = new byte[] { 1, 110, 67, 10 };
			var msg = new byte[] { 20, 90, 77, 0, 89, 0, 34, 2, 2 };

			var checkSumInstance = new CheckSum(mainpolyn); //little-endian
			var checkSum = checkSumInstance.CalculateCheckSum(msg);
			var polynOfCheckSum = new RPolyn<FiniteFieldElement>(2, checkSum
				.Select(x => gf256.Get(new byte[] { x })).ToArray());

			var intermediatePolyn = new byte[] { 1, 110, 67, 10, 1 };
			var p1 = new RPolyn<FiniteFieldElement>(2, intermediatePolyn
				.Select(x => gf256.Get(new byte[] { x }))
				.ToArray());

			var p2 = new RPolyn<FiniteFieldElement>(2, msg
				.Select(x => gf256.Get(new byte[] { x }))
				.ToArray());

			var p = p2 % p1;

			Assert.AreEqual(p, polynOfCheckSum);
			//var bytesOfp = p
			//	.GetValue()
			//	.Select(x => x.GetByte().First())
			//	.ToArray();
		}
	}
}