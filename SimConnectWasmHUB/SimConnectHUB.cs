using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.FlightSimulator.SimConnect;

namespace SimConnectWasmHUB
{
    public class SimConnectHUB
    {
        // User-defined win32 event
        public const int WM_USER_SIMCONNECT = 0x0402;
        // Window handle
        private IntPtr _handle = new IntPtr(0);

        // SimConnect object
        private SimConnect _oSimConnect = null;
        // Connection Status
        private bool _bConnected = false;

        // Event handler to show results in textbox
        public event EventHandler<string> LogResult = null;

        // List of registered variables
        private List<VarData> Vars = new List<VarData>();

        // Client Area Data names
        private const string CLIENT_DATA_NAME_LVARS = "HABI_WASM.LVars";
        private const string CLIENT_DATA_NAME_COMMAND = "HABI_WASM.Command";
        private const string CLIENT_DATA_NAME_ACKNOWLEDGE = "HABI_WASM.Acknowledge";

        // Client Area Data ID's
        private enum CLIENT_DATA_ID
        {
            LVARS = 0,
            CMD = 1,
            ACK = 2
        }

        // Currently we are using fixed size strings of 256 characters
        private const int MESSAGE_SIZE = 256;

        // Structure sent back from WASM module to acknowledge for LVars
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct LVarAck
        {
            public UInt16 DefineID;
            public UInt16 Offset;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String str;
            public float value;
        };

        // Currently we only work with strings with a fixed size of 256
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct String256
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Value;
        }

        // Some "dummy" enums for type conversions
        private enum SIMCONNECT_DEFINITION_ID { }
        private enum SIMCONNECT_REQUEST_ID { }
        private enum EVENT_ID { }

        // Client Data Area DefineID's for Command and Acknowledge
        private enum CLIENTDATA_DEFINITION_ID
        {
            CMD,
            ACK
        }

        // Client Data Area RequestID's for receiving Acknowledge and LVARs
        private enum CLIENTDATA_REQUEST_ID
        {
            ACK,
            START_LVAR
        }

        // Keep Window Handle for connecting with SimConnect
        public void SetHandle(IntPtr handle)
        {
            _handle = handle;
        }

        // WndProc hook
        public void HandleWndProc(ref Message m)
        {
            if (m.Msg == WM_USER_SIMCONNECT)
            {
                ReceiveSimConnectMessage();
            }
        }

        // Connection status
        public bool IsConnected()
        {
            return _bConnected;
        }

        // MAIN SimConnect methods

        public void Connect()
        {
            if (_bConnected)
            {
                LogResult?.Invoke(this, "Already connected");
                return;
            }

            try
            {
                _oSimConnect = new SimConnect("SimConnectHub", _handle, WM_USER_SIMCONNECT, null, 0);

                // Listen for connect and quit msgs
                _oSimConnect.OnRecvOpen += SimConnect_OnRecvOpen;
                _oSimConnect.OnRecvQuit += SimConnect_OnRecvQuit;

                // Listen for Exceptions
                _oSimConnect.OnRecvException += SimConnect_OnRecvException;

                // Listen for SimVar Data
                _oSimConnect.OnRecvSimobjectData += SimConnect_OnRecvSimobjectData;

                // Listen for ClientData
                _oSimConnect.OnRecvClientData += SimConnect_OnRecvClientData;

                LogResult?.Invoke(this, "Connected");
            }
            catch (COMException ex)
            {
                LogResult?.Invoke(this, $"Connect Error: {ex.Message}");
            }
        }

        public void Disconnect()
        {
            if (!_bConnected)
            {
                LogResult?.Invoke(this, "Already disconnected");
                return;
            }

            // Raise event to notify client we've disconnected and get rid of client
            _oSimConnect?.Dispose(); // May have already been disposed or not even been created, e.g. Disconnect called before Connect
            _oSimConnect = null;
            _bConnected = false;
            LogResult?.Invoke(this, "Disconnected");
        }

        public void ReceiveSimConnectMessage()
        {
            try
            {
                _oSimConnect?.ReceiveMessage();
            }
            catch (Exception ex)
            {
                LogResult?.Invoke(this, $"ReceiveSimConnectMessage Error: {ex.Message}");
                Disconnect();
            }
        }

        private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            Debug.WriteLine("SimConnect_OnRecvOpen"); 
            
