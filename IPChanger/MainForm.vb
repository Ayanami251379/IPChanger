Imports System.ComponentModel
Imports System.Management
Imports System.Net
Imports System.Net.NetworkInformation

Public Class MainForm
    Private WithEvents bwNICRefresh As BackgroundWorker
    Private WithEvents bwNICLoader As BackgroundWorker
    Private WithEvents bwNICSaver As BackgroundWorker
    'Private WithEvents bwNICExporter As BackgroundWorker
    'Private WithEvents bwNICImporter As BackgroundWorker

    Private ClNetworkInterface As List(Of NetworkInterfaceClass) = Nothing

    Private GcNetworkInterface As NetworkInterfaceClass = Nothing

    Private GsQuery As String = ""
    Private GbResult As Boolean = False


    Private Function GetPath() As String
        Dim sFilePath As String = AppDomain.CurrentDomain.BaseDirectory
        If Not sFilePath.EndsWith("\") Then
            sFilePath = sFilePath + "\"
        End If

        sFilePath = sFilePath + "ConfigFiles\"
        If Not IO.Directory.Exists(sFilePath) Then IO.Directory.CreateDirectory(sFilePath)
        Return sFilePath
    End Function

#Region "NETSH Functions"
    Private Function SetInterfaceIP(ByVal InterfaceName As String, ByVal IPAddress As String, ByVal SubnetMask As String, Optional ByVal DefaultGateway As String = "") As Boolean
        Dim bRerutn As Boolean = False

        Try
            Dim Proc As New Process
            Dim ProcInfo As New ProcessStartInfo("netsh")

            Dim Args As New System.Text.StringBuilder
            Args.Append(String.Format("interface ip set address {0} ", InterfaceName))
            Args.Append(String.Format("static {0} {1} ", IPAddress, SubnetMask))
            If Not GcNetworkInterface.DefaultGateway.Trim = "" Then
                Args.Append(String.Format("{0} 1 ", DefaultGateway))
            End If

            ProcInfo.Arguments = Args.ToString
            ProcInfo.CreateNoWindow = True

            Proc.StartInfo = ProcInfo
            Proc.Start()
            Proc.WaitForExit()

            If Proc.ExitCode = 0 Then
                bRerutn = True
            End If
            Proc.Dispose()
        Catch ex As Exception
            bRerutn = False
        End Try

        Return bRerutn
    End Function

    Private Function SetInterfaceDNS1(ByVal InterfaceName As String, ByVal DNS As String) As Boolean
        Dim bRerutn As Boolean = False

        Try
            Dim Proc As New Process
            Dim ProcInfo As New ProcessStartInfo("netsh")

            ProcInfo.Arguments = String.Format("interface ip set dns {0} static {1}", InterfaceName, DNS)
            ProcInfo.CreateNoWindow = True

            Proc.StartInfo = ProcInfo
            Proc.Start()
            Proc.WaitForExit()

            If Proc.ExitCode = 0 Then
                bRerutn = True
            End If
            Proc.Dispose()
        Catch ex As Exception
            bRerutn = False
        End Try

        Return bRerutn
    End Function

    Private Function SetInterfaceDNS2(ByVal InterfaceName As String, ByVal DNS As String) As Boolean
        Dim bRerutn As Boolean = False

        Try
            Dim Proc As New Process
            Dim ProcInfo As New ProcessStartInfo("netsh")

            ProcInfo.Arguments = String.Format("interface ip set dns {0} {1} index=2", InterfaceName, DNS)
            ProcInfo.CreateNoWindow = True

            Proc.StartInfo = ProcInfo
            Proc.Start()
            Proc.WaitForExit()

            If Proc.ExitCode = 0 Then
                bRerutn = True
            End If
            Proc.Dispose()
        Catch ex As Exception
            bRerutn = False
        End Try

        Return bRerutn
    End Function
#End Region

    Private Sub SetControlsEnabled(ByVal bEnabled As Boolean)
        cbNIC.Enabled = bEnabled
        btnNICRefresh.Enabled = bEnabled
        btnNICLoad.Enabled = bEnabled
        btnNICSave.Enabled = bEnabled
        btnNICExport.Enabled = bEnabled
        btnNICImport.Enabled = bEnabled
        txtNICMAC.Enabled = bEnabled
        txtNICAddress.Enabled = bEnabled
        txtNICSubnetMask.Enabled = bEnabled
        txtNICDefaultGateway.Enabled = bEnabled
        txtNICDNS1.Enabled = bEnabled
        txtNICDNS2.Enabled = bEnabled
    End Sub



    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        StartNicRefresh()
    End Sub



    Private Sub btnNICRefresh_Click(sender As Object, e As EventArgs) Handles btnNICRefresh.Click
        StartNicRefresh()
    End Sub

    Private Sub btnNICLoad_Click(sender As Object, e As EventArgs) Handles btnNICLoad.Click
        StartNicLoader()
    End Sub

    Private Sub btnNICSave_Click(sender As Object, e As EventArgs) Handles btnNICSave.Click
        StartNicSaver()
    End Sub

    Private Sub btnNICExport_Click(sender As Object, e As EventArgs) Handles btnNICExport.Click
        If cbNIC.SelectedIndex <= -1 Then
            MsgBox("Please select an interface.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "NO NIC SELECTED")
            Exit Sub
        End If

        SetControlsEnabled(False)
        Dim ExportInterface As NetworkInterfaceClass = Nothing

        Try
            For Each networkInterface As NetworkInterfaceClass In ClNetworkInterface
                If networkInterface.Description.Trim.ToLower = GsQuery.Trim.ToLower Then
                    ExportInterface = networkInterface
                End If
            Next

            If IsNothing(ExportInterface) Then
                MsgBox("Somthing went wrong.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "ERROR FINDING INTERFACE")
                Exit Sub
            End If

            Dim sfd As New SaveFileDialog
            sfd.Filter = "IP changer data file.|*.ipcfg"
            sfd.InitialDirectory = GetPath()
            sfd.FileName = ExportInterface.Description

            If sfd.ShowDialog = DialogResult.OK Then
                If ImportExportUtility.ExportFile(ExportInterface, sfd.FileName) Then
                    MsgBox("File saved.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "EXPORT SUCCESS")
                Else
                    MsgBox("Could not save file.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "ERROR WHILE EXPORTING")
                End If
            End If
        Catch ex As Exception

        End Try
        SetControlsEnabled(True)

    End Sub

    Private Sub btnNICImport_Click(sender As Object, e As EventArgs) Handles btnNICImport.Click
        If cbNIC.SelectedIndex <= -1 Then
            MsgBox("Please select an interface.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "NO NIC SELECTED")
            Exit Sub
        End If

        SetControlsEnabled(False)
        Try
            Dim ofd As New OpenFileDialog
            ofd.Filter = "IP changer data file.|*.ipcfg"
            ofd.InitialDirectory = GetPath()

            If ofd.ShowDialog = DialogResult.OK Then
                Dim ImportInterface As NetworkInterfaceClass = ImportExportUtility.ImportFile(ofd.FileName)
                If IsNothing(ImportInterface) Then
                    MsgBox("Could not load file.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "ERROR WHILE IMPORTING")
                Else
                    txtNICAddress.Text = ImportInterface.IPAddress
                    txtNICSubnetMask.Text = ImportInterface.SubnetMask
                    txtNICDefaultGateway.Text = ImportInterface.DefaultGateway
                    txtNICDNS1.Text = ImportInterface.DNS1
                    txtNICDNS2.Text = ImportInterface.DNS2

                    MsgBox("File loaded.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "IMPORT SUCCESS")
                End If
            End If
        Catch ex As Exception

        End Try
        SetControlsEnabled(True)

    End Sub

    Private Sub cbNIC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbNIC.SelectedIndexChanged
        StartNicLoader()
    End Sub


#Region "Background Workers"
    Private Sub StartWorker(ByRef Worker As BackgroundWorker)
        If Not IsNothing(Worker) Then
            If Worker.IsBusy Then
                Worker.CancelAsync()
            End If
        End If
        Worker = Nothing

        Worker = New BackgroundWorker

        Worker.WorkerReportsProgress = True
        Worker.WorkerSupportsCancellation = True

        Worker.RunWorkerAsync()
    End Sub

    Private Sub StartNicRefresh()
        cbNIC.Items.Clear()
        ClNetworkInterface = New List(Of NetworkInterfaceClass)
        SetControlsEnabled(False)
        'StartWorker(bwNICRefresh)

        txtNICMAC.Text = ""
        txtNICAddress.Text = ""
        txtNICSubnetMask.Text = ""
        txtNICDefaultGateway.Text = ""
        txtNICDNS1.Text = ""
        txtNICDNS2.Text = ""

        If Not IsNothing(bwNICRefresh) Then
            If bwNICRefresh.IsBusy Then
                bwNICRefresh.CancelAsync()
            End If
        End If
        bwNICRefresh = Nothing

        bwNICRefresh = New BackgroundWorker

        bwNICRefresh.WorkerReportsProgress = True
        bwNICRefresh.WorkerSupportsCancellation = True

        bwNICRefresh.RunWorkerAsync()
    End Sub

    Private Sub StartNicLoader()
        If cbNIC.SelectedIndex <= -1 Then
            MsgBox("Please select an interface.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "NO NIC SELECTED")
            Exit Sub
        End If
        SetControlsEnabled(False)
        txtNICMAC.Text = ""
        txtNICAddress.Text = ""
        txtNICSubnetMask.Text = ""
        txtNICDefaultGateway.Text = ""
        txtNICDNS1.Text = ""
        txtNICDNS2.Text = ""

        GsQuery = cbNIC.Items.Item(cbNIC.SelectedIndex).ToString.Trim
        'StartWorker(bwNICLoader)

        If Not IsNothing(bwNICLoader) Then
            If bwNICLoader.IsBusy Then
                bwNICLoader.CancelAsync()
            End If
        End If
        bwNICLoader = Nothing

        bwNICLoader = New BackgroundWorker

        bwNICLoader.WorkerReportsProgress = True
        bwNICLoader.WorkerSupportsCancellation = True

        bwNICLoader.RunWorkerAsync()
    End Sub

    Private Sub StartNicSaver()
        If cbNIC.SelectedIndex <= -1 Then
            MsgBox("Please select an interface.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "NO NIC SELECTED")
            Exit Sub
        End If
        SetControlsEnabled(False)

        GsQuery = cbNIC.Items.Item(cbNIC.SelectedIndex).ToString.Trim

        GcNetworkInterface = New NetworkInterfaceClass
        GcNetworkInterface.IPAddress = txtNICAddress.Text
        GcNetworkInterface.SubnetMask = txtNICSubnetMask.Text
        GcNetworkInterface.DefaultGateway = txtNICDefaultGateway.Text
        GcNetworkInterface.DNS1 = txtNICDNS1.Text
        GcNetworkInterface.DNS2 = txtNICDNS2.Text

        'StartWorker(bwNICSaver)
        If Not IsNothing(bwNICSaver) Then
            If bwNICSaver.IsBusy Then
                bwNICSaver.CancelAsync()
            End If
        End If
        bwNICSaver = Nothing

        bwNICSaver = New BackgroundWorker

        bwNICSaver.WorkerReportsProgress = True
        bwNICSaver.WorkerSupportsCancellation = True

        bwNICSaver.RunWorkerAsync()
    End Sub

    'Private Sub StartNicExporter()
    '    SetControlsEnabled(False)
    '    'StartWorker(bwNICExporter)
    '    If Not IsNothing(bwNICExporter) Then
    '        If bwNICExporter.IsBusy Then
    '            bwNICExporter.CancelAsync()
    '        End If
    '    End If
    '    bwNICExporter = Nothing
    '
    '    bwNICExporter = New BackgroundWorker
    '
    '    bwNICExporter.WorkerReportsProgress = True
    '    bwNICExporter.WorkerSupportsCancellation = True
    '
    '    bwNICExporter.RunWorkerAsync()
    'End Sub
    '
    'Private Sub StartNicImporter()
    '    SetControlsEnabled(False)
    '    'StartWorker(bwNICImporter)
    '    If Not IsNothing(bwNICImporter) Then
    '        If bwNICImporter.IsBusy Then
    '            bwNICImporter.CancelAsync()
    '        End If
    '    End If
    '    bwNICImporter = Nothing
    '
    '    bwNICImporter = New BackgroundWorker
    '
    '    bwNICImporter.WorkerReportsProgress = True
    '    bwNICImporter.WorkerSupportsCancellation = True
    '
    '    bwNICImporter.RunWorkerAsync()
    'End Sub

#Region "Refresh"
    Private Sub bwNICRefresh_DoWork(sender As Object, e As DoWorkEventArgs) Handles bwNICRefresh.DoWork
        'Dim objMC As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
        'Dim objMOC As ManagementObjectCollection = objMC.GetInstances()
        '
        'For Each objMO As ManagementObject In objMOC
        '    If (Not CBool(objMO("IPEnabled"))) Then
        '        Continue For
        '    End If
        '    'Caption
        '
        '    Dim NICCaption As String = objMO("Caption")
        '    If Not String.IsNullOrEmpty(NICCaption) Then bwNICRefresh.ReportProgress(0, NICCaption)
        'Next

        'Try
        '    'Dim ipProperties As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties
        '
        '    Dim strBldr As New System.Text.StringBuilder
        '    Dim ipProperties As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties
        '    strBldr.Append("Host name: ".PadRight(15) & ipProperties.HostName & ControlChars.NewLine)
        '    strBldr.Append("Domain name: ".PadRight(15) & ipProperties.DomainName & ControlChars.NewLine)
        '    strBldr.AppendLine("===================================================================================")
        '
        '    For Each networkCard As NetworkInterface In NetworkInterface.GetAllNetworkInterfaces
        '        If Not String.IsNullOrEmpty(networkCard.Description) Then bwNICRefresh.ReportProgress(0, networkCard.Description)
        '
        '        strBldr.Append("Interface: " & networkCard.Id & ControlChars.NewLine)
        '        strBldr.Append("" & ControlChars.Tab & " Name: ".PadRight(15) & networkCard.Name & ControlChars.NewLine)
        '        strBldr.Append("" & ControlChars.Tab & " Description: ".PadRight(15) & networkCard.Description & ControlChars.NewLine)
        '        strBldr.Append("" & ControlChars.Tab & " Status: ".PadRight(15) & networkCard.OperationalStatus.ToString() & ControlChars.NewLine)
        '        strBldr.Append("" & ControlChars.Tab & " Speed: ".PadRight(15) & (networkCard.Speed / 1000000).ToString("#,000") & " Mbps" & ControlChars.NewLine)
        '        strBldr.Append("" & ControlChars.Tab & " MAC Address: ".PadRight(15) & networkCard.GetPhysicalAddress.ToString & ControlChars.NewLine)
        '        Dim IPProp As IPInterfaceProperties = networkCard.GetIPProperties
        '        If Not IPProp Is Nothing Then
        '            strBldr.Append("" & ControlChars.Tab & " DNS Enabled: ".PadRight(15) & IPProp.IsDnsEnabled.ToString & ControlChars.NewLine)
        '            strBldr.Append("" & ControlChars.Tab & " Dynamics DNS: ".PadRight(15) & IPProp.IsDynamicDnsEnabled.ToString & ControlChars.NewLine)
        '        End If
        '        Dim IPv4 As IPv4InterfaceProperties = networkCard.GetIPProperties.GetIPv4Properties
        '        If Not IPv4 Is Nothing Then
        '            strBldr.Append("" & ControlChars.Tab & " DHCP Enabled: ".PadRight(15) & IPv4.IsDhcpEnabled.ToString & ControlChars.NewLine)
        '            strBldr.Append("" & ControlChars.Tab & " MTU Setting: ".PadRight(15) & IPv4.Mtu.ToString & ControlChars.NewLine)
        '            strBldr.Append("" & ControlChars.Tab & " Uses WINS: ".PadRight(15) & IPv4.UsesWins.ToString & ControlChars.NewLine)
        '        End If
        '        strBldr.Append("" & ControlChars.Tab & " Gateway Address:" & ControlChars.NewLine)
        '        For Each gatewayAddr As GatewayIPAddressInformation In networkCard.GetIPProperties.GatewayAddresses
        '            strBldr.Append("" & ControlChars.Tab & "" & ControlChars.Tab & " Gateway entry: " & gatewayAddr.Address.ToString & ControlChars.NewLine)
        '        Next
        '        strBldr.Append("" & ControlChars.Tab & " DNS Settings:" & ControlChars.NewLine)
        '        Dim address As IPAddress
        '        For Each address In networkCard.GetIPProperties.DnsAddresses
        '            strBldr.Append("" & ControlChars.Tab & "" & ControlChars.Tab & " DNS entry: ".PadRight(17) & address.ToString & ControlChars.NewLine)
        '        Next
        '        'strBldr.Append("Current IP Connections:" & ControlChars.NewLine)
        '        'For Each tcpConnection As TcpConnectionInformation In IPGlobalProperties.GetIPGlobalProperties.GetActiveTcpConnections
        '        '    strBldr.Append("" & ControlChars.Tab & " Connection Info:" & ControlChars.NewLine)
        '        '    strBldr.Append("" & ControlChars.Tab & "" & ControlChars.Tab & " Remote Address: " & tcpConnection.RemoteEndPoint.Address.ToString & ControlChars.NewLine)
        '        '    strBldr.Append("" & ControlChars.Tab & "" & ControlChars.Tab & " State: ".PadRight(17) & tcpConnection.State.ToString & ControlChars.NewLine)
        '        'Next
        '        strBldr.AppendLine("===================================================================================")
        '    Next
        '
        '    Debug.Print(strBldr.ToString())
        'Catch ex As Exception
        '
        'End Try

        Try
            For Each InternalIP In Dns.GetHostEntry(Dns.GetHostName).AddressList
                If InternalIP.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                    Dim NetworkInterface As New NetworkInterfaceClass(InternalIP.ToString)
                    ClNetworkInterface.Add(NetworkInterface)
                    If Not String.IsNullOrEmpty(NetworkInterface.Description) Then bwNICRefresh.ReportProgress(0, NetworkInterface.Description)
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub bwNICRefresh_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles bwNICRefresh.ProgressChanged
        cbNIC.Items.Add(e.UserState.ToString.Trim)
    End Sub

    Private Sub bwNICRefresh_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bwNICRefresh.RunWorkerCompleted
        SetControlsEnabled(True)
    End Sub
#End Region

#Region "Loader"
    Private Sub bwNICLoader_DoWork(sender As Object, e As DoWorkEventArgs) Handles bwNICLoader.DoWork
        Try
            For Each networkInterface As NetworkInterfaceClass In ClNetworkInterface
                If networkInterface.Description.Trim.ToLower = GsQuery.Trim.ToLower Then
                    bwNICLoader.ReportProgress(0, networkInterface.MACAddress)
                    bwNICLoader.ReportProgress(1, networkInterface.IPAddress)
                    bwNICLoader.ReportProgress(2, networkInterface.SubnetMask)
                    bwNICLoader.ReportProgress(3, networkInterface.DefaultGateway)
                    bwNICLoader.ReportProgress(4, networkInterface.DNS1)
                    bwNICLoader.ReportProgress(5, networkInterface.DNS2)
                End If
            Next
        Catch ex As Exception

        End Try

    End Sub
    Private Sub bwNICLoader_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles bwNICLoader.ProgressChanged
        If String.IsNullOrEmpty(e.UserState) Then Exit Sub
        Select Case e.ProgressPercentage
            Case 0 : txtNICMAC.Text = e.UserState.ToString.Trim
            Case 1 : txtNICAddress.Text = e.UserState.ToString.Trim
            Case 2 : txtNICSubnetMask.Text = e.UserState.ToString.Trim
            Case 3 : txtNICDefaultGateway.Text = e.UserState.ToString.Trim
            Case 4 : txtNICDNS1.Text = e.UserState.ToString.Trim
            Case 5 : txtNICDNS2.Text = e.UserState.ToString.Trim
        End Select
    End Sub

    Private Sub bwNICLoader_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bwNICLoader.RunWorkerCompleted
        SetControlsEnabled(True)
    End Sub
#End Region

#Region "Saver"
    Private Sub bwNICSaver_DoWork(sender As Object, e As DoWorkEventArgs) Handles bwNICSaver.DoWork
        Try
            GbResult = False
            Dim TargetInterface As NetworkInterfaceClass = Nothing

            For Each networkInterface As NetworkInterfaceClass In ClNetworkInterface
                If networkInterface.Description.Trim.ToLower = GsQuery.Trim.ToLower Then
                    TargetInterface = networkInterface
                End If
            Next

            'GcNetworkInterface

            'Dim Proc As New Process
            'Dim ProcInfo As New ProcessStartInfo("netsh")
            '
            'Dim Args As New System.Text.StringBuilder
            'Args.Append(String.Format("interface ip set address {0} ", TargetInterface.Name))
            'Args.Append(String.Format("static {0} {1} ", GcNetworkInterface.IPAddress, GcNetworkInterface.SubnetMask))
            'If Not GcNetworkInterface.DefaultGateway.Trim = "" Then
            '    Args.Append(String.Format("{0} 1 ", GcNetworkInterface.DefaultGateway))
            'End If
            '
            'ProcInfo.Arguments = Args.ToString
            '
            'Proc.StartInfo = ProcInfo
            'Proc.Start()
            'If Proc.ExitCode = 0 Then
            '
            'End If
            'Proc.Dispose()

            If TargetInterface.IPAddress.Trim = GcNetworkInterface.IPAddress.Trim And TargetInterface.SubnetMask.Trim = GcNetworkInterface.SubnetMask.Trim And TargetInterface.DefaultGateway.Trim = GcNetworkInterface.DefaultGateway.Trim Then
                If GcNetworkInterface.DNS1.Trim & GcNetworkInterface.DNS2.Trim = "" Then
                Else
                    If GcNetworkInterface.DNS1.Trim = "" Then
                        SetInterfaceDNS1(TargetInterface.Name, GcNetworkInterface.DNS2)
                    Else
                        SetInterfaceDNS1(TargetInterface.Name, GcNetworkInterface.DNS1)

                        If Not GcNetworkInterface.DNS2.Trim = "" Then SetInterfaceDNS2(TargetInterface.Name, GcNetworkInterface.DNS2)
                    End If
                End If

                GbResult = True
            Else
                If SetInterfaceIP(TargetInterface.Name, GcNetworkInterface.IPAddress, GcNetworkInterface.SubnetMask, GcNetworkInterface.DefaultGateway) Then
                    GbResult = True

                    If GcNetworkInterface.DNS1.Trim & GcNetworkInterface.DNS2.Trim = "" Then
                    Else
                        If GcNetworkInterface.DNS1.Trim = "" Then
                            SetInterfaceDNS1(TargetInterface.Name, GcNetworkInterface.DNS2)
                        Else
                            SetInterfaceDNS1(TargetInterface.Name, GcNetworkInterface.DNS1)

                            If Not GcNetworkInterface.DNS2.Trim = "" Then SetInterfaceDNS2(TargetInterface.Name, GcNetworkInterface.DNS2)
                        End If
                    End If
                End If
            End If



        Catch ex As Exception

        End Try
    End Sub

    Private Sub bwNICSaver_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles bwNICSaver.ProgressChanged

    End Sub

    Private Sub bwNICSaver_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bwNICSaver.RunWorkerCompleted
        If GbResult Then
            MsgBox("Interface changes saved.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "SUCCESS")
            StartNicRefresh()
        Else
            MsgBox("Interface changes failed to save.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "FAILURE")
        End If
        SetControlsEnabled(True)
    End Sub
#End Region

    '#Region "Exporter"
    '    Private Sub bwNICExporter_DoWork(sender As Object, e As DoWorkEventArgs) Handles bwNICExporter.DoWork
    '
    '    End Sub
    '
    '    Private Sub bwNICExporter_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles bwNICExporter.ProgressChanged
    '
    '    End Sub
    '
    '    Private Sub bwNICExporter_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bwNICExporter.RunWorkerCompleted
    '
    '    End Sub
    '#End Region
    '
    '#Region "Importer"
    '    Private Sub bwNICImporter_DoWork(sender As Object, e As DoWorkEventArgs) Handles bwNICImporter.DoWork
    '
    '    End Sub
    '
    '    Private Sub bwNICImporter_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles bwNICImporter.ProgressChanged
    '
    '    End Sub
    '
    '    Private Sub bwNICImporter_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bwNICImporter.RunWorkerCompleted
    '
    '    End Sub
    '#End Region

#End Region




End Class
