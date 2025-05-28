import { createAsyncThunk } from "@reduxjs/toolkit";
import { dynamicApi } from "../../apiClient";

export const deleteExperience = createAsyncThunk(
    'experience/deleteExperience',
    async (id: string, thunkAPI) => {
        try {
            await dynamicApi({
                method: 'DELETE',
                url: '/Owner/DeleteExperience',
                data: {id},
                withCredentials: true
            });

            return;

        } catch (error: any) {
            if (error.response.status === 400) {
                return thunkAPI.rejectWithValue(error.response.data);
            }
            return thunkAPI.rejectWithValue(error.message);
        }
    }
);