            _bConnected = true;

            LogResult?.Invoke(this, $"- Application name: {data.szApplicationName}");
            LogResult?.Invoke(this, $"- Application Version {data.dwApplicationVersionMajor}.{data.dwApplicationVersionMinor} - build {data.dwApplicationBuildMajor}.{data.dwApplicationBuildMinor}");
            LogResult?.Invoke(this, $"- SimConnect  Version {data.dwSimConnectVersionMajor}.{data.dwSimConnectVersionMinor} - build {data.dwSimConnectBuildMajor}.{data.dwSimConnectBuildMinor}");

            InitializeClientDataAreas();
        }

        private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            Debug.WriteLine("SimConnect_OnRecvQuit");
            Disconnect();

            _bConnected = false;
        }

        private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            SIMCONNECT_EXCEPTION eException = (SIMCONNECT_EXCEPTION)data.dwException;
            LogResult?.Invoke(this, $"SimConnectDLL.SimConnect_OnRecvException - {eException}");
        }

        private void SimConnect_OnRecvSimobjectData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
        {
            if (_oSimConnect != null)
            {
                VarData vInList = Vars.Find(x => x.cType == 'A' && x.uRequestID == data.dwRequestID);
                if (vInList != null)
                {
                    vInList.oValue = data.dwData[0];
                    LogResult?.Invoke(this, $"{vInList} value = {vInList.oValue}");
                }
            }
        }

        private void SimConnect_OnRecvClientData(SimConnect sender, SIMCONNECT_RECV_CLIENT_DATA data)
        {
            Debug.WriteLine($"SimConnect_OnRecvClientData() - RequestID = {data.dwRequestID}");

            if (data.dwRequestID == (uint)CLIENTDATA_REQUEST_ID.ACK)
            {
                try
                {
                    var ackData = (LVarAck)(data.dwData[0]);
                    Debug.WriteLine($"----> Acknowledge: ID: {ackData.DefineID}, Offset: {ackData.Offset}, String: {ackData.str}, value: {ackData.value}");

                    // if LVar DefineID already exists, ignore it, otherwise we will get "DUPLICATE_ID" exception
                    if (Vars.Exists(x => x.cType == 'L' && x.uDefineID == ackData.DefineID))
                        return;

                    // find the LVar, and update it with ackData
                    VarData vInList = Vars.Find(x => x.cType == 'L' && $"(L:{x.sName})" == ackData.str);
                    if (vInList == null)
                        return;
                    vInList.SetID(ackData.DefineID, VarData.AUTO_ID); // use ackData.DefineID and next available RequestID
                    vInList.uOffset = ackData.Offset;
                    vInList.oValue = ackData.value;

                    _oSimConnect?.AddToClientDataDefinition(
                        (SIMCONNECT_DEFINITION_ID)ackData.DefineID,
                        (uint)ackData.Offset,
                        sizeof(float),
                        0,
                        0);
                    Debug.WriteLine($"----> AddToClientDataDefinition ID:{ackData.DefineID} Offset:{ackData.Offset} Size:{sizeof(float)}");

                    _oSimConnect?.RegisterStruct<SIMCONNECT_RECV_CLIENT_DATA, float>((SIMCONNECT_DEFINITION_ID)vInList.uDefineID);

                    _oSimConnect?.RequestClientData(
                        CLIENT_DATA_ID.LVARS,
                        (SIMCONNECT_REQUEST_ID)vInList.uRequestID,
                        (SIMCONNECT_DEFINITION_ID)vInList.uDefineID,
                        SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET, // data will be sent whenever SetClientData is used on this client area (even if this defineID doesn't change)
                        SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.CHANGED, // if this is used, this defineID only is sent when its value has changed
                        0, 0, 0);

                    LogResult?.Invoke(this, $"{vInList} value = {vInList.oValue}");
                }
                catch (Exception ex)
                {
                    LogResult?.Invoke(this, $"SimConnect_OnRecvClientData Error: {ex.Message}");
                }
            }
            else if (data.dwRequestID >= (uint)CLIENTDATA_REQUEST_ID.START_LVAR)
            {
                VarData vInList = Vars.Find(x => x.cType == 'L' && x.uDefineID == data.dwDefineID);

                if (vInList != null)
                {
                    vInList.oValue = data.dwData[0];
                    LogResult?.Invoke(this, $"{vInList} value = {vInList.oValue}");
                }
                else
                    LogResult?.Invoke(this, $"LVar data received: unknown LVar DefineID {data.dwDefineID}");
            }
        }

        private void InitializeClientDataAreas()
        {
            Debug.WriteLine("InitializeClientDataAreas()");

            try
            {
                // register Client Data (for LVars)
                _oSimConnect.MapClientDataNameToID(CLIENT_DATA_NAME_LVARS, CLIENT_DATA_ID.LVARS);
                _oSimConnect.CreateClientData(CLIENT_DATA_ID.LVARS, SimConnect.SIMCONNECT_CLIENTDATA_MAX_SIZE, SIMCONNECT_CREATE_CLIENT_DATA_FLAG.DEFAULT);

                // register Client Data (for WASM Module Commands)
                _oSimConnect.MapClientDataNameToID(CLIENT_DATA_NAME_COMMAND, CLIENT_DATA_ID.CMD);
                _oSimConnect.CreateClientData(CLIENT_DATA_ID.CMD, MESSAGE_SIZE, SIMCONNECT_CREATE_CLIENT_DATA_FLAG.DEFAULT);
                _oSimConnect.AddToClientDataDefinition(CLIENTDATA_DEFINITION_ID.CMD, 0, MESSAGE_SIZE, 0, 0);

                // register Client Data (for LVar acknowledge)
                _oSimConnect.MapClientDataNameToID(CLIENT_DATA_NAME_ACKNOWLEDGE, CLIENT_DATA_ID.ACK);
                _oSimConnect.CreateClientData(CLIENT_DATA_ID.ACK, (uint)Marshal.SizeOf<LVarAck>(), SIMCONNECT_CREATE_CLIENT_DATA_FLAG.DEFAULT);
                _oSimConnect.AddToClientDataDefinition(CLIENTDATA_DEFINITION_ID.ACK, 0, (uint)Marshal.SizeOf<LVarAck>(), 0, 0);
                _oSimConnect.RegisterStruct<SIMCONNECT_RECV_CLIENT_DATA, LVarAck>(CLIENTDATA_DEFINITION_ID.ACK);
                _oSimConnect.RequestClientData(
                    CLIENT_DATA_ID.ACK,
                    CLIENTDATA_REQUEST_ID.ACK,
                    CLIENTDATA_DEFINITION_ID.ACK,
                    SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET,
                    SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.DEFAULT,
                    0, 0, 0);
            }
            catch (Exception ex)
            {
                LogResult?.Invoke(this, $"InitializeClientDataAreas Error: {ex.Message}");
            }

        }

        public void SendWASMCmd(String command)
        {
            Debug.WriteLine($"SendWASMCmd({command})");

            if (!_bConnected)
            {
                Debug.WriteLine("Not connected");
                return;
            }

            String256 cmd;
            cmd.Value = command;

            try
            {
                _oSimConnect.SetClientData(
                    CLIENT_DATA_ID.CMD,
                    CLIENTDATA_DEFINITION_ID.CMD,
                    SIMCONNECT_CLIENT_DATA_SET_FLAG.DEFAULT,
                    0,
                    cmd
                );
            }
            catch (Exception ex)
            {
                LogResult?.Invoke(this, $"SendWASMCmd Error: {ex.Message}");
            }
        }

        // Button actions

        public bool AddVariable(string sVar)
        {
            if (!_bConnected)
            {
                LogResult?.Invoke(this, "Not connected"); 
                return false;
            }

            VarData v = new VarData();
            ParseResult res = v.ParseVarString(sVar);
            if (res != ParseResult.Ok)
            {
                LogResult?.Invoke(this, res.ToString());
                return false;
            }

            if (Vars.Exists(x => x == v))
            {
                LogResult?.Invoke(this, "Variable already exists");
                return false;
            }

            Vars.Add(v);

            // Register the variable
            switch (v.cType)
            {
                case 'A':
                    // Register a SimVar
                    try
                    {
                        // Registration of a SimVar
                        v.SetID(VarData.AUTO_ID, VarData.AUTO_ID);
                        _oSimConnect.AddToDataDefinition(
                            (SIMCONNECT_DEFINITION_ID)v.uDefineID,
                            v.sName,
                            v.sUnit,
                            v.scDataType,
                            0.0f,
                            SimConnect.SIMCONNECT_UNUSED);

                        switch (v.scDataType)
                        {
                            case SIMCONNECT_DATATYPE.INT32:
                                _oSimConnect.RegisterDataDefineStruct<Int32>((SIMCONNECT_DEFINITION_ID)v.uDefineID);
                                break;
                            case SIMCONNECT_DATATYPE.INT64:
                                _oSimConnect.RegisterDataDefineStruct<Int64>((SIMCONNECT_DEFINITION_ID)v.uDefineID);
                                break;
                            case SIMCONNECT_DATATYPE.FLOAT32:
                                _oSimConnect.RegisterDataDefineStruct<float>((SIMCONNECT_DEFINITION_ID)v.uDefineID);
                                break;
                            case SIMCONNECT_DATATYPE.FLOAT64:
                                _oSimConnect.RegisterDataDefineStruct<Double>((SIMCONNECT_DEFINITION_ID)v.uDefineID);
                                break;
                            case SIMCONNECT_DATATYPE.STRING256:
                                _oSimConnect.RegisterDataDefineStruct<String256>((SIMCONNECT_DEFINITION_ID)v.uDefineID);
                                break;
                        }

                        _oSimConnect.RequestDataOnSimObject(
                            (SIMCONNECT_REQUEST_ID)v.uRequestID,
                            (SIMCONNECT_DEFINITION_ID)v.uDefineID,
                            0,
                            SIMCONNECT_PERIOD.SIM_FRAME,
                            SIMCONNECT_DATA_REQUEST_FLAG.CHANGED,
                            0, 0, 0);

                        LogResult?.Invoke(this, $"{v} SimVar added and registered");
                    }
                    catch (Exception ex)
                    {
                        LogResult?.Invoke(this, $"Register SimVar Error: {ex.Message}");
                    }
                    break;
                case 'L':
                    // Register an LVar
                    SendWASMCmd($"HW.Reg.(L:{v.sName})");
                    LogResult?.Invoke(this, $"{v} LVar added and registered");
                    break;
                case 'K':
                    // Register a K-event
                    try
                    {
                        if (v.sName.IndexOf('.') == -1)
                        {
                            v.SetID(VarData.AUTO_ID, VarData.NOTUSED_ID);
                            // Registration of a SimConnect Event
                            _oSimConnect.MapClientEventToSimEvent((EVENT_ID)v.uDefineID, v.sName);
                            //_oSimConnect.TransmitClientEvent(0, EVENT_ID.MY_EVENT, 4, (EVENT_ID)SimConnect.SIMCONNECT_GROUP_PRIORITY_HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            LogResult?.Invoke(this, $"{v} SimConnect Event added and registered");
                        }
                        else
                        {
                            // Registration of a Custom Event (no registration is needed as we are using execute_)
                            LogResult?.Invoke(this, $"{v} Custom Event added");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogResult?.Invoke(this, $"Register K-event Error: {ex.Message}");
                    }
                    break;
                default:
                    break;
            }

            return true;
        }

        public bool RemoveVariable(string sVar)
        {
            if (!_bConnected)
            {
                LogResult?.Invoke(this, "Not connected");
                return false;
            }

            VarData v = new VarData();
            ParseResult res = v.ParseVarString(sVar);
            if (res != ParseResult.Ok)
            {
                LogResult?.Invoke(this, res.ToString());
                return false;
            }

            VarData vInList = Vars.Find(x => x == v);
            if (vInList == null)
            {
                LogResult?.Invoke(this, "Variable not registered");
                return false;
            }

            if (!Vars.Remove(vInList))
            {
                LogResult?.Invoke(this, "Remove failed");
                return false;
            }

            // Unregister the variable
            switch (vInList.cType)
            {
                case 'A':
                    // Unregister a SimVar
                    try
                    {
                        _oSimConnect.RequestDataOnSimObject(
                            (SIMCONNECT_REQUEST_ID)vInList.uRequestID,
                            (SIMCONNECT_DEFINITION_ID)vInList.uDefineID,
                            0,
                            SIMCONNECT_PERIOD.NEVER,
                            SIMCONNECT_DATA_REQUEST_FLAG.DEFAULT,
                            0, 0, 0);
                        _oSimConnect.ClearDataDefinition((SIMCONNECT_DEFINITION_ID)vInList.uDefineID);

                        LogResult?.Invoke(this, $"{vInList} SimVar removed and Unregistered");
                    }
                    catch (Exception ex)
                    {
                        LogResult?.Invoke(this, $"Unregister SimVar Error: {ex.Message}");
                    }
                    break;

                case 'L':
                    // Unregister an LVar
                    try
                    {
                        _oSimConnect?.RequestClientData(
                            CLIENT_DATA_ID.LVARS,
                            (SIMCONNECT_REQUEST_ID)vInList.uRequestID,
                            (SIMCONNECT_DEFINITION_ID)vInList.uDefineID,
                            SIMCONNECT_CLIENT_DATA_PERIOD.NEVER,
                            SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.DEFAULT,
                            0, 0, 0);
                        _oSimConnect.ClearClientDataDefinition((SIMCONNECT_DEFINITION_ID)vInList.uDefineID);

                        LogResult?.Invoke(this, $"{vInList} LVar removed and Unregistered");
                    }
                    catch (Exception ex)
                    {
                        LogResult?.Invoke(this, $"Unregister LVar Error: {ex.Message}");
                    }
                    break;

                case 'K':
                    // Unregister a KVar --> Not really neccessary
                    LogResult?.Invoke(this, $"{vInList} Event removed");
                    break;
            }

            return true;
        }

        public static dynamic Cast(dynamic obj, Type castTo)
        {
            return Convert.ChangeType(obj, castTo);
        }

        public bool SetVariable(string sVar, string sValue)
        {
            if (!_bConnected)
            {
                LogResult?.Invoke(this, "Not connected");
                return false;
            }

            if (!double.TryParse(sValue, out double dValue))
            {
                if (sValue == "")
                    dValue = 0;
                else
                {
                    LogResult?.Invoke(this, $"\"{sValue}\" is not a number");
                    return false;
                }
            }

            VarData v = new VarData();
            ParseResult res = v.ParseVarString(sVar);
            if (res != ParseResult.Ok)
            {
                LogResult?.Invoke(this, res.ToString());
                return false;
            }

            VarData vInList = Vars.Find(x => x == v);
            if (vInList == null)
            {
                LogResult?.Invoke(this, "Variable not registered");
                return false;
            }

            switch (vInList.cType)
            {
                case 'A':
                    try
                    {
                        vInList.oValue = Cast(dValue, vInList.oValue.GetType());

                        _oSimConnect.SetDataOnSimObject((SIMCONNECT_DEFINITION_ID)vInList.uDefineID, 0, SIMCONNECT_DATA_SET_FLAG.DEFAULT, vInList.oValue);

                        LogResult?.Invoke(this, $"{vInList} SimVar value set to {sValue}");
                    }
                    catch (Exception ex)
                    {
                        LogResult?.Invoke(this, $"SetValue SimVar Error: {ex.Message}");
                    }
                    break;

                case 'L':
                    SendWASMCmd($"HW.Set.{sValue} (>L:{vInList.sName})");
                    LogResult?.Invoke(this, $"{vInList} SimVar value set to {sValue}");
                    break;

                case 'K':
                    if (v.sName.IndexOf('.') == -1)
                        try
                        {
                            _oSimConnect.TransmitClientEvent(
                                0, 
                                (EVENT_ID)v.uDefineID, 
                                (uint)dValue, 
                                (EVENT_ID)SimConnect.SIMCONNECT_GROUP_PRIORITY_HIGHEST, 
                                SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            LogResult?.Invoke(this, $"{vInList} SimConnect Event triggered with {(sValue == "" ? "no" : "")} value {sValue}");
                        }
                        catch (Exception ex)
                        {
                            LogResult?.Invoke(this, $"SetValue SimConnect Event Error: {ex.Message}");
                        }
                    else
                    {
                        if (sValue == "")
                            SendWASMCmd($"HW.Set.(>K:{v.sName})");
                        else
                            SendWASMCmd($"HW.Set.{sValue} (>K:{v.sName})");
                        LogResult?.Invoke(this, $"{vInList} Custom Event triggered with {(sValue == "" ? "no" : "")} value {sValue}");
                    }
                    break;
            }

            return true;
        }
    }
}
