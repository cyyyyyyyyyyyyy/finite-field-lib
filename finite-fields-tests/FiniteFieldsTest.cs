using finite_fields;
using System.ComponentModel.DataAnnotations;

namespace FiniteFieldsTests
{
	[TestClass]
	public class FiniteFieldsTest
	{
		[TestMethod]
		public void Demo()
		{
			// create prime finite field
			int n = 7;
			var gf7 = new PrimeFiniteField(n);
			//n is prime and greater than 1

			// create finite field
			int[] factorPolyn = new int[] { 1, 1, 0, 0, 0, 0, 1 };
			var gf64 = new FiniteField(2, factorPolyn);
			//where factorPolyn - is irreducible over GF2 (or over respective prime finite field)
			//for GF64 this polynomial's degree should be 6 (where 6 is the exponent in 2^6 = 64)
			//-----------------------------------------------------------------------------------

			// get element from prime field
			gf7.Get(5);
			gf7.Get(11); // will return element of value: 11 % 7 = 4
			gf7.GetAdditiveIdent(); // zero of the field
			gf7.GetMultiplicativeIdent(); // one of the field

			// get element from field
			gf64.Get(new int[] { 1, 0, 1, 1, 0, 0 }); // 1 + x^2 + x^3 - method cuts off zeros automaticaly
			gf64.Get(new int[] { 1, 0, 1, 1, 0, 0, 1, 1 }); // as in the prime field
			gf64.GetAdditiveIdent();
			gf64.GetMultiplicativeIdent();
			//-----------------------------------------------------------------------------------

			var gf256 = new FiniteField(2, new int[] { 1, 0, 1, 1, 1, 0, 0, 0, 1 } );
			var e = gf256.Get(new byte[] { 240 }); // initialization of element from bytes array
			e.GetByte(); // self explanatory

			//-----------------------------------------------------------------------------------
			//element manipulation: prime fields

			var e1 = gf7.Get(4);
			var e2 = gf7.Get(5);

			var unPlus = +e1; // = 4 
			var unMinus = -e2; // = 2
			var sum = e1 + e2; // = 2
			var diff = e1 - e2; // = 6
			var prod = e1 * e2; // = 6
			var inv = e1.Inverse(); // = 2
			var quot = e1 / e2; // = 5

			bool fl = e1.Equals(gf7.Get(4)); // true

			//-----------------------------------------------------------------------------------
			//element manipulation

			//var e1 = gf256.Get(new byte[] { 40 } );
			//var e2 = gf256.Get(new byte[] { 241 });

			//var unPlus = +e1; // = 40
			//var unMinus = -e2; // = 241
			//var sum = e1 + e2; // = 217
			//var diff = e1 - e2; // = 217
			//var prod = e1 * e2; // = 144
			//var inv = e1.Inverse(); // = 112
			//var quot = e1 / e2; // = 218

			//bool fl = e1.Equals(gf256.Get(new byte[] { 40 })); // true
		}

