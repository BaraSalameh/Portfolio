import { createAsyncThunk } from "@reduxjs/toolkit";
import { dynamicApi } from "../../apiClient";

export const signMessage = createAsyncThunk(
    'contactMessage/signMessage',
    async (id: string, thunkAPI) => {
        try {
            await dynamicApi({
                method: 'POST',
                url: '/Owner/SignMessage',
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