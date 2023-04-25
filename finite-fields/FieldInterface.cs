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
		IFiniteFieldElement<E>
	{
		public int GetCharacteristic();
		public IFiniteField<E> GetField();
		public bool IsWellDefinedWith(E other);
		public E Inverse();
	}

	public interface IFiniteField<T> where T : IFiniteFieldElement<T>
	{
		public int GetCharacteristic();
		public T GetAdditiveIdent();
		public T GetMultiplicativeIdent();
	}
}
