using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VmStatus_XCP_ng_Center.Properties;

namespace VmStatus_XCP_ng_Center
{
  public class MonitorVMDetails
  {
    internal string Status { get; set; }
    internal string Hostname { get; set; }
    internal string Username { get; set; }
    internal string Password { get; set; }
    internal int Port { get; set; }
    internal string VmUUID { get; set; }
    internal string ErrorMessage { get; set;  }
    private SshClient SshVmClient;

    //TODO depending on how many status we end up with, maybe make into a static list.
    //TODO add language file and replace entries making program multi language
    private static string connected = "connected";
    private static string disconnected = "disconnected";
    private static string error = "error";
    private static string nr = "nr";
    private static string completed = "completed";
    private static string unknown = "unknown";
    
    public MonitorVMDetails()
    {
      //TODO - this is the long way and not necessary right now however will be required for list to be added later, new class of MonitorVMList will create this class for each VM, at that time the construct will be cahnged to take arguments. 

      Hostname = Resources.hostname;
      Username = Resources.username;
      Password = Resources.password;
      Port = Int16.Parse(Resources.port);
      VmUUID = Resources.vmUUID;
      Status = unknown;
      ConnectVM();
    }

    internal string ConnectVM()
    {
      if(Status.Equals(connected))
      {
        return (nr);
      }

      try
      {
        SshVmClient = new SshClient(Hostname, Port, Username, Password);
        SshVmClient.Connect();
      }
      catch (Exception e)
      {
        Status = error;
        ErrorMessage = e.Message;
      }

      Status = connected;
      return (connected);
    }

    internal string DisconnectVM()
    {
      if ( SshVmClient == null || Status.Equals(disconnected))
      {
        return (nr);
      }

      try
      {
        SshVmClient.Disconnect();
      }
      catch (Exception ex)
      {
        Status = error;
        ErrorMessage = ex.Message;
        return (Status);
      }

      Status = disconnected;
      return (disconnected);
    }
    internal string UpdateStatus()
    {
      //TODO add command of whoami - confirm response = connect no response = disconnected.
      return (Status);
    }

    internal string StartVM(MonitorVMDetails currentVM)
    {
      if((currentVM.Status.Equals(unknown) || !currentVM.Status.Equals(connected)) && currentVM.ConnectVM().Equals(error))
      {
        currentVM.Status = error;
        return (error);
      }
      try
      {
        currentVM.SshVmClient.RunCommand($"sudo xe vm-start vm={ currentVM.VmUUID } ");
      }
      catch (Exception ex)
      {
        ErrorMessage = ex.Message;
        currentVM.Status = error;
        return (error);
      }
      return (completed);
    }
    internal string ShutdownVM(MonitorVMDetails currentVM)
    {
      if(currentVM.Status.Equals(error) && !currentVM.ConnectVM().Equals(connected))
      {
        return (error);
      }
      else if(!currentVM.Status.Equals(connected) && !currentVM.ConnectVM().Equals(connected))
      {
        return (currentVM.Status);
      }
      try
      {
        currentVM.SshVmClient.RunCommand($"sudo xe vm-shutdown vm={ Resources.vmUUID } ");
      }
      catch (Exception ex)
      {
        currentVM.ErrorMessage = ex.Message;
        return(currentVM.ErrorMessage);
      }
      return(completed);
    }

  }
}
