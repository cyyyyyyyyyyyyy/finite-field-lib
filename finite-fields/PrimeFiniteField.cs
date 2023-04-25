using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace finite_fields
{
	public class PrimeFiniteField
	{
		private readonly int _primeChar;
		public PrimeFiniteField(int PrimeFieldCharacteristic)
		{
			if (PrimeFieldCharacteristic < 1)
				throw new ArgumentException("Text");

			_primeChar = PrimeFieldCharacteristic;
		}

		public int GetCharacteristic() => _primeChar;
		public PrimeFiniteFieldElement GetAdditiveIdent() => new(_primeChar, 0);
		public PrimeFiniteFieldElement GetMultiplicativeIdent() => new(_primeChar, 1);
		public PrimeFiniteFieldElement Get(int Value) => new(_primeChar, Value); // check value correctness

	}
	public class PrimeFiniteFieldElement
	{
		private readonly int _primeChar; //unsigned int?
		private readonly int _value;
		public PrimeFiniteFieldElement(int PrimeFieldCharacterictic, int Value)
		{
			// check _primechar is prime and _value < _char'
			if (PrimeFieldCharacterictic < 1)
				throw new ArgumentException("Text");

			_primeChar = PrimeFieldCharacterictic;
			_value = Value % _primeChar;
			if (_value < 0)
				_value += _primeChar;
		}
		public int GetValue() => _value;
		public int GetCharacteristic() => _primeChar;
		private static int ModularExp(int b, int exp, int m)
		{
			//int exp = _primeChar - 2;
			int res = 1;
			while (exp > 0)
			{
				if (exp % 2 == 1)
					res = (res * b) % m;
				exp = exp >> 1;
				b = (b * b) % m;
			}
			return res;
		}
		public static PrimeFiniteFieldElement operator +(PrimeFiniteFieldElement e)
			=> e;
		public static PrimeFiniteFieldElement operator +(PrimeFiniteFieldElement a1, PrimeFiniteFieldElement a2)
		{
			if (!a1._primeChar.Equals(a2._primeChar))
				throw new ArgumentException("Incorrect characteristics");
			return new PrimeFiniteFieldElement(a1._primeChar, (a1._value + a2._value) % a1._primeChar);
		}

		public static PrimeFiniteFieldElement operator *(PrimeFiniteFieldElement m1, PrimeFiniteFieldElement m2)
		{
			if (!m1._primeChar.Equals(m2._primeChar))
				throw new ArgumentException("Incorrect characteristics");
			return new PrimeFiniteFieldElement(m1._primeChar, (m1._value * m2._value) % m1._primeChar);
		}

		public static PrimeFiniteFieldElement operator -(PrimeFiniteFieldElement e)
			=> new PrimeFiniteFieldElement(e._primeChar, e._value == 0 ? 0 : (e._primeChar - e._value));

		public PrimeFiniteFieldElement Inverse()
		{
			if (this._value == 0)
				throw new ArgumentException("Cannot find inverse to additive neutral");
			return new PrimeFiniteFieldElement(_primeChar, ModularExp(this._value, this._primeChar - 2, this._primeChar));
		}

		public static PrimeFiniteFieldElement operator -(PrimeFiniteFieldElement m, PrimeFiniteFieldElement s)
			=> m + (-s);

		public static PrimeFiniteFieldElement operator /(PrimeFiniteFieldElement d1, PrimeFiniteFieldElement d2)
			=> d1 * d2.Inverse();

		public override bool Equals(object? obj)
		{
			if (obj == null)
				return false;
			if (obj is PrimeFiniteFieldElement)
				if ((obj as PrimeFiniteFieldElement)._primeChar.Equals(this._primeChar)
					&& (obj as PrimeFiniteFieldElement)._value.Equals(this._value))
					return true;

			return false;
		}
	}
}
