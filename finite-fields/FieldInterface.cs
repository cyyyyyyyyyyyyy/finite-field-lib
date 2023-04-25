using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace finite_fields
{
	public interface IFiniteFieldElement<E> : 
		IUnaryPlusOperators<E, E>,
		IUnaryNegationOperators<E, E>,
		IAdditionOperators<E, E, E>,
		ISubtractionOperators<E, E, E>,
		IMultiplyOperators<E, E, E>,
		IDivisionOperators<E, E, E>
		where E :
		//IUnaryPlusOperators<E, E>,
		//IUnaryNegationOperators<E, E>,
		//IAdditionOperators<E, E, E>,
		//ISubtractionOperators<E, E, E>,
		//IMultiplyOperators<E, E, E>,
		//IDivisionOperators<E, E, E>,
		IFiniteFieldElement<E>
	{
		internal int[] InternalGetValue();
		public int GetCharacteristic();
		public IFiniteField<E> GetField();
		public bool CorrectnessCheck(E other);
		public E Inverse();
	}

	public interface IFiniteField<T> where T : IFiniteFieldElement<T>
	{
		public int GetCharacteristic();
		public T GetAdditiveIdent();
		public T GetMultiplicativeIdent();
	}
	//public interface IFieldOfElement<E, F> where E : IFieldElement<E> where F : IField<E>
	//{
	//	public F GetField();
	//}
}
