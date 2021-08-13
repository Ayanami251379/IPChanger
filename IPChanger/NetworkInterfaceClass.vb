Imports System.Net
Imports System.Net.NetworkInformation

Public Class NetworkInterfaceClass
    Public Property Name As String
        Get
            Return GsName
        End Get
        Set(value As String)
            GsName = value.Trim
        End Set
    End Property
    Private GsName As String = ""

    Public Property Description As String
        Get
            Return GsDescription
        End Get
        Set(value As String)
            GsDescription = value.Trim
        End Set
    End Property
    Private GsDescription As String = ""

    Public Property MACAddress As String
        Get
            Return GsMACAddress
        End Get
        Set(value As String)
            GsMACAddress = value.Trim
        End Set
    End Property
    Private GsMACAddress As String = ""

    Public Property IPAddress As String
        Get
            Return GsIPAddress
        End Get
        Set(value As String)
            GsIPAddress = value.Trim
        End Set
    End Property
    Private GsIPAddress As String = ""

    Public Property SubnetMask As String
        Get
            Return GsSubnetMask
        End Get
        Set(value As String)
            GsSubnetMask = value.Trim
        End Set
    End Property
    Private GsSubnetMask As String = ""

    Public Property DefaultGateway As String
        Get
            Return GsDefaultGateway
        End Get
        Set(value As String)
            GsDefaultGateway = value.Trim
        End Set
    End Property
    Private GsDefaultGateway As String = ""

    Public Property DNS1 As String
        Get
            Return GsDNS1
        End Get
        Set(value As String)
            GsDNS1 = value.Trim
        End Set
    End Property
    Private GsDNS1 As String = ""

    Public Property DNS2 As String
        Get
            Return GsDNS2
        End Get
        Set(value As String)
            GsDNS2 = value.Trim
        End Set
    End Property
    Private GsDNS2 As String = ""

    Public Sub New()

    End Sub

    Public Sub New(ByVal IP As String)
        Try
            For Each InternalIP In Dns.GetHostEntry(Dns.GetHostName).AddressList
                If InternalIP.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                    If IP.Trim = InternalIP.ToString.Trim Then
                        GsIPAddress = IP.Trim

                        For Each networkCard As NetworkInterface In NetworkInterface.GetAllNetworkInterfaces
                            For Each ipAddressInformation As UnicastIPAddressInformation In networkCard.GetIPProperties().UnicastAddresses
                                If ipAddressInformation.Address.AddressFamily = Sockets.AddressFamily.InterNetwork Then

                                    If InternalIP.Equals(ipAddressInformation.Address) Then

                                        If Not String.IsNullOrEmpty(networkCard.Name) Then GsName = networkCard.Name.Trim
                                        If Not String.IsNullOrEmpty(networkCard.Description) Then GsDescription = networkCard.Description.Trim

                                        Try
                                            If Not networkCard.GetPhysicalAddress.ToString.Trim = "" Then
                                                Dim SimpleMAC(5) As String
                                                Try
                                                    SimpleMAC(0) = networkCard.GetPhysicalAddress.ToString.Trim.Substring(0, 2)
                                                    SimpleMAC(1) = networkCard.GetPhysicalAddress.ToString.Trim.Substring(2, 2)
                                                    SimpleMAC(2) = networkCard.GetPhysicalAddress.ToString.Trim.Substring(4, 2)
                                                    SimpleMAC(3) = networkCard.GetPhysicalAddress.ToString.Trim.Substring(6, 2)
                                                    SimpleMAC(4) = networkCard.GetPhysicalAddress.ToString.Trim.Substring(8, 2)
                                                    SimpleMAC(5) = networkCard.GetPhysicalAddress.ToString.Trim.Substring(10, 2)
                                                    'GsMACAddress = String.Join(":", SimpleMAC).Substring(0, 17)
                                                    GsMACAddress = String.Join(":", SimpleMAC).Trim
                                                Catch ex As Exception
                                                    GsMACAddress = "[ERROR TRANSLATING]"
                                                End Try
                                            End If
                                        Catch ex As Exception

                                        End Try

                                        GsSubnetMask = ipAddressInformation.IPv4Mask.ToString.Trim

                                        Try
                                            For Each gatewayAddr As GatewayIPAddressInformation In networkCard.GetIPProperties.GatewayAddresses
                                                If Not gatewayAddr.Address.ToString.StartsWith("0") Then
                                                    GsDefaultGateway = gatewayAddr.Address.ToString.Trim
                                                End If
                                            Next
                                        Catch ex As Exception

                                        End Try

                                        Try
                                            Dim bFilledDNS1 As Boolean = False
                                            Dim bFilledDNS2 As Boolean = False

                                            For Each address In networkCard.GetIPProperties.DnsAddresses
                                                If bFilledDNS1 And bFilledDNS2 Then Exit For

                                                If Not address.ToString.Trim.StartsWith("0") Then
                                                    If Not bFilledDNS1 Then
                                                        bFilledDNS1 = True
                                                        GsDNS1 = address.ToString.Trim
                                                    Else
                                                        If Not bFilledDNS2 Then
                                                            bFilledDNS2 = True
                                                            GsDNS2 = address.ToString.Trim
                                                        End If
                                                    End If
                                                End If

                                            Next
                                        Catch ex As Exception

                                        End Try

                                        Exit For
                                    End If

                                End If
                            Next
                        Next

                        Exit For
                    End If
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub
End Class
