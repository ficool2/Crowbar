Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Explicit)>
Public Class SourceMdlAnimationValue09

	'FROM: [1999] HLStandardSDK\SourceCode\engine\studio.h
	'// animation frames
	'typedef union 
	'{
	'	struct {
	'		byte	valid;
	'		byte	total;
	'	} num;
	'	short		value;
	'} mstudioanimvalue_t;


	<FieldOffset(0)> Public valid As Byte
	<FieldOffset(1)> Public total As Byte

	<FieldOffset(0)> Public value As Int16

End Class
