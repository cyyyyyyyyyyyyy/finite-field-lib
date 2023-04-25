using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace finite_fields
{
	public class PrimeFiniteField : IFiniteField<PrimeFiniteFieldElement>
	{
		private readonly int _primeChar;
		public PrimeFiniteField(int PrimeFieldCharacteristic)
		{
			//PrimeFieldCharacteristic should be prime
			if (PrimeFieldCharacteristic < 1)
				throw new ArgumentException("Error in PrimeFiniteField: PrimeFieldCharacteristic should be prime and greater than 1");

			_primeChar = PrimeFieldCharacteristic;
		}

		public int GetCharacteristic() => _primeChar;
		public PrimeFiniteFieldElement GetAdditiveIdent() => new(_primeChar, 0, this);
		public PrimeFiniteFieldElement GetMultiplicativeIdent() => new(_primeChar, 1, this);
		public PrimeFiniteFieldElement Get(int IntegerValue) => new(_primeChar, IntegerValue, this); // check value correctness

		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(obj, null))
				return false;
			//if (obj.GetType() != this.GetType())
			//	return false;
			if (obj is not PrimeFiniteField ppf)
				return false;

			if (ppf._primeChar.Equals(this._primeChar))
				return true;

			return false;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
	public class PrimeFiniteFieldElement : IFiniteFieldElement<PrimeFiniteFieldElement>
	{
		private readonly int _primeChar;
		private readonly PrimeFiniteField _field;
		private readonly int _value;
		public PrimeFiniteFieldElement(int PrimeFieldCharacterictic, int IntegerValue, PrimeFiniteField FiniteField)
		{
			//clone FiniteField
			//PrimeFieldCharacteristic should be prime
			if (PrimeFieldCharacterictic < 1)
				throw new ArgumentException("Error in PrimeFiniteFieldElement: PrimeFieldCharacteristic should be prime and greater than 1");

			_primeChar = PrimeFieldCharacterictic;

			if (_primeChar != FiniteField.GetCharacteristic())
				throw new ArgumentException("Error in PrimeFiniteFieldElement: FiniteField.FiniteField.GetCharacteristic() should equal PrimeFieldCharacteristic");

			_field = FiniteField;
			_value = IntegerValue % _primeChar;
			if (_value < 0)
				_value += _primeChar;
		}
		//public int[] InternalGetValue() => new[] { _value };
		public int GetValue() => _value;
		public int GetCharacteristic() => _primeChar;
		public IFiniteField<PrimeFiniteFieldElement> GetField() => _field;
		public bool IsWellDefinedWith(PrimeFiniteFieldElement other) => this._field.Equals(other._field);
		private static int ModularExp(int b, int exp, int m)
		{
			//need to refactor - not effective
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
			if (!a1.IsWellDefinedWith(a2))
				throw new ArgumentException("Incorrect characteristics");
			return new PrimeFiniteFieldElement(a1._primeChar, (a1._value + a2._value) % a1._primeChar, a1._field);
		}

		public static PrimeFiniteFieldElement operator *(PrimeFiniteFieldElement m1, PrimeFiniteFieldElement m2)
		{
			if (!m1.IsWellDefinedWith(m2))
				throw new ArgumentException("Incorrect characteristics");
			return new PrimeFiniteFieldElement(m1._primeChar, (m1._value * m2._value) % m1._primeChar, m1._field);
		}

		public static PrimeFiniteFieldElement operator -(PrimeFiniteFieldElement e)
			=> new PrimeFiniteFieldElement(e._primeChar, e._value == 0 ? 0 : (e._primeChar - e._value), e._field);

		public PrimeFiniteFieldElement Inverse()
		{
			if (this._value == 0)
				throw new ArgumentException("Cannot find multiplicative inverse to additive identity");
			return new PrimeFiniteFieldElement(_primeChar, ModularExp(this._value, this._primeChar - 2, this._primeChar), this._field); //o_o
		}

		public static PrimeFiniteFieldElement operator -(PrimeFiniteFieldElement m, PrimeFiniteFieldElement s)
			=> m + (-s);

		public static PrimeFiniteFieldElement operator /(PrimeFiniteFieldElement d1, PrimeFiniteFieldElement d2)
			=> d1 * d2.Inverse();

		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(obj, null))
				return false;
			//if (obj.GetType() != this.GetType())
			//	return false;
			if (obj is not PrimeFiniteFieldElement pffe) // pffe = prime finite field element
				return false;

			if (pffe.IsWellDefinedWith(this) //self-explanatory: correctness and value check
				&& pffe._value.Equals(this._value))
				return true;

			return false;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
