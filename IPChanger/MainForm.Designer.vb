<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.cbNIC = New System.Windows.Forms.ComboBox()
        Me.btnNICRefresh = New System.Windows.Forms.Button()
        Me.btnNICLoad = New System.Windows.Forms.Button()
        Me.btnNICSave = New System.Windows.Forms.Button()
        Me.lblNICAddress = New System.Windows.Forms.Label()
        Me.lblNICSubnetMask = New System.Windows.Forms.Label()
        Me.lblNICDefaultGateway = New System.Windows.Forms.Label()
        Me.lblNICDNS1 = New System.Windows.Forms.Label()
        Me.lblNICDNS2 = New System.Windows.Forms.Label()
        Me.txtNICAddress = New System.Windows.Forms.TextBox()
        Me.txtNICSubnetMask = New System.Windows.Forms.TextBox()
        Me.txtNICDefaultGateway = New System.Windows.Forms.TextBox()
        Me.txtNICDNS1 = New System.Windows.Forms.TextBox()
        Me.txtNICDNS2 = New System.Windows.Forms.TextBox()
        Me.btnNICImport = New System.Windows.Forms.Button()
        Me.btnNICExport = New System.Windows.Forms.Button()
        Me.txtNICMAC = New System.Windows.Forms.TextBox()
        Me.lblNICMAC = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cbNIC
        '
        Me.cbNIC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbNIC.FormattingEnabled = True
        Me.cbNIC.Location = New System.Drawing.Point(12, 12)
        Me.cbNIC.Name = "cbNIC"
        Me.cbNIC.Size = New System.Drawing.Size(408, 24)
        Me.cbNIC.TabIndex = 0
        '
        'btnNICRefresh
        '
        Me.btnNICRefresh.Location = New System.Drawing.Point(12, 42)
        Me.btnNICRefresh.Name = "btnNICRefresh"
        Me.btnNICRefresh.Size = New System.Drawing.Size(132, 30)
        Me.btnNICRefresh.TabIndex = 1
        Me.btnNICRefresh.Text = "Refresh"
        Me.btnNICRefresh.UseVisualStyleBackColor = True
        '
        'btnNICLoad
        '
        Me.btnNICLoad.Location = New System.Drawing.Point(150, 42)
        Me.btnNICLoad.Name = "btnNICLoad"
        Me.btnNICLoad.Size = New System.Drawing.Size(132, 30)
        Me.btnNICLoad.TabIndex = 2
        Me.btnNICLoad.Text = "Load"
        Me.btnNICLoad.UseVisualStyleBackColor = True
        '
        'btnNICSave
        '
        Me.btnNICSave.Location = New System.Drawing.Point(288, 42)
        Me.btnNICSave.Name = "btnNICSave"
        Me.btnNICSave.Size = New System.Drawing.Size(132, 30)
        Me.btnNICSave.TabIndex = 3
        Me.btnNICSave.Text = "Save"
        Me.btnNICSave.UseVisualStyleBackColor = True
        '
        'lblNICAddress
        '
        Me.lblNICAddress.AutoSize = True
        Me.lblNICAddress.Location = New System.Drawing.Point(12, 110)
        Me.lblNICAddress.Name = "lblNICAddress"
        Me.lblNICAddress.Size = New System.Drawing.Size(61, 16)
        Me.lblNICAddress.TabIndex = 4
        Me.lblNICAddress.Text = "Address"
        '
        'lblNICSubnetMask
        '
        Me.lblNICSubnetMask.AutoSize = True
        Me.lblNICSubnetMask.Location = New System.Drawing.Point(12, 139)
        Me.lblNICSubnetMask.Name = "lblNICSubnetMask"
        Me.lblNICSubnetMask.Size = New System.Drawing.Size(91, 16)
        Me.lblNICSubnetMask.TabIndex = 5
        Me.lblNICSubnetMask.Text = "Subnet Mask"
        '
        'lblNICDefaultGateway
        '
        Me.lblNICDefaultGateway.AutoSize = True
        Me.lblNICDefaultGateway.Location = New System.Drawing.Point(12, 168)
        Me.lblNICDefaultGateway.Name = "lblNICDefaultGateway"
        Me.lblNICDefaultGateway.Size = New System.Drawing.Size(117, 16)
        Me.lblNICDefaultGateway.TabIndex = 6
        Me.lblNICDefaultGateway.Text = "Default Gateway"
        '
        'lblNICDNS1
        '
        Me.lblNICDNS1.AutoSize = True
        Me.lblNICDNS1.Location = New System.Drawing.Point(12, 197)
        Me.lblNICDNS1.Name = "lblNICDNS1"
        Me.lblNICDNS1.Size = New System.Drawing.Size(45, 16)
        Me.lblNICDNS1.TabIndex = 7
        Me.lblNICDNS1.Text = "DNS 1"
        '
        'lblNICDNS2
        '
        Me.lblNICDNS2.AutoSize = True
        Me.lblNICDNS2.Location = New System.Drawing.Point(12, 226)
        Me.lblNICDNS2.Name = "lblNICDNS2"
        Me.lblNICDNS2.Size = New System.Drawing.Size(45, 16)
        Me.lblNICDNS2.TabIndex = 8
        Me.lblNICDNS2.Text = "DNS 2"
        '
        'txtNICAddress
        '
        Me.txtNICAddress.Location = New System.Drawing.Point(135, 107)
        Me.txtNICAddress.Name = "txtNICAddress"
        Me.txtNICAddress.Size = New System.Drawing.Size(285, 23)
        Me.txtNICAddress.TabIndex = 9
        '
        'txtNICSubnetMask
        '
        Me.txtNICSubnetMask.Location = New System.Drawing.Point(135, 136)
        Me.txtNICSubnetMask.Name = "txtNICSubnetMask"
        Me.txtNICSubnetMask.Size = New System.Drawing.Size(285, 23)
        Me.txtNICSubnetMask.TabIndex = 10
        '
        'txtNICDefaultGateway
        '
        Me.txtNICDefaultGateway.Location = New System.Drawing.Point(135, 165)
        Me.txtNICDefaultGateway.Name = "txtNICDefaultGateway"
        Me.txtNICDefaultGateway.Size = New System.Drawing.Size(285, 23)
        Me.txtNICDefaultGateway.TabIndex = 11
        '
        'txtNICDNS1
        '
        Me.txtNICDNS1.Location = New System.Drawing.Point(135, 194)
        Me.txtNICDNS1.Name = "txtNICDNS1"
        Me.txtNICDNS1.Size = New System.Drawing.Size(285, 23)
        Me.txtNICDNS1.TabIndex = 12
        '
        'txtNICDNS2
        '
        Me.txtNICDNS2.Location = New System.Drawing.Point(135, 223)
        Me.txtNICDNS2.Name = "txtNICDNS2"
        Me.txtNICDNS2.Size = New System.Drawing.Size(285, 23)
        Me.txtNICDNS2.TabIndex = 13
        '
        'btnNICImport
        '
        Me.btnNICImport.Location = New System.Drawing.Point(220, 252)
        Me.btnNICImport.Name = "btnNICImport"
        Me.btnNICImport.Size = New System.Drawing.Size(200, 30)
        Me.btnNICImport.TabIndex = 14
        Me.btnNICImport.Text = "Import"
        Me.btnNICImport.UseVisualStyleBackColor = True
        '
        'btnNICExport
        '
        Me.btnNICExport.Location = New System.Drawing.Point(12, 252)
        Me.btnNICExport.Name = "btnNICExport"
        Me.btnNICExport.Size = New System.Drawing.Size(200, 30)
        Me.btnNICExport.TabIndex = 15
        Me.btnNICExport.Text = "Export"
        Me.btnNICExport.UseVisualStyleBackColor = True
        '
        'txtNICMAC
        '
        Me.txtNICMAC.Location = New System.Drawing.Point(135, 78)
        Me.txtNICMAC.Name = "txtNICMAC"
        Me.txtNICMAC.ReadOnly = True
        Me.txtNICMAC.Size = New System.Drawing.Size(285, 23)
        Me.txtNICMAC.TabIndex = 17
        '
        'lblNICMAC
        '
        Me.lblNICMAC.AutoSize = True
        Me.lblNICMAC.Location = New System.Drawing.Point(12, 81)
        Me.lblNICMAC.Name = "lblNICMAC"
        Me.lblNICMAC.Size = New System.Drawing.Size(97, 16)
        Me.lblNICMAC.TabIndex = 16
        Me.lblNICMAC.Text = "MAC Address"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(432, 294)
        Me.Controls.Add(Me.txtNICMAC)
        Me.Controls.Add(Me.lblNICMAC)
        Me.Controls.Add(Me.btnNICExport)
        Me.Controls.Add(Me.btnNICImport)
        Me.Controls.Add(Me.txtNICDNS2)
        Me.Controls.Add(Me.txtNICDNS1)
        Me.Controls.Add(Me.txtNICDefaultGateway)
        Me.Controls.Add(Me.txtNICSubnetMask)
        Me.Controls.Add(Me.txtNICAddress)
        Me.Controls.Add(Me.lblNICDNS2)
        Me.Controls.Add(Me.lblNICDNS1)
        Me.Controls.Add(Me.lblNICDefaultGateway)
        Me.Controls.Add(Me.lblNICSubnetMask)
        Me.Controls.Add(Me.lblNICAddress)
        Me.Controls.Add(Me.btnNICSave)
        Me.Controls.Add(Me.btnNICLoad)
        Me.Controls.Add(Me.btnNICRefresh)
        Me.Controls.Add(Me.cbNIC)
        Me.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "IP Changer"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cbNIC As ComboBox
    Friend WithEvents btnNICRefresh As Button
    Friend WithEvents btnNICLoad As Button
    Friend WithEvents btnNICSave As Button
    Friend WithEvents lblNICAddress As Label
    Friend WithEvents lblNICSubnetMask As Label
    Friend WithEvents lblNICDefaultGateway As Label
    Friend WithEvents lblNICDNS1 As Label
    Friend WithEvents lblNICDNS2 As Label
    Friend WithEvents txtNICAddress As TextBox
    Friend WithEvents txtNICSubnetMask As TextBox
    Friend WithEvents txtNICDefaultGateway As TextBox
    Friend WithEvents txtNICDNS1 As TextBox
    Friend WithEvents txtNICDNS2 As TextBox
    Friend WithEvents btnNICImport As Button
    Friend WithEvents btnNICExport As Button
    Friend WithEvents txtNICMAC As TextBox
    Friend WithEvents lblNICMAC As Label
End Class
