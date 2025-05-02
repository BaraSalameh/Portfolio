import { EducationFormData } from "@/lib/schemas/educationSchema";
import { transformPayload } from "@/lib/utils/appFunctions";
import { createAsyncThunk } from "@reduxjs/toolkit";
import { dynamicApi } from "../apiClient";

export const addEditEducation = createAsyncThunk(
    'education/addEditEducation',
    async (payload: EducationFormData, thunkAPI) => {
        try {
            const request = transformPayload(payload);


            const response = await dynamicApi({
                method: 'POST',
                url: '/Owner/AddEditEducation',
                data: request,
                withCredentials: true
            });

            return;

        } catch (error: any) {
            if (error.response?.data) {
                return thunkAPI.rejectWithValue(error.response.data);
            }
            return thunkAPI.rejectWithValue(['Unexpected error occurred']);
        }
    }
);