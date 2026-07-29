Public Class SourceMdlFileData04
	Inherits SourceMdlFileDataBase

	Public Sub New()
		MyBase.New()

		Me.theChecksumIsValid = False
	End Sub

	'Public id(3) As Char
	'Public version As Integer
	Public unknown01 As Integer

	Public boneCount As Integer
	Public bodyPartCount As Integer
	Public unknown02 As Integer
	Public sequenceDescCount As Integer
	Public unknown03 As Integer

	Public unknown04 As Integer

	Public theBodyParts As List(Of SourceMdlBodyPart04)
	Public theBones As List(Of SourceMdlBone04)
	Public theSequenceDescs As List(Of SourceMdlSequenceDesc04)

	Public theBoneTransforms As List(Of SourceBoneTransform04)

End Class
