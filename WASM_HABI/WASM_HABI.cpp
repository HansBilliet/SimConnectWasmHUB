// WASM_HABI.cpp

#include <stdio.h>
#include <MSFS/MSFS.h>
#include <MSFS/MSFS_WindowsTypes.h>
#include <SimConnect.h>
#include <MSFS/Legacy/gauges.h>

#include <cstring>
#include <vector>
#include <chrono>

#include "WASM_HABI.h"
#include <iostream>

using namespace std;

const char* WASM_Name = "HABI_WASM";
const char* WASM_Version = "00.01";

const char* CLIENT_DATA_NAME_LVARS = "HABI_WASM.LVars";
const SIMCONNECT_CLIENT_DATA_ID CLIENT_DATA_ID_LVARS = 0;

const char* CLIENT_DATA_NAME_COMMAND = "HABI_WASM.Command";
const SIMCONNECT_CLIENT_DATA_ID CLIENT_DATA_ID_COMMAND = 1;

const char* CLIENT_DATA_NAME_ACKNOWLEDGE = "HABI_WASM.Acknowledge";
const SIMCONNECT_CLIENT_DATA_ID CLIENT_DATA_ID_ACKNOWLEDGE = 2;

const SIMCONNECT_CLIENT_DATA_DEFINITION_ID DATA_DEFINITION_ID_STRING_COMMAND = 0;
const SIMCONNECT_CLIENT_DATA_DEFINITION_ID DATA_DEFINITION_ID_ACKNOWLEDGE = 1;
const UINT16 START_LVAR_DEFINITION = 10;

const int MESSAGE_SIZE = 256;

HANDLE g_hSimConnect;

const char* CMD_Set = "HW.Set.";
const char* CMD_Reg = "HW.Reg.";

struct LVar {
	UINT16 ID;
	UINT16 Offset;
	char Name[MESSAGE_SIZE];
	float Value;
};
vector<LVar> LVars;

enum eEvents
{
	EVENT_FLIGHT_LOADED,
	EVENT_FRAME,
	EVENT_TEST,
	//EVENT_1SEC,
	//EVENT_FLIGHTLOADED,
	//EVENT_AIRCRAFTLOADED
};

enum eRequestID
{
	CMD
};

enum HABI_WASM_GROUP
{
	GROUP
};

//uint64_t millis()
//{
//	uint64_t ms = std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::high_resolution_clock::
//		now().time_since_epoch()).count();
//	return ms;
//}
//
//// Get time stamp in microseconds.
//uint64_t micros()
//{
//	uint64_t us = std::chrono::duration_cast<std::chrono::microseconds>(std::chrono::high_resolution_clock::
//		now().time_since_epoch()).count();
//	return us;
//}
//
//// Get time stamp in nanoseconds.
//uint64_t nanos()
//{
//	uint64_t ns = std::chrono::duration_cast<std::chrono::nanoseconds>(std::chrono::high_resolution_clock::
//		now().time_since_epoch()).count();
//	return ns;
//}

bool FindLVar(char* sLVar, LVar *pLVar)
{
	for (auto& lv : LVars)
	{
		if (strcmp(lv.Name, sLVar) == 0)
		{
			pLVar->ID = lv.ID;
			pLVar->Offset = lv.Offset;
			pLVar->Name[0] = '\0';
			strncat(pLVar->Name, lv.Name, 255);
			pLVar->Value = lv.Value;

			return true;
		}
	}

	return false;
}

