Imports System.IO

Public Class SourceMdlFile04

#Region "Creation and Destruction"

	Public Sub New(ByVal mdlFileReader As BinaryReader, ByVal mdlFileData As SourceMdlFileData04)
		Me.theInputFileReader = mdlFileReader
		Me.theMdlFileData = mdlFileData

		Me.theMdlFileData.theFileSeekLog.FileSize = Me.theInputFileReader.BaseStream.Length
	End Sub

	Public Sub New(ByVal mdlFileWriter As BinaryWriter, ByVal mdlFileData As SourceMdlFileData04)
		Me.theOutputFileWriter = mdlFileWriter
		Me.theMdlFileData = mdlFileData
	End Sub

#End Region

#Region "Methods"

	Public Sub ReadMdlHeader()
		'Dim inputFileStreamPosition As Long
		Dim fileOffsetStart As Long
		Dim fileOffsetEnd As Long
		'Dim fileOffsetStart2 As Long
		'Dim fileOffsetEnd2 As Long

		fileOffsetStart = Me.theInputFileReader.BaseStream.Position

		Me.theMdlFileData.id = Me.theInputFileReader.ReadChars(4)
		Me.theMdlFileData.theID = Me.theMdlFileData.id
		Me.theMdlFileData.version = Me.theInputFileReader.ReadInt32()

		Me.theMdlFileData.theActualFileSize = Me.theInputFileReader.BaseStream.Length

		Me.theMdlFileData.unknown01 = Me.theInputFileReader.ReadInt32()

		Me.theMdlFileData.boneCount = Me.theInputFileReader.ReadInt32()
		Me.theMdlFileData.bodyPartCount = Me.theInputFileReader.ReadInt32()
		Me.theMdlFileData.unknownCount = Me.theInputFileReader.ReadInt32()
		Me.theMdlFileData.sequenceDescCount = Me.theInputFileReader.ReadInt32()
		Me.theMdlFileData.sequenceFrameCount = Me.theInputFileReader.ReadInt32()

		Me.theMdlFileData.unknown02 = Me.theInputFileReader.ReadInt32()

		fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
		Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "MDL File Header")
	End Sub

	Public Sub ReadBones()
		If Me.theMdlFileData.boneCount > 0 Then
			Dim fileOffsetStart As Long
			Dim fileOffsetEnd As Long

			Try
				fileOffsetStart = Me.theInputFileReader.BaseStream.Position

				Me.theMdlFileData.theBones = New List(Of SourceMdlBone04)(Me.theMdlFileData.boneCount)
				For boneIndex As Integer = 0 To Me.theMdlFileData.boneCount - 1
					Dim aBone As New SourceMdlBone04()

					aBone.parentBoneIndex = Me.theInputFileReader.ReadInt32()
					aBone.flags = Me.theInputFileReader.ReadInt32()
					aBone.position.x = Me.theInputFileReader.ReadSingle()
					aBone.position.y = Me.theInputFileReader.ReadSingle()
					aBone.position.z = Me.theInputFileReader.ReadSingle()
					'If aBone.parentBoneIndex > -1 Then
					'	aBone.position.x += Me.theMdlFileData.theBones(aBone.parentBoneIndex).position.x
					'	aBone.position.y += Me.theMdlFileData.theBones(aBone.parentBoneIndex).position.y
					'	aBone.position.z += Me.theMdlFileData.theBones(aBone.parentBoneIndex).position.z
					'End If
					'If aBone.parentBoneIndex > -1 Then
					'	aBone.position.x -= Me.theMdlFileData.theBones(aBone.parentBoneIndex).position.x
					'	aBone.position.y -= Me.theMdlFileData.theBones(aBone.parentBoneIndex).position.y
					'	aBone.position.z -= Me.theMdlFileData.theBones(aBone.parentBoneIndex).position.z
					'End If
					'aBone.rotationX.the16BitValue = Me.theInputFileReader.ReadUInt16()
					'aBone.rotationY.the16BitValue = Me.theInputFileReader.ReadUInt16()
					'aBone.rotationZ.the16BitValue = Me.theInputFileReader.ReadUInt16()
					'aBone.positionX.the16BitValue = Me.theInputFileReader.ReadUInt16()
					'aBone.positionY.the16BitValue = Me.theInputFileReader.ReadUInt16()
					'aBone.positionZ.the16BitValue = Me.theInputFileReader.ReadUInt16()
					'aBone.rotationX.the16BitValue = Me.theInputFileReader.ReadUInt16()
					'aBone.rotationY.the16BitValue = Me.theInputFileReader.ReadUInt16()
					'aBone.rotationZ.the16BitValue = Me.theInputFileReader.ReadUInt16()

					Me.theMdlFileData.theBones.Add(aBone)
				Next

				fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
				Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "theMdlFileData.theBones " + Me.theMdlFileData.theBones.Count.ToString())

				'Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "theMdlFileData.theBones alignment")
			Catch ex As Exception
				Dim debug As Integer = 4242
			End Try
		End If
	End Sub

	Public Sub ReadSequenceDescs()
		If Me.theMdlFileData.sequenceDescCount > 0 Then
			Dim fileOffsetStart As Long
			Dim fileOffsetEnd As Long

			Try
				'fileOffsetStart = Me.theInputFileReader.BaseStream.Position

				Me.theMdlFileData.theSequenceDescs = New List(Of SourceMdlSequenceDesc04)(Me.theMdlFileData.sequenceDescCount)
				For sequenceDescIndex As Integer = 0 To Me.theMdlFileData.sequenceDescCount - 1
					fileOffsetStart = Me.theInputFileReader.BaseStream.Position

					Dim aSequenceDesc As New SourceMdlSequenceDesc04()

					aSequenceDesc.name = Me.theInputFileReader.ReadChars(aSequenceDesc.name.Length)
					aSequenceDesc.theName = aSequenceDesc.name
					aSequenceDesc.theName = StringClass.ConvertFromNullTerminatedOrFullLengthString(aSequenceDesc.theName)
					aSequenceDesc.frameCount = Me.theInputFileReader.ReadInt32()
					aSequenceDesc.flag = Me.theInputFileReader.ReadInt32()

					Me.theMdlFileData.theSequenceDescs.Add(aSequenceDesc)

					fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
					Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aSequenceDesc theName = " + aSequenceDesc.theName)
				Next

				'fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
				'Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "theMdlFileData.theSequenceHeaders " + Me.theMdlFileData.theSequenceHeaders.Count.ToString())

				'Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "theMdlFileData.theBones alignment")

				fileOffsetStart = Me.theInputFileReader.BaseStream.Position

				For i As Integer = 0 To Me.theMdlFileData.unknownCount - 1
					Dim unknown As Integer = Me.theInputFileReader.ReadInt32()
				Next

				fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
				Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "In-between Me.theMdlFileData.theSequenceDescs and aSequenceDesc.theSequences (4 bytes for each unknown in theMdlFileData.unknownCount)")

				For sequenceDescIndex As Integer = 0 To Me.theMdlFileData.theSequenceDescs.Count - 1
					Dim aSequenceDesc As SourceMdlSequenceDesc04
					aSequenceDesc = Me.theMdlFileData.theSequenceDescs(sequenceDescIndex)

					Me.ReadSequenceFrames(aSequenceDesc)
				Next
			Catch ex As Exception
				Dim debug As Integer = 4242
			End Try
		End If
	End Sub

	Public Sub ReadBodyParts()
		If Me.theMdlFileData.bodyPartCount > 0 Then
			Dim fileOffsetStart As Long
			Dim fileOffsetEnd As Long

			Try
				'fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
				'Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 8, "theMdlFileData.theBodyParts pre-alignment")

				'fileOffsetStart = Me.theInputFileReader.BaseStream.Position

				Me.theMdlFileData.theBodyParts = New List(Of SourceMdlBodyPart04)(Me.theMdlFileData.bodyPartCount)
				For bodyPartIndex As Integer = 0 To Me.theMdlFileData.bodyPartCount - 1
					fileOffsetStart = Me.theInputFileReader.BaseStream.Position

					Dim aBodyPart As New SourceMdlBodyPart04()

					aBodyPart.name = Me.theInputFileReader.ReadChars(aBodyPart.name.Length)
					aBodyPart.theName = aBodyPart.name
					aBodyPart.theName = StringClass.ConvertFromNullTerminatedOrFullLengthString(aBodyPart.theName)
					aBodyPart.flags = Me.theInputFileReader.ReadInt32()
					aBodyPart.modelCount = Me.theInputFileReader.ReadInt32()

					Me.theMdlFileData.theBodyParts.Add(aBodyPart)

					fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
					Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aBodyPart name = " + aBodyPart.theName)

					Me.ReadModels(aBodyPart)
					Me.SetFileNameForMeshes(aBodyPart)
				Next

				'fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
				'Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "theMdlFileData.theBodyParts " + Me.theMdlFileData.theBodyParts.Count.ToString())

				'Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "theMdlFileData.theBodyParts alignment")
			Catch ex As Exception
				Dim debug As Integer = 4242
			End Try
		End If
	End Sub

	'' The bone positions and rotations do not seem correct, so get them from the first sequence's first frame.
	'Public Sub GetBoneDataFromFirstSequenceFirstFrame()
	'	Dim aSequence As SourceMdlSequenceDesc04
	'	Dim anAnimation As SourceMdlAnimation04
	'	Dim aBone As SourceMdlBone04
	'	aSequence = Me.theMdlFileData.theSequences(0)

	'	For boneIndex As Integer = 0 To Me.theMdlFileData.theBones.Count - 1
	'		aBone = Me.theMdlFileData.theBones(boneIndex)
	'		anAnimation = aSequence.theAnimations(boneIndex)

	'		aBone.position.x = anAnimation.theBonePositionsAndRotations(0).position.x
	'		aBone.position.y = anAnimation.theBonePositionsAndRotations(0).position.y
	'		aBone.position.z = anAnimation.theBonePositionsAndRotations(0).position.z
	'		aBone.rotation.x = anAnimation.theBonePositionsAndRotations(0).rotation.x
	'		aBone.rotation.y = anAnimation.theBonePositionsAndRotations(0).rotation.y
	'		aBone.rotation.z = anAnimation.theBonePositionsAndRotations(0).rotation.z
	'	Next
	'End Sub

	Public Sub ReadUnreadBytes()
		Me.theMdlFileData.theFileSeekLog.LogUnreadBytes(Me.theInputFileReader)
	End Sub

	Public Sub BuildBoneTransforms()
		Me.theMdlFileData.theBoneTransforms = New List(Of SourceBoneTransform04)(Me.theMdlFileData.theBones.Count)
		For boneIndex As Integer = 0 To Me.theMdlFileData.theBones.Count - 1
			Dim aBone As SourceMdlBone04
			Dim boneTransform As New SourceBoneTransform04()
			Dim parentBoneIndex As Integer

			aBone = Me.theMdlFileData.theBones(boneIndex)

			Dim boneMatrixColumn0 As New SourceVector()
			Dim boneMatrixColumn1 As New SourceVector()
			Dim boneMatrixColumn2 As New SourceVector()
			Dim boneMatrixColumn3 As New SourceVector()
			'MathModule.AngleMatrix(aBone.rotation.x, aBone.rotation.y, aBone.rotation.z, boneMatrixColumn0, boneMatrixColumn1, boneMatrixColumn2, boneMatrixColumn3)
			'MathModule.AngleMatrix(aBone.rotation.z, aBone.rotation.x, aBone.rotation.y, boneMatrixColumn0, boneMatrixColumn1, boneMatrixColumn2, boneMatrixColumn3)
			MathModule.AngleMatrix(0, 0, 0, boneMatrixColumn0, boneMatrixColumn1, boneMatrixColumn2, boneMatrixColumn3)

			boneMatrixColumn3.x = aBone.position.x
			boneMatrixColumn3.y = aBone.position.y
			boneMatrixColumn3.z = aBone.position.z

			parentBoneIndex = Me.theMdlFileData.theBones(boneIndex).parentBoneIndex
			If parentBoneIndex = -1 Then
				boneTransform.matrixColumn0.x = boneMatrixColumn0.x
				boneTransform.matrixColumn0.y = boneMatrixColumn0.y
				boneTransform.matrixColumn0.z = boneMatrixColumn0.z
				boneTransform.matrixColumn1.x = boneMatrixColumn1.x
				boneTransform.matrixColumn1.y = boneMatrixColumn1.y
				boneTransform.matrixColumn1.z = boneMatrixColumn1.z
				boneTransform.matrixColumn2.x = boneMatrixColumn2.x
				boneTransform.matrixColumn2.y = boneMatrixColumn2.y
				boneTransform.matrixColumn2.z = boneMatrixColumn2.z
				boneTransform.matrixColumn3.x = boneMatrixColumn3.x
				boneTransform.matrixColumn3.y = boneMatrixColumn3.y
				boneTransform.matrixColumn3.z = boneMatrixColumn3.z
			Else
				Dim parentBoneTransform As SourceBoneTransform04
				parentBoneTransform = Me.theMdlFileData.theBoneTransforms(parentBoneIndex)

				'			R_ConcatTransforms( g_bonetransform[pbones[i].parent], bonematrix, g_bonetransform[i] );
				MathModule.R_ConcatTransforms(parentBoneTransform.matrixColumn0, parentBoneTransform.matrixColumn1, parentBoneTransform.matrixColumn2, parentBoneTransform.matrixColumn3, boneMatrixColumn0, boneMatrixColumn1, boneMatrixColumn2, boneMatrixColumn3, boneTransform.matrixColumn0, boneTransform.matrixColumn1, boneTransform.matrixColumn2, boneTransform.matrixColumn3)
			End If

			Me.theMdlFileData.theBoneTransforms.Add(boneTransform)
		Next
	End Sub

