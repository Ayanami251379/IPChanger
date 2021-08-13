Imports MadMilkman.Ini

Public Class ImportExportUtility
    Public Shared Function ImportFile(ByVal Path As String) As NetworkInterfaceClass
        Dim ImportData As NetworkInterfaceClass = Nothing

        Try

            If IO.File.Exists(Path) Then
                ImportData = New NetworkInterfaceClass

                Dim LoadFile As New IniFile()
                LoadFile.Load(Path)

                For Each LoadSection As IniSection In LoadFile.Sections
                    If LoadSection.Name.Trim() = "Network Interface" Then
                        For Each LoadKey As IniKey In LoadSection.Keys
                            Select Case LoadKey.Name.Trim().ToLower
                                Case "ipaddress" : ImportData.IPAddress = LoadKey.Value
                                Case "subnetmask" : ImportData.SubnetMask = LoadKey.Value
                                Case "defaultgateway" : ImportData.DefaultGateway = LoadKey.Value
                                Case "dns1" : ImportData.DNS1 = LoadKey.Value
                                Case "dns2" : ImportData.DNS2 = LoadKey.Value
                            End Select
                        Next
                    End If
                Next

            End If

        Catch ex As Exception
            ImportData = Nothing
        End Try

        Return ImportData
    End Function

    Public Shared Function ExportFile(ByVal NetworkInterface As NetworkInterfaceClass, ByVal Path As String) As Boolean
        Try

            If IO.File.Exists(Path) Then IO.File.Delete(Path)

            Dim SaveFile As New IniFile(New IniOptions())
            Dim FileSection As New IniSection(SaveFile, "Network Interface")

            Dim IPAddress As New IniKey(SaveFile, "IPAddress", NetworkInterface.IPAddress)
            Dim SubNetMask As New IniKey(SaveFile, "SubnetMask", NetworkInterface.SubnetMask)
            Dim DefaultGateway As New IniKey(SaveFile, "DefaultGateway", NetworkInterface.DefaultGateway)
            Dim DNS1 As New IniKey(SaveFile, "DNS1", NetworkInterface.DNS1)
            Dim DNS2 As New IniKey(SaveFile, "DNS2", NetworkInterface.DNS2)

            SaveFile.Sections.Add(FileSection)

            FileSection.Keys.Add(IPAddress)
            FileSection.Keys.Add(SubNetMask)
            FileSection.Keys.Add(DefaultGateway)
            FileSection.Keys.Add(DNS1)
            FileSection.Keys.Add(DNS2)

            SaveFile.Save(Path)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
