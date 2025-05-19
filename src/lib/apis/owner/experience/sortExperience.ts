import { createAsyncThunk } from "@reduxjs/toolkit";
import { dynamicApi } from "../../apiClient";

export const sortExperience = createAsyncThunk(
    'experience/sortExperience',
    async (payload: string[], thunkAPI) => {
        try {
            const response = await dynamicApi({
                method: 'POST',
                url: '/Owner/SortExperience',
                data: {experienceIdsInOrder: payload},
                withCredentials: true
            });

            if (response.status === 400) return thunkAPI.rejectWithValue(response.data);

            return;

        } catch (error: any) {
            return thunkAPI.rejectWithValue(error.message);
        }
    }
);