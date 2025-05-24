import { transformPayload } from "@/lib/utils/appFunctions";
import { createAsyncThunk } from "@reduxjs/toolkit";
import { dynamicApi } from "../../apiClient";
import { ProjectTechnologyFormData } from "@/lib/schemas";

export const addEditDeleteProjectTechnology = createAsyncThunk(
    'projectTechnology/addEditDeleteProjectTechnology',
    async (payload: ProjectTechnologyFormData, thunkAPI) => {
        try {
            const request = transformPayload(payload);

            const response = await dynamicApi({
                method: 'POST',
                url: '/Owner/AddEditDeleteProjectTechnology',
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