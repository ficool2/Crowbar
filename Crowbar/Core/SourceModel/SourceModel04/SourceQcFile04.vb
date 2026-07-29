Imports System.IO
Imports System.Text

Public Class SourceQcFile04

#Region "Create and Destroy"

	Public Sub New(ByVal outputFileStream As StreamWriter, ByVal outputPathFileName As String, ByVal mdlFileData As SourceMdlFileData04, ByVal modelName As String)
		Me.theOutputFileStreamWriter = outputFileStream
		Me.theMdlFileData = mdlFileData
		Me.theModelName = modelName

		Me.theOutputPath = FileManager.GetPath(outputPathFileName)
		Me.theOutputFileNameWithoutExtension = Path.GetFileNameWithoutExtension(outputPathFileName)
	End Sub

#End Region

#Region "Methods"

	Public Sub WriteHeaderComment()
		Common.WriteHeaderComment(Me.theOutputFileStreamWriter)
	End Sub

	Public Sub WriteBodyGroupCommand()
		Dim line As String = ""
		Dim aBodyPart As SourceMdlBodyPart04
		Dim aBodyModel As SourceMdlModel04

		If Me.theMdlFileData.theBodyParts IsNot Nothing AndAlso Me.theMdlFileData.theBodyParts.Count > 0 Then
			Me.theOutputFileStreamWriter.WriteLine()

			For bodyPartIndex As Integer = 0 To Me.theMdlFileData.theBodyParts.Count - 1
				aBodyPart = Me.theMdlFileData.theBodyParts(bodyPartIndex)

				line = "$bodygroup "
				line += """"
				line += aBodyPart.theName
				line += """"
				Me.theOutputFileStreamWriter.WriteLine(line)

				line = "{"
				Me.theOutputFileStreamWriter.WriteLine(line)

				If aBodyPart.theModels IsNot Nothing AndAlso aBodyPart.theModels.Count > 0 Then
					For modelIndex As Integer = 0 To aBodyPart.theModels.Count - 1
						aBodyModel = aBodyPart.theModels(modelIndex)

						line = vbTab
						aBodyModel.theSmdFileName = SourceFileNamesModule.CreateBodyGroupSmdFileName(aBodyModel.theSmdFileName, bodyPartIndex, modelIndex, 0, Me.theModelName, "")
						line += "studio "
						line += """"
						line += aBodyModel.theSmdFileName
						line += """"
						Me.theOutputFileStreamWriter.WriteLine(line)
					Next
				End If

				line = "}"
				Me.theOutputFileStreamWriter.WriteLine(line)
			Next
		End If
	End Sub

	Public Sub WriteModelNameCommand()
		Dim line As String = ""
		Dim modelPathFileName As String

		' MDL04 does not store model name, so use the file name.
		'modelPathFileName = Me.theMdlFileData.theModelName
		modelPathFileName = Me.theModelName

		Me.theOutputFileStreamWriter.WriteLine()

		line = "$modelname "
		line += """"
		line += modelPathFileName
		line += """"
		Me.theOutputFileStreamWriter.WriteLine(line)
	End Sub

	Public Sub WriteSequenceCommands()
		Dim line As String = ""
		Dim aSequence As SourceMdlSequenceDesc04

		If Me.theMdlFileData.theSequenceDescs IsNot Nothing AndAlso Me.theMdlFileData.theSequenceDescs.Count > 0 Then
			Me.theOutputFileStreamWriter.WriteLine()

			For sequenceGroupIndex As Integer = 0 To Me.theMdlFileData.theSequenceDescs.Count - 1
				aSequence = Me.theMdlFileData.theSequenceDescs(sequenceGroupIndex)

				If TheApp.Settings.DecompileQcUseMixedCaseForKeywordsIsChecked Then
					line = "$Sequence "
				Else
					line = "$sequence "
				End If
				line += """"
				line += aSequence.theName
				line += """"
				'NOTE: Opening brace must be on same line as the command.
				line += " {"
				Me.theOutputFileStreamWriter.WriteLine(line)

				aSequence.theSmdRelativePathFileName = SourceFileNamesModule.CreateAnimationSmdRelativePathFileName(aSequence.theSmdRelativePathFileName, Me.theModelName, aSequence.theName, -1)
				line = vbTab
				line += """"
				line += FileManager.GetPathFileNameWithoutExtension(aSequence.theSmdRelativePathFileName)
				line += """"
				Me.theOutputFileStreamWriter.WriteLine(line)

				line = "}"
				Me.theOutputFileStreamWriter.WriteLine(line)
			Next
		End If
	End Sub

#End Region

#Region "Private Delegates"

#End Region

#Region "Private Methods"

#End Region

#Region "Constants"

#End Region

#Region "Data"

	Private theOutputFileStreamWriter As StreamWriter
	Private theMdlFileData As SourceMdlFileData04
	Private theModelName As String

	Private theOutputPath As String
	Private theOutputFileNameWithoutExtension As String

#End Region

End Class
