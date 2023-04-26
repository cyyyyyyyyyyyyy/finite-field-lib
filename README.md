# finite-field-lib
Library is used to manipulate the elements of finite fields

## Create Field
```c#
// create prime finite field
int n = 7;
var gf7 = new PrimeFiniteField(n);
// n is prime and greater than 1

// create finite field
int[] factorPolyn = new int[] { 1, 1, 0, 0, 0, 0, 1 };
var gf64 = new FiniteField(2, factorPolyn);
// where factorPolyn - is irreducible over GF2 (or over respective prime finite field)
// for GF64 this polynomial's degree should equal to 6 (where 6 is the exponent in 2^6 = 64)
```
## Get element from field
```c#
// get element from prime field
gf7.Get(5);
gf7.Get(11); // will return element of value: 11 % 7 = 4
gf7.GetAdditiveIdent(); // zero of the field
gf7.GetMultiplicativeIdent(); // one of the field

// get element from field
gf64.Get(new int[] { 1, 0, 1, 1, 0, 0 }); // 1 + x^2 + x^3 - method cuts off zeros automaticaly
gf64.Get(new int[] { 1, 0, 1, 1, 0, 0, 1, 1 }); // as in the prime field
gf64.GetAdditiveIdent();
gf64.GetMultiplicativeIdent();

// get element from bytes (only for fields where characteristic is equal to 2
// and factor polynomial's degree is divisible by 8)
var gf256 = new FiniteField(2, new int[] { 1, 0, 1, 1, 1, 0, 0, 0, 1 } );
var e = gf256.Get(new byte[] { 240 }); // initialization of element from bytes array
e.GetByte();
```
## Element manipulation
```c#
// element manipulation: prime fields

var e1 = gf7.Get(4);
var e2 = gf7.Get(5);

var unPlus = +e1; // = 4 
var unMinus = -e2; // = 2
var sum = e1 + e2; // = 2
var diff = e1 - e2; // = 6
var prod = e1 * e2; // = 6
var inv = e1.Inverse(); // = 2
var quot = e1 / e2; // = 5

bool fl = e1.Equals(gf7.Get(4)); // true

// element manipulation

var e1 = gf256.Get(new byte[] { 40 } );
var e2 = gf256.Get(new byte[] { 241 });

var unPlus = +e1; // = 40
var unMinus = -e2; // = 241
var sum = e1 + e2; // = 217
var diff = e1 - e2; // = 217
var prod = e1 * e2; // = 144
var inv = e1.Inverse(); // = 112
var quot = e1 / e2; // = 218

bool fl = e1.Equals(gf256.Get(new byte[] { 40 })); // true
```
# crc-lib
Toy implementation of crc-32 checksum calculator

## Initialization
```c#
// provide four bytes as an input to the constructor
var checkSumInstance = new CheckSum(new byte[] { 1, 89, 12, 45});
```
## Calculate checksum
```c#
// create message as array of bytes
// notice: byte order is little-endian
var msg = new byte[] { 245, 1, 90, 0, 12, 46, 23, 85, 85, 0, 1};
// calculate checksum
var initialCheckSum = checkSumInstance.CalculateCheckSum(msg); // {236, 83, 61, 83}
```
## Check checksums
```c#
// check
var randomCheckSum = new byte[] { 90, 1, 78, 13 };
var copiedCheckSum = new byte[initialCheckSum.Length];
initialCheckSum.CopyTo(copiedCheckSum, 0);

bool check1 = checkSumInstance.Check(initialCheckSum, randomCheckSum); // false
bool check2 = checkSumInstance.Check(initialCheckSum, copiedCheckSum); // true
```
