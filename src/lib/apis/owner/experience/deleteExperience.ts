import { createAsyncThunk } from "@reduxjs/toolkit";
import { dynamicApi } from "../../apiClient";

export const deleteExperience = createAsyncThunk(
    'experience/deleteExperience',
    async (id: string, thunkAPI) => {
        try {
            const response = await dynamicApi({
                method: 'DELETE',
                url: '/Owner/DeleteExperience',
                data: {id},
                withCredentials: true
            });

            if(response.status === 400) return thunkAPI.rejectWithValue(response.data);

            return;

        } catch (error: any) {
            return thunkAPI.rejectWithValue(error.message);
        }
    }
);