import { createAsyncThunk } from "@reduxjs/toolkit";
import { dynamicApi } from "../../apiClient";

export const sortProject = createAsyncThunk(
    'projectTechnology/sortProject',
    async (payload: string[], thunkAPI) => {
        try {
            await dynamicApi({
                method: 'POST',
                url: '/Owner/SortProject',
                data: {ProjectIdsInOrder: payload},
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