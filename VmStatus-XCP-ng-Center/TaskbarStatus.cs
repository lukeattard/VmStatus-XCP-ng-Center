using Renci.SshNet;
using System;
using System.Windows.Forms;
using VmStatus_XCP_ng_Center;
using VmStatus_XCP_ng_Center.Properties;

public class TaskbarStatus : ApplicationContext
{
  private NotifyIcon TrayIcon;
  MonitorVMDetails SingleVM = new MonitorVMDetails();
  internal string StatusMessage = "";

  public TaskbarStatus()
  {
    // Initialize Tray Icon
    TrayIcon = new NotifyIcon()
    {
      Icon = Resources.NotifyIcon,
      ContextMenu = new ContextMenu(new MenuItem[] {
       new MenuItem("Start", StartVM_Click),
       new MenuItem("Shutdown VM", ShutdownVM_Click),
       new MenuItem("Settings", SettingsVM_Click),
       new MenuItem("Exit", OnExit_Click)
      }),
      Visible = true

    };
  }
  void StartVM_Click(object sender, EventArgs e)
  {
    StatusMessage = SingleVM.StartVM(SingleVM);
  }
  void ShutdownVM_Click(object sender, EventArgs e)
  {
    StatusMessage = SingleVM.ShutdownVM(SingleVM);
  }
  void SettingsVM_Click(object sender, EventArgs e)
  {
    //TODO - add a form to display the current VM and have the ability to change the settings.  
    //TODO-LT - Change to list of VM's that can be added to - store in a XML file with hashing of pass, or maybe add a SQLlight DB, LDAP, or MYSQL
  }
  void OnExit_Click(object sender, EventArgs e)
  {
    // Hide tray icon, otherwise it will remain shown until user mouses over it
    TrayIcon.Visible = false;

    Application.Exit();
  }
}