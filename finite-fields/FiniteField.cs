using finite_fields;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
		private readonly FiniteFieldElement _polyn;
		//private readonly PrimeFiniteField _primefield;

		public FiniteField(int PrimeFieldCharacterictic, int[] IntegerPolynomial)
		{
			if (PrimeFieldCharacterictic < 1)
				throw new ArgumentException("Text");

			_primeChar = PrimeFieldCharacterictic;
			_dim = IntegerPolynomial.Length - 1;
			/*var polyn = new PrimeFiniteFieldElement[_dim];
			for (int i = 1; i < _dim; i++)
				polyn[i] = new(_primeChar, IntegerPolynomial[i]);
			_polyn = new FiniteFieldElement(_primeChar, polyn);*/
		}
		public int GetCharacteristic() => _primeChar;
		public int GetDimension() => _dim;
		public int[] GetPolynomial()
		{
			throw new NotImplementedException();
			/*var res = new int[_dim];
			for (int i = 1; i < _dim; i++)
				res[i] = this._polyn.GetValue;

			return res;*/
		}

		/*public FiniteFieldElement GetZero()
		{
			//return new FiniteFieldElement(_primeChar, new int[_dim]);
			var val = new PrimeFiniteFieldElement[_dim];
			for (int i = 0; i < _dim; i++)
				val[i] = _primefield.GetAdditiveIdent();

			return new FiniteFieldElement(_primeChar, val);
		}
		public FiniteFieldElement GetOne()
		{
			var val = new PrimeFiniteFieldElement[_dim];
			for (int i = 1; i < _dim; i++)
				val[i] = _primefield.GetAdditiveIdent();
			val[0] = _primefield.GetMultiplicativeIdent();

			return new FiniteFieldElement(_primeChar, val);
		}
		public FiniteFieldElement Sum(FiniteFieldElement a1, FiniteFieldElement a2) 
		{
			if (charCheck(a1, a2))
			{
				for ()
			}
			throw new ArgumentException("");
		}*/
	}

	public class FiniteFieldElement
	{
		private readonly int _primeChar;
		private readonly int _dim;
		private readonly PrimeFiniteFieldElement[] _value;

		public FiniteFieldElement(int PrimeFieldCharacteristic, PrimeFiniteFieldElement[] PolynomialValue)
		{
			if (PrimeFieldCharacteristic < 1)
				throw new ArgumentException("Text");
			for (int i = 0; i < PolynomialValue.Length; i++)
				if (!PolynomialValue[i].GetCharacteristic().Equals(PrimeFieldCharacteristic))
					throw new ArgumentException("Text");

			_primeChar = PrimeFieldCharacteristic;
			_value = PolynomialValue;
			_dim = _value.Length;
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

			return new FiniteFieldElement(this._primeChar, res);
		}
		public static FiniteFieldElement operator +(FiniteFieldElement pe)
			=> pe;
		public static FiniteFieldElement operator -(FiniteFieldElement pe)
			=> pe.Map((e, i) => -e);
		public static FiniteFieldElement operator +(FiniteFieldElement pa1, FiniteFieldElement pa2)
			//=> pa1.Map((e, i) => e + pa2._value[Array.IndexOf(pa1._value, e)]);
			=> pa1.Map((e, i) => e + pa2._value[i]);
		public static FiniteFieldElement operator -(FiniteFieldElement pm, FiniteFieldElement ps)
			//=> pm.Map(e => e - ps._value[Array.IndexOf(ps._value, e)]);
			=> pm.Map((e, i) => e - ps._value[i]);
		public static FiniteFieldElement operator %(FiniteFieldElement p1, FiniteFieldElement p2)
		{
			throw new NotImplementedException();
		}
		public static FiniteFieldElement operator *(FiniteFieldElement m1, FiniteFieldElement m2)
		{
			throw new NotImplementedException();
			/*var res = new FiniteFieldElement[m1.];
			for (int i = 0; i < m1._value.Length; i++)
				for (int j = 0; j < m2._value.Length; j++)*/

		}
	}
}
