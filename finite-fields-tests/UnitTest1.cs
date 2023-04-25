using finite_fields;
namespace finite_fields_tests
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
	}
}