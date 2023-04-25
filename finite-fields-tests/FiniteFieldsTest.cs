using finite_fields;
namespace FiniteFieldsTests
{
	[TestClass]
	public class FiniteFieldsTest
	{
		[TestMethod]
		public void GetZero_GF2()
		{
			PrimeFiniteField gf2 = new PrimeFiniteField(2);
			PrimeFiniteFieldElement gf2_0 = gf2.GetAdditiveIdent();

			int val = gf2_0.GetValue();
			Assert.AreEqual(0, val);
		}
		[TestMethod]
		public void GetZero_GF7()
		{
			PrimeFiniteField gf7 = new PrimeFiniteField(7);
			PrimeFiniteFieldElement gf7_0 = gf7.GetAdditiveIdent();

			int val = gf7_0.GetValue();
			Assert.AreEqual(0, val);
		}
		[TestMethod]
		public void GetZero_GF89()
		{
			PrimeFiniteField gf89 = new PrimeFiniteField(89);
			PrimeFiniteFieldElement gf89_0 = gf89.GetAdditiveIdent();

			int val = gf89_0.GetValue();
			Assert.AreEqual(0, val);
		}
		[TestMethod]
		public void GetOne_GF7()
		{
			PrimeFiniteField gf7 = new PrimeFiniteField(7);
			PrimeFiniteFieldElement gf7_1 = gf7.GetMultiplicativeIdent();

			int val = gf7_1.GetValue();
			Assert.AreEqual(1, val);
		}
		[TestMethod]
		public void GetElement_GF7()
		{
			int a = 4;
			int b = 11;
			int c = -3;
			int d = -10;
			PrimeFiniteField gf7 = new PrimeFiniteField(7);
			PrimeFiniteFieldElement gf7_a = gf7.Get(a);
			PrimeFiniteFieldElement gf7_b = gf7.Get(b);
			PrimeFiniteFieldElement gf7_c = gf7.Get(c);
			PrimeFiniteFieldElement gf7_d = gf7.Get(d);

			int val_a = gf7_a.GetValue();
			int val_b = gf7_b.GetValue();
			int val_c = gf7_c.GetValue();
			int val_d = gf7_d.GetValue();
			bool flag = val_a == val_b && val_b == val_c && val_c == val_d;
			Assert.IsTrue(flag);
		}
		[TestMethod]
		public void UnaryPlus_GF7()
		{
			int a = 3;
			PrimeFiniteField gf7 = new PrimeFiniteField(7);
			PrimeFiniteFieldElement gf7_a = gf7.Get(a);

			Assert.AreEqual(gf7_a, +gf7_a);
		}
		[TestMethod]
		public void UnaryMinus_GF7()
		{
			int a = 6;
			PrimeFiniteField gf7 = new PrimeFiniteField(7);
			PrimeFiniteFieldElement gf7_a = gf7.Get(a);
			PrimeFiniteFieldElement gf7_minus_a = -gf7_a;
			PrimeFiniteFieldElement gf7_0 = gf7.GetAdditiveIdent();

			Assert.AreEqual(gf7_a + gf7_minus_a, gf7_0);
		}

		[TestMethod]
		public void AddZero_GF7()
		{
			PrimeFiniteField gf7 = new PrimeFiniteField(7);
			PrimeFiniteFieldElement gf7_0 = gf7.GetAdditiveIdent();
			bool cnf = true;

			for (int i = 0; i < 7; i++)
			{
				var gf7_i = gf7.Get(i);
				cnf = cnf && (gf7_i + gf7_0).Equals(gf7_i);
			}

			Assert.IsTrue(cnf);
		}

		[TestMethod]
		public void MultiplyByZero_GF7()
		{
			PrimeFiniteField gf7 = new PrimeFiniteField(7);
			PrimeFiniteFieldElement gf7_0 = gf7.GetAdditiveIdent();
			bool cnf = true;

			for (int i = 0; i < gf7.GetCharacteristic(); i++)
			{
				var gf7_i = gf7.Get(i);
				cnf = cnf && (gf7_i * gf7_0).Equals(gf7_0);
			}

			Assert.IsTrue(cnf);
		}

