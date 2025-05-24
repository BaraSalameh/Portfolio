import { transformPayload } from "@/lib/utils/appFunctions";
import { createAsyncThunk } from "@reduxjs/toolkit";
import { dynamicApi } from "../../apiClient";
import { ExperienceFormData } from "@/lib/schemas";

export const addEditExperience = createAsyncThunk(
    'experience/addEditExperience',
    async (payload: ExperienceFormData, thunkAPI) => {
        try {
            const request = transformPayload(payload);

            const response = await dynamicApi({
                method: 'POST',
                url: '/Owner/addEditExperience',
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