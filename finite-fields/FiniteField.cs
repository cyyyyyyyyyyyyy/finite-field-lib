using System.Numerics;

namespace finite_fields
{
	public class FiniteField : IFiniteField<FiniteFieldElement>
	{
		private readonly int _primeChar;
		private readonly int _dim;
		private readonly RPolyn<PrimeFiniteFieldElement> _polyn;

		public FiniteField(int PrimeFieldCharacteristic, int[] IntegerPolynomial)
		{
			//PrimeFieldCharacteristic should be prime
			//IntegerPolynomial should be irreducible
			if (PrimeFieldCharacteristic < 1)
				throw new ArgumentException("Error in FiniteField: PrimeFieldCharacteristic should be prime and greater than 1");

			_primeChar = PrimeFieldCharacteristic;

			//int[] copiedPolyn = new int[IntegerPolynomial.Length];
			//Array.Copy(IntegerPolynomial, copiedPolyn, IntegerPolynomial.Length);
			_polyn = new RPolyn<PrimeFiniteFieldElement>(_primeChar, IntegerPolynomial);
			_dim = _polyn.GetLength() - 1;
		}
		public FiniteField(int PrimeFieldCharacteristic, PrimeFiniteFieldElement[] ElementPolynomial)
		{
			//PrimeFieldCharacteristic should be prime
			//IntegerPolynomial should be irreducible
			if (PrimeFieldCharacteristic < 1)
				throw new ArgumentException("Error in FiniteField: PrimeFieldCharacteristic should be prime and greater than 1");

			_primeChar = PrimeFieldCharacteristic;

			//PrimeFiniteFieldElement[] copiedPolyn = new PrimeFiniteFieldElement[ElementPolynomial.Length];
			//Array.Copy(ElementPolynomial, copiedPolyn, ElementPolynomial.Length);
			_polyn = new RPolyn<PrimeFiniteFieldElement>(_primeChar, ElementPolynomial);
			_dim = _polyn.GetLength();
		}
		public int GetCharacteristic() => _primeChar;
		public int GetDimension() => _dim;
		public int[] GetIntPolynomial() => _polyn.GetValue().Select(x => x.GetValue()).ToArray();
		public RPolyn<PrimeFiniteFieldElement> GetPolynomial() => _polyn;

		public FiniteFieldElement GetAdditiveIdent()
			=> new(_primeChar, new RPolyn<PrimeFiniteFieldElement>(_primeChar, new int[] { 0 }), this);
		public FiniteFieldElement GetMultiplicativeIdent()
			=> new(_primeChar, new RPolyn<PrimeFiniteFieldElement>(_primeChar, new int[] { 1 }), this);
		public FiniteFieldElement Get(int[] IntegerPolynomial)
			=> new(_primeChar, new RPolyn<PrimeFiniteFieldElement>(_primeChar, IntegerPolynomial), this);
		public FiniteFieldElement Get(byte[] bytes)
		{
			if ((this._primeChar != 2)||(this._dim % 8 != 0))
				throw new ArgumentException("Text");

			string[] strbytes = new string[bytes.Length];//representation of byte array as correct binary string array
			for (int i = 0; i < bytes.Length; i++)
			{
				char[] charArr = Convert.ToString(bytes[i], 2)/*.PadLeft(8, '0')*/.ToCharArray();
				Array.Reverse(charArr);
				strbytes[i] = new string(charArr);
			}

			//int lastIndex = strbytes.Length - 1;
			string binString;
			int[] res = new int[strbytes.Length * 8 /*+ strbytes[lastIndex].Length*/];
			for (int i = 0; i < strbytes.Length; i++)
			{
				binString = strbytes[i];
				for (int j = 0; j < binString.Length; j++)
				{
					res[i * 8 + j] = Convert.ToInt32(binString[j]);
				}
				for (int j = binString.Length; j < 8; j++)
				{
					res[i * 8 + j] = 0;
				}
			}

			//cutting zeros off in the constructor
			return new FiniteFieldElement(this._primeChar, new RPolyn<PrimeFiniteFieldElement>(_primeChar, res), this);
		}
		//public RPolyn<FiniteFieldElement> GetPolyn(byte[] bytes)
		//{
		//	throw new NotImplementedException();
		//}

		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(obj, null))
				return false;
			//if (obj.GetType() != this.GetType())
			//	return false;
			if (obj is not FiniteField ff)
				return false;

