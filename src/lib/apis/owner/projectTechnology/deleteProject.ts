import { createAsyncThunk } from "@reduxjs/toolkit";
import { dynamicApi } from "../../apiClient";

export const deleteProject = createAsyncThunk(
    'projectTechnology/deleteProject',
    async (id: string, thunkAPI) => {
        try {
            await dynamicApi({
                method: 'DELETE',
                url: '/Owner/DeleteProject',
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