void RegisterLVar(char* sLVar)
{
	fprintf(stderr, "%s: RegisterLVar -> Name \"%s\"", WASM_Name, sLVar);

	LVar lv;
	HRESULT hr;

	// Search if LVar is already registered
	if (!FindLVar(sLVar, &lv))
	{
		fprintf(stderr, "%s: RegisterLVar -> Creating new LVar", WASM_Name);

		// Add new LVar in the list
		UINT16 sz = LVars.size();
		lv.ID = sz + START_LVAR_DEFINITION;
		lv.Offset = sz * sizeof(float);
		lv.Name[0] = '\0';
		strncat(lv.Name, sLVar, 255);
		lv.Value = 0;
		LVars.push_back(lv);

		hr = SimConnect_AddToClientDataDefinition(
			g_hSimConnect,
			lv.ID,			// DefineID
			lv.Offset,		// Offset
			sizeof(FLOAT32)	// Size
		);

		fprintf(stderr, "%s: RegisterLVar -> ID: %u Offset: %u Value: %d", WASM_Name, lv.ID, lv.Offset, lv.Value);
	}

	// send acknowledge back to client
	hr = SimConnect_SetClientData(
		g_hSimConnect,
		CLIENT_DATA_ID_ACKNOWLEDGE,
		DATA_DEFINITION_ID_ACKNOWLEDGE,
		SIMCONNECT_CLIENT_DATA_SET_FLAG_DEFAULT,
		0,
		sizeof(lv),
		&lv
	);

	fprintf(stderr, "%s: RegisterLVar -> Sent Acknowledge ID: %u Offset: %u Value: %d", WASM_Name, lv.ID, lv.Offset, lv.Value);
}

void ReadLVars()
{
	for (auto& lv : LVars)
	{
		FLOAT64 val = 0;
		execute_calculator_code(lv.Name, &val, (SINT32 *)0, (PCSTRINGZ *)0);

		if (lv.Value != val)
		{
			lv.Value = val;

			fprintf(stderr, "%s: LVar %s with ID %u and Offset %u changed", WASM_Name, lv.Name, lv.ID, lv.Offset);
			fprintf(stderr, "%s: New value is %f", WASM_Name, lv.Value);

			HRESULT hr = SimConnect_SetClientData(
				g_hSimConnect,
				CLIENT_DATA_ID_LVARS,
				lv.ID,
				SIMCONNECT_CLIENT_DATA_SET_FLAG_DEFAULT,
				0,
				sizeof(lv.Value),
				&lv.Value
			);

			if (hr != S_OK)
			{
				fprintf(stderr, "%s: Error on Setting Client Data. %u, SimVar: %s (ID: %u)", WASM_Name);
			}
		}
	}
}