#End Region

#Region "Private Methods"

	Private Sub ReadSequenceFrames(ByVal aSequenceDesc As SourceMdlSequenceDesc04)
		If aSequenceDesc.frameCount > 0 Then
			Dim fileOffsetStart As Long
			Dim fileOffsetEnd As Long

			Try
				aSequenceDesc.theSequenceFrames = New List(Of SourceMdlSequenceFrame04)(aSequenceDesc.frameCount)
				For sequenceIndex As Integer = 0 To aSequenceDesc.frameCount - 1
					fileOffsetStart = Me.theInputFileReader.BaseStream.Position

					Dim aSequenceFrame As New SourceMdlSequenceFrame04()

					aSequenceFrame.frameId = Me.theInputFileReader.ReadSingle()
					For x As Integer = 0 To aSequenceFrame.unknown.Length - 1
						aSequenceFrame.unknown(x) = Me.theInputFileReader.ReadInt32()
					Next
					aSequenceFrame.frame_root_motion = New SourceVector()
					aSequenceFrame.frame_root_motion.x = Me.theInputFileReader.ReadSingle()
					aSequenceFrame.frame_root_motion.y = Me.theInputFileReader.ReadSingle()
					aSequenceFrame.frame_root_motion.z = Me.theInputFileReader.ReadSingle()

					aSequenceDesc.theSequenceFrames.Add(aSequenceFrame)

					fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
					Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aSequence")

					Me.ReadSequenceValues(aSequenceFrame)
				Next

				'fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
				'Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aSequenceHeader.theSequences " + aSequenceHeader.theSequences.Count.ToString())

				'fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
				'Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 8, "aSequenceHeader.theSequences and aSequence.theUnknownValues alignment")
			Catch ex As Exception
				Dim debug As Integer = 4242
			End Try
		End If
	End Sub

	Private Sub ReadSequenceValues(ByVal aSequence As SourceMdlSequenceFrame04)
		If Me.theMdlFileData.theBones.Count > 0 Then
			Dim fileOffsetStart As Long
			Dim fileOffsetEnd As Long

			Try
				fileOffsetStart = Me.theInputFileReader.BaseStream.Position

				aSequence.theSequenceValues = New List(Of SourceMdlSequenceValue04)(Me.theMdlFileData.theBones.Count)
				For sequenceIndex As Integer = 0 To Me.theMdlFileData.theBones.Count - 1
					Dim aSequenceValue As New SourceMdlSequenceValue04()

					aSequenceValue.rotationX = Me.theInputFileReader.ReadUInt16()
					aSequenceValue.rotationY = Me.theInputFileReader.ReadUInt16()
					aSequenceValue.rotationZ = Me.theInputFileReader.ReadUInt16()

					aSequence.theSequenceValues.Add(aSequenceValue)
				Next

				fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
				Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aSequence.theSequenceValues " + aSequence.theSequenceValues.Count.ToString())

				'Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "theMdlFileData.theBones alignment")
			Catch ex As Exception
				Dim debug As Integer = 4242
			End Try
		End If
	End Sub

	Private Sub ReadModels(ByVal aBodyPart As SourceMdlBodyPart04)
		If aBodyPart.modelCount > 0 Then
			Dim fileOffsetStart As Long
			Dim fileOffsetEnd As Long

			Try
				'fileOffsetStart = Me.theInputFileReader.BaseStream.Position

				aBodyPart.theModels = New List(Of SourceMdlModel04)(aBodyPart.modelCount)
				For modelIndex As Integer = 0 To aBodyPart.modelCount - 1
					fileOffsetStart = Me.theInputFileReader.BaseStream.Position

					Dim aModel As New SourceMdlModel04()

					aModel.unknownSingle = Me.theInputFileReader.ReadSingle()
					aModel.vertexCount = Me.theInputFileReader.ReadInt32()
					aModel.normalCount = Me.theInputFileReader.ReadInt32()
					aModel.meshCount = Me.theInputFileReader.ReadInt32()

					aBodyPart.theModels.Add(aModel)

					fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
					Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aModel")

					Me.ReadVertexes(aModel)
					Me.ReadNormals(aModel)
					Me.ReadMeshes(aModel)
				Next

				'fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
				'Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aBodyPart.theModels " + aBodyPart.theModels.Count.ToString())

				'Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "theMdlFileData.theBones alignment")
			Catch ex As Exception
				Dim debug As Integer = 4242
			End Try
		End If
	End Sub

	Private Sub ReadNormals(ByVal aModel As SourceMdlModel04)
		If aModel.normalCount > 0 Then
			Dim fileOffsetStart As Long
			Dim fileOffsetEnd As Long

			Try
				fileOffsetStart = Me.theInputFileReader.BaseStream.Position

				aModel.theNormals = New List(Of SourceMdlNormal04)(aModel.normalCount)
				For normalIndex As Integer = 0 To aModel.normalCount - 1
					Dim aNormal As New SourceMdlNormal04()

					aNormal.index = Me.theInputFileReader.ReadInt32()
					aNormal.vector.x = Me.theInputFileReader.ReadSingle()
					aNormal.vector.y = Me.theInputFileReader.ReadSingle()
					aNormal.vector.z = Me.theInputFileReader.ReadSingle()

					aModel.theNormals.Add(aNormal)
				Next

				fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
				Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aModel.theNormals " + aModel.theNormals.Count.ToString())

				'Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "theMdlFileData.theBones alignment")
			Catch ex As Exception
				Dim debug As Integer = 4242
			End Try
		End If
	End Sub

	Private Sub ReadVertexes(ByVal aModel As SourceMdlModel04)
		If aModel.vertexCount > 0 Then
			Dim fileOffsetStart As Long
			Dim fileOffsetEnd As Long

			Try
				fileOffsetStart = Me.theInputFileReader.BaseStream.Position

				aModel.theVertexes = New List(Of SourceMdlVertex04)(aModel.vertexCount)
				For vertexIndex As Integer = 0 To aModel.vertexCount - 1
					Dim aVertex As New SourceMdlVertex04()

					aVertex.boneIndex = Me.theInputFileReader.ReadInt32()
					aVertex.vector.x = Me.theInputFileReader.ReadSingle()
					aVertex.vector.y = Me.theInputFileReader.ReadSingle()
					aVertex.vector.z = Me.theInputFileReader.ReadSingle()

					aModel.theVertexes.Add(aVertex)
				Next

				fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
				Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aModel.theVertexes " + aModel.theVertexes.Count.ToString())

				'Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "theMdlFileData.theBones alignment")
			Catch ex As Exception
				Dim debug As Integer = 4242
			End Try
		End If
	End Sub

	Private Sub ReadMeshes(ByVal aModel As SourceMdlModel04)
		If aModel.meshCount > 0 Then
			Dim fileOffsetStart As Long
			Dim fileOffsetEnd As Long

			Try
				fileOffsetStart = Me.theInputFileReader.BaseStream.Position

				aModel.theMeshes = New List(Of SourceMdlMesh04)(aModel.meshCount)
				For meshIndex As Integer = 0 To aModel.meshCount - 1
					fileOffsetStart = Me.theInputFileReader.BaseStream.Position

					Dim aMesh As New SourceMdlMesh04()

					aMesh.name = Me.theInputFileReader.ReadChars(aMesh.name.Length)
					aMesh.theName = aMesh.name
					aMesh.theName = StringClass.ConvertFromNullTerminatedOrFullLengthString(aMesh.theName)
					aMesh.faceCount = Me.theInputFileReader.ReadInt32()
					aMesh.unknownCount = Me.theInputFileReader.ReadInt32()
					aMesh.textureWidth = Me.theInputFileReader.ReadUInt32()
					aMesh.textureHeight = Me.theInputFileReader.ReadUInt32()

					aModel.theMeshes.Add(aMesh)

					fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
					Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aMesh name = " + aMesh.theName)

					Me.ReadFaces(aMesh)
					Me.ReadTextureBmpData(aMesh)
				Next

				'fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
				'Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aModel.theMeshes " + aModel.theMeshes.Count.ToString())

				'Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "theMdlFileData.theBones alignment")
			Catch ex As Exception
				Dim debug As Integer = 4242
			End Try
		End If
	End Sub

	Private Sub ReadFaces(ByVal aMesh As SourceMdlMesh04)
		If aMesh.faceCount > 0 Then
			Dim fileOffsetStart As Long
			Dim fileOffsetEnd As Long

			Try
				fileOffsetStart = Me.theInputFileReader.BaseStream.Position

				aMesh.theFaces = New List(Of SourceMdlFace04)(aMesh.faceCount)
				For faceIndex As Integer = 0 To aMesh.faceCount - 1
					Dim aFace As New SourceMdlFace04()

					'For x As Integer = 0 To aFace.vertexIndex.Length - 1
					'	aFace.vertexIndex(x) = Me.theInputFileReader.ReadInt32()
					'Next
					For x As Integer = 0 To aFace.vertexInfo.Length - 1
						aFace.vertexInfo(x).vertexIndex = Me.theInputFileReader.ReadInt32()
						aFace.vertexInfo(x).normalIndex = Me.theInputFileReader.ReadInt32()
						aFace.vertexInfo(x).s = Me.theInputFileReader.ReadInt32()
						aFace.vertexInfo(x).t = Me.theInputFileReader.ReadInt32()
					Next

					aMesh.theFaces.Add(aFace)
				Next

				fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
				Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aMesh.theFaces " + aMesh.theFaces.Count.ToString())

				'Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "theMdlFileData.theBones alignment")
			Catch ex As Exception
				Dim debug As Integer = 4242
			End Try
		End If
	End Sub

	Private Sub ReadTextureBmpData(ByVal aMesh As SourceMdlMesh04)
		Dim fileOffsetStart As Long
		Dim fileOffsetEnd As Long

		Try
			fileOffsetStart = Me.theInputFileReader.BaseStream.Position

			aMesh.theTextureBmpData = New List(Of Byte)(CType(aMesh.textureWidth * aMesh.textureHeight, Integer))
			For byteIndex As Long = 0 To (aMesh.textureWidth * aMesh.textureHeight + 256 * 3) - 1
				Dim data As Byte

				data = Me.theInputFileReader.ReadByte()

				aMesh.theTextureBmpData.Add(data)
			Next

			fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
			Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aMesh.theTextureBmpData")
		Catch ex As Exception
			Dim debug As Integer = 4242
		End Try
	End Sub

	Private Sub SetFileNameForMeshes(ByVal aBodyPart As SourceMdlBodyPart04)
		Dim aModel As SourceMdlModel04
		Dim aMesh As SourceMdlMesh04
		Dim textureFileName As String

		For bodyPartIndex As Integer = 0 To Me.theMdlFileData.theBodyParts.Count - 1
			aBodyPart = Me.theMdlFileData.theBodyParts(bodyPartIndex)
			For modelIndex As Integer = 0 To aBodyPart.theModels.Count - 1
				aModel = aBodyPart.theModels(modelIndex)
				For meshIndex As Integer = 0 To aModel.theMeshes.Count - 1
					aMesh = aModel.theMeshes(meshIndex)
					Try
						textureFileName = "bodypart" + bodyPartIndex.ToString() + "_model" + modelIndex.ToString() + "_mesh" + meshIndex.ToString() + ".bmp"
						aMesh.theTextureFileName = textureFileName
					Catch ex As Exception
						Dim debug As Integer = 4242
					End Try
				Next
			Next
		Next
	End Sub

#End Region

#Region "Data"

	Protected theInputFileReader As BinaryReader
	Protected theOutputFileWriter As BinaryWriter

	Protected theMdlFileData As SourceMdlFileData04

#End Region

End Class
