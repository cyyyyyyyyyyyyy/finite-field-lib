using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace finite_fields
{
	public class RPolyn // restricted polynomial: i dunno -_-
	{
		private readonly int _primeChar;
		private readonly int _length;
		private readonly PrimeFiniteFieldElement[] _value;
		public RPolyn(int Characteristic, int[] IntegerPolynomial) 
		{
			// no restrictions on characteristics
			if (Characteristic < 1)
				throw new ArgumentException("Text");

			_primeChar = Characteristic;

			if (IntegerPolynomial.Length < 1)
				throw new ArgumentException("Text");

			//check for redundant zeros
			int lengthCutoffZeros = 1;
			for (int j = 0; j < IntegerPolynomial.Length; j++)
				if (IntegerPolynomial[j] != 0)
					lengthCutoffZeros = j + 1;

			_length = lengthCutoffZeros;
			//fill
			//PrimeFiniteFieldElement[] val = new PrimeFiniteFieldElement[_length];
			//for (int i = 0; i < _length; i++)
				//val[i] = new PrimeFiniteFieldElement(_primeChar, IntegerPolynomial[i]);


			_value = Fill(_length, i => new PrimeFiniteFieldElement(_primeChar, IntegerPolynomial[i]));
		}
		public RPolyn(int Characteristic, PrimeFiniteFieldElement[] ElementPolynomial)
		{
			if (Characteristic < 1)
				throw new ArgumentException("Text");

			_primeChar = Characteristic;

			if (ElementPolynomial.Length < 1)
				throw new ArgumentException("Text");

			int lengthCutoffZeros = 1;
			PrimeFiniteFieldElement zero = new PrimeFiniteFieldElement(_primeChar, 0);

			for (int j = 0; j < ElementPolynomial.Length; j++)
				if (!ElementPolynomial[j].Equals(zero))
					lengthCutoffZeros = j + 1;

			_length = lengthCutoffZeros;

			if (_length != ElementPolynomial.Length)
				_value = Fill(_length, i => ElementPolynomial[i]);
			else _value = ElementPolynomial;
		}
		public int GetCharacteristic() => _primeChar;
		public int GetLength() => _length;
		public int[] GetValue()
		{
			int[] res = new int[_length];
			for (int i = 0; i < _length; i++)
				res[i] = _value[i].GetValue();

			return res;
		}
		private static PrimeFiniteFieldElement[] Fill(int Length, Func<int, PrimeFiniteFieldElement> fun) 
		{
			PrimeFiniteFieldElement[] res = new PrimeFiniteFieldElement[Length];
			for (int i = 0; i < Length; i++)
				res[i] = fun(i);

			return res;
		}
		private RPolyn Map(Func<PrimeFiniteFieldElement, int, PrimeFiniteFieldElement> fun)
		{
			var res = new PrimeFiniteFieldElement[this._length];
			for (int i = 0; i < _length; i++)
				res[i] = fun(this._value[i], i);

			return new RPolyn(this._primeChar, res);
		}

		private static RPolyn Map(RPolyn p1, RPolyn p2, Func<PrimeFiniteFieldElement, PrimeFiniteFieldElement, PrimeFiniteFieldElement> fun)
		{
			if (!p1._primeChar.Equals(p2._primeChar))
				throw new ArgumentException("text");

			//yeah i know this code is bad
			RPolyn m;
			RPolyn l;
			if (p1._length > p2._length)
			{
				m = p1;
				l = p2;
			}
			else
			{
				m = p2;
				l = p1;
			}

			var res = new PrimeFiniteFieldElement[m._length];
			for (int i = 0; i < l._length; i++)
				res[i] = fun(p1._value[i], p2._value[i]);

			if (l._length == p1._length)
			{
				for (int j = l._length; j < m._length; j++)
					res[j] = fun(new PrimeFiniteFieldElement(p1._primeChar, 0), p2._value[j]);
			}
			else
			{
				for (int j = l._length; j < m._length; j++)
					res[j] = fun(p1._value[j], new PrimeFiniteFieldElement(p1._primeChar, 0));
			}

			return new RPolyn(p1._primeChar, res);
		}
		public static RPolyn operator +(RPolyn pe)
			=> pe;
		public static RPolyn operator -(RPolyn pe)
			=> pe.Map((e, i) => -e);
		public static RPolyn operator +(RPolyn pa1, RPolyn pa2)
		{
			return Map(pa1, pa2, (e1, e2) => e1 + e2);
		} //maybe to just restrict the length of polyns, so that they are the same? - becuz why add redundant functionality?
		public static RPolyn operator -(RPolyn pm, RPolyn ps)
		{
			return Map(pm, ps, (e1, e2) => e1 - e2);
		}			
		public static RPolyn operator %(RPolyn p1, RPolyn p2)
		{
			throw new NotImplementedException();
			// I guess p2 should be irreducible
			if (!p1._primeChar.Equals(p2._primeChar))
				throw new ArgumentException("text");

			/*int p1LeadIndex = p1._length;
			int p1LeadCoef;
			do
			{
				p1LeadIndex--;
				p1LeadCoef = p1._value[p1LeadIndex].GetValue();
			} while ((p1LeadCoef != 0)&&(p1LeadIndex >= 0));

			int p2LeadIndex = p2._length;
			int p2LeadCoef;
			do
			{
				p2LeadIndex--;
				p2LeadCoef = p2._value[p2LeadIndex].GetValue();
			} while ((p2LeadCoef != 0)&&(p2LeadIndex >= 0));*/

			if (p1._length < p2._length)
				return p1; // not really

			PrimeFiniteFieldElement[] remainder = p1._value; // array?
			//quotient = new double[p1leadindex - p2leadindex + 1];
			for (int i = 0; i < p1._length - p2._length + 1; i++) // ok
			{
				PrimeFiniteFieldElement coeff = remainder[p1._length - 1 - i] / p2._value[p2._length - 1]; // if coeff is zero?
			    //quotient[quotient.length - i - 1] = coeff;
				if (coeff.GetValue() == 0)
					continue;

				for (int j = 0; j < p2._length; j++)
				{
					remainder[p1._length - 1 - i - j] -= coeff * p2._value[p2._length - j - 1];
				}
			}


		}
		public static RPolyn operator *(RPolyn pm1, RPolyn pm2)
		{
			if (!pm1._primeChar.Equals(pm2._primeChar))
				throw new ArgumentException("text");

			//fill
			//var res = new PrimeFiniteFieldElement[pm1._length + pm2._length - 2];
			//for (int k = 0; k < res.Length; k++)
			//	res[k] = new PrimeFiniteFieldElement(pm1._primeChar, 0);

			var res = Fill(pm1._length + pm2._length - 1, i => new PrimeFiniteFieldElement(pm1._primeChar, 0));

			for (int i = 0; i < pm1._length; i++)
				for (int j = 0; j < pm2._length; j++)
					res[i + j] += pm1._value[i] * pm2._value[j];

			return new RPolyn(pm1._primeChar, res); // need to get remainder******
		}

		public override bool Equals(object? obj)
		{
			if (obj == null)
				return false;
			if (obj is RPolyn)
				for (int i = 0; i < this._length; i++)
					if (!this._value[i].Equals((obj as RPolyn)._value[i]))
						return false;
				return true;
			//return false;
		}
	}
}
