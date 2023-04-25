using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace finite_fields
{
	public class RPolyn//<F> where F : PrimeFiniteFieldElement
	{
		private readonly int _primeChar;
		private readonly int _length;
		private readonly PrimeFiniteField _field;
		private readonly PrimeFiniteFieldElement[] _value;
		public RPolyn(int Characteristic, int[] IntegerPolynomial)
		{
			if (Characteristic < 1)
				throw new ArgumentException("Text");

			_primeChar = Characteristic;
			_field = new PrimeFiniteField(_primeChar);

			if (IntegerPolynomial.Length < 1)
				throw new ArgumentException("Text");

			//cut off zeros
			int lengthCutoffZeros = 1;
			for (int j = 0; j < IntegerPolynomial.Length; j++)
				if (IntegerPolynomial[j] != 0)
					lengthCutoffZeros = j + 1;

			_length = lengthCutoffZeros;

			_value = Fill(_length, i => new PrimeFiniteFieldElement(_primeChar, IntegerPolynomial[i]));
		}
		public RPolyn(int Characteristic, PrimeFiniteFieldElement[] ElementPolynomial)
		{
			if (Characteristic < 1)
				throw new ArgumentException("Text");

			if (ElementPolynomial.Length < 1)
				throw new ArgumentException("Text");

			_primeChar = Characteristic;
			_field = new PrimeFiniteField(_primeChar);

			//cut off zeros
			int lengthCutoffZeros = 1;
			for (int j = 0; j < ElementPolynomial.Length; j++)
				if (!ElementPolynomial[j].Equals(_field.GetAdditiveIdent()))
					lengthCutoffZeros = j + 1;

			//characteristics correctness
			for (int i = 0; i < lengthCutoffZeros; i++)
				if (!ElementPolynomial[i].GetCharacteristic().Equals(_primeChar))
					throw new ArgumentException("Text");

			_length = lengthCutoffZeros;

			//fill
			if (_length != ElementPolynomial.Length)
				_value = Fill(_length, i => ElementPolynomial[i]); //characteristics check?
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
					res[j] = fun(p1._field.GetAdditiveIdent(), p2._value[j]);
			}
			else
			{
				for (int j = l._length; j < m._length; j++)
					res[j] = fun(p1._value[j], p1._field.GetAdditiveIdent());
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
		}
		public static RPolyn operator -(RPolyn pm, RPolyn ps)
		{
			return Map(pm, ps, (e1, e2) => e1 - e2);
		}
		public static RPolyn operator %(RPolyn p1, RPolyn p2)
		{
			//p2 - can be reducible
			if (!p1._primeChar.Equals(p2._primeChar))
				throw new ArgumentException("text");

			if (p1._length < p2._length)
				return p1; // not really; upd: really

			PrimeFiniteFieldElement[] remainder = p1._value;
			//PrimeFiniteFieldElement[] quotient = new PrimeFiniteFieldElement[p1._length - p2._length + 1];
			for (int i = 0; i < p1._length - p2._length + 1; i++) // ok
			{
				PrimeFiniteFieldElement coeff = remainder[p1._length - 1 - i] / p2._value[p2._length - 1]; // if coeff is zero?
																										   //quotient[quotient.Length - i - 1] = coeff;
				if (coeff.GetValue() == 0)
					continue;

				for (int j = 0; j < p2._length; j++)
				{
					remainder[p1._length - 1 - i - j] -= coeff * p2._value[p2._length - j - 1];
				}
			}

			return new RPolyn(p1._primeChar, remainder);
		}
		public static RPolyn operator *(RPolyn pm1, RPolyn pm2)
		{
			if (!pm1._primeChar.Equals(pm2._primeChar))
				throw new ArgumentException("text");

			var res = Fill(pm1._length + pm2._length - 1, i => pm1._field.GetAdditiveIdent());

			for (int i = 0; i < pm1._length; i++)
				for (int j = 0; j < pm2._length; j++)
					res[i + j] += pm1._value[i] * pm2._value[j];

			return new RPolyn(pm1._primeChar, res);
		}

		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(obj, null))
				return false;

			if (obj.GetType() != this.GetType())
				return false;

			if (!(obj as RPolyn)._primeChar.Equals(this._primeChar)
				|| !(obj as RPolyn)._length.Equals(this._length)) // if char-cs and length of RPolyns aren't equal - not OK
				return false;

			for (int i = 0; i < this._length; i++)
				if (!this._value[i].Equals((obj as RPolyn)._value[i])) // compares only _value, characteristic of concrete RPolyn is OK
					return false;
			return true;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
