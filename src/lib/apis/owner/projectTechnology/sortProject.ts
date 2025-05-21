import { createAsyncThunk } from "@reduxjs/toolkit";
import { dynamicApi } from "../../apiClient";

export const sortProject = createAsyncThunk(
    'projectTechnology/sortProject',
    async (payload: string[], thunkAPI) => {
        try {
            const response = await dynamicApi({
                method: 'POST',
                url: '/Owner/SortProject',
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