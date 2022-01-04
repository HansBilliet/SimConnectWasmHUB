using Microsoft.FlightSimulator.SimConnect;
using System;

namespace SimConnectWasmHUB
{
    public class VarData : IEquatable<VarData>
    {
        public const UInt16 AUTO_ID = 0xFFFF;
        public const UInt16 NOTUSED_ID = 0xFFFE;

        static private UInt16 uNewDefineID = 0;
        static private UInt16 uNewRequestID = 10;

        private char _cType;
        private string _sName;
        private string _sUnit;
        private SIMCONNECT_DATATYPE _scDataType;
        private UInt16 _uDefineID;
        private UInt16 _uRequestID;
        private UInt16 _uOffset;
        private object _oValue;

        public char cType { get { return _cType; } set { _cType = value; } }
        public String sName { get { return _sName; } set { _sName = value.Trim().ToUpper(); } }
        public String sUnit { get { return _sUnit; } set { _sUnit = value.Trim(); } }
        public SIMCONNECT_DATATYPE scDataType { get { return _scDataType; } set { _scDataType = value; } }
        public UInt16 uDefineID { get { return _uDefineID; } private set { _uDefineID = value; } }
        public UInt16 uRequestID { get { return _uRequestID; } private set { _uRequestID = value; } }
        public UInt16 uOffset { get { return _uOffset; } set { _uOffset = value; } }
        public object oValue { get { return _oValue; } set { _oValue = value; } }

        public void SetID(UInt16 DefineID, UInt16 RequestID)
        {
            _uDefineID = (DefineID == AUTO_ID) ? uNewDefineID++ : DefineID;
            _uRequestID = (RequestID == AUTO_ID) ? uNewRequestID++ : RequestID;
        }

        private string sKey
        {
            get { return cType + sName + sUnit + scDataType; }
        }

        public override string ToString()
        {
            return $"Type: {cType} Name: {sName} Unit: {sUnit} VarType: {scDataType}";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VarData);
        }

        public bool Equals(VarData other)
        {
            if (other == null) return false;
            return (this.sKey.Equals(other.sKey));
        }

        public static bool operator ==(VarData obj1, VarData obj2)
        {
            if (ReferenceEquals(obj1, obj2)) { return true; }
            if (obj1 is null) { return false; }
            if (obj2 is null) { return false; }

            return obj1.Equals(obj2);
        }

        public static bool operator !=(VarData obj1, VarData obj2)
        {
            return !(obj1 == obj2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = _cType.GetHashCode();
                hashCode = (hashCode * 397) ^ _sName.GetHashCode();
                hashCode = (hashCode * 397) ^ _sUnit.GetHashCode();
                return hashCode;
            }
        }

        public ParseResult ParseVarString(string sVar)
        {
            // Variable type - currently supported "A:", "L:" and "K:"
            if (sVar.Length < 2)
                return ParseResult.InvalidVarType;

            // split rest of sVar in the components Name, Unit and VarType
            string[] sVarParts = sVar.Substring(2, sVar.Length - 2).Split(',');

            switch (sVar.Substring(0, 2).ToUpper())
            {
                case "A:": // Example: "A:AUTOPILOT ALTITUDE LOCK VAR:3,feet,FLOAT64"
                    cType = 'A';
                    // Name
                    if (sVarParts.GetUpperBound(0) < 0)
                        return ParseResult.MissingName;
                    sName = sVarParts[0];

                    // Unit
                    if (sVarParts.GetUpperBound(0) < 1)
                        return ParseResult.MissingUnit;
                    sUnit = sVarParts[1];

                    // DataType
                    if (sVarParts.GetUpperBound(0) < 2)
                        return ParseResult.MissingDataType;
                    switch (sVarParts[2].ToUpper())
                    {
                        case "INT32":
                            scDataType = SIMCONNECT_DATATYPE.INT32;
                            break;
                        case "INT64":
                            scDataType = SIMCONNECT_DATATYPE.INT64;
                            break;
                        case "FLOAT32":
                            scDataType = SIMCONNECT_DATATYPE.FLOAT32;
                            break;
                        case "FLOAT64":
                            scDataType = SIMCONNECT_DATATYPE.FLOAT64;
                            break;
                        case "STRING256":
                            scDataType = SIMCONNECT_DATATYPE.STRING256;
                            break;
                        default:
                            return ParseResult.UnsupportedDataType;
                    }
                    break;
                
                case "L:": // Example: "L:A32NX_EFIS_L_OPTION,enum"
                    cType = 'L';
                    // Name
                    if (sVarParts.GetUpperBound(0) < 0)
                        return ParseResult.MissingName;
                    sName = sVarParts[0];

                    // Unit
                    if (sVarParts.GetUpperBound(0) < 1)
                        return ParseResult.MissingUnit;
                    sUnit = sVarParts[1];

                    // DataType
                    scDataType = SIMCONNECT_DATATYPE.FLOAT64;
                    break;
                
                case "K:": // Example: "K:A32NX.FCU_HDG_INC"
                    cType = 'K';
                    // Name
                    if (sVarParts.GetUpperBound(0) < 0)
                        return ParseResult.MissingName;
                    sName = sVarParts[0];

                    // Unit
                    sUnit = "N/A";

                    // DataType
                    scDataType = SIMCONNECT_DATATYPE.INT32;
                    break;
                
                default:
                    return ParseResult.InvalidVarType;
            }

            return ParseResult.Ok;
        }
    }

    public enum ParseResult
    {
        Ok,
        InvalidVarType,
        MissingName,
        MissingUnit,
        MissingDataType,
        UnsupportedDataType
    }

}
