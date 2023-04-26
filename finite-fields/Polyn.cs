using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace finite_fields
{
	public class Polyn<FE> where FE : IFiniteFieldElement<FE>
	{
		private readonly int _primeChar; // correctness
		private readonly int _length;
		private readonly IFiniteField<FE> _field; //correctness
		private readonly FE[] _value;
		public Polyn(int PrimeFieldCharacteristic, int[] IntegerPolynomialValue) // for Polyn<PrimeFiniteFieldElement>
		{
			if (PrimeFieldCharacteristic < 1)
				throw new ArgumentException("Error in Polyn: PrimeFieldCharacteristic should be prime and greater than 1");

			if (IntegerPolynomialValue.Length < 1)
				throw new ArgumentException("Error in Polyn: IntegerPolynomialValue's length should be greater or equal to 1");

			_primeChar = PrimeFieldCharacteristic;
			_field = (IFiniteField<FE>)(new PrimeFiniteField(_primeChar));

			//cut off zeros
			int lengthCutoffZeros = 1;
			for (int j = 0; j < IntegerPolynomialValue.Length; j++)
				if (IntegerPolynomialValue[j] != 0)
					lengthCutoffZeros = j + 1;
			_length = lengthCutoffZeros;

			//fill
			_value = Fill(_length, i =>
			(FE)(IFiniteFieldElement<PrimeFiniteFieldElement>)
			(new PrimeFiniteFieldElement(_primeChar, IntegerPolynomialValue[i], new PrimeFiniteField(_primeChar))));
		}
		public Polyn(int PrimeFieldCharacteristic, FE[] ElementPolynomialValue) // for both
		{
			if (PrimeFieldCharacteristic < 1)
				throw new ArgumentException("Error in Polyn: PrimeFieldCharacteristic should be prime and greater than 1");

			if (ElementPolynomialValue.Length < 1)
				throw new ArgumentException("Error in Polyn: ElementPolynomialValue's length should be greater or equal to 1");

			_primeChar = PrimeFieldCharacteristic;

			//correctness
			for (int i = 0; i < ElementPolynomialValue.Length - 1; i++)
				if (!ElementPolynomialValue[i].IsOperationCorrectWith(ElementPolynomialValue[i + 1])) //OK
					throw new ArgumentException("Incorrect ElementPolynomialValue: every element should belong to the same (Prime)FiniteField");

			_field = ElementPolynomialValue[0].GetField();
			//cut off zeros
			int lengthCutoffZeros = 1;
			for (int j = 0; j < ElementPolynomialValue.Length; j++)
				if (!ElementPolynomialValue[j].Equals(_field.GetAdditiveIdent()))
					lengthCutoffZeros = j + 1;

			_length = lengthCutoffZeros;

			//fill

			if (_length != ElementPolynomialValue.Length)
				_value = Fill(_length, i => ElementPolynomialValue[i]); //characteristics check?
			else _value = ElementPolynomialValue;
		}
		public int GetCharacteristic() => _primeChar;
		public int GetLength() => _length;
		public FE[] GetValue() => _value;

		private bool IsOperationCorrectWith(Polyn<FE> other)
			=> this._primeChar.Equals(other._primeChar) && this._field.Equals(other._field);
		private static FE[] Fill(int Length, Func<int, FE> fun)
		{
			FE[] res = new FE[Length];
			for (int i = 0; i < Length; i++)
				res[i] = fun(i);

			return res;
		}
		private Polyn<FE> Map(Func<FE, int, FE> fun)
		{
			var res = new FE[this._length];
			for (int i = 0; i < _length; i++)
				res[i] = fun(this._value[i], i);

			return new Polyn<FE>(this._primeChar, res);
		}

		private static Polyn<FE> Map(Polyn<FE> p1, Polyn<FE> p2, Func<FE, FE, FE> fun)
		{
			if (!p1.IsOperationCorrectWith(p2))
				throw new ArgumentException("Operation (addition) is not correct with given polynomials");

			Polyn<FE> m;
			Polyn<FE> l;
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

			var res = new FE[m._length];
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

			return new Polyn<FE>(p1._primeChar, res);
		}
		public static Polyn<FE> operator +(Polyn<FE> pe)
			=> pe;
		public static Polyn<FE> operator -(Polyn<FE> pe)
			=> pe.Map((e, i) => -e);
		public static Polyn<FE> operator +(Polyn<FE> pa1, Polyn<FE> pa2)
		{
			return Map(pa1, pa2, (e1, e2) => e1 + e2);
		}
		public static Polyn<FE> operator -(Polyn<FE> pm, Polyn<FE> ps)
		{
			return Map(pm, ps, (e1, e2) => e1 - e2);
		}
		public static Polyn<FE> operator %(Polyn<FE> p1, Polyn<FE> p2)
		{
			//p2 - can be reducible
			if (!p1.IsOperationCorrectWith(p2)) // check for zero
				throw new ArgumentException("Operation (division) is not correct with given polynomials");
			if ((p2._value.Length == 1) && (p2._value[0].Equals(p2._field.GetAdditiveIdent())))
				throw new ArgumentException("Cannot divide by zero");

			if (p1._length < p2._length)
				return p1; // not really; upd: really

			FE[] remainder = p1._value;
			//PrimeFiniteFieldElement[] quotient = new PrimeFiniteFieldElement[p1._length - p2._length + 1];
			for (int i = 0; i < p1._length - p2._length + 1; i++) // ok
			{
				FE coeff = remainder[p1._length - 1 - i] / p2._value[p2._length - 1]; // if coeff is zero?
				//quotient[quotient.Length - i - 1] = coeff;
				if (coeff.Equals(p1._field.GetAdditiveIdent()))
					continue;

				for (int j = 0; j < p2._length; j++)
				{
					remainder[p1._length - 1 - i - j] -= coeff * p2._value[p2._length - j - 1];
				}
			}

			return new Polyn<FE>(p1._primeChar, remainder);
		}
		public static Polyn<FE> operator *(Polyn<FE> pm1, Polyn<FE> pm2)
		{
			if (!pm1.IsOperationCorrectWith(pm2))
				throw new ArgumentException("Operation (multiplication) is not correct with given polynomials");

			var res = Fill(pm1._length + pm2._length - 1, i => pm1._field.GetAdditiveIdent());

			for (int i = 0; i < pm1._length; i++)
				for (int j = 0; j < pm2._length; j++)
					res[i + j] += pm1._value[i] * pm2._value[j];

			return new Polyn<FE>(pm1._primeChar, res);
		}

		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(obj, null))
				return false;
			if (obj is not Polyn<FE> p)
				return false;

			if (!p._primeChar.Equals(this._primeChar)
				|| !p._length.Equals(this._length)) // if char-cs and length of RPolyns aren't equal - not OK
				return false;

			for (int i = 0; i < this._length; i++)
				if (!this._value[i].Equals(p._value[i])) // compares only _value, characteristic of concrete Polyn is OK
					return false;
			return true;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