			if (ff._primeChar.Equals(this._primeChar)
				&& ff._dim.Equals(this._dim)
				&& ff._polyn.Equals(this._polyn))
				return true;

			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	public class FiniteFieldElement : IFiniteFieldElement<FiniteFieldElement>
	{
		private readonly int _primeChar;
		private readonly int _dim;
		private readonly RPolyn<PrimeFiniteFieldElement> _value;
		private readonly RPolyn<PrimeFiniteFieldElement> _fpolyn; // factoriazation polynomial
		private readonly FiniteField _field;

		public FiniteFieldElement(int PrimeFieldCharacteristic, RPolyn<PrimeFiniteFieldElement> PolynValue, FiniteField FiniteField)
		{
			if (PrimeFieldCharacteristic < 1)
				throw new ArgumentException("Text");

			if (!(PrimeFieldCharacteristic == PolynValue.GetCharacteristic())
				|| !(PrimeFieldCharacteristic == FiniteField.GetCharacteristic()))
				throw new ArgumentException("Text");


			_primeChar = PrimeFieldCharacteristic;
			_field = FiniteField;
			_fpolyn = FiniteField.GetPolynomial();
			_dim = _fpolyn.GetLength() - 1;

			_value = PolynValue % _fpolyn;
		}		
		public int[] GetValue() => _value.GetValue().Select(x => x.GetValue()).ToArray();
		public int GetCharacteristic() => _primeChar;
		public IFiniteField<FiniteFieldElement> GetField() => _field;
		public bool IsWellDefinedWith(FiniteFieldElement other) => this._field.Equals(other._field);
		public int GetDimension() => _dim;
		public byte[] GetByte()
		{
			if ((this._primeChar != 2) || (this._dim % 8 != 0))
				throw new ArgumentException();

			var val = this.GetValue();
			byte[] bytes = new byte[((val.Length - 1) / 8) + 1];
			int b = 1;
			int k = 0;
			for (int i = 0; i < val.Length; i++)
			{
				bytes[k] += Convert.ToByte(val[i] * b);
				b *= 2;
				if (b % 256 == 0)
				{
					b = 1;
					k++;
				}
			}

			return bytes;
		}
		private static RPolyn<PrimeFiniteFieldElement> ModularExp(RPolyn<PrimeFiniteFieldElement> b, int exp, RPolyn<PrimeFiniteFieldElement> m)
		{
			//int exp = _primeChar - 2;
			var gf = new FiniteField(b.GetCharacteristic(), m.GetValue());
			//var gf = new FiniteField(b.GetCharacteristic(), m);
			var res = gf.GetMultiplicativeIdent()._value;
			while (exp > 0)
			{
				if (exp % 2 == 1)
					res = (res * b) % m;
				exp >>= 1;
				b = (b * b) % m;
			}
			return res;
		}
		public static FiniteFieldElement operator +(FiniteFieldElement pe)
			=> pe;
		public static FiniteFieldElement operator -(FiniteFieldElement pe)
			=> new (pe._primeChar, -pe._value, pe._field);
		public static FiniteFieldElement operator +(FiniteFieldElement pa1, FiniteFieldElement pa2)
		{
			//if (!pa1._primeChar.Equals(pa2._primeChar) || !pa1._dim.Equals(pa2._dim)) // do i need to check factor polynimial equality?
			//	throw new ArgumentException("text");
			if (!pa1.IsWellDefinedWith(pa2))
				throw new ArgumentException("Text");
			return new (pa1._primeChar, pa1._value + pa2._value, pa1._field);
		}
		public static FiniteFieldElement operator -(FiniteFieldElement pm, FiniteFieldElement ps)
		{
			return pm + (-ps);
		}
		public static FiniteFieldElement operator *(FiniteFieldElement m1, FiniteFieldElement m2)
		{
			//if (!m1._primeChar.Equals(m2._primeChar) || !m1._dim.Equals(m2._dim))
			//	throw new ArgumentException("text");
			if (!m1.IsWellDefinedWith(m2))
				throw new ArgumentException("Text");
			return new (m1._primeChar, m1._value * m2._value, m1._field);
		}
		public FiniteFieldElement Inverse()
		{
			//var gf = new FiniteField(this._primeChar, this._fpolyn.GetValue());
			if (this.Equals(this._field.GetAdditiveIdent()))
				throw new ArgumentException("text");

			return new (this._primeChar, ModularExp(this._value, (int)(Math.Pow(this._primeChar, this._dim)) - 2, this._fpolyn), this._field);
		}
		public static FiniteFieldElement operator /(FiniteFieldElement d1, FiniteFieldElement d2)
		{
			return d1 * d2.Inverse();
		}
		public override bool Equals(object? obj)
		{
			if (obj == null)
				return false;
			//if (obj.GetType() != this.GetType())
			//	return false;
			if (obj is not FiniteFieldElement ffe)
				return false;

			//if (!ffe._primeChar.Equals(this._primeChar)
			//	|| !ffe._dim.Equals(this._dim)) // char-c and dim check
			//	return false;
			if (!this.IsWellDefinedWith(ffe))
				return false;

			if (!ffe._value.Equals(this._value)) // values check
				return false;

			return true;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