void CALLBACK MyDispatchProc(SIMCONNECT_RECV* pData, DWORD cbData, void* pContext)
{
	switch (pData->dwID)
	{
		case SIMCONNECT_RECV_ID_EVENT:
		{
			SIMCONNECT_RECV_EVENT* evt = (SIMCONNECT_RECV_EVENT*)pData;

			if (evt->uEventID == EVENT_TEST)
			{
				DWORD d = evt->dwData;
				char sCmd[1024];

				sprintf(sCmd, "%u (>L:A32NX_EFIS_L_OPTION, enum) %u (>L:A32NX_EFIS_R_OPTION, enum)", d, d);
				execute_calculator_code(sCmd, nullptr, nullptr, nullptr);
				fprintf(stderr, "%s: execute_calculator_code(\"%s\"))\n", WASM_Name, sCmd);
			}
			break; // end case SIMCONNECT_RECV_ID_EVENT
		}

		//case SIMCONNECT_RECV_ID_EVENT:
		//{
		//	SIMCONNECT_RECV_EVENT* evt = (SIMCONNECT_RECV_EVENT*)pData;

		//	switch (evt->uEventID)
		//	{
		//		case EVENT_1SEC:
		//		{
		//			// L:A32NX_EFIS_L_OPTION, enum
		//			// L:A32NX_EFIS_R_OPTION, enum
		//			// K:FUELSYSTEM_PUMP_TOGGLE
		//			// K:A32NX.FCU_HDG_INC

		//			uint64_t tStart;
		//			uint64_t tEnd;
		//			FLOAT64 val = 0;
		//			ID varID;
		//			PCSTRINGZ sCompiled;
		//			UINT32 uCompiledSize;

		//			if (LVars.size() != 0)
		//			{

		//				// MEASUREMENT 1 - using execute_calculator_code

		//				// start of measurement
		//				tStart = micros();

		//				// perform action
		//				for (auto& lv : LVars)
		//				{
		//					for (int i = 0; i < 500; i++)
		//					{
		//						execute_calculator_code("(L:A32NX_EFIS_L_OPTION)", &val, (SINT32*)0, (PCSTRINGZ*)0);
		//					}
		//				}

		//				// end of measurment
		//				tEnd = micros();

		//				fprintf(stderr, "%s: MEAS1: 500 times %u variables - Time: %llu - Last val: %f\n", WASM_Name, LVars.size(), tEnd - tStart, val);

		//				// MEASUREMENT 2 - using pre-compiled calculator code

		//				// start of measurement
		//				tStart = micros();

		//				// perform action
		//				for (auto& lv : LVars)
		//				{
		//					gauge_calculator_code_precompile(&sCompiled, &uCompiledSize, "(L:A32NX_EFIS_L_OPTION)");

		//					for (int i = 0; i < 500; i++)
		//					{
		//						execute_calculator_code(sCompiled, &val, (SINT32*)0, (PCSTRINGZ*)0);
		//					}
		//				}

		//				// end of measurment
		//				tEnd = micros();

		//				fprintf(stderr, "%s: MEAS2: 500 times %u size %u - Time: %llu - Last val: %f\n", WASM_Name, LVars.size(), uCompiledSize, tEnd - tStart, val);

		//				// MEASUREMENT 3 - using Gauge API

		//				// start of measurement
		//				tStart = micros();

		//				// perform action
		//				for (auto& lv : LVars)
		//				{
		//					varID = check_named_variable("A32NX_EFIS_L_OPTION");

		//					for (int i = 0; i < 500; i++)
		//					{
		//						val = get_named_variable_value(varID);
		//					}
		//				}

		//				// end of measurment
		//				tEnd = micros();

		//				fprintf(stderr, "%s: MEAS3: 500 times %u varID %i - Time: %llu - Last val: %f\n", WASM_Name, LVars.size(), varID, tEnd - tStart, val);
		//			}

		//			break;
		//		}
		//		default:
		//			fprintf(stderr, "%s: SIMCONNECT_RECV_ID_EVENT - Event: UNKNOWN %u\n", WASM_Name, evt->uEventID);
		//			break; // end case default
		//	}

		//	break; // end case SIMCONNECT_RECV_ID_EVENT
		//}

		//case SIMCONNECT_RECV_ID_EVENT_FILENAME:
		//{
		//	SIMCONNECT_RECV_EVENT_FILENAME* recv_data = (SIMCONNECT_RECV_EVENT_FILENAME*)pData;
		//	fprintf(stderr, "%s: SIMCONNECT_RECV_ID_EVENT_FILENAME - Event: %u - File: %s\n", WASM_Name, recv_data->uEventID, recv_data->szFileName);

		//	break; // end case SIMCONNECT_RECV_ID_EVENT_FILENAME
		//}

		case SIMCONNECT_RECV_ID_CLIENT_DATA:
		{
			SIMCONNECT_RECV_CLIENT_DATA* recv_data = (SIMCONNECT_RECV_CLIENT_DATA*)pData;

			switch (recv_data->dwRequestID)
			{
				case CMD: // "HW.Set." or "HW.Reg." received
				{
					char* sCmd = (char*)&recv_data->dwData;
					fprintf(stderr, "%s: SIMCONNECT_RECV_ID_CLIENT_DATA - CMD \"%s\"\n", WASM_Name, sCmd);

					// "HW.Set."
					if (strncmp(sCmd, CMD_Set, strlen(CMD_Set)) == 0)
					{
						execute_calculator_code(&sCmd[strlen(CMD_Set)], nullptr, nullptr, nullptr);
						break;
					}

					// "HW.Reg."
					if (strncmp(sCmd, CMD_Reg, strlen(CMD_Reg)) == 0)
					{
						RegisterLVar(&sCmd[strlen(CMD_Reg)]);
						break;
					}

					break; // end case CMD
				}

				default:
					fprintf(stderr, "%s: SIMCONNECT_RECV_ID_CLIENT_DATA - UNKNOWN request %u\n", WASM_Name, recv_data->dwRequestID);
					break; // end case default
			}

			break; // end case SIMCONNECT_RECV_ID_CLIENT_DATA
		}

		case SIMCONNECT_RECV_ID_EVENT_FRAME:
		{
			ReadLVars();
			break; // end case SIMCONNECT_RECV_ID_EVENT_FRAME
		}

		default:
			break; // end case default
	}
}

