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
		private readonly PrimeFiniteFieldElement[] _polyn;
		private readonly PrimeFiniteField _primefield;

		public FiniteField(int PrimeFieldCharacterictic, int[] IntegerPolynomial)
		{
			if (PrimeFieldCharacterictic < 1)
				throw new ArgumentException("Text");

			_primeChar = PrimeFieldCharacterictic;
			_dim = IntegerPolynomial.Length - 1;
			var polyn = new PrimeFiniteFieldElement[_dim];
			for (int i = 1; i < _dim; i++)
				polyn[i] = new(_primeChar, IntegerPolynomial[i]);
			_polyn = polyn;//new FiniteFieldElement(_primeChar, polyn);

			_primefield = new PrimeFiniteField(PrimeFieldCharacterictic);
		}
		public int GetCharacteristic() => _primeChar;
		public int GetDimension() => _dim;
		public int[] GetPolynomial()
		{
			//throw new NotImplementedException();
			var res = new int[_dim];
			for (int i = 1; i < _dim; i++)
				res[i] = this._polyn[i].GetValue();

			return res;
		}

		public FiniteFieldElement GetZero()
		{
			//return new FiniteFieldElement(_primeChar, new int[_dim]);
			var val = new PrimeFiniteFieldElement[_dim];
			for (int i = 0; i < _dim; i++)
				val[i] = _primefield.GetAdditiveIdent();

			return new FiniteFieldElement(_primeChar, val, _polyn);
		}
		public FiniteFieldElement GetOne()
		{
			var val = new PrimeFiniteFieldElement[_dim];
			for (int i = 1; i < _dim; i++)
				val[i] = _primefield.GetAdditiveIdent();
			val[0] = _primefield.GetMultiplicativeIdent();

			return new FiniteFieldElement(_primeChar, val, _polyn);
		}
	}

	public class FiniteFieldElement
	{
		private readonly int _primeChar;
		private readonly int _dim;
		private readonly PrimeFiniteFieldElement[] _value;
		private readonly PrimeFiniteFieldElement[] _fpolyn;

		public FiniteFieldElement(int PrimeFieldCharacteristic, PrimeFiniteFieldElement[] PolynomialValue, PrimeFiniteFieldElement[] FactorPolynomial)
		{
			if (PrimeFieldCharacteristic < 1)
				throw new ArgumentException("Text");
			for (int i = 0; i < PolynomialValue.Length; i++)
				if (!PolynomialValue[i].GetCharacteristic().Equals(PrimeFieldCharacteristic))
					throw new ArgumentException("Text");

			_primeChar = PrimeFieldCharacteristic;
			_value = PolynomialValue;
			_dim = _value.Length;
			_fpolyn = FactorPolynomial;
		}

		public int GetCharacteristic() => _primeChar;
		public int GetDimension() => _dim;
		public int[] GetValue()
		{
			var res = new int[_dim];
			for (int i = 1; i < _dim; i++)
				res[i] = this._value[i].GetValue();

			return res;
		}
		private FiniteFieldElement Map(Func<PrimeFiniteFieldElement, int, PrimeFiniteFieldElement> fun)
		{
			var res = new PrimeFiniteFieldElement[_dim];
			for (int i = 1; i < _dim; i++)
				res[i] = fun(this._value[i], i);

			return new FiniteFieldElement(this._primeChar, res, this._fpolyn);
		}
		public static FiniteFieldElement operator +(FiniteFieldElement pe)
			=> pe;
		public static FiniteFieldElement operator -(FiniteFieldElement pe)
			=> pe.Map((e, i) => -e);
		public static FiniteFieldElement operator +(FiniteFieldElement pa1, FiniteFieldElement pa2)
			//=> pa1.Map((e, i) => e + pa2._value[Array.IndexOf(pa1._value, e)]);
			=> pa1._primeChar.Equals(pa2._primeChar) && pa1._dim.Equals(pa2._dim) ? 
			   pa1.Map((e, i) => e + pa2._value[i]) : throw new ArgumentException("text");
		public static FiniteFieldElement operator -(FiniteFieldElement pm, FiniteFieldElement ps)
			//=> pm.Map(e => e - ps._value[Array.IndexOf(ps._value, e)]);
			=> pm._primeChar.Equals(ps._primeChar) && pm._dim.Equals(ps._dim) ? 
			   pm.Map((e, i) => e - ps._value[i]) : throw new ArgumentException("text");
		private static FiniteFieldElement Remainder(PrimeFiniteFieldElement[] d1, PrimeFiniteFieldElement[] d2)
		{
			throw new NotImplementedException();
		}

		public static FiniteFieldElement operator *(FiniteFieldElement m1, FiniteFieldElement m2)
		{
			//throw new NotImplementedException();
			if ((!m1._primeChar.Equals(m2._primeChar))||(!m1._dim.Equals(m2._dim)))
				throw new ArgumentException("text");

			var res = new PrimeFiniteFieldElement[m1._dim + m2._dim - 2];
			for (int k = 0; k < res.Length; k++)
				res[k] = new PrimeFiniteFieldElement(m1._primeChar, 0);

			for (int i = 0; i < m1._dim; i++)
				for (int j = 0; j < m2._dim; j++)
					res[i + j] += m1._value[i] * m2._value[j];

			return Remainder(res, m1._fpolyn); // need to get reminder******
		}
		public FiniteFieldElement Inverse()
		{
			throw new NotImplementedException();
		}
		public static FiniteFieldElement operator /(FiniteFieldElement d1, FiniteFieldElement d2)
		{
			throw new NotImplementedException();
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