		[TestMethod]
		public void MultiplyByOne_GF7()
		{
			PrimeFiniteField gf7 = new PrimeFiniteField(7);
			PrimeFiniteFieldElement gf7_1 = gf7.GetMultiplicativeIdent();
			bool cnf = true;

			for (int i = 0; i < gf7.GetCharacteristic(); i++)
			{
				var gf7_i = gf7.Get(i);
				cnf = cnf && (gf7_i * gf7_1).Equals(gf7_i);
			}

			Assert.IsTrue(cnf);
		}
		[TestMethod]
		public void BinaryPlus_GF7()
		{
			var gf7 = new PrimeFiniteField(7);
			var gf7_4 = gf7.Get(4);
			var gf7_5 = gf7.Get(5);
			var sum = gf7_4 + gf7_5;
			var expectedsum = gf7.Get(2);

			Assert.AreEqual(expectedsum, sum);
		}
		[TestMethod]
		public void BinaryMinus_GF7()
		{
			var gf7 = new PrimeFiniteField(7);
			var gf7_1 = gf7.Get(1);
			var gf7_5 = gf7.Get(5);
			var res = gf7_1 - gf7_5;
			var expectedres = gf7.Get(3);

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void Asterisk_GF7()
		{
			var gf7 = new PrimeFiniteField(7);
			var gf7_6 = gf7.Get(6);
			var gf7_5 = gf7.Get(5);
			var res = gf7_6 * gf7_5;
			var expectedres = gf7.Get(2);

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void Inverse_GF7()
		{
			var gf7 = new PrimeFiniteField(7);
			var gf7_4 = gf7.Get(4);
			var res = gf7_4.Inverse();
			var expectedres = gf7.Get(2);

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void Slash_GF7()
		{
			var gf7 = new PrimeFiniteField(7);
			var gf7_3 = gf7.Get(3);
			var gf7_6 = gf7.Get(6);
			var res = gf7_3 / gf7_6;
			var expectedres = gf7.Get(4);

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void Initialization_Polyn()
		{
			var p = new RPolyn(7, new int[] { 12, 7, -12, 67, -90 });
			var exp = new RPolyn(7, new int[] { 5, 0, 2, 4, 1 });

			Assert.AreEqual(exp, p);
		}
		[TestMethod]

		//cutoffmethod
		public void CutOffZeros_Polyn()
		{
			var res = new RPolyn(7, new int[] { 0, 6, 3, 0, 1, 1, 0, 0 });
			var expectedres = new RPolyn(7, new int[] { 0, 6, 3, 0, 1, 1 });

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void BinaryPlus_Polyn()
		{
			var p1 = new RPolyn(7, new int[] { 0, 6, 1, 2, 5, 1 });
			var p2 = new RPolyn(7, new int[] { 1, 4, 3, 1, 2, 3 });

			var res = p1 + p2;
			var expres = new RPolyn(7, new int[] { 1, 3, 4, 3, 0, 4 });
			Assert.AreEqual(expres, res);
		}
		[TestMethod]
		public void BinaryMinus_Polyn()
		{
			var p1 = new RPolyn(7, new int[] { 3, 1, 1, 2, 1, 5 });
			var p2 = new RPolyn(7, new int[] { 3, 5, 3, 5, });

			var res = p1 - p2;
			var expres = new RPolyn(7, new int[] { 0, 3, 5, 4, 1, 5 });
			Assert.AreEqual(expres, res);
		}

		[TestMethod]

		public void Asterisk_Polyn()
		{
			var p1 = new RPolyn(7, new int[] { 3, 1, 1 });
			var p2 = new RPolyn(7, new int[] { 3, 5, 3, 2, 4 });

			var res = p1 * p2;
			var expectedres = new RPolyn(7, new int[] { 2, 4, 3, 0, 3, 6, 4 });
			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void RemainderTest1_Polyn()
		{
			var p1 = new RPolyn(7, new int[] { 3, 2, 3 });
			var p2 = new RPolyn(7, new int[] { 1, 2, 6, 4, 4 });

			var res = p1 % p2;
			var expectedres = new RPolyn(7, new int[] { 3, 2, 3 });

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void RemainderTest2_Polyn()
		{
			var p1 = new RPolyn(7, new int[] { 3, 2, 1 });
			var p2 = new RPolyn(7, new int[] { 1, 2, 6, 4, 4 });

			var res = p2 % p1;
			var expectedres = new RPolyn(7, new int[] { 2, 3 });

			Assert.AreEqual(expectedres, res);
		}
	}
}