		[TestMethod]
		public void EqualsTest1()
		{
			PrimeFiniteField gf7 = new PrimeFiniteField(7);
			var gf7_2_1 = gf7.Get(2);
			var gf7_2_2 = gf7.Get(2);
			var gf7_2_3 = gf7_2_1;

			var gf7_3_1 = gf7.Get(3);
			var gf7_3_2 = gf7.Get(3);
			var gf7_3_3 = gf7_2_1;

			bool b1 = gf7_2_1 == gf7_2_2; // false
			bool b2 = gf7_2_1 == gf7_2_3; // true
			bool b3 = gf7_2_2 == gf7_2_3; // false
			bool b4 = gf7_2_1.Equals(gf7_2_2); // true
			bool b5 = gf7_2_1.Equals(gf7_2_3); // true
			bool b6 = gf7_2_2.Equals(gf7_2_3); // true

			bool b7 = gf7_2_1 == gf7_3_1; // false
			bool b8 = gf7_2_1.Equals(gf7_3_1); // false

			bool cnf = !b1 && b2 && !b3 && b4 && b5 && b6 && !b7 && !b8;
			Assert.IsTrue(cnf);
		}
		[TestMethod]
		public void PFF_Equals()
		{
			var gf11_1 = new PrimeFiniteField(11);
			var gf11_2 = new PrimeFiniteField(11);
			var gf13 = new PrimeFiniteField(13);

			bool cnf = gf11_1.Equals(gf11_2) && (!gf11_1.Equals(gf13));
			Assert.IsTrue(cnf);
		}

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
			var p = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 12, 7, -12, 67, -90 });
			var exp = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 5, 0, 2, 4, 1 });

			Assert.AreEqual(exp, p);
		}
		[TestMethod]

		public void CutOffZeros_Polyn()
		{
			var res = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 0, 6, 3, 0, 1, 1, 0, 0 });
			var expectedres = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 0, 6, 3, 0, 1, 1 });

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void UnaryMinus_Polyn()
		{
			var p = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 6, 1, 3, 2, 0, 5 });

			var res = p - p;
			var expectedres = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 0 });
			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void UnaryPlus_Polyn()
		{
			var expectedres = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 6, 1, 3, 2, 0, 5 });
			var res = +expectedres;

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void BinaryPlus_Polyn()
		{
			var p1 = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 0, 6, 1, 2, 5, 1 });
			var p2 = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 1, 4, 3, 1, 2, 3 });

			var res = p1 + p2;
			var expres = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 1, 3, 4, 3, 0, 4 });
			Assert.AreEqual(expres, res);
		}
		[TestMethod]
		public void BinaryMinus_Polyn()
		{
			var p1 = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 3, 1, 1, 2, 1, 5 });
			var p2 = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 3, 5, 3, 5, });

			var res = p1 - p2;
			var expres = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 0, 3, 5, 4, 1, 5 });
			Assert.AreEqual(expres, res);
		}

		[TestMethod]

		public void Asterisk_Polyn()
		{
			var p1 = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 3, 1, 1 });
			var p2 = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 3, 5, 3, 2, 4 });

			var res = p1 * p2;
			var expectedres = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 2, 4, 3, 0, 3, 6, 4 });
			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void RemainderTest1_Polyn()
		{
			var p1 = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 3, 2, 3 });
			var p2 = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 1, 2, 6, 4, 4 });

			var res = p1 % p2;
			var expectedres = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 3, 2, 3 });

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void RemainderTest2_Polyn()
		{
			var p1 = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 3, 2, 1 });
			var p2 = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 1, 2, 6, 4, 4 });

			var res = p2 % p1;
			var expectedres = new RPolyn<PrimeFiniteFieldElement>(7, new int[] { 2, 3 });

			Assert.AreEqual(expectedres, res);
		}

		[TestMethod]
		public void GetZero_GF16()
		{
			var gf16 = new FiniteField(2, new int[] { 1, 1, 0, 0, 1 });
			var res = gf16.GetAdditiveIdent();
			var expectedres = new FiniteFieldElement(2, new RPolyn<PrimeFiniteFieldElement>(2, new int[] { 0 }), gf16);

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void GetOne_GF16()
		{
			var gf16 = new FiniteField(2, new int[] { 1, 1, 0, 0, 1 });
			var res = gf16.GetAdditiveIdent();
			var expectedres = new FiniteFieldElement(2, new RPolyn<PrimeFiniteFieldElement>(2, new int[] { 0 }), gf16);

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void Get_GF16()
		{
			var gf16 = new FiniteField(2, new int[] { 1, 1, 0, 0, 1 });
			var res = gf16.Get(new int[] {0, 1, 1, 0, 1, 1});
			var expectedres = new FiniteFieldElement(2, new RPolyn<PrimeFiniteFieldElement>(2, new int[] { 1, 1 }), gf16);

			Assert.AreEqual(expectedres, res);	
		}
		[TestMethod]
		public void UnaryPlus_GF16()
		{
			var gf16 = new FiniteField(2, new int[] { 1, 1, 0, 0, 1 });
			var expectedres = gf16.Get(new int[] { 0, 1, 1, 0, 1, 1 });
			var res = +expectedres;

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void UnaryMinus_GF16()
		{
			var gf16 = new FiniteField(2, new int[] { 1, 1, 0, 0, 1 });
			var e = gf16.Get(new int[] { 0, 1, 1, 1});
			var res = e - e;
			var expectedres = gf16.GetAdditiveIdent();

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void BinaryPlus_GF16()
		{
			var gf16 = new FiniteField(2, new int[] { 1, 1, 0, 0, 1 });
			var a = gf16.Get(new int[] { 0, 1, 1});
			var b = gf16.Get(new int[] { 0, 1, 0, 1});

			var res = a + b;
			var expectedres = gf16.Get(new int[] { 0, 0, 1, 1});

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void BinaryMinus_GF16()
		{
			var gf16 = new FiniteField(2, new int[] { 1, 1, 0, 0, 1 });
			var a = gf16.Get(new int[] { 0, 1, 1 });
			var b = gf16.Get(new int[] { 0, 1, 1, 1 });

			var res = a - b;
			var expectedres = gf16.Get(new int[] {0, 0, 0, 1});
		}
		[TestMethod]
		public void Asterisk_GF16()
		{
			var gf16 = new FiniteField(2, new int[] { 1, 1, 0, 0, 1 });
			var a = gf16.Get(new int[] { 1, 1, 0, 1 });
			var b = gf16.Get(new int[] { 0, 1, 1, 1 });

			var res = a * b;
			var expectedres = gf16.Get(new int[] { 0, 0, 0, 1});

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]

		public void Inverse_GF16()
		{
			var gf16 = new FiniteField(2, new int[] { 1, 1, 0, 0, 1 });
			var a = gf16.Get(new int[] { 1, 0, 0, 1 });
			var inv_a = a.Inverse();

			var res = inv_a * a;
			var expectedres = gf16.GetMultiplicativeIdent();

			Assert.AreEqual(expectedres, res);
		}
		[TestMethod]
		public void Slash_GF16()
		{
			var gf16 = new FiniteField(2, new int[] { 1, 1, 0, 0, 1 });
			var d1 = gf16.Get(new int[] { 1, 1, 0, 1});
			var d2 = gf16.Get(new int[] { 0, 0, 0, 1});

			var res = d1 / d2;
			var expectedres = gf16.Get(new int[] { 1, 1});

			Assert.AreEqual(expectedres, res);
		}

		[TestMethod]
		public void GetBytesTest_GF256()
		{
			var gf256 = new FiniteField(2, new int[] { 1, 0, 1, 1, 1, 0, 0, 0, 1}); 
			var e = gf256.Get(new int[] { 1, 1, 0, 1});
			byte[] res = e.GetByte();
			byte[] expectedres = new byte[1] { 11 };

			bool cnf = true;
			for (int i = 0; i < res.Length; i++)
				cnf = cnf && res[i].Equals(expectedres[i]);

			Assert.IsTrue(cnf);
		}
		[TestMethod]
		public void GetBytesTest_GF512()
		{
			var gf512 = new FiniteField(2, new int[] { 1, 0, 0, 0, 1, 0, 0, 0, 0, 1 });
			var e = gf512.Get(new int[] { 1, 0, 1, 1, 0, 1, 0, 0, 1 });
			bool flag = false;
			try
			{
				byte[] res = e.GetByte();
			} catch (ArgumentException ex)
			{
				flag = true;
			}
			Assert.IsTrue(flag);
		}

		[TestMethod]
		public void GetBytesTest_GF2_24()
		{
			var gf2_24 = new FiniteField(2, 
				new int[] { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }); //x^25 + x^3 + 1
			var e = gf2_24.Get(
				new int[] { 1, 1, 0, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 1 });

			byte[] res = e.GetByte();
			byte[] expectedres = new byte[3] { 147, 230, 165 };

			bool cnf = true;
			for (int i = 0; i < res.Length; i++)
				cnf = cnf && res[i].Equals(expectedres[i]);

			Assert.IsTrue(cnf);
		}

		[TestMethod]
		public void GetFromBytesTest()
		{
			//rewrite
			bool cnf = true;

			var gf256 = new FiniteField(2, new int[] { 1, 0, 1, 1, 1, 0, 0, 0, 1 });
			var e2 = gf256.Get(new int[] { 1, 0, 1, 1, 0, 1, 0, 0, 1 }); //length is 9 so it will get the remainder
			byte[] bytes2 = e2.GetByte();
			var e2_fromBytes = gf256.Get(bytes2);
			cnf = cnf && e2_fromBytes.Equals(e2);

			var gf2_24 = new FiniteField(2,
				new int[] { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
			var e3 = gf2_24.Get(
				new int[] { 1, 1, 0, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 1 });

			byte[] bytes3 = e3.GetByte();
			var e3_fromBytes = gf2_24.Get(bytes3);
			cnf = cnf && e3_fromBytes.Equals(e3);

			Assert.IsTrue(cnf);
		}

		[TestMethod]
		public void PolynFFE_BinaryPlus()
		{
			var gf256 = new FiniteField(2, new int[] { 1, 0, 1, 1, 1, 0, 0, 0, 1 });
			var p1 = new RPolyn<FiniteFieldElement>(2, new FiniteFieldElement[] { 
				gf256.Get(new byte[] { 56 }),
				gf256.Get(new byte[] { 140 }),
				gf256.Get(new byte[] { 1, }),
				gf256.Get(new byte[] { 13 })
			});

			var p2 = new RPolyn<FiniteFieldElement>(2, new FiniteFieldElement[] {
				gf256.Get(new byte[] { 0 }),
				gf256.Get(new byte[] { 137 }),
				gf256.Get(new byte[] { 201 }),
				gf256.Get(new byte[] { 1 })
			});

			var res = p1 + p2;
			//var bres = res.GetValue().Select(x => x.GetByte()).ToArray();
			var expectedres1 = new RPolyn<FiniteFieldElement>(2, new FiniteFieldElement[] {
				gf256.Get(new byte[] { 56 }) + gf256.Get(new byte[] { 0 }),
				gf256.Get(new byte[] { 140 }) + gf256.Get(new byte[] { 137 }),
				gf256.Get(new byte[] { 1, }) + gf256.Get(new byte[] { 201 }),
				gf256.Get(new byte[] { 13 }) + gf256.Get(new byte[] { 1 })
			});
			var expectedres2 = new RPolyn<FiniteFieldElement>(2, new FiniteFieldElement[] {
				gf256.Get(new byte[] { 56 }),
				gf256.Get(new byte[] { 5 }),
				gf256.Get(new byte[] { 200, }),
				gf256.Get(new byte[] { 12 })
			}); // it's just xor

			bool cnf = res.Equals(expectedres1) && expectedres1.Equals(expectedres2);
			Assert.IsTrue(cnf);
		}
		[TestMethod]
		public void PolynFFE_BinaryMinus()
		{
			var gf256 = new FiniteField(2, new int[] { 1, 0, 1, 1, 1, 0, 0, 0, 1 });
			var p1 = new RPolyn<FiniteFieldElement>(2, new FiniteFieldElement[] {
				gf256.Get(new byte[] { 56 }),
				gf256.Get(new byte[] { 140 }),
				gf256.Get(new byte[] { 1 }),
				gf256.Get(new byte[] { 13 })
			});

			var p2 = new RPolyn<FiniteFieldElement>(2, new FiniteFieldElement[] {
				gf256.Get(new byte[] { 0 }),
				gf256.Get(new byte[] { 137 }),
				gf256.Get(new byte[] { 201 }),
				gf256.Get(new byte[] { 1 })
			});

			var res = p1 - p2;
			//var bres = res.GetValue().Select(x => x.GetByte()).ToArray();
			var expectedres1 = new RPolyn<FiniteFieldElement>(2, new FiniteFieldElement[] {
				gf256.Get(new byte[] { 56 }) - gf256.Get(new byte[] { 0 }),
				gf256.Get(new byte[] { 140 }) - gf256.Get(new byte[] { 137 }),
				gf256.Get(new byte[] { 1 }) - gf256.Get(new byte[] { 201 }),
				gf256.Get(new byte[] { 13 }) - gf256.Get(new byte[] { 1 })
			});
			var expectedres2 = new RPolyn<FiniteFieldElement>(2, new FiniteFieldElement[] {
				gf256.Get(new byte[] { 56 }),
				gf256.Get(new byte[] { 5 }),
				gf256.Get(new byte[] { 200, }),
				gf256.Get(new byte[] { 12 })
			}); // it's just xor

			bool cnf = res.Equals(expectedres1) && expectedres1.Equals(expectedres2);
			Assert.IsTrue(cnf);
		}
		[TestMethod]
		public void PolynFFE_Asterisk()
		{
			var gf256 = new FiniteField(2, new int[] { 1, 0, 1, 1, 1, 0, 0, 0, 1 });
			var p1 = new RPolyn<FiniteFieldElement>(2, new FiniteFieldElement[] {
				gf256.Get(new byte[] { 37 }),
				gf256.Get(new byte[] { 212 }),
				gf256.Get(new byte[] { 13 }),
				gf256.Get(new byte[] { 70 })
			});

			var p2 = new RPolyn<FiniteFieldElement>(2, new FiniteFieldElement[] {
				gf256.Get(new byte[] { 1 }),
				gf256.Get(new byte[] { 0 }),
				gf256.Get(new byte[] { 123 }),
				gf256.Get(new byte[] { 51 })
			});

			var res = p1 * p2;
			var expectedres = new RPolyn<FiniteFieldElement>(2, new FiniteFieldElement[] {
				gf256.Get(new byte[] { 37 }),
				gf256.Get(new byte[] { 212 }),
				gf256.Get(new byte[] { 92 }) ,
				gf256.Get(new byte[] { 101 }),
				gf256.Get(new byte[] { 74 }),
				gf256.Get(new byte[] { 214 }),
				gf256.Get(new byte[] { 246 })
			});
			Assert.IsTrue(res.Equals(expectedres));
		}

		[TestMethod]
		public void PolynFFE_Remainder()
		{
			var gf256 = new FiniteField(2, new int[] { 1, 0, 1, 1, 1, 0, 0, 0, 1 });
			var p1 = new RPolyn<FiniteFieldElement>(2, new FiniteFieldElement[] {
				gf256.Get(new byte[] { 37 }),
				gf256.Get(new byte[] { 212 }),
				gf256.Get(new byte[] { 13 }),
				gf256.Get(new byte[] { 70 })
			});

			var p2 = new RPolyn<FiniteFieldElement>(2, new FiniteFieldElement[] {
				gf256.Get(new byte[] { 1 }),
				gf256.Get(new byte[] { 0 }),
				gf256.Get(new byte[] { 123 }),
				gf256.Get(new byte[] { 51 })
			});

			var res = p2 % p1;
			var expectedres = new RPolyn<FiniteFieldElement>(2, new FiniteFieldElement[] {
				gf256.Get(new byte[] { 30 }),
				gf256.Get(new byte[] { 199 }),
				gf256.Get(new byte[] { 74 })
			});
			Assert.IsTrue(res.Equals(expectedres));
		}
	}
}