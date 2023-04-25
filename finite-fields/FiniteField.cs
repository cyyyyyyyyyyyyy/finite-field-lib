using finite_fields;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace finite_fields
{
	public class FiniteField
	{
		private readonly int _primeChar;
		private readonly int _dim;
		private readonly RPolyn _polyn;

		public FiniteField(int PrimeFieldCharacterictic, int[] IntegerPolynomial)
		{
			if (PrimeFieldCharacterictic < 1)
				throw new ArgumentException("Text");

			_primeChar = PrimeFieldCharacterictic;

			_polyn = new RPolyn(_primeChar, IntegerPolynomial);
			_dim = _polyn.GetLength() - 1;

		}
		public int GetCharacteristic() => _primeChar;
		public int GetDimension() => _dim;
		public int[] GetPolynomial() => _polyn.GetValue();

		public FiniteFieldElement GetAdditiveNeutral()
		{
			return new FiniteFieldElement(_primeChar, new RPolyn(_primeChar, new int[] { 0 }), _polyn);
		}
		public FiniteFieldElement GetMultiplicativeNeutral()
		{
			return new FiniteFieldElement(_primeChar, new RPolyn(_primeChar, new int[] { 1 }), _polyn);
		}
		public FiniteFieldElement Get(int[] IntegerPolynomial)
		{
			//throw new NotImplementedException();
			return new FiniteFieldElement(_primeChar, new RPolyn(_primeChar, IntegerPolynomial), _polyn);
		}
	}

	public class FiniteFieldElement
	{
		private readonly int _primeChar;
		private readonly int _dim;
		private readonly RPolyn _value;
		private readonly RPolyn _fpolyn; // factoriazation polynomial; or maybe i just need _field there

		public FiniteFieldElement(int PrimeFieldCharacteristic, RPolyn PolynomialValue, RPolyn FactorPolynomial)
		{
			if (PrimeFieldCharacteristic < 1)
				throw new ArgumentException("Text");

			if (!(PrimeFieldCharacteristic == PolynomialValue.GetCharacteristic())
				|| !(PrimeFieldCharacteristic == FactorPolynomial.GetCharacteristic()))
				throw new ArgumentException("Text");


			_primeChar = PrimeFieldCharacteristic;
			_fpolyn = FactorPolynomial;
			_dim = _fpolyn.GetLength() - 1;

			_value = PolynomialValue % _fpolyn;			
		}

		public int GetCharacteristic() => _primeChar;
		public int GetDimension() => _dim;
		public int[] GetValue() => _value.GetValue();
		private static RPolyn ModularExp(RPolyn b, int exp, RPolyn m)
		{
			//int exp = _primeChar - 2;
			var gf = new FiniteField(b.GetCharacteristic(), m.GetValue());
			var res = gf.GetMultiplicativeNeutral()._value;
			while (exp > 0)
			{
				if (exp % 2 == 1)
					res = (res * b) % m;
				exp = exp >> 1;
				b = (b * b) % m;
			}
			return res;
		}
		public static FiniteFieldElement operator +(FiniteFieldElement pe)
			=> pe;
		public static FiniteFieldElement operator -(FiniteFieldElement pe)
			=> new FiniteFieldElement(pe._primeChar, -pe._value, pe._fpolyn);
		public static FiniteFieldElement operator +(FiniteFieldElement pa1, FiniteFieldElement pa2)
		{
			if (!pa1._primeChar.Equals(pa2._primeChar) || !pa1._dim.Equals(pa2._dim)) // do i need to check factor polynimial equality?
				throw new ArgumentException("text");
			return new FiniteFieldElement(pa1._primeChar, pa1._value + pa2._value, pa1._fpolyn);
		}
		public static FiniteFieldElement operator -(FiniteFieldElement pm, FiniteFieldElement ps)
		{
			return pm + (-ps);
		}
		public static FiniteFieldElement operator *(FiniteFieldElement m1, FiniteFieldElement m2)
		{
			if (!m1._primeChar.Equals(m2._primeChar) || !m1._dim.Equals(m2._dim))
				throw new ArgumentException("text");

			return new FiniteFieldElement(m1._primeChar, m1._value * m2._value, m1._fpolyn);
		}
		public FiniteFieldElement Inverse()
		{
			var gf = new FiniteField(this._primeChar, this._fpolyn.GetValue());
			if (this.Equals(gf.GetAdditiveNeutral()))
				throw new ArgumentException("text");

			return new FiniteFieldElement(this._primeChar, ModularExp(this._value, (int)(Math.Pow(this._primeChar, this._dim)) - 2, this._fpolyn), this._fpolyn);
		}
		public static FiniteFieldElement operator /(FiniteFieldElement d1, FiniteFieldElement d2)
		{
			return d1 * d2.Inverse();
		}
		public override bool Equals(object? obj)
		{
			if (obj == null)
				return false;
			else
			{
				if (obj is FiniteFieldElement)
				{
					if (!(obj as FiniteFieldElement)._primeChar.Equals(this._primeChar)
						||!(obj as FiniteFieldElement)._dim.Equals(this._dim)) // char-c and dim check
						return false;

					if (!(obj as FiniteFieldElement)._value.Equals(this._value)) // values check
						return false;

					return true;
				}
				return false;
			}
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
	class BinaryFiniteField : FiniteField
	{
		public BinaryFiniteField(int[] IntegerPolynomial) : base(2, IntegerPolynomial) { }

		public byte[] GetByte()
		{
			throw new NotImplementedException();
		}
	}
}