void RegisterClientDataArea()
{
	HRESULT hr;

	// Create Client Data Area for the LVars
	// Max size of Client Area Data can be 8192 which is 2048 LVars of type float
	// If more LVars are required, then multiple Client Data Areas would need to be foreseen
	hr = SimConnect_MapClientDataNameToID(g_hSimConnect, CLIENT_DATA_NAME_LVARS, CLIENT_DATA_ID_LVARS);
	if (hr != S_OK) {
		fprintf(stderr, "%s: Error creating Client Data Area %s. %u", WASM_Name, CLIENT_DATA_NAME_LVARS, hr);
		return;
	}
	SimConnect_CreateClientData(g_hSimConnect, CLIENT_DATA_ID_LVARS, SIMCONNECT_CLIENTDATA_MAX_SIZE, SIMCONNECT_CREATE_CLIENT_DATA_FLAG_DEFAULT);

	// Create Client Data Area for a command ("WH.Reg.[LVar]" or "WH.Set.[LVar]")
	// Max size of string in SimConnect is 256 characters (including the '/0')
	// If longer commands are required, then they would need to be split in several strings
	hr = SimConnect_MapClientDataNameToID(g_hSimConnect, CLIENT_DATA_NAME_COMMAND, CLIENT_DATA_ID_COMMAND);
	if (hr != S_OK) {
		fprintf(stderr, "%s: Error creating Client Data Area %s. %u", WASM_Name, CLIENT_DATA_NAME_COMMAND, hr);
		return;
	}
	SimConnect_CreateClientData(g_hSimConnect, CLIENT_DATA_ID_COMMAND, MESSAGE_SIZE, SIMCONNECT_CREATE_CLIENT_DATA_FLAG_DEFAULT);

	// Create Client Data Area to acknowledge a registration back to the client
	hr = SimConnect_MapClientDataNameToID(g_hSimConnect, CLIENT_DATA_NAME_ACKNOWLEDGE, CLIENT_DATA_ID_ACKNOWLEDGE);
	if (hr != S_OK) {
		fprintf(stderr, "%s: Error creating Client Data Area %s. %u", WASM_Name, CLIENT_DATA_NAME_ACKNOWLEDGE, hr);
		return;
	}
	SimConnect_CreateClientData(g_hSimConnect, CLIENT_DATA_ID_ACKNOWLEDGE, sizeof(LVar), SIMCONNECT_CREATE_CLIENT_DATA_FLAG_DEFAULT);

	// This Data Definition will be used with the COMMAND Client Area Data
	hr = SimConnect_AddToClientDataDefinition(
		g_hSimConnect,
		DATA_DEFINITION_ID_STRING_COMMAND,
		0,				// Offset
		MESSAGE_SIZE				// Size
		);

	// This Data Definition will be used with the ACKNOWLEDGE Client Area Data
	hr = SimConnect_AddToClientDataDefinition(
		g_hSimConnect,
		DATA_DEFINITION_ID_ACKNOWLEDGE,
		0,				// Offset
		sizeof(LVar)	// Size
		);

	// We immediately start to listen to commands coming from the client
	SimConnect_RequestClientData(
		g_hSimConnect,
		CLIENT_DATA_ID_COMMAND,						// ClientDataID
		CMD,										// RequestID
		DATA_DEFINITION_ID_STRING_COMMAND,			// DefineID
		SIMCONNECT_CLIENT_DATA_PERIOD_ON_SET,		// Trigger when data is set
		SIMCONNECT_CLIENT_DATA_REQUEST_FLAG_DEFAULT	// Always receive data
		);
}

