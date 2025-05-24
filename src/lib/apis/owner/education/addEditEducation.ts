import { EducationFormData } from "@/lib/schemas";
import { transformPayload } from "@/lib/utils/appFunctions";
import { createAsyncThunk } from "@reduxjs/toolkit";
import { dynamicApi } from "../../apiClient";

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

            if (response.status === 400) return thunkAPI.rejectWithValue(response.data);

            return;

        } catch (error: any) {
            return thunkAPI.rejectWithValue(error.message);
        }
    }
);