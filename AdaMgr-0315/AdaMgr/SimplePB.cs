using System;
using System.Runtime.InteropServices;
using System.Text;

// This class "wraps" the SimplePB.dll. It encapsulates the underlying dll function 
// calls with overloaded versions of those functions for compatibility with the .Net Framework.
// 
// The SimplePB class initiates calls to the unmanaged SimplePB.dll functions as Platform Invokes and
// utilizes the Sysetm.Runtime.InteropServices.DllImport attribute for this purpose.
// 
// Many of the unmanaged functions take a pointer to an array of characters, either a char* or a char**, as an argument.
// To accommodate this, the wrapper class will convert a .Net String value to a Byte array on the unmanaged heap and
// pass a System.IntPtr to the array as an argument. In the case of a char* parameter, the IntPtr is passed by value; for a 
// char**, the IntPtr is passed by reference (ref). 


namespace AdaMgr
{

    public class SimplePB
    {

        //Import the SimplePB.Dll and declare the ClosePort function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "ClosePort", CallingConvention = CallingConvention.StdCall)]
        public static extern int ClosePort();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the CloseIPPort function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "CloseIPPort", CallingConvention = CallingConvention.StdCall)]
        public static extern int CloseIPPort();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the SetSecurity function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "SetSecurity", CallingConvention = CallingConvention.StdCall)]
        public static extern int SetSecurity(int security_code);

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the FileControl function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "FileControl", CallingConvention = CallingConvention.StdCall)]
        private static extern FileControlReturn FileControl(int pakbus_address, DeviceTypeCodes device_type, System.IntPtr file_name, FileControlCommandType command);

        //Wrapper for the FileControl function
        public static FileControlReturn FileControl(int pakBusAddress, DeviceTypeCodes deviceType, string fileName, FileControlCommandType command)
        {
            //Get pointer to the 'char' parameter created on the unmanaged heap
            IntPtr pfileName = getBytesForString(fileName);

            //Call the SimplePB.Dll function
            FileControlReturn rtn = FileControl(pakBusAddress, deviceType, pfileName, command);

            //Free the memory allocated on the unmanaged heap
            Marshal.FreeHGlobal(pfileName);

            //Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the FileSend function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "File_Send", CallingConvention = CallingConvention.StdCall)]
        private static extern FileSendReturn File_Send(int pakbus_address, DeviceTypeCodes device_type, System.IntPtr file_name, out System.IntPtr return_data, out int return_len);

        //Wrapper for the FileSend function
        public static FileSendReturn FileSend(int pakBusAddress, DeviceTypeCodes deviceType, string fileName, ref string returnData)
        {
            //Declare variables for 'returned' parameters
            IntPtr returnPtr;
            int returnPtrLength;

            //Get pointer to the 'char' parameter created on the unmanaged heap
            IntPtr pfileName = getBytesForString(fileName);

            //Call the SimplePB.Dll function
            FileSendReturn rtn = File_Send(pakBusAddress, deviceType, pfileName, out returnPtr, out returnPtrLength);

            //Use the 'returned' parameters to retrieve the string data
            returnData = getStringFromIntPtr(returnPtr, returnPtrLength);

            //Free the memory allocated on the unmanaged heap
            Marshal.FreeHGlobal(pfileName);

            //Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the GetAddress function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "GetAddress", CallingConvention = CallingConvention.StdCall)]
        private static extern GetAddressReturn GetAddress(DeviceTypeCodes device_type, out System.IntPtr return_data, out int return_len);

        //Wrapper for the GetAddress function
        public static GetAddressReturn GetAddress(DeviceTypeCodes deviceType, ref string returnData)
        {
            //Declare variables for 'returned' parameters
            IntPtr returnPtr;
            int returnPtrLength;

            //Call the SimplePB.Dll function
            GetAddressReturn rtn = GetAddress(deviceType, out returnPtr, out returnPtrLength);

            //Use the 'returned' parameters to retrieve the string data
            returnData = getStringFromIntPtr(returnPtr, returnPtrLength);

            //Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the GetClock function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "GetClock", CallingConvention = CallingConvention.StdCall)]
        private static extern ClockReturn GetClock(int pakbus_address, DeviceTypeCodes device_type, out System.IntPtr return_data, out int return_data_len);

        //Wrapper for the GetClock function
        public static ClockReturn GetClock(int pakBusAddress, DeviceTypeCodes deviceType, ref string returnData)
        {
            //Declare variables for 'returned' parameters
            IntPtr returnPtr;
            int returnPtrLength;

            //Call the SimplePB.Dll function
            ClockReturn rtn = GetClock(pakBusAddress, deviceType, out returnPtr, out returnPtrLength);

            //Use the 'returned' parameters to retrieve the string data
            returnData = getStringFromIntPtr(returnPtr, returnPtrLength);

            //Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the GetCommaData function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "GetCommaData", CallingConvention = CallingConvention.StdCall)]
        private static extern GetDataReturn GetCommaData(int pakbus_address, DeviceTypeCodes device_type, int table_no, int record_no, out System.IntPtr return_data, out int return_len);

        //Wrapper for the GetCommaData function
        public static GetDataReturn GetCommaData(int pakBusAddress, DeviceTypeCodes deviceType, int tableNumber, int recordNumber, ref string returnData)
        {
            //Declare variables for 'returned' parameters
            IntPtr returnPtr;
            int returnPtrLength;

            //Call the SimplePB.Dll function
            GetDataReturn rtn = GetCommaData(pakBusAddress, deviceType, tableNumber, recordNumber, out returnPtr, out returnPtrLength);

            //Use the 'returned' parameters to retrieve the string data
            returnData = getStringFromIntPtr(returnPtr, returnPtrLength);

            //Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the GetData function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "GetData", CallingConvention = CallingConvention.StdCall)]
        public static extern GetDataReturn GetData(int pakbus_address, DeviceTypeCodes device_type, int table_no, int record_no, out System.IntPtr return_data, out int return_data_len);

        //Wrapper for the GetData function
        public static GetDataReturn GetData(int pakBusAddress, DeviceTypeCodes deviceType, int tableNumber, int recordNumber, ref string returnData)
        {
            //Declare variables for 'returned' parameters
            IntPtr returnPtr;
            int returnPtrLength;

            //Call the SimplePB.Dll function
            GetDataReturn rtn = GetData(pakBusAddress, deviceType, tableNumber, recordNumber, out returnPtr, out returnPtrLength);

            //Use the 'returned' parameters to retrieve the string data
            returnData = getStringFromIntPtr(returnPtr, returnPtrLength);

            //Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the GetDataHeader function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "GetDataHeader", CallingConvention = CallingConvention.StdCall)]
        private static extern GetDataReturn GetDataHeader(int pakbus_address, DeviceTypeCodes device_type, int table_no, out System.IntPtr return_data, out int return_len);

        //Wrapper for the GetDataHeader function
        public static GetDataReturn GetDataHeader(int pakBusAddress, DeviceTypeCodes deviceType, int tableNumber, ref string returnData)
        {
            //Declare variables for 'returned' parameters
            IntPtr returnPtr;
            int returnPtrLength;

            //Call the SimplePB.Dll function
            GetDataReturn rtn = GetDataHeader(pakBusAddress, deviceType, tableNumber, out returnPtr, out returnPtrLength);

            //Use the 'returned' parameters to retrieve the string data
            returnData = getStringFromIntPtr(returnPtr, returnPtrLength);

            //Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the GetDLLVersion function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "GetDLLVersion", CallingConvention = CallingConvention.StdCall)]
        private static extern int GetDLLVersion(out System.IntPtr return_data, out int return_data_len);

        //Wrapper for the GetDLLVersion function
        public static string GetDLLVersion()
        {
            //Declare variables for 'returned' parameters
            IntPtr returnPtr;
            int returnPtrLength;

            //Call the SimplePB.Dll function
            GetDLLVersion(out returnPtr, out returnPtrLength);

            //Use the 'returned' parameters to retrieve the string data
            return getStringFromIntPtr(returnPtr, returnPtrLength);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //Import the SimplePB.Dll and declare the GetStatus function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "GetStatus", CallingConvention = CallingConvention.StdCall)]
        private static extern GetStatusReturn GetStatus(int pakbus_address, DeviceTypeCodes device_type, out System.IntPtr return_data, out int return_data_len);

        //Warapper for the GetStatus function
        public static GetStatusReturn GetStatus(int pakBusAddress, DeviceTypeCodes deviceType, ref string returnData)
        {
            //Declare variables for 'returned' parameters
            IntPtr returnPtr;
            int returnPtrLength;

            //Call the SimplePB.Dll function
            GetStatusReturn rtn = GetStatus(pakBusAddress, deviceType, out returnPtr, out returnPtrLength);

            //Use the 'returned' parameters to retrieve the string data
            returnData = getStringFromIntPtr(returnPtr, returnPtrLength);

            //Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the GetTableNames function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "GetTableNames", CallingConvention = CallingConvention.StdCall)]
        private static extern GetTablesReturn GetTableNames(int pakbus_address, DeviceTypeCodes device_type, out System.IntPtr return_data, out int return_data_len);

        //Wrapper for the GetTableNames function
        public static GetTablesReturn GetTableNames(int pakBusAddress, DeviceTypeCodes deviceType, ref string returnData)
        {
            //Declare variables for 'returned' parameters
            IntPtr returnPtr;
            int returnPtrLength;

            //Call the SimplePB.Dll function
            GetTablesReturn rtn = GetTableNames(pakBusAddress, deviceType, out returnPtr, out returnPtrLength);

            //Use the 'returned' parameters to retrieve the string data
            returnData = getStringFromIntPtr(returnPtr, returnPtrLength);

            //Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the GetValue function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "GetValue", CallingConvention = CallingConvention.StdCall)]
        private static extern GetValueReturn GetValue(int pakbus_address, DeviceTypeCodes device_type, int swath, System.IntPtr table_name, System.IntPtr field_name, out System.IntPtr return_data, out int return_data_len);

        //Wrapper for the GetValue function
        public static GetValueReturn GetValue(int pakBusAddress, DeviceTypeCodes deviceType, int swath, string tableName, string fieldName, ref string returnData)
        {
            //Declare variables for 'returned' parameters
            IntPtr returnPtr;
            int returnPtrLength;

            //Get pointers to the 'char' parameters created on the unmanaged heap
            IntPtr ptableName = getBytesForString(tableName);
            IntPtr pfieldName = getBytesForString(fieldName);

            //Call the SimplePB.Dll function
            GetValueReturn rtn = GetValue(pakBusAddress, deviceType, swath, ptableName, pfieldName, out returnPtr, out returnPtrLength);

            //Use the 'returned' parameters to retrieve the string data
            returnData = getStringFromIntPtr(returnPtr, returnPtrLength);

            //Free the memory allocated on the unmanged heap
            Marshal.FreeHGlobal(ptableName);
            Marshal.FreeHGlobal(pfieldName);

            //Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
       //  EntryPoint = "OpenPort",
        //Import the SimplePB.Dll and declare the OpenPort function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "OpenPort", CallingConvention = CallingConvention.StdCall)]
        public static extern int OpenPort(int comPortNumber, int baudRate);

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the OpenIPPort function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "OpenIPPort", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        private static extern int OpenIPPort(System.IntPtr ip_address, int tcp_port);

        //Wrapper for the OpenIPPort function
        public static int OpenIPPort(string iPAddress, int tcpPort)
        {
            //Get pointer to the 'char' parameter created on the unmanaged heap
            IntPtr piPAddress = getBytesForString(iPAddress);

            //Call the SimplePB.Dll function
            int rtn = OpenIPPort(piPAddress, tcpPort);

            //Free the memory allocated on the unmanaged heap
            Marshal.FreeHGlobal(piPAddress);

            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the SetClock function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "SetClock", CallingConvention = CallingConvention.StdCall)]
        private static extern ClockReturn SetClock(int pakbus_address, DeviceTypeCodes device_type, out System.IntPtr return_data, out int return_data_len);

        //Wrapper for the SetClock function
        public static ClockReturn SetClock(int pakBusAddress, DeviceTypeCodes deviceType, ref string returnData)
        {
            //Declare variables for 'returned' parameters
            IntPtr returnPtr;
            int returnPtrLength;

            //Call the SimplePB.Dll function
            ClockReturn rtn = SetClock(pakBusAddress, deviceType, out returnPtr, out returnPtrLength);

            //Use the 'returned' parameters to retrieve the string data
            returnData = getStringFromIntPtr(returnPtr, returnPtrLength);

            //Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the SetValue function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "SetValue", CallingConvention = CallingConvention.StdCall)]
        private static extern SetValueReturn SetValue(int pakbus_address, DeviceTypeCodes device_type, System.IntPtr table_name, System.IntPtr field_name, System.IntPtr value);

        //Wrapper for the SetValue function
        public static SetValueReturn SetValue(int pakBusAddress, DeviceTypeCodes deviceType, string tableName, string fieldName, string value)
        {
            //Get pointers to the 'char' parameters created on the unmanaged heap
            IntPtr ptableName = getBytesForString(tableName);
            IntPtr pfieldName = getBytesForString(fieldName);
            IntPtr pvalue = getBytesForString(value);

            //Call the SimplePB.Dll function
            SetValueReturn rtn = SetValue(pakBusAddress, deviceType, ptableName, pfieldName, pvalue);

            //Free the memory allocated on the unmanged heap
            Marshal.FreeHGlobal(ptableName);
            Marshal.FreeHGlobal(pfieldName);
            Marshal.FreeHGlobal(pvalue);

            //Return success code to the calling function
            return rtn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        // Takes a pointer to an array of bytes or (char*) on the unmanaged heap,
        // converts the array to a string value on the managed heap and returns the String.
        // Assumes a byte array of UTF8 chars.
        private static string getStringFromIntPtr(IntPtr ptrToString, int dataLength)
        {
            if ((ptrToString != IntPtr.Zero))
            {
                byte[] MyData = new byte[dataLength];
                Marshal.Copy(ptrToString, MyData, 0, dataLength);
                return Encoding.UTF8.GetString(MyData);
            }
            else
            {
                return String.Empty;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Converts the String value to an Array of UTF8 encoded Bytes on the unmanaged heap and
        // returns a System.IntPtr to the Array.
        private static IntPtr getBytesForString(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            IntPtr unmanagedMemory;
            try
            {
                unmanagedMemory = Marshal.AllocHGlobal((bytes.Length + 1));
                Marshal.Copy(bytes, 0, unmanagedMemory, bytes.Length);
                // ' write null terminating character
                Marshal.WriteByte(unmanagedMemory, bytes.Length, 0);
            }
            catch
            {
                unmanagedMemory = IntPtr.Zero;
            }
            return unmanagedMemory;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Import the SimplePB.Dll and declare the GetTableRecordsCount function signature
        [DllImportAttribute("SimplePB.dll", EntryPoint = "GetTableRecordsCount", CallingConvention = CallingConvention.StdCall)]
        public static extern GetDataReturn GetTableRecordsCount(int pakbus_address, DeviceTypeCodes device_type, int table_no, ref UInt32 records_count);

    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public enum ClockReturn
    {

        // ''clock_success -> 0
        clock_success = 0,

        // ''clock_comm_failure -> -1
        clock_comm_failure = -1,

        // ''clock_port_not_opened -> -2
        clock_port_not_opened = -2,
    }
    public enum DeviceTypeCodes
    {

        // ''device_cr200 -> 1
        CR200 = 1,

        // ''device_cr10xpb -> 2
        CR10XPB = 2,

        // ''device_cr1000 -> 3
        CR1000 = 3,

        // ''device_cr3000 -> 4
        CR3000 = 4,

        // ''device_cr800 -> 5
        CR800 = 5,

        // ''device_cr6 -> 9
        CR6 = 9,

        // ''device_cr300 -> 13
        CR300 = 13
    }

    public enum FileControlCommandType
    {

        // ''command_compile_and_run -> 1
        command_compile_and_run = 1,

        // ''command_set_run_on_power_up -> 2
        command_set_run_on_power_up = 2,

        // ''command_make_hidden -> 3
        command_make_hidden = 3,

        // ''command_delete_file -> 4
        command_delete_file = 4,

        // ''command_format_device -> 5
        command_format_device = 5,

        // ''command_compile_and_run_leave_tables -> 6
        command_compile_and_run_leave_tables = 6,

        // ''command_stop_program -> 7
        command_stop_program = 7,

        // ''command_stop_program_and_delete -> 8
        command_stop_program_and_delete = 8,

        // ''command_make_os -> 9
        command_make_os = 9,

        // ''command_compile_and_run_no_power_up -> 10
        command_compile_and_run_no_power_up = 10,

        // ''command_pause -> 11
        command_pause = 11,

        // ''command_resume -> 12
        command_resume = 12,

        // ''command_stop_delete_and_run -> 13
        command_stop_delete_and_run = 13,

        // ''command_stop_delete_and_run_no_power -> 14
        command_stop_delete_and_run_no_power = 14,
    }
    public enum FileControlReturn
    {

        // ''filecontrol_success -> 0
        filecontrol_success = 0,

        // ''filecontrol_comm_failure -> -1
        filecontrol_comm_failure = -1,

        // ''filecontrol_not_open -> -2
        filecontrol_not_open = -2,
    }
    public enum GetAddressReturn
    {

        // ''getaddr_success -> 0
        getaddr_success = 0,

        // ''getaddr_comm_failure -> -1
        getaddr_comm_failure = -1,

        // ''getaddr_not_open -> -2
        getaddr_not_open = -2,
    }
    public enum GetDataReturn
    {

        // ''getdata_more -> 1
        getdata_more = 1,

        // ''getdata_complete -> 0
        getdata_complete = 0,

        // ''getdata_comm_failure -> -1
        getdata_comm_failure = -1,

        // ''getdata_not_open -> -2
        getdata_not_open = -2,

        // ''getdata_invalid_table_no -> -3
        getdata_invalid_table_no = -3,
    }
    public enum GetValueReturn
    {

        // ''getval_success -> 0
        getval_success = 0,

        // ''getval_comm_failure -> -1
        getval_comm_failure = -1,

        // ''getval_port_not_opened -> -2
        getval_port_not_opened = -2,
    }
    public enum GetTablesReturn
    {

        // ''gettables_success -> 0
        gettables_success = 0,

        // ''gettables_comm_failure -> 1
        gettables_comm_failure = 1,

        // ''gettables_not_open -> -2
        gettables_not_open = -2,
    }
    public enum GetStatusReturn
    {

        // ''getstatus_success -> 0
        getstatus_success = 0,

        // ''getstatus_comm_failure -> -1
        getstatus_comm_failure = -1,

        // ''getstatus_not_open -> -2
        getstatus_not_open = -2,
    }
    public enum FileSendReturn
    {

        // ''filesend_more -> 1
        filesend_more = 1,

        // ''filesend_complete -> 0
        filesend_complete = 0,

        // ''filesend_comm_failure -> -1
        filesend_comm_failure = -1,

        // ''filesend_not_opened -> -2
        filesend_not_opened = -2,

        // ''filesend_cant_open_source -> -3
        filesend_cant_open_source = -3,

        // ''filesend_file_name_invalid -> -4
        filesend_file_name_invalid = -4,

        // ''filesend_logger_timeout -> -5
        filesend_logger_timeout = -5,

        // ''filesend_invalid_file_offset -> -6
        filesend_invalid_file_offset = -6,

        // ''filesend_datalogger_error -> -7
        filesend_datalogger_error = -7,

        // ''filesend_filecontrol_error -> -8
        filesend_filecontrol_error = -8,

        // ''filesend_cant_get_prog_status -> -9
        filesend_cant_get_prog_status = -9,
    }
    public enum SetValueReturn
    {

        // ''setval_success -> 0
        setval_success = 0,

        // ''setval_comm_failure -> -1
        setval_comm_failure = -1,

        // ''setval_not_opened -> -2
        setval_not_opened = -2,
    }
}