extern "C" MSFS_CALLBACK void module_init(void)
{
	HRESULT hr;

	g_hSimConnect = 0;

	fprintf(stderr, "%s: Initializing WASM version [%s]", WASM_Name, WASM_Version);

	hr = SimConnect_Open(&g_hSimConnect, WASM_Name, (HWND)NULL, 0, 0, 0);
	if (hr != S_OK)
	{
		fprintf(stderr, "%s: SimConnect_Open failed.\n", WASM_Name);
		return;
	}

	hr = SimConnect_SubscribeToSystemEvent(g_hSimConnect, EVENT_FRAME, "Frame");
	if (hr != S_OK)
	{
		fprintf(stderr, "%s: SimConnect_SubsribeToSystemEvent \"Frame\" failed.\n", WASM_Name);
		return;
	}

	//hr = SimConnect_SubscribeToSystemEvent(g_hSimConnect, EVENT_1SEC, "1sec");
	//if (hr != S_OK)
	//{
	//	fprintf(stderr, "%s: SimConnect_SubsribeToSystemEvent \"1sec\" failed.\n", WASM_Name);
	//	return;
	//}

	//hr = SimConnect_SubscribeToSystemEvent(g_hSimConnect, EVENT_AIRCRAFTLOADED, "AircraftLoaded");
	//if (hr != S_OK)
	//{
	//	fprintf(stderr, "%s: SimConnect_SubsribeToSystemEvent \"AircraftLoaded\" failed.\n", WASM_Name);
	//	return;
	//}

	//hr = SimConnect_SubscribeToSystemEvent(g_hSimConnect, EVENT_FLIGHTLOADED, "FlightLoaded");
	//if (hr != S_OK)
	//{
	//	fprintf(stderr, "%s: SimConnect_SubsribeToSystemEvent \"FlightLoaded\" failed.\n", WASM_Name);
	//	return;
	//}

	// Add EVENT_TEST and let it do something
	hr = SimConnect_MapClientEventToSimEvent(g_hSimConnect, EVENT_TEST, "HABI_WASM.EVENT_TEST");
	if (hr != S_OK)
	{
		fprintf(stderr, "%s: SimConnect_MapClientEventToSimEvent failed.\n", WASM_Name);
		return;
	}
	hr = SimConnect_AddClientEventToNotificationGroup(g_hSimConnect, HABI_WASM_GROUP::GROUP, EVENT_TEST, false);
	if (hr != S_OK)
	{
		fprintf(stderr, "%s: SimConnect_AddClientEventToNotificationGroup failed.\n", WASM_Name);
		return;
	}
	hr = SimConnect_SetNotificationGroupPriority(g_hSimConnect, HABI_WASM_GROUP::GROUP, SIMCONNECT_GROUP_PRIORITY_HIGHEST);
	if (hr != S_OK)
	{
		fprintf(stderr, "%s: SimConnect_SetNotificationGroupPriority failed.\n", WASM_Name);
		return;
	}

	hr = SimConnect_CallDispatch(g_hSimConnect, MyDispatchProc, NULL);
	if (hr != S_OK)
	{
		fprintf(stderr, "%s: SimConnect_CallDispatch failed.\n", WASM_Name);
		return;
	}

	RegisterClientDataArea();

	fprintf(stderr, "%s: Initialization completed", WASM_Name);
}

extern "C" MSFS_CALLBACK void module_deinit(void)
{
	fprintf(stderr, "%s: De-initializing WASM version [%s]", WASM_Name, WASM_Version);

	if (!g_hSimConnect)
	{
		fprintf(stderr, "%s: SimConnect handle was not valid.\n", WASM_Name);
		return;
	}

	HRESULT hr = SimConnect_Close(g_hSimConnect);
	if (hr != S_OK)
	{
		fprintf(stderr, "%s: SimConnect_Close failed.\n", WASM_Name);
		return;
	}

	fprintf(stderr, "%s: De-initialization completed", WASM_Name);
}
