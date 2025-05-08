import { createAsyncThunk } from "@reduxjs/toolkit";
import { dynamicApi } from "../../apiClient";

export const sortEducation = createAsyncThunk(
    'education/sortEducation',
    async (payload: string[], thunkAPI) => {
        try {
            const response = await dynamicApi({
                method: 'POST',
                url: '/Owner/ReOrderEducation',
                data: {educationIdsInOrder: payload},
                withCredentials: true
            });

            if (response.status === 400) return thunkAPI.rejectWithValue(response.data);

            return;

        } catch (error: any) {
            return thunkAPI.rejectWithValue(error.message);
        }
    }
);