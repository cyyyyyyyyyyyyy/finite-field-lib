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
		private readonly PrimeFiniteField _primefield;

		public FiniteField(int PrimeFieldCharacterictic, int[] IntegerPolynomial)
		{
			if (PrimeFieldCharacterictic < 1)
				throw new ArgumentException("Text");

			_primeChar = PrimeFieldCharacterictic;

			//_dim = IntegerPolynomial.Length - 1;
			//var polyn = new PrimeFiniteFieldElement[_dim];
			//for (int i = 1; i < _dim; i++)
			//	polyn[i] = new(_primeChar, IntegerPolynomial[i]);
			//_polyn = polyn;//new FiniteFieldElement(_primeChar, polyn);

			_primefield = new PrimeFiniteField(_primeChar);

			_polyn = new RPolyn(_primeChar, IntegerPolynomial);
			_dim = _polyn.GetLength() - 1;

		}
		public int GetCharacteristic() => _primeChar;
		public int GetDimension() => _dim;
		public int[] GetPolynomial()
		{
			//throw new NotImplementedException();
			//var res = new int[_dim];
			//for (int i = 1; i < _dim; i++)
			//	res[i] = this._polyn[i].GetValue();

			//return res;
			return _polyn.GetValue();
		}

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
		public int[] GetValue()
		{
			//var res = new int[_dim];
			//for (int i = 1; i < _dim; i++)
			//	res[i] = this._value[i].GetValue();

			//return res;
			return _value.GetValue();
		}
		/*private FiniteFieldElement Map(Func<PrimeFiniteFieldElement, int, PrimeFiniteFieldElement> fun)
		{
			var res = new PrimeFiniteFieldElement[_dim];
			for (int i = 1; i < _dim; i++)
				res[i] = fun(this._value[i], i);

			return new FiniteFieldElement(this._primeChar, res, this._fpolyn);
		}*/
		public static FiniteFieldElement operator +(FiniteFieldElement pe)
			=> pe;
		public static FiniteFieldElement operator -(FiniteFieldElement pe)
			=> new FiniteFieldElement(pe._primeChar, -pe._value, pe._fpolyn); //pe.Map((e, i) => -e); 
		public static FiniteFieldElement operator +(FiniteFieldElement pa1, FiniteFieldElement pa2)
		//=> pa1.Map((e, i) => e + pa2._value[Array.IndexOf(pa1._value, e)]);
		//=> pa1._primeChar.Equals(pa2._primeChar) && pa1._dim.Equals(pa2._dim) ? 
		//   pa1.Map((e, i) => e + pa2._value[i]) : throw new ArgumentException("text");
		{
			if (!pa1._primeChar.Equals(pa2._primeChar) || !pa1._dim.Equals(pa2._dim)) // do i need to check polynimial equality?
				throw new ArgumentException("text");
			return new FiniteFieldElement(pa1._primeChar, pa1._value + pa2._value, pa1._fpolyn);
		}
		public static FiniteFieldElement operator -(FiniteFieldElement pm, FiniteFieldElement ps)
		//=> pm.Map(e => e - ps._value[Array.IndexOf(ps._value, e)]);
		//=> pm._primeChar.Equals(ps._primeChar) && pm._dim.Equals(ps._dim) ? 
		//   pm.Map((e, i) => e - ps._value[i]) : throw new ArgumentException("text");
		{
			if (!pm._primeChar.Equals(ps._primeChar) || !pm._dim.Equals(ps._dim)) 
				throw new ArgumentException("text");
			return new FiniteFieldElement(pm._primeChar, pm._value - ps._value, pm._fpolyn);
		}
		public static FiniteFieldElement operator *(FiniteFieldElement m1, FiniteFieldElement m2)
		{
			throw new NotImplementedException();
		}
		public FiniteFieldElement Inverse()
		{
			throw new NotImplementedException();
		}
		public static FiniteFieldElement operator /(FiniteFieldElement d1, FiniteFieldElement d2)
		{
			throw new NotImplementedException();
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
